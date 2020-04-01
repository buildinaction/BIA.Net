// <copyright file="IUnitOfWork.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Domain.Core
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// The interface for IUnitOfWork base on the pattern 'Unit of Work'.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Commit changes on the current data context.
        /// </summary>
        /// <returns>The number of element affected.</returns>
        Task<int> CommitAsync();

        /// <summary>
        /// Rollback changes in the current context.
        /// </summary>
        void RollbackChanges();
    }
}