﻿// <copyright file="TGenericDBContainer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Model
{
    using System;
    using System.Data.Entity;
    using DAL;

    /// <summary>
    /// The TGenericRepository ofer the posibility to create a repository to manage advanced acces of the entity framework
    /// </summary>
    /// <typeparam name="ProjectDBContext">The type of the project database context.</typeparam>
    /// <typeparam name="ProjectDBContainer">The type of the project database container.</typeparam>
    /// <seealso cref="BIA.Net.Model.DAL.IGenericRepository{T, ProjectDBContext}" />
    public partial class TGenericDBContainer<ProjectDBContext, ProjectDBContainer>
        where ProjectDBContext : DbContext, new()
        where ProjectDBContainer : TDBContainer<ProjectDBContext>, new()
    {
        /// <summary>
        /// The database container
        /// </summary>
        private ProjectDBContainer dbContainer = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="TGenericDBContainer{ProjectDBContext, ProjectDBContainer}"/> class.
        /// </summary>
        /// <param name="contextGuid">The context unique identifier.</param>
        public TGenericDBContainer(Guid contextGuid)
        {
            this.ContextGuid = contextGuid;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TGenericDBContainer{ProjectDBContext, ProjectDBContainer}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public TGenericDBContainer(ProjectDBContainer context)
        {
            this.dbContainer = context;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is in transaction.
        /// </summary>
        public bool IsInTransaction
        {
            get { return this.DbContainer.isInTransaction; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is in transaction.
        /// </summary>
        public bool IsMoq
        {
            get { return this.DbContainer.IsMoq; }
        }

        /// <summary>
        /// Gets the database container.
        /// </summary>
        protected ProjectDBContainer DbContainer
        {
            get
            {
                if (this.dbContainer == null)
                {
                    this.dbContainer = TGenericContext<ProjectDBContext, ProjectDBContainer>.GetDbContainer(this.ContextGuid);
                }

                return this.dbContainer;
            }
        }

        /// <summary>
        /// Gets the project db context
        /// </summary>
        protected ProjectDBContext Db
        {
            get { return this.DbContainer.db; }
        }

        /// <summary>
        /// Gets or sets the context unique identifier.
        /// </summary>
        private Guid ContextGuid { get; set; }

        /// <summary>
        /// Returns the context without filter. WARNING : It should be use only for optimisation else use GetStandardQuery.
        /// </summary>
        /// <returns>The context without filter</returns>
        public ProjectDBContext GetProjectDBContextForOptim()
        {
            return this.DbContainer.db;
        }

        /// <summary>
        /// Returns the context without filter. WARNING : It should be use only for optimisation else use GetStandardQuery.
        /// </summary>
        /// <returns>The context without filter</returns>
        public ProjectDBContext GetProjectDBContext()
        {
            return this.DbContainer.db;
        }
    }
}
