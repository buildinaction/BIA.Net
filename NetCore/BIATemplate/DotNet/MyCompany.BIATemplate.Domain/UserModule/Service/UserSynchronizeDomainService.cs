// <copyright file="UserSynchronizeDomainService.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Domain.UserModule.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.ActiveDirectory;
    using Microsoft.Extensions.Options;
    using MyCompany.BIATemplate.Crosscutting.Common.Configuration.BiaNet;
    using MyCompany.BIATemplate.Domain.Core;
    using MyCompany.BIATemplate.Domain.UserModule.Aggregate;

    /// <summary>
    /// The service used for synchronization between AD and DB.
    /// </summary>
    public class UserSynchronizeDomainService : IUserSynchronizeDomainService
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
        /// Initializes a new instance of the <see cref="UserSynchronizeDomainService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="configuration">The configuration of the BiaNet section.</param>
        /// <param name="adHelper">The AD helper.</param>
        public UserSynchronizeDomainService(IGenericRepository repository, IOptions<BiaNetSection> configuration, IADHelper adHelper)
        {
            this.repository = repository;
            this.configuration = configuration.Value;
            this.adHelper = adHelper;
        }

        /// <inheritdoc cref="IUserSynchronizeDomainService.SynchronizeFromADGroupAsync"/>
        public async Task SynchronizeFromADGroupAsync()
        {
            List<User> users = (await this.repository.GetAllAsync<User>()).ToList();
            var group = this.configuration.Roles.Where(w => w.Type == "AD" && w.Label == "User").Select(s => s.Value)
                .FirstOrDefault();
            List<UserAD> adUsers = this.adHelper.GetAllUsersInGroup(group, this.configuration.Authentication.ADDomain).ToList();
            var usersToAdd = new List<User>();

            foreach (var adUser in adUsers)
            {
                var foundUser = users.FirstOrDefault(a => a.Login == adUser.Login);

                if (foundUser == null)
                {
                    // Create the missing user
                    var user = new User
                    {
                        Guid = adUser.Guid,
                        Login = adUser.Login,
                        FirstName = adUser.FirstName?.Length > 50 ? adUser.FirstName?.Substring(0, 50) : adUser.FirstName ?? string.Empty,
                        LastName = adUser.LastName?.Length > 50 ? adUser.LastName?.Substring(0, 50) : adUser.LastName ?? string.Empty,
                        IsActive = true,
                        Country = adUser.Country?.Length > 10 ? adUser.Country?.Substring(0, 10) : adUser.Country ?? string.Empty,
                        Department = adUser.Department?.Length > 50 ? adUser.Department?.Substring(0, 50) : adUser.Department ?? string.Empty,
                        DistinguishedName = adUser.DistinguishedName?.Length > 250 ? adUser.DistinguishedName?.Substring(0, 250) : adUser.DistinguishedName,
                        Manager = adUser.Manager?.Length > 250 ? adUser.Manager?.Substring(0, 250) : adUser.Manager,
                        Email = adUser.Email?.Length > 256 ? adUser.Email?.Substring(0, 256) : adUser.Email ?? string.Empty,
                        ExternalCompany = adUser.ExternalCompany?.Length > 50 ? adUser.ExternalCompany?.Substring(0, 50) : adUser.ExternalCompany,
                        IsEmployee = adUser.IsEmployee,
                        IsExternal = adUser.IsExternal,
                        Company = adUser.Company?.Length > 50 ? adUser.Company?.Substring(0, 50) : adUser.Company,
                        DaiDate = DateTime.Now,
                        Office = adUser.Office?.Length > 20 ? adUser.Office?.Substring(0, 20) : adUser.Office ?? string.Empty,
                        Site = adUser.Site?.Length > 50 ? adUser.Site?.Substring(0, 50) : adUser.Site,
                        SubDepartment = adUser.SubDepartment?.Length > 50 ? adUser.SubDepartment?.Substring(0, 50) : adUser.SubDepartment,
                    };
                    usersToAdd.Add(user);
                }
                else if (!foundUser.IsActive)
                {
                    foundUser.IsActive = true;
                    this.repository.Update(foundUser);
                }
            }

            this.repository.AddRange(usersToAdd);

            // Check users to deactivate
            var adUserLogins = adUsers.Select(s => s.Login).ToList();
            foreach (var user in users)
            {
                if (!adUserLogins.Contains(user.Login) && user.IsActive)
                {
                    user.IsActive = false;
                    this.repository.Update(user);
                }
            }

            await this.repository.UnitOfWork.CommitAsync();
        }
    }
}