namespace $safeprojectname$.Jwt
{
    using Business.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Net.Http.Headers;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    public class BiaJwtAuthorizeAttribute : AuthorizeAttribute
    {
        //
        // Summary:
        //     Indicates whether the specified control is authorized.
        //
        // Parameters:
        //   actionContext:
        //     The context.
        //
        // Returns:
        //     true if the control is authorized; otherwise, false.
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            UserInfo userInfo = null;

            try
            {
                userInfo = (UserInfo)UserInfo.GetCurrentUserInfo();
            }
            catch (Exception ex)
            {
                actionContext.Response =
                new System.Net.Http.HttpResponseMessage(
                System.Net.HttpStatusCode.Unauthorized)
                {
                    ReasonPhrase = ex.Message
                };

                return false;
            }

            if (userInfo != null && !string.IsNullOrWhiteSpace(userInfo.Login))
            {
                if (!string.IsNullOrWhiteSpace(this.Roles))
                {
                    List<string> allowedRoles = this.Roles?.Split(',')?.Select(x => x.Trim()).ToList();
                    return allowedRoles.Intersect(userInfo.Roles, StringComparer.OrdinalIgnoreCase).Any();
                }
                else
                {
                    return true;
                }
            }

            return false;
        }
    }
}