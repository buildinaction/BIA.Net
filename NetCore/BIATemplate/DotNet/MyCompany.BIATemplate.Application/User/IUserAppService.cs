// <copyright file="IUserAppService.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Application.User
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MyCompany.BIATemplate.Domain.Dto.Bia;
    using MyCompany.BIATemplate.Domain.Dto.User;

    /// <summary>
    /// The interface defining the application service for user.
    /// </summary>
    public interface IUserAppService
    {
        /// <summary>
        /// Get all existing users filtered.
        /// </summary>
        /// <param name="filter">Used to filter the users.</param>
        /// <returns>The list of users found.</returns>
        Task<IEnumerable<UserDto>> GetAllAsync(string filter);

        /// <summary>
        /// Get all existing users filtered.
        /// </summary>
        /// <param name="filters">Used to filter the users.</param>
        /// <returns>The list of users found and the total number of user.</returns>
        Task<(IEnumerable<UserDto> Users, int Total)> GetAllAsync(LazyLoadDto filters);

        /// <summary>
        /// Get all rights for a user with its login.
        /// </summary>
        /// <param name="login">The user login.</param>
        /// <returns>The list of right.</returns>
        Task<UserRightDto> GetRightsForUserAsync(string login);

        /// <summary>
        /// Gets user info with its login.
        /// </summary>
        /// <param name="login">The login to search with.</param>
        /// <returns>The user.</returns>
        Task<UserInfoDto> GetUserInfoAsync(string login);

        /// <summary>
        /// Gets the profile of the given user.
        /// </summary>
        /// <param name="login">The user login.</param>
        /// <returns>The user profile.</returns>
        Task<UserProfileDto> GetUserProfileAsync(string login);

        /// <summary>
        /// Gets all AD user corresponding to a filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>The top 10 users found.</returns>
        Task<IEnumerable<UserADDto>> GetAllADUserAsync(string filter);

        /// <summary>
        /// Add a list of users in a group in AD.
        /// </summary>
        /// <param name="users">The list of users to add.</param>
        Task AddInGroupAsync(IEnumerable<UserADDto> users);

        /// <summary>
        /// Remove a user in a group in AD.
        /// </summary>
        /// <param name="id">The identifier of the user to remove.</param>
        Task RemoveInGroupAsync(int id);

        /// <summary>
        /// Synchronize the users with the AD.
        /// </summary>
        Task SynchronizeWithADAsync();
    }
}