namespace $safeprojectname$.Services.Authentication
{
    using BIA.Net.Authentication.Business;
    using BIA.Net.Authentication.Business.Helpers;
    using BIA.Net.Authentication.Business.Synchronize;
    using BIA.Net.Business.Services;
    using BIA.Net.Common;
    using BIA.Net.Common.Helpers;
    using DTO;
    using Helpers;
    using Services;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.DirectoryServices.AccountManagement;
    using System.Linq;

    /// <summary>
    /// Service to acces user in DB
    /// </summary>
    public class ServiceSynchronizeUser : AServiceSynchronizeUser<ADUserInfo, UserDTO>
    {
        /// <summary>
        /// The service ASP net users
        /// </summary>
        private ServiceUser srvUser = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceSynchronizeUser"/> class.
        /// </summary>
        public ServiceSynchronizeUser()
        {
        }

        /// <summary>
        /// Gets or sets the service ASP net users.
        /// </summary>
        /// <value>
        /// The service ASP net users.
        /// </value>
        private ServiceUser ServiceUser
        {
            get
            {
                if (srvUser == null)
                {
                    srvUser = BIAUnity.Resolve<ServiceUser>();
                }

                return srvUser;
            }

            set
            {
                srvUser = value;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
        }

        /// <summary>
        /// Resets the dai to enable.
        /// </summary>
        /// <param name="user">The ASP net user.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>the aspnet user</returns>
        public override ILinkedProperties SetUserValidity(ILinkedProperties user, bool value)
        {
            UserDTO updatedUser;
            ((UserDTO)user).DAIEnable = value;
            updatedUser = ServiceUser.SetUserValidity((UserDTO)user);
            return updatedUser;
        }

        /// <summary>
        /// Inserts the specified ASP user.
        /// </summary>
        /// <param name="user">The ASP user.</param>
        /// <returns>user insered</returns>
        protected override ILinkedProperties Insert(ILinkedProperties user)
        {
            return ServiceUser.Insert((UserDTO)user);
        }

        /// <summary>
        /// Gets all users in db.
        /// </summary>
        /// <returns>all users</returns>
        protected override List<ILinkedProperties> GetAllUsersInDB()
        {
            return ServiceUser.GetAll(AllServicesDTO.ServiceAccessMode.All).Select(a => (ILinkedProperties)a).ToList();
        }
    }
}