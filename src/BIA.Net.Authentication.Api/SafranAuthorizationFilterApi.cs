// <copyright file="MyAuthorizationFilter.cs" company="Safran">
// Copyright (c) Safran. All rights reserved.
// </copyright>

namespace BIA.Net.Authentication.Api
{
    using Web;
    using Business;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Security.Principal;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using BIA.Net.Authentication.Business.Helpers;

    /// <summary>
    /// Authorization Filter
    /// </summary>
    /// <seealso cref="System.Web.Mvc.IAuthorizationFilter" />
    /// <typeparam name="TServiceSynchronizeUser">The type of the service to synchronize users in DB.</typeparam>
    /// <typeparam name="TUserInfo">The type of the format stocked in session variable.</typeparam>
    public class SafranAuthorizationFilterApi<TUserInfo, TUserDB> : BaseAuthorizationFilter<TUserInfo, TUserDB>, IAuthorizationFilter
        where TUserInfo : AUserInfo<TUserDB>, new()
        where TUserDB : IUserDB, new()
    {
        public bool AllowMultiple
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SafranAuthorizationFilterApi{TServiceSynchronizeUser, TUserInfo}"/> class.
        /// </summary>
        public SafranAuthorizationFilterApi(string roles = null, List<RolesRedirectURL> rolesRedirect = null, string rolesAllowAnonymous = null) 
            : base(roles, rolesRedirect, rolesAllowAnonymous)
        { }

        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            /*
            var principal = actionContext.RequestContext.Principal;
            HttpSessionState Session = HttpContext.Current.Session;

            TUserInfo user = default(TUserInfo);
            user = PrepareUserInfo(principal, Session);
            actionContext.RequestContext.Principal = (IPrincipal) user;*/

            TUserInfo user = (TUserInfo)AUserInfo<TUserDB>.GetCurrentUserInfo();

            RolesRedirectURL roleRedirect;
            RolesRedirectAction action = CheckAuthorize(user, out roleRedirect, IsAllowAnonymous, DisableRedirectRoles, actionContext);

            switch (action)
            {
                case RolesRedirectAction.Unauthorized:
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
                case RolesRedirectAction.Redirect:
                    throw new HttpResponseException(roleRedirect.HttpResponseMessage);
                default:
                    return continuation();
            }
        }


        private bool IsAllowAnonymous(HttpActionContext actionContext)
        {
            bool isAllowed = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>(true).Any()
                || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>(true).Any();

            return isAllowed;
        }

        private static List<string> DisableRedirectRoles(HttpActionContext actionContext)
        {
            if (actionContext.ActionDescriptor.GetCustomAttributes<DisableRedirectAttribute>(true).Any()
                || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<DisableRedirectAttribute>(true).Any())
            {
                List<string> roles = new List<string>();
                var actionAttributes = actionContext.ActionDescriptor.GetCustomAttributes<DisableRedirectAttribute>(true);
                var controllerAttributes = actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<DisableRedirectAttribute>(true);
                var attributes = actionAttributes.Concat(controllerAttributes).OfType<DisableRedirectAttribute>().ToList();
                foreach (var disableRedirectAttribute in attributes)
                {
                    List<string> curentRoles = disableRedirectAttribute.GetRoles();
                    if (curentRoles == null) return new List<string>();
                    else
                    {
                        foreach (string role in curentRoles)
                        {
                            if (!roles.Contains(role)) roles.Add(role);
                        }
                    }
                }
                return roles;
            }
            return null;
        }

        static public void ConnectUser(TUserDB aspNetUser, string secondaryLogin)
        {
            HttpContext.Current.User = (IPrincipal)ConnectUser(HttpContext.Current.Session, aspNetUser, secondaryLogin);
        }

        static public void DisconnectUser()
        {
            HttpContext.Current.User = (IPrincipal)DisconnectUser(HttpContext.Current.Session);
        }
    }
}