// <copyright file="IGenericRepository.cs" company="BIA.NET">
// Copyright (c) BIA.NET. All rights reserved.
// </copyright>

namespace BIA.Net.Model.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Access mode
    /// </summary>
    public enum AccessMode
    {
        /// <summary>
        /// All
        /// </summary>
        All,

        /// <summary>
        /// The read
        /// </summary>
        Read,

        /// <summary>
        /// The write
        /// </summary>
        Write,

        /// <summary>
        /// The delete
        /// </summary>
        Delete
    }

    /// <summary>
    /// The TGenericRepository ofer the posibility to create a repository to manage advanced acces of the entity framework
    /// </summary>
    /// <typeparam name="Entity">Type of the entity</typeparam>
    /// <typeparam name="ProjectDBContext">The type of the project database context.</typeparam>
    /// <seealso cref="BIA.Net.Model.DAL.IGenericRepository{T, ProjectDBContext}" />
    public interface IGenericRepository<Entity, ProjectDBContext>
    {
        /// <summary>
        /// Gets or sets the filter context for all access.
        /// </summary>
        Expression<Func<Entity, bool>> FilterContext { get; set; }

        /// <summary>
        /// Gets or sets the filter context for read access.
        /// </summary>
        Expression<Func<Entity, bool>> FilterContextRead { get; set; }

        /// <summary>
        /// Gets or sets the filter context for write access.
        /// </summary>
        Expression<Func<Entity, bool>> FilterContextWrite { get; set; }

        /// <summary>
        /// Gets or sets the filter context for delete access.
        /// </summary>
        Expression<Func<Entity, bool>> FilterContextDelete { get; set; }

        /// <summary>
        /// Gets or sets the list of field to include.
        /// </summary>
        List<Expression<Func<Entity, dynamic>>> ListInclude { get; set; }

        /// <summary>
        /// Gets the standard query that return all item depending of the access mode pass in parameter (read by default).
        /// </summary>
        /// <param name="mode">The mode. (Read by default)</param>
        /// <returns>The standard query</returns>
        IQueryable<Entity> GetStandardQuery(AccessMode mode = AccessMode.Read);

        /// <summary>
        /// Execute a stored procedure that does not end with a SELECT
        /// </summary>
        /// <param name="storedProcedureParameter"><see cref="StoredProcedureParameter"/></param>
        /// <returns>The result returned by the database after executing the command.</returns>
        int ExecuteProcedureNonQuery(StoredProcedureParameter storedProcedureParameter);

        /// <summary>
        /// Execute a stored procedure of type SELECT
        /// </summary>
        /// <param name="storedProcedureParameter"><see cref="StoredProcedureParameter"/></param>
        /// <returns>List of Entity</returns>
        List<Entity> ExecuteProcedureReader(StoredProcedureParameter storedProcedureParameter);

        /// <summary>
        /// Execute a stored procedure of type SELECT
        /// </summary>
        /// <typeparam name="T">Entity or EntityDTO</typeparam>
        /// <param name="storedProcedureParameter"><see cref="StoredProcedureParameter"/></param>
        /// <returns>List of Entity or EntityDTO</returns>
        List<T> ExecuteProcedureReader<T>(StoredProcedureParameter storedProcedureParameter);

        /// <summary>
        /// Gets the primary keys properties.
        /// </summary>
        /// <returns>The primary keys properties</returns>
        EntityKeyHelper.KeyProperties[] GetPrimaryKeysProperties();

        /// <summary>
        /// Gets the primary keys of entity.
        /// </summary>
        /// <typeparam name="Entity2">The type of the entity.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The primary keys</returns>
        object[] GetPrimaryKeys<Entity2>(Entity2 value)
            where Entity2 : ObjectRemap;

        /// <summary>
        /// Updates the values of an entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <param name="valuesToUpdate">The values to update.</param>
        /// <returns>Returns the updated entity</returns>
        Entity UpdateValues(Entity entity, List<string> valuesToUpdate);

        /// <summary>
        /// Updates the values of an entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <param name="valuesToUpdate">The values to update.</param>
        /// <returns>Returns the updated entity</returns>
        Entity UpdateValues(Entity entity, object[] primaryKeys, List<string> valuesToUpdate);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="param">The parameter.</param>
        /// <returns>Returns the updated entity</returns>
        Entity Update(Entity entity, GenericRepositoryParmeter param = null);

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="param">The parameter.</param>
        /// <returns>Returns the insered entity</returns>
        /// <exception cref="System.Exception">Already created object.</exception>
        Entity Insert(Entity entity, GenericRepositoryParmeter param = null);

        /// <summary>
        /// Duplicates the specified entity by primary key.
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        /// <param name="param">The parameter.</param>
        /// <returns>Returns the duplicated entity</returns>
        Entity Duplicate(object primaryKey, GenericRepositoryParmeter param = null);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="param">The parameter.</param>
        /// <returns>Returns 0 if OK, -1 if KO</returns>
        int Delete(Entity entity, GenericRepositoryParmeter param = null);

        /// <summary>
        /// Deletes the entity by identifier.
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        /// <param name="param">The parameter.</param>
        /// <returns>Returns 0 if OK, -1 if KO</returns>
        int DeleteById(object primaryKey, GenericRepositoryParmeter param = null);

        /// <summary>
        /// Returns the object with the primary key specified
        /// </summary>
        /// <param name="keyValue_s">The primary key or keys</param>
        /// <param name="mode">Precise the usage (All/Read/Write)</param>
        /// <param name="expFieldsToInclude">The fields to include as expression.</param>
        /// <param name="sFieldsToInclude">The fields to include as string.</param>
        /// <returns>The result mapped to the specified type</returns>
        Entity Find(object keyValue_s, AccessMode mode = AccessMode.Read, List<Expression<Func<Entity, dynamic>>> expFieldsToInclude = null, List<string> sFieldsToInclude = null);

        /// <summary>
        /// Gets the find query.
        /// </summary>
        /// <param name="keyValue_s">The key value_s.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="expFieldsToInclude">The fields to include as expression.</param>
        /// <param name="sFieldsToInclude">The fields to include as string.</param>
        /// <returns>the find query</returns>
        IQueryable<Entity> GetFindQuery(object keyValue_s, AccessMode mode = AccessMode.Read, List<Expression<Func<Entity, dynamic>>> expFieldsToInclude = null, List<string> sFieldsToInclude = null);

        /// <summary>
        /// Returns the context without filter. WARNING : It should be use only for optimisation else use GetStandardQuery.
        /// </summary>
        /// <returns>The context without filter</returns>
        ProjectDBContext GetProjectDBContextForOptim();
    }
}
