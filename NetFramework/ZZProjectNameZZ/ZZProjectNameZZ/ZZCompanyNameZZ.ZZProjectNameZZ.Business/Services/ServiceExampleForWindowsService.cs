// <copyright file="ServiceMember.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace ZZCompanyNameZZ.ZZProjectNameZZ.Business.Services
{
    using BIA.Net.Business.Services;
    using BIA.Net.Common;
    using Business.DTO;
    using Common;
    using Model;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Service to manipulate Member
    /// </summary>
    public class ServiceExampleForWindowsService : TServiceDTO<SiteDTO, Site, ZZProjectNameZZDBContainer>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceExampleForWindowsService"/> class.
        /// </summary>
        /*/// <param name="userInfoContainer">parameters of service.</param>*/
        public ServiceExampleForWindowsService(/*IUserInfoContainer userInfoContainer*/)
        {
        }

        /// <summary>
        /// Run the service
        /// </summary>
        /// <returns>The time before next start</returns>
        public int Run()
        {
            TraceManager.Debug("ServiceExampleForService", "Run", "Begin.");
            int nextChangeIn = AppSettingsReader.IntervalSync;
            try
            {
                List<SiteDTO> sites = GetAll(AllServicesDTO.ServiceAccessMode.All);
                foreach (SiteDTO site in sites)
                {
                    Console.WriteLine("Site : " + site.Title);
                }
            }
            catch (Exception e)
            {
                TraceManager.Error("ServiceExampleForService", "Run", "Error : " + e.Message);
            }

            TraceManager.Debug("ServiceExampleForService", "Run", "Stop.");
            return nextChangeIn;
        }
    }
}