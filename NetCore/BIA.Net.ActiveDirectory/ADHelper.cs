// <copyright file="ADHelper.cs" company="BIA.Net">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.ActiveDirectory
{
    using System;
    using System.Collections.Generic;
    using System.DirectoryServices;
    using System.DirectoryServices.AccountManagement;
    using System.Linq;
    using System.Security.Principal;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Helper to get information from AD.
    /// </summary>
    public class ADHelper : IADHelper
    {
        /// <summary>
        /// Groups cached.
        /// </summary>
        private static readonly Dictionary<string, string> CacheGroupName = new Dictionary<string, string>();

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<ADHelper> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ADHelper"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ADHelper(ILogger<ADHelper> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc cref="IADHelper.GetGroups(WindowsIdentity)"/>
        public List<string> GetGroups(WindowsIdentity windowsIdentity)
        {
            List<string> result = new List<string>();
            if (windowsIdentity?.Groups != null)
            {
                this.logger.LogDebug("Nb group in AD : " + windowsIdentity.Groups.Count);

                foreach (IdentityReference group in windowsIdentity.Groups)
                {
                    try
                    {
                        string groupValue = group.Value;
                        if (!CacheGroupName.TryGetValue(groupValue, out var groupName))
                        {
                            this.logger.LogDebug("Try resolve name : " + groupValue);
                            groupName = group.Translate(typeof(NTAccount)).ToString();
                            CacheGroupName.Add(groupValue, groupName);
                            this.logger.LogDebug("Name resolve : " + groupName);
                        }

                        result.Add(groupName);
                    }
                    catch (Exception e)
                    {
                        this.logger.LogDebug("Error");
                        this.logger.LogWarning("Error when treat " + group.Value, e);
                    }
                }

                result.Sort();
                this.logger.LogDebug("End");
            }

            return result;
        }

        /// <inheritdoc cref="IADHelper.GetGroups(string, string)"/>
        public List<string> GetGroups(string userName, string domain)
        {
            this.logger.LogDebug("Begin for : " + userName);

            if (string.IsNullOrEmpty(domain))
            {
                return new List<string>();
            }

            try
            {
                WindowsIdentity windowsIdentity = new WindowsIdentity(userName + "@" + domain);
                this.logger.LogInformation("User : " + userName + "find in Domain : " + domain);
                return this.GetGroups(windowsIdentity);
            }
            catch (Exception)
            {
                this.logger.LogInformation("Could not find user : " + userName + " in Domain : " + domain);
            }

            return new List<string>();
        }

        /// <inheritdoc cref="IADHelper.IsUserInGroup"/>
        public bool IsUserInGroup(string login, string group, string domain)
        {
            if (string.IsNullOrEmpty(domain))
            {
                return false;
            }

            List<UserPrincipal> listUsers = new List<UserPrincipal>();
            List<GroupPrincipal> listTreatedGroups = new List<GroupPrincipal>();
            GroupPrincipal groupPrincipal = null;

            try
            {
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, domain);
                groupPrincipal = GroupPrincipal.FindByIdentity(ctx, group);
                if (groupPrincipal == null)
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                this.logger.LogWarning("Could not join Domain :" + domain, e);
            }

            this.logger.LogDebug("Group " + group + " not found in domain " + domain);

            this.GetAllUsersFromGroupRecursively(groupPrincipal, listUsers, listTreatedGroups);

            return listUsers.Any(u => u.SamAccountName == login);
        }

        /// <inheritdoc cref="IADHelper.SearchUsers"/>
        public List<UserAD> SearchUsers(string search, string domain)
        {
            List<UserAD> usersInfo = new List<UserAD>();
            if (string.IsNullOrEmpty(search) || string.IsNullOrEmpty(domain))
            {
                return usersInfo;
            }

            List<DirectoryEntry> usersMatches = new List<DirectoryEntry>();

            try
            {
                using (var entry = new DirectoryEntry($"LDAP://{domain}"))
                {
                    using (var searcher = new DirectorySearcher(entry))
                    {
                        searcher.Filter = $"(&(objectCategory=person)(objectClass=user)(|(givenname=*{search}*)(sn=*{search}*)(SAMAccountName=*{search}*)))";
                        searcher.SizeLimit = 10;
                        usersMatches.AddRange(searcher.FindAll().Cast<SearchResult>().Select(s => s.GetDirectoryEntry()).ToList());
                    }
                }
            }
            catch (Exception e)
            {
                this.logger.LogWarning("Could not join Domain :" + domain, e);
            }

            usersMatches.Take(10).ToList().ForEach(
                um => { usersInfo.Add(ConvertToUserAD(um)); });

            return usersInfo;
        }

        /// <inheritdoc cref="IADHelper.AddUsersInGroup"/>
        public void AddUsersInGroup(IEnumerable<Guid> usersGuid, string groupName, string domain)
        {
            try
            {
                using (PrincipalContext context = new PrincipalContext(ContextType.Domain, domain))
                {
                    GroupPrincipal group = GroupPrincipal.FindByIdentity(context, groupName);
                    if (group == null)
                    {
                        return;
                    }

                    foreach (var guid in usersGuid)
                    {
                        group.Members.Add(context, IdentityType.Guid, guid.ToString());
                    }

                    group.Save();
                }
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "An error occured while adding the users with GUID : " + string.Join(',', usersGuid));
            }
        }

        /// <inheritdoc cref="IADHelper.RemoveUsersInGroup"/>
        public void RemoveUsersInGroup(IEnumerable<Guid> usersGuid, string groupName, string domain)
        {
            try
            {
                using (PrincipalContext context = new PrincipalContext(ContextType.Domain, domain))
                {
                    GroupPrincipal group = GroupPrincipal.FindByIdentity(context, groupName);
                    if (group == null)
                    {
                        return;
                    }

                    foreach (var guid in usersGuid)
                    {
                        group.Members.Remove(context, IdentityType.Guid, guid.ToString());
                    }

                    group.Save();
                }
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "An error occured while adding the user with GUID : " + string.Join(',', usersGuid));
            }
        }

        /// <inheritdoc cref="IADHelper.GetAllUsersInGroup"/>
        public IEnumerable<UserAD> GetAllUsersInGroup(string group, string domain)
        {
            if (string.IsNullOrEmpty(group) || string.IsNullOrEmpty(domain))
            {
                return new List<UserAD>();
            }

            List<UserPrincipal> listUsers = new List<UserPrincipal>();
            List<GroupPrincipal> listTreatedGroups = new List<GroupPrincipal>();
            GroupPrincipal groupPrincipal = null;

            try
            {
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, domain);
                groupPrincipal = GroupPrincipal.FindByIdentity(ctx, group);
                if (groupPrincipal == null)
                {
                    return new List<UserAD>();
                }
            }
            catch (Exception e)
            {
                this.logger.LogWarning("Could not join Domain :" + domain, e);
            }

            this.logger.LogDebug("Group " + group + " not found in domain " + domain);

            this.GetAllUsersFromGroupRecursively(groupPrincipal, listUsers, listTreatedGroups);

            return listUsers.Select(GetUser).Where(w => w != null).ToList();
        }

        /// <summary>
        /// Get all user from a group.
        /// </summary>
        /// <param name="group">The group to search in.</param>
        /// <param name="listUsers">The users found.</param>
        /// <param name="listTreatedGroups">The group already treated.</param>
        private void GetAllUsersFromGroupRecursively(GroupPrincipal group, List<UserPrincipal> listUsers, List<GroupPrincipal> listTreatedGroups)
        {
            listTreatedGroups.Add(group);
            this.logger.LogDebug("Treat group " + group.Name);
            try
            {
                // iterate over members
                foreach (Principal principal in group.GetMembers())
                {
                    Console.WriteLine("{0}: {1}", principal.StructuralObjectClass, principal.DisplayName);
                    this.logger.LogDebug("Treat member " + principal.Name);

                    // do whatever you need to do to those members
                    if (principal is UserPrincipal userPrincipal)
                    {
                        if (!listUsers.Select(u => u.DistinguishedName).Contains(userPrincipal.DistinguishedName))
                        {
                            listUsers.Add(userPrincipal);
                        }
                    }
                    else if (principal is GroupPrincipal groupPrincipal && !listTreatedGroups.Select(g => g.DistinguishedName).Contains(groupPrincipal.DistinguishedName))
                    {
                        this.GetAllUsersFromGroupRecursively(groupPrincipal, listUsers, listTreatedGroups);
                    }
                }

                this.logger.LogDebug("Group " + group.Name + " treated.");
            }
            catch (Exception e)
            {
                this.logger.LogWarning("GetAllUsersFromGroupRecursively crash", e);
            }
        }

        /// <summary>
        /// Get a user AD from a user principal.
        /// </summary>
        /// <param name="userAD">The user principal.</param>
        /// <returns>The user AD.</returns>
        private static UserAD GetUser(UserPrincipal userAD)
        {
            var entry = userAD.GetUnderlyingObject() as DirectoryEntry;
            return entry == null ? null : ConvertToUserAD(entry);
        }

        private static UserAD ConvertToUserAD(DirectoryEntry entry)
        {
            var user = new UserAD
            {
                FirstName = entry.Properties["GivenName"].Value?.ToString(),
                LastName = entry.Properties["sn"].Value?.ToString(),
                Login = entry.Properties["SAMAccountName"].Value?.ToString(),
                Guid = (byte[])entry.Properties["objectGuid"].Value != null ? new Guid((byte[])entry.Properties["objectGuid"].Value) : Guid.NewGuid(),
                Country = entry.Properties["c"].Value?.ToString(),
                Company = entry.Properties["company"].Value?.ToString(),
                Department = entry.Properties["department"].Value?.ToString(),
                DistinguishedName = entry.Properties["distinguishedName"].Value?.ToString(),
                Email = entry.Properties["mail"].Value?.ToString(),
                IsEmployee = true,
                Manager = entry.Properties["manager"].Value?.ToString(),
                Office = entry.Properties["physicalDeliveryOfficeName"].Value?.ToString(),
                Site = entry.Properties["description"].Value?.ToString(),
            };

            // Set external company
            var jobTitle = entry.Properties["title"].Value?.ToString();

            if (!string.IsNullOrEmpty(jobTitle) && jobTitle.IndexOf(':') <= 0)
            {
                string[] extInfo = jobTitle.Split(':');
                if (extInfo[0] == "EXT" && extInfo.Length != 2)
                {
                    user.IsEmployee = false;
                    user.IsExternal = true;
                    user.ExternalCompany = extInfo[1];
                }
            }

            // Set sub department
            string fullDepartment = user.Department;
            if (!string.IsNullOrWhiteSpace(fullDepartment) && fullDepartment.IndexOf('-') > 0)
            {
                user.Department = fullDepartment.Substring(0, fullDepartment.IndexOf('-') - 1);
                if (fullDepartment.Length > fullDepartment.IndexOf('-') + 2)
                {
                    user.SubDepartment = fullDepartment.Substring(fullDepartment.IndexOf('-') + 3);
                }
            }

            return user;
        }
    }
}