// <copyright file="UserInfo.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace $safeprojectname$.Helpers
{
    using BIA.Net.Authentication.Business.Helpers;
    using BIA.Net.Business.Services;
    using BIA.Net.Common.Helpers;
    using Business.DTO;
    using Business.Services;
    using Common;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Class to define identity.
    /// </summary>
    public class UserInfo : AUserInfo<UserDTO>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfo"/> class.
        /// </summary>
        public UserInfo()
        {
        }
        #endregion Constructors

        #region Properties

        /*
        public override UserDTO LinkedProperties
        {
            get
            {
                return Properties;
            }

            set
            {
                Properties = value;
            }
        }

        protected override void RefreshLinkedProperties()
        {
            base.RefreshProperties();
        }
        */
        #endregion Properties

        #region Methods

        /// <summary>
        /// Set the Role and add custum role.
        /// </summary>
        /// <param name="basicRoles">basics roles</param>
        public override void CustomCodeRoles(List<string> basicRoles)
        {
            ServiceUser serviceUser = BIAUnity.Resolve<ServiceUser>();
            UserDTO userProperties = (UserDTO)serviceUser.FindByLogin(Login, AllServicesDTO.ServiceAccessMode.All);

            // Compute here custom roles (Warning: not computed in WebApi without seesion because Members is null)
            if (userProperties?.Members != null && userProperties.Members.Any(m => m.MemberRole.Any(r => r.Id == Constants.RoleSiteAdminId)))
            {
                basicRoles.Add(Constants.RoleSiteAdmin);
            }

            if (userProperties?.Members != null && userProperties.Members.Count() > 0)
            {
                basicRoles.Add(Constants.RoleSiteMember);
            }
        }

        /// <inheritdoc/>
        protected override void RefreshPropertiesInBuilding()
        {
            if (Roles.Contains("User"))
            {
                ServiceUser serviceUser = BIAUnity.Resolve<ServiceUser>();
                UserDTO userProperties = (UserDTO)serviceUser.FindByLogin(this.Login, AllServicesDTO.ServiceAccessMode.All);

                if (userProperties != null)
                {
                    if (!userProperties.IsValid)
                    {
                        userProperties.IsValid = true;
                        serviceUser.SetUserValidity(userProperties);
                    }

                    userInfoContainer.propertiesKey = Login;
                    propertiesInBuilding = userProperties;
                    return;
                }
            }

            base.RefreshPropertiesInBuilding();
        }
        #endregion Methods
    }
}