// <copyright file="AppServiceBase.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIADemo.Application.Bia
{
    using MyCompany.BIADemo.Domain.Core;

    /// <summary>
    /// The base class for all application service.
    /// </summary>
    public abstract class AppServiceBase
    {
        /// <summary>
        /// The generic repository.
        /// </summary>
        private readonly IGenericRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppServiceBase"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        protected AppServiceBase(IGenericRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Gets the repository.
        /// </summary>
        protected IGenericRepository Repository => this.repository;
    }
}