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
        /// <returns>all user exclude member of a site</returns>
        public virtual List<UserDTO> GetAllExcludeSite(int? siteId)
        {
            if (siteId == null)
            {
                return GetAll();
            }

            IQueryable<User> query = Repository.GetStandardQuery(BIA.Net.Model.DAL.AccessMode.Read);
            List<UserDTO> list = query.Where(a => !a.Members.Any(m => m.Site.Id == siteId)).Select(GetSelectorExpression()).ToList();
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
            IQueryable<User> query = Repository.GetStandardQuery(TranslateAccess(mode));
            return ToListDTO(query.Where(a => a.Login == login)).SingleOrDefault();
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
            IQueryable<User> query = Repository.GetStandardQuery(BIA.Net.Model.DAL.AccessMode.Read);
            UserDTO userDTO = query.Where(a => a.Members.Any(m => m.Id == id)).Select(GetSelectorExpression()).SingleOrDefault();
            return userDTO;
        }
    }
}