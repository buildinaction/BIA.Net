// <copyright file="ServiceSite.cs" company="$companyName$">
// Copyright (c) $companyName$. All rights reserved.
// </copyright>

namespace $safeprojectname$.Services
{
    using BIA.Net.Authentication.Business.Helpers;
    using BIA.Net.Business.Services;
    using BIA.Net.Common.Helpers;
    using Business.DTO;
    using Business.Helpers;
    using Common;
    using Model;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Service to manipulate Site
    /// </summary>
    public class ServiceSite : TServiceDTO<SiteDTO, Site, $saferootprojectname$DBContainer>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceSite"/> class.
        /// </summary>
        public ServiceSite()
        {
            UserInfo userInfo = (UserInfo)UserInfo.GetCurrentUserInfo();
            int userId = userInfo.Properties.Id;
            List<string> roles = userInfo.Roles;
            if (userId != 0)
            {
                Repository.FilterContextRead = s => roles.Any(r => r == Constants.RoleAdmin) || s.Members.Any(m => m.User.Id == userId);
                Repository.FilterContextWrite = s => roles.Any(r => r == Constants.RoleAdmin) || s.Members.Any(m => m.User.Id == userId && m.MemberRole.Any(mr => mr.Id == Constants.RoleSiteAdminId));
            }
            else
            {
                // filters used by webapi
                string userLogin = userInfo.Login;
                Repository.FilterContextRead = s => roles.Any(r => r == Constants.RoleAdmin) || s.Members.Any(m => m.User.Login == userLogin);
                Repository.FilterContextWrite = s => roles.Any(r => r == Constants.RoleAdmin) || s.Members.Any(m => m.User.Login == userLogin && m.MemberRole.Any(mr => mr.Id == Constants.RoleSiteAdminId));
            }
        }

        /// <summary>
        /// Get all the sites and the admins of this site
        /// </summary>
        /// <returns>list of sites and the admins of this site</returns>
        public List<SiteAdminsDTO> GetAllSitesAndAdmins()
        {
            List<SiteAdminsDTO> sitesAdmins = new List<SiteAdminsDTO>();

            List<SiteDTO> sites = GetAll(AllServicesDTO.ServiceAccessMode.All);

            if (sites != null && sites.Any())
            {
                ServiceMember serviceMember = BIAUnity.Resolve<ServiceMember>();
                foreach (SiteDTO site in sites)
                {
                    SiteAdminsDTO sitesAdminsDTO = new SiteAdminsDTO
                    {
                        Admins = new List<AdminDTO>(),
                        Site = site.Title
                    };

                    List<MemberDTO> admins = serviceMember.GetAllAdminsFromSite(site.Id);
                    if (admins != null && admins.Any())
                    {
                        foreach (MemberDTO admin in admins)
                        {
                            if (admin.User != null && !string.IsNullOrEmpty(admin.User.Login))
                            {
                                AdminDTO adminDto = new AdminDTO
                                {
                                    Email = admin.User.Email,
                                    FirstName = admin.User.FirstName,
                                    LastName = admin.User.LastName,
                                    Login = admin.User.Login,
                                    Language = admin.User.Country
                                };

                                sitesAdminsDTO.Admins.Add(adminDto);
                            }
                        }
                    }

                    sitesAdmins.Add(sitesAdminsDTO);
                }
            }

            return sitesAdmins;
        }
    }
}