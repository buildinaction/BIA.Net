// <copyright file="ServiceMember.cs" company="$companyName$">
// Copyright (c) $companyName$. All rights reserved.
// </copyright>

namespace $safeprojectname$.Services
{
    using BIA.Net.Business.Services;
    using Business.DTO;
    using Common;
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Service to manipulate Member
    /// </summary>
    public class ServiceMember : TServiceDTO<MemberDTO, Member, $saferootprojectname$DBContainer>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceMember"/> class.
        /// </summary>
        public ServiceMember()
        {
            Repository.ListInclude = new List<Expression<Func<Member, dynamic>>>() { (m => m.User) };
        }

        /// <summary>
        /// Gets all member of a site.
        /// </summary>
        /// <param name="siteId">The site identifier.</param>
        /// <returns>all member of a site</returns>
        public virtual List<MemberDTO> GetAllFromSite(int? siteId)
        {
            List<MemberDTO> list = null;

            if (!siteId.HasValue)
            {
                list = this.GetAll();
            }
            else
            {
                list = this.GetAllWhere(m => m.Site.Id == siteId.Value, AllServicesDTO.ServiceAccessMode.Read);
            }

            return list;
        }

        /// <summary>
        /// Get all admins of the site
        /// </summary>
        /// <param name="siteId">id of the site</param>
        /// <returns>list of admin</returns>
        public List<MemberDTO> GetAllAdminsFromSite(int siteId)
        {
            return this.GetAllWhere(m => m.Site.Id == siteId && m.MemberRole.Any(x => x.Id == Constants.RoleSiteAdminId), AllServicesDTO.ServiceAccessMode.All);
        }
    }
}