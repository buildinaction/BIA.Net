// <copyright file="ServiceSite.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace ZZCompanyNameZZ.ZZProjectNameZZ.Business.Services
{
    using BIA.Net.Authentication.Business.Helpers;
    using BIA.Net.Business.Services;
    using BIA.Net.Common.Helpers;
    using BIA.Net.Model;
    using Business.DTO;
    using Business.Helpers;
    using Common;
    using Model;
    using Model.DAL;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic;

    /// <summary>
    /// Service to manipulate Site
    /// </summary>
    public class ServiceSite : TServiceDTO<SiteDTO, Site, ZZProjectNameZZDBContainer>
    {
        /// <summary>
        /// the service member
        /// </summary>
        private readonly ServiceMember serviceMember;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceSite"/> class.
        /// </summary>
        /// <param name="serviceMember">The service member.</param>
        public ServiceSite(ServiceMember serviceMember)
        {
            this.serviceMember = serviceMember;

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
                foreach (SiteDTO site in sites)
                {
                    SiteAdminsDTO sitesAdminsDTO = new SiteAdminsDTO
                    {
                        Admins = new List<AdminDTO>(),
                        Site = site.Title
                    };

                    List<MemberDTO> admins = this.serviceMember.GetAllAdminsFromSite(site.Id);
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

        /// <summary>
        /// Updates site with members/memberRole
        /// </summary>
        /// <param name="siteToUpdate">The value.</param>
        /// <returns>site updated</returns>
        public SiteDTO UpdateWithMembers(SiteDTO siteToUpdate)
        {
            Site site = new Site();
            GetMapper().MapToModel(siteToUpdate, site);
            GenericRepositoryParmeter cascadeUpdtateParam = new GenericRepositoryParmeter()
            {
                SubListRules = new Dictionary<string, SubListRule>()
                {
                    { "Members", new SubListRule { CascadeUpdate = true } }
                }
            };
            Site siteUpdated = this.Repository.Update(site, cascadeUpdtateParam);
            return this.Find(siteUpdated.Id);
        }

        /// <summary>
        /// Deletes the by identifier with members.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>nb deleted</returns>
        public int DeleteByIdWithMembers(int id)
        {
            SiteDTO siteToDelete = this.Find(id);

            if (siteToDelete != null)
            {
                bool wasInTransaction = GenericTransaction.BeginTransaction();
                try
                {
                    if (siteToDelete.MembersIds != null)
                    {
                        foreach (int membersId in siteToDelete.MembersIds)
                        {
                            this.serviceMember.DeleteById(membersId);
                        }
                    }

                    return this.DeleteById(id);
                }
                finally
                {
                    GenericTransaction.EndTransaction(wasInTransaction);
                }
            }

            return default;
        }

        /// <summary>
        /// Return the list of sites managed by the current user
        /// </summary>
        /// <returns>a list of siteDTO</returns>
        public List<SiteDTO> GetListSiteManagedByCurrentUser()
        {
            return GetAll(AllServicesDTO.ServiceAccessMode.Write).Distinct().OrderBy(s => s.Title).ToList();
        }

        /// <summary>
        /// Return the list of sites where the user is member
        /// </summary>
        /// <returns>a list of siteDTO</returns>
        public List<SiteDTO> GetListSiteCurrentUserIsMember()
        {
            return GetAll(AllServicesDTO.ServiceAccessMode.Read).Distinct().OrderBy(s => s.Title).ToList();
        }
    }
}