// <copyright file="ExampleService.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace $safeprojectname$
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using BIA.Net.Common.Helpers;
    using Business.DTO;
    using Business.Services;

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ExampleService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ExampleService.svc or ExampleService.svc.cs at the Solution Explorer and start debugging.

    /// <summary>
    /// Service Exemple
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ExampleService : IExampleService
    {
        /// <summary>
        /// return true if ok.
        /// </summary>
        /// <returns>true if ok</returns>
        public bool Ping()
        {
            return true;
        }

        /// <summary>
        /// Get all User
        /// </summary>
        /// <returns>list of <see cref="UserDTO"/></returns>
        public List<UserDTO> GetExampleAllUser()
        {
            ServiceUser serviceUser = BIAUnity.Resolve<ServiceUser>();
            List<UserDTO> user = serviceUser.GetAll();

            return user;
        }

        /// <summary>
        /// Get list of users by company
        /// </summary>
        /// <param name="companyName">company name</param>
        /// <param name="externalCompanyName">external company name</param>
        /// <returns>list of <see cref="UserDTO"/></returns>
        public List<UserDTO> GetExampleUsersByCompany(string companyName, string externalCompanyName)
        {
            ServiceExampleForStoredProcedure serviceExampleForStoredProcedure = BIAUnity.Resolve<ServiceExampleForStoredProcedure>();
            List<UserDTO> user = serviceExampleForStoredProcedure.GetExampleUsersByCompany(companyName, externalCompanyName);

            return user;
        }
    }
}
