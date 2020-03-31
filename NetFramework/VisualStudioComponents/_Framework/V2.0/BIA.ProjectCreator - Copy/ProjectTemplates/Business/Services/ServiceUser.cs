// <copyright file="ServiceUser.cs" company="$companyName$">
// Copyright (c) $companyName$. All rights reserved.
// </copyright>

namespace $safeprojectname$.Services
{
    using BIA.Net.Business.Services;
    using Business.DTO;
    using Model;
    using System.Collections.Generic;
    using System.Linq;
    using static BIA.Net.Business.Services.AllServicesDTO;

    /// <summary>
    /// Service to manipulate User
    /// </summary>
    public class ServiceUser : TServiceDTO<UserDTO, User, $saferootprojectname$DBContainer>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceUser"/> class.
        /// </summary>
        public ServiceUser()
        {
            Repository.FilterContextRead = a => a.DAIEnable == true;
        }

        /// <summary>
        /// Resets the dai enable.
        /// </summary>
        /// <param name="user">The ASP net user.</param>
        /// <returns>Resets the dai enable</returns>
        public UserDTO SetUserValidity(UserDTO user)
        {
            Repository.FilterContextWrite = a => true;
            UserDTO updatedUser = UpdateValues(user, new List<string>() { nameof(UserDTO.DAIEnable) });
            Repository.FilterContextWrite = null;
            return updatedUser;
        }

        /// <summary>
        /// Gets all user exclude member of a site.
        /// </summary>
        /// <param name="siteId">The site identifier.</param>
        /// <param name="memberId">The id member to include in filter</param>
        /// <returns>all user exclude member of a site</returns>
        public virtual List<UserDTO> GetAllExcludeSite(int? siteId, int? memberId = null)
        {
            List<UserDTO> list = null;

            if (siteId == null)
            {
                list = this.GetAll();
            }
            else
            {
                if (memberId == null)
                {
                    list = this.GetAllWhere(a => !a.Members.Any(m => m.Site.Id == siteId), ServiceAccessMode.Read);
                }
                else
                {
                    list = this.GetAllWhere(a => !a.Members.Any(m => m.Site.Id == siteId) || a.Members.Any(m => m.Site.Id == siteId && m.Id == memberId), ServiceAccessMode.Read);
                }
            }

            return list;
        }

        /// <summary>
        /// Finds user by login.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <param name="mode">The mode.</param>
        /// <returns>UserDTO</returns>
        public UserDTO FindByLogin(string login, ServiceAccessMode mode = ServiceAccessMode.Read)
        {
            return this.GetAllWhere(a => a.Login == login, mode)?.SingleOrDefault();
        }

        /// <summary>
        /// Inserts the user.
        /// </summary>
        /// <param name="user">The ASP user.</param>
        /// <returns>UserDTO insered</returns>
        public UserDTO InsertUser(UserDTO user)
        {
            return Insert(user);
        }

        /// <summary>
        /// Gets the user by member identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The user</returns>
        public UserDTO GetByMemberId(int id)
        {
            return this.GetAllWhere(a => a.Members.Any(m => m.Id == id), ServiceAccessMode.Read)?.SingleOrDefault();
        }
    }
}