// <copyright file="FilterConfig.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace ZZCompanyNameZZ.ZZProjectNameZZ.MVC
{
    using BIA.Net.Authentication.MVC;
    using Business.DTO;
    using MVC.Helpers;
    using System.Web.Mvc;

    /// <summary>
    /// Filter Configuration
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Registers the global filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new BIAAuthorizationFilterMVC<UserInfoMVC, UserDTO>(roles: "User,Admin,Service,Internal"));
        }
    }
}