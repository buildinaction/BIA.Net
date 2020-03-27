// <copyright file="IADHelper.cs" company="BIA.Net">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.ActiveDirectory
{
    using System;
    using System.Collections.Generic;
    using System.Security.Principal;

    /// <summary>
    /// The interface defining the AD helper.
    /// </summary>
    public interface IADHelper
    {
        /// <summary>
        /// Get the groups from a windows identity (from IIS).
        /// </summary>
        /// <param name="windowsIdentity">The windows identity of the current user.</param>
        /// <returns>The group list.</returns>
        List<string> GetGroups(WindowsIdentity windowsIdentity);

        /// <summary>
        /// Gets the groups of the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="domain">The AD domain.</param>
        /// <returns>the list of the group of the user.</returns>
        List<string> GetGroups(string userName, string domain);

        /// <summary>
        /// Check if a user is in a AD group.
        /// </summary>
        /// <param name="login">The user login.</param>
        /// <param name="group">The AD group.</param>
        /// <param name="domain">The AD domain.</param>
        /// <returns>A boolean indicating whether the user is in the group.</returns>
        bool IsUserInGroup(string login, string group, string domain);

        /// <summary>
        /// Search all user whose match the Query.
        /// </summary>
        /// <param name="search">String to find in the AD.</param>
        /// <param name="domain">The AD domain.</param>
        /// <returns>The list of users.</returns>
        List<UserAD> SearchUsers(string search, string domain);

        /// <summary>
        /// Add a user in a group of the AD.
        /// </summary>
        /// <param name="usersGuid">The user GUID list.</param>
        /// <param name="groupName">The group name.</param>
        /// <param name="domain">The domain.</param>
        void AddUsersInGroup(IEnumerable<Guid> usersGuid, string groupName, string domain);

        /// <summary>
        /// Remove a user in a group of the AD.
        /// </summary>
        /// <param name="usersGuid">The user GUID list.</param>
        /// <param name="groupName">The group name.</param>
        /// <param name="domain">The domain.</param>
        void RemoveUsersInGroup(IEnumerable<Guid> usersGuid, string groupName, string domain);

        /// <summary>
        /// Return all users recursively in a group.
        /// </summary>
        /// <param name="group">The AD group.</param>
        /// <param name="domain">The domain.</param>
        /// <returns>The list of users.</returns>
        IEnumerable<UserAD> GetAllUsersInGroup(string group, string domain);
    }
}