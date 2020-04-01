// <copyright file="IUserSynchronizeDomainService.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Domain.UserModule.Service
{
    using System.Threading.Tasks;

    /// <summary>
    /// The interface defining the user synchronize domain service.
    /// </summary>
    public interface IUserSynchronizeDomainService
    {
        /// <summary>
        /// Synchronize the users in DB from the AD User group.
        /// </summary>
        /// <returns>The result of the task.</returns>
        Task SynchronizeFromADGroupAsync();
    }
}