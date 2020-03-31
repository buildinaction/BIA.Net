// <copyright file="UserRightDomainService.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIADemo.Domain.UserModule.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using BIA.Net.ActiveDirectory;
    using Microsoft.Extensions.Options;
    using MyCompany.BIADemo.Crosscutting.Common.Configuration.BiaNet;
    using MyCompany.BIADemo.Crosscutting.Common.Enum;
    using MyCompany.BIADemo.Domain.Core;
    using MyCompany.BIADemo.Domain.Dto.User;
    using MyCompany.BIADemo.Domain.UserModule.Aggregate;

    /// <summary>
    /// The domain service used for user right.
    /// </summary>
    public class UserRightDomainService : IUserRightDomainService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IGenericRepository repository;

        /// <summary>
        /// The configuration of the BiaNet section.
        /// </summary>
        private readonly BiaNetSection configuration;

        /// <summary>
        /// The AD helper.
        /// </summary>
        private readonly IADHelper adHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRightDomainService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="configuration">The configuration of the BiaNet section.</param>
        /// <param name="adHelper">The AD helper.</param>
        public UserRightDomainService(IGenericRepository repository, IOptions<BiaNetSection> configuration, IADHelper adHelper)
        {
            this.repository = repository;
            this.configuration = configuration.Value;
            this.adHelper = adHelper;
        }

        /// <inheritdoc cref="IUserRightDomainService.GetRightsForUserAsync"/>
        public async Task<UserRightDto> GetRightsForUserAsync(string login)
        {
            var rolesSection = this.configuration.Roles;
            Enum.TryParse(this.configuration.Authentication.ADRolesMode, out ADRolesMode adRolesMode);
            var domain = this.configuration.Authentication.ADDomain;

            var adRoles = new List<string>();
            foreach (var role in rolesSection)
            {
                switch (role.Type)
                {
                    case "Fake":
                        adRoles.Add(role.Label);
                        break;

                    case "AD":
                        var group = role.Value;
                        if (this.IsInGroup(group, adRolesMode, login, domain))
                        {
                            adRoles.Add(role.Label);
                            break;
                        }

                        break;
                }
            }

            var customRoles = (await this.repository
                .GetBySpecAsync(
                    s => new UserRoleSelectResult
                    {
                        Roles = s.MemberRoles.Select(mr => mr.Role.Code),
                        UserId = s.UserId,
                    },
                    MemberSpecification.SearchForLogin(login))).Distinct().FirstOrDefault();

            var roles = adRoles;
            if (customRoles != null)
            {
                roles.AddRange(customRoles.Roles);
            }

            var rights = this.configuration.Permissions.ToList();
            var userRights = rights.Where(w => w.Roles.Any(a => roles.Contains(a))).Select(s => s.Name).Distinct().ToList();

            var userRoles = new UserRightDto { Rights = userRights, Login = login, UserId = customRoles?.UserId ?? 0 };

            return userRoles;
        }

        /// <summary>
        /// Check if a group is in a given list.
        /// </summary>
        /// <param name="groupToFind">The group to find.</param>
        /// <param name="groupList">The group list to search in.</param>
        /// <returns>A boolean indicating whether the group is in the list.</returns>
        private static bool IsGroupInList(string groupToFind, List<string> groupList)
        {
            if (groupToFind.IndexOfAny(new char[] { '*', '.', '(', ')', '+', '[', ']' }) != -1)
            {
                Regex regex = new Regex("^" + groupToFind + "$");
                return groupList.Any(group => regex.IsMatch(group));
            }
            else
            {
                return groupList.Contains(groupToFind);
            }
        }

        /// <summary>
        /// Check if a user is in a group.
        /// </summary>
        /// <param name="group">The group to search in.</param>
        /// <param name="adRolesMode">The configuration for searching in AD or IIS.</param>
        /// <param name="login">The user login.</param>
        /// <param name="domain">The AD domain.</param>
        /// <returns>A boolean indicating whether the user is in the group.</returns>
        private bool IsInGroup(string group, ADRolesMode adRolesMode, string login, string domain)
        {
            switch (adRolesMode)
            {
                case ADRolesMode.IisGroup:
                    return IsGroupInList(group, this.adHelper.GetGroups(WindowsIdentity.GetCurrent()));

                case ADRolesMode.ADUserFirst:
                    return IsGroupInList(group, this.adHelper.GetGroups(login, domain));

                case ADRolesMode.ADGroupFirst:
                    return this.adHelper.IsUserInGroup(login, group, domain);

                default:
                    return false;
            }
        }
    }
}