// <copyright file="IUserRightDomainService.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIADemo.Domain.UserModule.Service
{
    using System.Threading.Tasks;
    using MyCompany.BIADemo.Domain.Dto.User;

    /// <summary>
    /// The interface defining the user right domain service.
    /// </summary>
    public interface IUserRightDomainService
    {
        /// <summary>
        /// Get all rights for a user login.
        /// </summary>
        /// <param name="login">The user login.</param>
        /// <returns>The DTO containing the right list.</returns>
        Task<UserRightDto> GetRightsForUserAsync(string login);
    }
}