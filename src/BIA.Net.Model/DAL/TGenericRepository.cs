// <copyright file="TGenericRepository.cs" company="BIA.NET">
// Copyright (c) BIA.NET. All rights reserved.
// </copyright>

namespace BIA.Net.Model
{
    using BIA.Net.Common.Helpers;
    using Common;
    using DAL;
    using Microsoft.Samples.EntityDataReader;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Core.Objects.DataClasses;
    using System.Data.Entity.Infrastructure;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Linq.Expressions;
    using System.Reflection;
    using Utility;

    /// <summary>
    /// Internal flag to perform correct action durring cascade
    /// </summary>
    internal enum TypeOfCascade
    {
        /// <summary>
        /// The creation
        /// </summary>
        Creation = 1,

        /// <summary>
        /// The update
        /// </summary>
        Update = 2,

        /// <summary>
        /// The deletion
        /// </summary>
        Deletion = 3
    }

#pragma warning disable SA1202 // Elements must be ordered by access
#pragma warning disable SA1204 // Elements must be ordered by access
#pragma warning disable CS0162 // Unreachable code detected

    /// <summary>
    /// The TGenericRepository ofer the posibility to create a repository to manage advanced acces of the entity framework
    /// </summary>
    /// <typeparam name="Entity">Type of the entity</typeparam>
    /// <typeparam name="ProjectDBContext">The type of the project database context.</typeparam>
    /// <typeparam name="ProjectDBContainer">The type of the project database container.</typeparam>
    /// <seealso cref="BIA.Net.Model.DAL.IGenericRepository{T, ProjectDBContext}" />
    public partial class TGenericRepository<Entity, ProjectDBContext> : IGenericRepository<Entity, ProjectDBContext>
        where Entity : ObjectRemap, new()
        where ProjectDBContext : DbContext, new()
    {
#if DEBUG
        /// <summary>
        /// Enable the trace for all action
        /// </summary>
        private const bool PerfomanceTrace = true;
#else
        /// <summary>
        /// Disable the trace for all action
        /// </summary>
        private const bool PerfomanceTrace = false;
#endif

        /// <summary>
        /// Tab use in trace
        /// </summary>
        private string remapTab = string.Empty;

        /// <summary>
        /// The database set
        /// </summary>
        private DbSet<Entity> dbSet = null;

        /// <summary>
        /// instance of dbContainer
        /// </summary>
        private TDBContainer<ProjectDBContext> dbContainer = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="TGenericRepository{Entity, ProjectDBContext, ProjectDBContainer}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public TGenericRepository()
        {
        }

        /// <summary>
        /// Gets or sets The minimum keys value to compute on insert
        /// </summary>
        public Dictionary<string, object> MinInsertKeysValue { get; set; }

        /// <summary>
        /// Gets or sets the filter context for all access.
        /// </summary>
        public Expression<Func<Entity, bool>> FilterContext { get; set; }

        /// <summary>
        /// Gets or sets the filter context for read access.
        /// </summary>
        public Expression<Func<Entity, bool>> FilterContextRead { get; set; }

        /// <summary>
        /// Gets or sets the filter context for write access.
        /// </summary>
        public Expression<Func<Entity, bool>> FilterContextWrite { get; set; }

        /// <summary>
        /// Gets or sets the filter context for delete access.
        /// </summary>
        public Expression<Func<Entity, bool>> FilterContextDelete { get; set; }

        /// <summary>
        /// Gets or sets the list of field to include.
        /// </summary>
        public List<Expression<Func<Entity, dynamic>>> ListInclude { get; set; }

        /// <summary>
        /// Gets instance of dbContainer
        /// </summary>
        protected TDBContainer<ProjectDBContext> DbContainer
        {
            get
            {
                if (this.dbContainer == null)
                {
                    this.dbContainer = BIAUnity.Resolve<TDBContainer<ProjectDBContext>>();
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
        /// Gets a value indicating whether this instance is in transaction.
        /// </summary>
        protected bool IsInTransaction
        {
            get { return this.DbContainer.IsInTransaction; }
        }

        /// <summary>
        /// Gets the database set.
        /// </summary>
        private DbSet<Entity> DbSet
        {
            get
            {
                if (this.dbSet == null)
                {
                    this.dbSet = this.Db.Set<Entity>();
                }

                return this.dbSet;
            }
        }

        /// <summary>
        /// Returns the context without filter. WARNING : It should be use only for optimisation else use GetStandardQuery.
        /// </summary>
        /// <returns>The context without filter</returns>
        public ProjectDBContext GetProjectDBContextForOptim()
        {
            return this.DbContainer.db;
        }

        /// <summary>
        /// Returns the object with the primary key specified
        /// </summary>
        /// <param name="keyValue_s">The primary key or keys</param>
        /// <param name="mode">Precise the usage (All/Read/Write)</param>
        /// <param name="expFieldsToInclude">The fields to include as expression.</param>
        /// <param name="sFieldsToInclude">The fields to include as string.</param>
        /// <returns>The result mapped to the specified type</returns>
        public virtual Entity Find(object keyValue_s, AccessMode mode = AccessMode.Read, List<Expression<Func<Entity, dynamic>>> expFieldsToInclude = null, List<string> sFieldsToInclude = null)
        {
            IQueryable<Entity> query = this.GetFindQuery(keyValue_s, mode, expFieldsToInclude, sFieldsToInclude);
            Entity entity = query.SingleOrDefault();
            return entity;
        }

        /// <summary>
        /// Gets the find query.
        /// </summary>
        /// <param name="keyValue_s">The key value_s.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="expFieldsToInclude">The fields to include as expression.</param>
        /// <param name="sFieldsToInclude">The fields to include as string.</param>
        /// <returns>the find query</returns>
        public virtual IQueryable<Entity> GetFindQuery(object keyValue_s, AccessMode mode = AccessMode.Read, List<Expression<Func<Entity, dynamic>>> expFieldsToInclude = null, List<string> sFieldsToInclude = null)
        {
            IQueryable<Entity> query = this.GetStandardQuery(mode);
            if (expFieldsToInclude != null)
            {
                foreach (Expression<Func<Entity, dynamic>> field in expFieldsToInclude)
                {
                    query = query.Include(field);
                }
            }

            if (sFieldsToInclude != null)
            {
                foreach (string field in sFieldsToInclude)
                {
                    query = query.Include(field);
                }
            }

            string searchIdentity = string.Empty;
            EntityKeyHelper.KeyProperties[] keyProperties = this.GetPrimaryKeysProperties();
            int countkey = 0;

            object[] keyValues = null;
            if (typeof(object[]).IsAssignableFrom(keyValue_s.GetType()))
            {
                keyValues = keyValue_s as object[];
            }
            else
            {
                keyValues = new object[] { keyValue_s };
            }

            foreach (EntityKeyHelper.KeyProperties prop in keyProperties)
            {
                Debug.Assert(countkey < keyValues.Length, "BIA.Net.Model : GetFindQuery pb with keySize");
                if (!string.IsNullOrEmpty(searchIdentity))
                {
                    searchIdentity = searchIdentity + " && ";
                }

                // Test done only with String and Int32 TODO : other test
                if (prop.typeName == "Byte" || prop.typeName == "Decimal" || prop.typeName == "Double" || prop.typeName == "Int16" || prop.typeName == "Int32" || prop.typeName == "Int64")
                {
                    searchIdentity = searchIdentity + prop.name + "==" + keyValues[countkey];
                }
                else if (prop.typeName == "Guid")
                {
                    searchIdentity = searchIdentity + prop.name + ".ToString() ==\"" + keyValues[countkey] + "\"";
                }
                else
                {
                    searchIdentity = searchIdentity + prop.name + "==\"" + keyValues[countkey] + "\"";
                }

                countkey++;
            }

            query = query.Where(searchIdentity);
            return query;
        }

        /// <summary>
        /// Returns the context filtered for specify acces mode
        /// </summary>
        /// <param name="mode">Precise the usage (All/Read/Write)</param>
        /// <returns>The result mapped to the specified type</returns>
        protected virtual IQueryable<Entity> ContextDbSet(AccessMode mode = AccessMode.Read)
        {
            IQueryable<Entity> dbcontext = this.DbSet;
            if (this.ListInclude != null)
            {
                for (int i = 0; i < this.ListInclude.Count; i++)
                {
                    dbcontext = dbcontext.Include(this.ListInclude[i]);
                }
            }

            Expression<Func<Entity, bool>> filter = this.GetFilter(mode);
            if (filter == null)
            {
                return dbcontext;
            }
            else
            {
                return dbcontext.Where(filter);
            }
        }

        /// <summary>
        /// Returns the filter to apply to the context for specify acces mode
        /// </summary>
        /// <param name="mode">Precise the usage (All/Read/Write)</param>
        /// <returns>The result mapped to the specified type</returns>
        private Expression<Func<Entity, bool>> GetFilter(AccessMode mode)
        {
            Expression<Func<Entity, bool>> filter = this.FilterContext;

            if (((mode == AccessMode.Read) || (mode == AccessMode.Write) || (mode == AccessMode.Delete))
                    && (this.FilterContextRead != null))
            {
                filter = this.FilterContextRead;
            }

            if (((mode == AccessMode.Write) || (mode == AccessMode.Delete))
                    && (this.FilterContextWrite != null))
            {
                filter = this.FilterContextWrite;
            }

            if ((mode == AccessMode.Delete)
                    && (this.FilterContextDelete != null))
            {
                filter = this.FilterContextDelete;
            }

            return filter;
        }

        /// <summary>
        /// Updates the values of an entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <param name="valuesToUpdate">The values to update.</param>
        /// <returns>Returns the updated entity</returns>
        public Entity UpdateValues(Entity entity, List<string> valuesToUpdate)
        {
            return this.Update(entity, new GenericRepositoryParmeter() { Values2Update = valuesToUpdate });
        }

        /// <summary>
        /// Updates the values of an entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <param name="valuesToUpdate">The values to update.</param>
        /// <returns>Returns the updated entity</returns>
        public Entity UpdateValues(Entity entity, object[] primaryKeys, List<string> valuesToUpdate)
        {
            return this.Update(entity, primaryKeys, new GenericRepositoryParmeter() { Values2Update = valuesToUpdate });
        }

        /// <summary>
        /// Saves the change.
        /// </summary>
        public void SaveChange()
        {
            if (!this.IsInTransaction)
            {
                this.Db.SaveChanges();
            }
        }

        /// <summary>
        /// Update without parameter for wizard
        /// </summary>
        /// <param name="entity">The value.</param>
        /// <returns>Returns the updated entity</returns>
        public virtual Entity W_Update(Entity entity)
        {
            return this.Update(entity, null);
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="param">The parameter.</param>
        /// <returns>Returns the updated entity</returns>
        public virtual Entity Update(Entity entity, GenericRepositoryParmeter param = null)
        {
            Debug.Assert(!AreKeysNull(this.GetPrimaryKeys(entity)), "The Id is missing.");
            return this.Update(entity, this.GetPrimaryKeys(entity), param);
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <param name="param">The parameter.</param>
        /// <returns>Returns the updated entity</returns>
        private Entity Update(Entity entity, object[] primaryKey, GenericRepositoryParmeter param = null)
        {
            List<string> lstFieldToInclude = this.GetFieldToInclude(param, AccessMode.Write);

            Entity dbobj = this.Find(primaryKey, AccessMode.Write, sFieldsToInclude: lstFieldToInclude);
            if (dbobj == null)
            {
                TraceManager.Debug("GenericRepository", "Update", "Element not found. Perhaps do not have right.");
                return null;
            }

            Debug.Assert(dbobj != entity, "Please create a new object to do an update, Don't use the entity frame work object");

            TraceManager.Debug("GenericRepository", "Update", "Update begin for element " + typeof(Entity).Name);
            try
            {
                this.RemapReferences(dbobj, entity, param);
                this.SaveChange();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                DBUtil.ReformatDBConstraintError(dbEx);
            }
            catch (DbUpdateException dbEx)
            {
                DBUtil.ReformatDBUpdateError(dbEx);
            }

            TraceManager.Debug("GenericRepository", "Update", "Update Finish for element " + typeof(Entity).Name);
            return dbobj;
        }

        /// <summary>
        /// Insert without parameter for wizard.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Returns the insered entity</returns>
        public virtual Entity W_Insert(Entity entity)
        {
            return this.Insert(entity, null);
        }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="param">The parameter.</param>
        /// <returns>Returns the insered entity</returns>
        /// <exception cref="System.Exception">Already created object.</exception>
        public virtual Entity Insert(Entity entity, GenericRepositoryParmeter param = null)
        {
            try
            {
                if (entity.dbCreatedObject != null)
                {
                    // If this exception appear you can comment it but inform gwen of the pb;
                    throw new Exception("Already created object.");
                }

                TraceManager.Debug("GenericRepository", "Insert", "Insert begin for element " + typeof(Entity).Name);
                EntityKeyHelper.AutoPrepareKeysIfRequiered(entity, this.Db, this.DbSet, this.MinInsertKeysValue);

                Entity newObj = new Entity();
                entity.dbCreatedObject = newObj;
                newObj.dbCreatedObject = newObj;
                this.RemapReferences(newObj, entity, param);
                this.DbSet.Add(newObj);
                this.SaveChange();
                TraceManager.Debug("GenericRepository", "Insert", "Insert Finish for element " + typeof(Entity).Name);
                return newObj;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                DBUtil.ReformatDBConstraintError(dbEx);
                return null;
            }
            catch (DbUpdateException dbEx)
            {
                DBUtil.ReformatDBUpdateError(dbEx);
                return null;
            }
        }

        /// <summary>
        /// Bulk copy list of entities.
        /// </summary>
        /// <param name="entities">list of entities to inserts</param>
        /// <param name="tableName">Name of the table in database. If tableName is null, tableName = Entity name</param>
        public virtual void BulkCopy(List<Entity> entities, string tableName = null)
        {
            if (entities != null && entities.Any())
            {
                DateTime beginDate = DateTime.Now;

                if (string.IsNullOrWhiteSpace(tableName))
                {
                    tableName = typeof(Entity).Name;
                }

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(this.DbContainer.db.Database.Connection.ConnectionString))
                {
                    bulkCopy.DestinationTableName = tableName;
                    ObjectContext objectContext = ((IObjectContextAdapter)this.DbContainer.db).ObjectContext;
                    IDataReader dr = entities.AsDataReader(objectContext);

                    // Get and fill Column Mappings
                    Dictionary<string, string> columnMappings = EntityDataReader<Entity>.ColumnMappings;
                    if (columnMappings != null && columnMappings.Any())
                    {
                        foreach (var columnMapping in columnMappings)
                        {
                            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(columnMapping.Key, columnMapping.Value));
                        }
                    }

                    bulkCopy.WriteToServer(dr);
                }

                TraceManager.Debug("GenericRepository", "BulkCopy", string.Format("End BulkCopy ({0} {1}) time = {2} ms", entities.Count, typeof(Entity).Name, Math.Floor((DateTime.Now - beginDate).TotalMilliseconds)));
            }
        }

        /// <summary>
        /// Duplicates the specified entity by primary key.
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        /// <param name="param">The parameter.</param>
        /// <returns>Returns the duplicated entity</returns>
        public virtual Entity Duplicate(object primaryKey, GenericRepositoryParmeter param = null)
        {
            TraceManager.Debug("GenericRepository", "Duplicate", "Begin Duplicate for element " + typeof(Entity).Name);
            List<string> lstFieldToInclude = this.GetFieldToInclude(param, AccessMode.Delete);
            Entity dbobj = this.Find(primaryKey, AccessMode.Read, sFieldsToInclude: lstFieldToInclude);
            Entity duplicated = this.Insert(dbobj, param);
            TraceManager.Debug("GenericRepository", "Duplicate", "End Duplicate for element " + typeof(Entity).Name);
            return duplicated;
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="param">The parameter.</param>
        /// <returns>Returns 0 if OK, -1 if KO</returns>
        public virtual int Delete(Entity entity, GenericRepositoryParmeter param = null)
        {
            return this.DeleteById(this.GetPrimaryKeys(entity), param);
        }

        /// <summary>
        /// Delete without parameter for wizard.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void W_Delete(Entity entity)
        {
            this.DeleteById(this.GetPrimaryKeys(entity), null);
        }

        /// <summary>
        /// Deletes the entity by identifier.
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        /// <param name="param">The parameter.</param>
        /// <returns>Returns 0 if OK, -1 if KO</returns>
        public virtual int DeleteById(object primaryKey, GenericRepositoryParmeter param = null)
        {
            TraceManager.Debug("GenericRepository", "DeleteById", "Begin DeleteById for element " + typeof(Entity).Name);
            List<string> lstFieldToInclude = !BIAUnity.IsMoq ? this.GetFieldToInclude(param, AccessMode.Delete) : null;
            Entity dbobj = this.Find(primaryKey, AccessMode.Delete, sFieldsToInclude: lstFieldToInclude);

            if (dbobj == null)
            {
                TraceManager.Debug("GenericRepository", "DeleteById", "Element not found. Perhaps do not have right.");
                return -1;
            }

            this.DeleteDBObject(dbobj, param);
            TraceManager.Debug("GenericRepository", "DeleteById", "End DeleteById for element " + typeof(Entity).Name);
            return 0;
        }

        /// <summary>
        /// Deletes the entity object.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="param">The parameter.</param>
        private void DeleteDBObject(Entity entity, GenericRepositoryParmeter param)
        {
            try
            {
                if (entity != null)
                {
                    if (this.Db.Entry(entity).State == EntityState.Detached)
                    {
                        this.DbSet.Attach(entity);
                    }

                    this.RemapReferences(entity, null, param);
                    dynamic obj = this.DbSet.Remove(entity);
                    this.SaveChange();
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                DBUtil.ReformatDBConstraintError(dbEx);
            }
            catch (DbUpdateException dbEx)
            {
                DBUtil.ReformatDBUpdateError(dbEx);
            }
        }

        /// <summary>
        /// Gets the field to include an a find depending on acces.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="mode">The mode.</param>
        /// <returns>The list of fields to include</returns>
        private List<string> GetFieldToInclude(GenericRepositoryParmeter param, AccessMode mode)
        {
            List<PropertyInfo> lstProp = typeof(Entity).GetProperties().ToList();
            List<string> retLstProp = new List<string>();

            string[] navigationProperties = EntityPropHelper.GetNavigationProperties<Entity>(this.Db);

            foreach (PropertyInfo prop in lstProp)
            {
                if (
                        (

                            // used for delete and duplicated
                            (mode == AccessMode.Delete)
                            &&
                            navigationProperties.Contains(prop.Name))
                        ||
                        (
                            (mode == AccessMode.Write)
                            &&
                            (
                                navigationProperties.Contains(prop.Name) &&
                                (
                                    (
                                        (param == null)
                                        ||
                                        ((param.Values2Update == null) && (param.Values2Exclude == null))
                                        ||
                                        ((param.Values2Update != null) && param.Values2Update.Contains(prop.Name))
                                        ||
                                        ((param.Values2Exclude != null) && !param.Values2Exclude.Contains(prop.Name)))
                                    ||
                                    prop.CustomAttributes.Any(c => c.AttributeType.Name == "RequiredAttribute")))))
                {
                    retLstProp.Add(prop.Name);
                }
            }

            return retLstProp;
        }

        /// <summary>
        /// Remaps the references.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <param name="dbParentObj">The database parent object.</param>
        /// <param name="targetParentObj">The target parent object.</param>
        /// <param name="param">The parameter.</param>
        public void RemapReferences<T1>(T1 dbParentObj, T1 targetParentObj, GenericRepositoryParmeter param = null)
            where T1 : ObjectRemap, new()
        {
            string oldTab = this.remapTab;
            this.remapTab = this.remapTab + "    ";

            EntityKeyHelper.KeyProperties[] keys = EntityKeyHelper.GetKeysProperties<T1>(this.Db);
            List<PropertyInfo> lstPropToTreate = EntityPropHelper.GetPropertiesToTreate(dbParentObj.GetType(), param, targetParentObj == null);

            foreach (PropertyInfo prop in lstPropToTreate)
            {
                string propName = prop.Name;
                bool isCollection = prop.PropertyType.Name == "ICollection`1";

                // if (IsValue2Treate(param, propName) || (isCollection && targetParentObj == null))
                {
                    if (typeof(ObjectRemap).IsAssignableFrom(prop.PropertyType))
                    {
                        if (PerfomanceTrace)
                        {
                            TraceManager.Debug("GenericRepository", "RemapReferences", this.remapTab + ">>>begin remap properties : " + prop.Name);
                        }

                        MethodInfo method = this.GetType().GetMethod("RemapUniqueReference");
                        MethodInfo generic = method.MakeGenericMethod(new[] { typeof(T1), prop.PropertyType });
                        generic.Invoke(this, new object[] { dbParentObj, targetParentObj, prop.Name, param });
                        if (PerfomanceTrace)
                        {
                            TraceManager.Debug("GenericRepository", "RemapReferences", this.remapTab + "<<<end remap properties : " + prop.Name);
                        }
                    }
                    else
                    {
                        if (isCollection)
                        {
                            Type itemType = prop.PropertyType.GetGenericArguments().Single();
                            if (typeof(ObjectRemap).IsAssignableFrom(itemType))
                            {
                                if (PerfomanceTrace)
                                {
                                    TraceManager.Debug("GenericRepository", "RemapReferences", this.remapTab + ">>>begin remap properties collection: " + prop.Name);
                                }

                                MethodInfo method = this.GetType().GetMethod("RemapMultiReference");
                                MethodInfo generic = method.MakeGenericMethod(new[] { typeof(T1), itemType });
                                generic.Invoke(this, new object[] { dbParentObj, targetParentObj, prop.Name, param });
                                if (PerfomanceTrace)
                                {
                                    TraceManager.Debug("GenericRepository", "RemapReferences", this.remapTab + "<<<end remap properties col: " + prop.Name);
                                }
                            }
                        }
                        else
                        {
                            if (targetParentObj != null)
                            {
                                if (EntityPropHelper.GetProperties(this.Db, dbParentObj, BIAUnity.IsMoq).Contains(propName))
                                {
                                    // 09/11/2016 : key should be update for not generated key but not when key is null (case in search by Unicity key)...
                                    object targetItem = prop.GetValue(targetParentObj);
                                    if (prop.CanRead && prop.CanWrite && (!keys.Any(r => propName == r.name) || !IsKeysNull(targetItem)))
                                    {
                                        object db_Item = prop.GetValue(dbParentObj);
                                        NotifyValueUpdated(propName, db_Item, targetItem, param);
                                        prop.SetValue(dbParentObj, targetItem);
                                    }
                                }
                                else
                                {
                                    if (PerfomanceTrace)
                                    {
                                        TraceManager.Debug("GenericRepository", "RemapReferences", this.remapTab + "<<<the col " + prop.Name + " is not field of " + typeof(T1));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            this.remapTab = oldTab;
        }

        /// <summary>
        /// Determines whether the value should be clone.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="propName">Name of the property.</param>
        /// <returns>True if the value should be clone</returns>
        private static bool IsValue2Clone(GenericRepositoryParmeter param, string propName)
        {
            if (param != null)
            {
                if (param.Values2Clone != null)
                {
                    if (param.Values2Clone.Contains(propName))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the sub list rule.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="propName">Name of the property.</param>
        /// <returns>The sub list rule.</returns>
        private static SubListRule GetSubListRule(GenericRepositoryParmeter param, string propName)
        {
            if (param != null)
            {
                if (param.SubListRules != null)
                {
                    if (param.SubListRules.ContainsKey(propName))
                    {
                        return param.SubListRules[propName];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the field rule.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="propName">Name of the property.</param>
        /// <returns>The field rule</returns>
        private static FieldRule GetFieldRule(GenericRepositoryParmeter param, string propName)
        {
            if (param != null)
            {
                if (param.FieldRules != null)
                {
                    if (param.FieldRules.ContainsKey(propName))
                    {
                        return param.FieldRules[propName];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Remaps the unique reference.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="dbParentObj">The database parent object.</param>
        /// <param name="targetParentObj">The target parent object.</param>
        /// <param name="childItemName">Name of the child item.</param>
        /// <param name="param">The parameter.</param>
        public void RemapUniqueReference<T1, T2>(T1 dbParentObj, T1 targetParentObj, string childItemName, GenericRepositoryParmeter param = null)
            where T2 : ObjectRemap, new()
            where T1 : ObjectRemap
        {
            PropertyInfo itemProp = dbParentObj.GetType().GetProperty(childItemName);
            if (targetParentObj != null)
            {
                T2 db_Item = (T2)itemProp.GetValue(dbParentObj);
                T2 targetItem = (T2)targetParentObj.GetType().GetProperty(childItemName).GetValue(targetParentObj);

                if (param != null)
                {
                    if (param.Ref2Exclude != null)
                    {
                        foreach (object ref2excl in param.Ref2Exclude)
                        {
                            if (this.IsSameItem(targetItem, ref2excl))
                            {
                                return;
                            }
                        }
                    }
                }

                if (targetItem != null)
                {
                    if (itemProp.CanWrite && (db_Item == null || !this.AreKeysEquals(this.GetPrimaryKeys(db_Item), this.GetPrimaryKeys(targetItem)) || dbParentObj == targetParentObj))
                    {
                        object createdObject = this.CallFindOrCreate(targetParentObj, childItemName, param, targetItem);
                        NotifyUniqueReferenceUpdated(childItemName, db_Item, targetItem, param);
                        /* TODO when move CascadeUpdate in param
                         * if ((param != null) && (param.CascadeUpdate))
                        {
                            CascadeRemap(dbItem, targetItem, targetParentObj, childItemName, param);
                        }*/
                        itemProp.SetValue(dbParentObj, createdObject);
                    }

                    // TODO else in case of CascadeUpdate do Notify + CasacadRemap
                }
                else if (itemProp.CanWrite && (db_Item != null || dbParentObj == targetParentObj))
                {
                    NotifyUniqueReferenceUpdated(childItemName, db_Item, null, param);
                    itemProp.SetValue(dbParentObj, null);

                    // TODO in case of CascadeUpdate do CasacadRemap
                }
            }
        }

        /// <summary>
        /// Notifies the unique reference updated.
        /// </summary>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="childItemName">Name of the child item.</param>
        /// <param name="dbItem">The database item.</param>
        /// <param name="dbTargetitem">The database targetitem.</param>
        /// <param name="param">The parameter.</param>
        private static void NotifyUniqueReferenceUpdated<T2>(string childItemName, T2 dbItem, T2 dbTargetitem, GenericRepositoryParmeter param)
            where T2 : ObjectRemap, new()
        {
            FieldRule fieldRule = GetFieldRule(param, childItemName);
            if (fieldRule != null)
            {
                if (fieldRule.ItemModifing != null)
                {
                    fieldRule.ItemModifing(dbItem, dbTargetitem);
                }
            }
        }

        /// <summary>
        /// Notifies the value updated.
        /// </summary>
        /// <param name="propName">Name of the property.</param>
        /// <param name="dbItem">The database item.</param>
        /// <param name="targetItem">The target item.</param>
        /// <param name="param">The parameter.</param>
        private static void NotifyValueUpdated(string propName, object dbItem, object targetItem, GenericRepositoryParmeter param)
        {
            if (targetItem != dbItem)
            {
                FieldRule fieldRule = GetFieldRule(param, propName);
                if (fieldRule != null)
                {
                    if (fieldRule.ItemModifing != null)
                    {
                        fieldRule.ItemModifing(dbItem, targetItem);
                    }
                }
            }
        }

        /// <summary>
        /// Remaps the multi reference.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="dbParentObj">The database parent object.</param>
        /// <param name="targetParentObj">The target parent object.</param>
        /// <param name="childItemListName">Name of the child item list.</param>
        /// <param name="param">The parameter.</param>
        /// <exception cref="System.Exception">This code is not to used. See with gwen.</exception>
        public void RemapMultiReference<T1, T2>(T1 dbParentObj, T1 targetParentObj, string childItemListName, GenericRepositoryParmeter param = null)
            where T2 : ObjectRemap, new()
            where T1 : ObjectRemap
        {
            Type noProxyT2 = ObjectContext.GetObjectType(typeof(T2));

            PropertyInfo valueProp = dbParentObj.GetType().GetProperty(childItemListName);
            ICollection<T2> db_ObjList = (ICollection<T2>)valueProp.GetValue(dbParentObj);
            List<T2> toDelete = new List<T2>();
            if (targetParentObj == null)
            {
                foreach (T2 item in db_ObjList)
                {
                    toDelete.Add(item);
                }

                foreach (T2 item in toDelete)
                {
                    NotifyListItemChange(dbParentObj, item, null, childItemListName, param, TypeOfCascade.Deletion);
                    db_ObjList.Remove(item);
                    if (IsValue2Clone(param, childItemListName))
                    {
                        this.CascadeRemap(item, null, targetParentObj, childItemListName, param);
                        this.Db.Set(noProxyT2).Remove(item);
                    }
                }

                db_ObjList.Clear();
            }
            else
            {
                ICollection<T2> targetObjList = (ICollection<T2>)targetParentObj.GetType().GetProperty(childItemListName).GetValue(targetParentObj);

                if (dbParentObj == targetParentObj)
                {
                    throw new Exception("This code is not to used. See with gwen.");
                }
                else if (targetObjList != db_ObjList)
                {
                    bool elemInList = false;
                    SubListRule subListRule = GetSubListRule(param, childItemListName);

                    foreach (T2 dbItem in db_ObjList)
                    {
                        T2 targetItem = null;
                        if (!this.IsIdentityInList(targetObjList, dbItem, dbParentObj, param, subListRule, out targetItem))
                        {
                            if ((subListRule == null) || subListRule.SuppressItemNotFound)
                            {
                                toDelete.Add(dbItem);
                            }
                        }
                        else
                        {
                            if ((subListRule != null) && subListRule.CascadeUpdate)
                            {
                                NotifyListItemChange(dbParentObj, dbItem, targetItem, childItemListName, param, TypeOfCascade.Update);
                                this.CascadeRemap(dbItem, targetItem, targetParentObj, childItemListName, param);
                            }

                            elemInList = true;
                        }
                    }

                    if (targetObjList != null)
                    {
                        foreach (T2 targetItem in targetObjList.ToList())
                        {
                            if (targetItem != null)
                            {
                                T2 db_Item = null;
                                if ((!elemInList) || (!this.IsIdentityInList(db_ObjList, targetItem, dbParentObj, param, subListRule, out db_Item)))
                                {
                                    object createdObject = this.CallFindOrCreate(targetParentObj, childItemListName, param, targetItem);
                                    NotifyListItemChange(dbParentObj, null, targetItem, childItemListName, param, TypeOfCascade.Creation);
                                    db_ObjList.Add((T2)createdObject);
                                }
                                else
                                {
                                    targetItem.dbCreatedObject = db_Item;
                                }
                            }
                        }
                    }

                    foreach (T2 item in toDelete)
                    {
                        NotifyListItemChange(dbParentObj, item, null, childItemListName, param, TypeOfCascade.Deletion);
                        db_ObjList.Remove(item);
                        if (this.IsParentDirectlyReferenced(item, dbParentObj))
                        {
                            this.CascadeRemap(item, null, targetParentObj, childItemListName, param);
                            this.Db.Set(noProxyT2).Remove(item);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Finds the or create.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="targetItem">The target item.</param>
        /// <param name="targetParentObj">The target parent object.</param>
        /// <param name="childItemListName">Name of the child item list.</param>
        /// <param name="param">The parameter.</param>
        /// <returns>The entity fund or created</returns>
        public T2 FindOrCreate<T1, T2>(T2 targetItem, T1 targetParentObj, string childItemListName, GenericRepositoryParmeter param)
            where T2 : ObjectRemap, new()
        {
            if (PerfomanceTrace)
            {
                TraceManager.Debug("GenericRepository", "FindOrCreate", this.remapTab + ">>>FindOrCreate: " + childItemListName);
            }

            Type noProxyT2 = ObjectContext.GetObjectType(typeof(T2));
            bool forceCreate = IsValue2Clone(param, childItemListName);
            object[] keys = this.GetPrimaryKeys(targetItem);

            if (!forceCreate && (!AreKeysNull(keys)))
            {
                T2 existingObj = (T2)this.Db.Set(noProxyT2).Find(keys);
                if (existingObj != null)
                {
                    targetItem.dbCreatedObject = existingObj;
                    if (PerfomanceTrace)
                    {
                        TraceManager.Debug("GenericRepository", "FindOrCreate", this.remapTab + "<<<FindOrCreate: " + childItemListName + " => by key=" + keys[0]);
                    }

                    return existingObj;
                }
            }
            else if (targetItem.dbCreatedObject != null)
            {
                return (T2)targetItem.dbCreatedObject;
            }

            T2 newObj = new T2();

            targetItem.dbCreatedObject = newObj;
            newObj.dbCreatedObject = newObj;
            this.CascadeRemap(newObj, targetItem, targetParentObj, childItemListName, param);
            this.Db.Set(noProxyT2).Add(newObj);

            if (!forceCreate && (!AreKeysNull(keys)))
            {
                Debug.Assert(this.AreKeysEquals(this.GetPrimaryKeys(newObj), keys), "The key should be initialized.");
            }

            if (PerfomanceTrace)
            {
                if (forceCreate)
                {
                    TraceManager.Debug("GenericRepository", "FindOrCreate", this.remapTab + "<<<FindOrCreate: " + childItemListName + " => forceCreate");
                }
                else if (AreKeysNull(keys))
                {
                    TraceManager.Debug("GenericRepository", "FindOrCreate", this.remapTab + "<<<FindOrCreate: " + childItemListName + " => key= 0");
                }
            }

            return newObj;
        }

        /// <summary>
        /// Calls the find or create.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="targetParentObj">The target parent object.</param>
        /// <param name="childItemName">Name of the child item.</param>
        /// <param name="param">The parameter.</param>
        /// <param name="targetItem">The target item.</param>
        /// <returns>The entity fund or created</returns>
        private object CallFindOrCreate<T1, T2>(T1 targetParentObj, string childItemName, GenericRepositoryParmeter param, T2 targetItem)
            where T1 : ObjectRemap
            where T2 : ObjectRemap, new()
        {
            Type newType = EntityTypeHelper.GetModelType(targetItem.GetType());
            MethodInfo method = this.GetType().GetMethod("FindOrCreate");
            MethodInfo generic = method.MakeGenericMethod(new[] { typeof(T1), newType });
            return generic.Invoke(this, new object[] { targetItem, targetParentObj, childItemName, param });
        }

        /// <summary>
        /// Cascades the remap.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="dbItem">The database item.</param>
        /// <param name="targetItem">The target item.</param>
        /// <param name="targetParentObj">The target parent object.</param>
        /// <param name="childItemListName">Name of the child item list.</param>
        /// <param name="param">The parameter.</param>
        private void CascadeRemap<T1, T2>(T2 dbItem, T2 targetItem, T1 targetParentObj, string childItemListName, GenericRepositoryParmeter param)
            where T2 : ObjectRemap, new()
        {
            if (PerfomanceTrace)
            {
                TraceManager.Debug("GenericRepository", "CascadeRemap", this.remapTab + ">>>CascadeRemap: " + childItemListName);
            }

            MethodInfo method = this.GetType().GetMethod("RemapReferences");
            MethodInfo generic = method.MakeGenericMethod(new[] { typeof(T2) });
            if (PerfomanceTrace)
            {
                TraceManager.Debug("GenericRepository", "CascadeRemap", this.remapTab + "after MakeGenericMethod: ");
            }

            GenericRepositoryParmeter subParam = new GenericRepositoryParmeter();
            if (param != null)
            {
                if ((param.CascadeParams != null) && param.CascadeParams.ContainsKey(childItemListName))
                {
                    subParam = param.CascadeParams[childItemListName];
                }
            }

            subParam.Ref2Exclude = new List<object>() { targetParentObj };
            if (PerfomanceTrace)
            {
                TraceManager.Debug("GenericRepository", "CascadeRemap", this.remapTab + "before Invoke: ");
            }

            generic.Invoke(this, new object[] { dbItem, targetItem, subParam });
            if (PerfomanceTrace)
            {
                TraceManager.Debug("GenericRepository", "CascadeRemap", this.remapTab + "<<<CascadeRemap: " + childItemListName);
            }
        }

        /// <summary>
        /// Notifies the list item change.
        /// </summary>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="parentObject">The parent object.</param>
        /// <param name="dbItem">The database item.</param>
        /// <param name="targetItem">The target item.</param>
        /// <param name="childItemListName">Name of the child item list.</param>
        /// <param name="param">The parameter.</param>
        /// <param name="typeOfCascde">The type of cascde.</param>
        private static void NotifyListItemChange<T2>(object parentObject, T2 dbItem, T2 targetItem, string childItemListName, GenericRepositoryParmeter param, TypeOfCascade typeOfCascde)
            where T2 : ObjectRemap, new()
        {
            SubListRule subListRule = GetSubListRule(param, childItemListName);
            if (subListRule != null)
            {
                switch (typeOfCascde)
                {
                    case TypeOfCascade.Creation:
                        if (subListRule.ItemAdding != null)
                        {
                            subListRule.ItemAdding(parentObject, targetItem);
                        }

                        break;
                    case TypeOfCascade.Update:
                        if (subListRule.ItemUpdating != null)
                        {
                            subListRule.ItemUpdating(parentObject, dbItem, targetItem);
                        }

                        break;
                    case TypeOfCascade.Deletion:
                        if (subListRule.ItemDeleting != null)
                        {
                            subListRule.ItemDeleting(parentObject, dbItem);
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// Determines whether the identity is in list.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="originalList">The original list.</param>
        /// <param name="newItem">The new item.</param>
        /// <param name="parentItem">The parent item.</param>
        /// <param name="param">The parameter.</param>
        /// <param name="subList">The sub list.</param>
        /// <param name="findItem">The find item.</param>
        /// <returns>True if the identity is in list</returns>
        private bool IsIdentityInList<T1, T2>(ICollection<T2> originalList, T2 newItem, T1 parentItem, GenericRepositoryParmeter param, SubListRule subList, out T2 findItem)
            where T2 : ObjectRemap
            where T1 : ObjectRemap
        {
            if (PerfomanceTrace)
            {
                TraceManager.Debug("GenericRepository", "IdentityInList", this.remapTab + ">>>IdentityInList ");
            }

            object[] primaryKeys = this.GetPrimaryKeys(newItem);
            if (originalList != null)
            {
                foreach (T2 item in originalList)
                {
                    object[] keys = this.GetPrimaryKeys(item);
                    if (AreKeysNull(primaryKeys) || AreKeysNull(keys))
                    {
                        if ((subList == null) || (subList.UnicityKeys == null))
                        {
                            if (this.CompareItems(newItem, item, parentItem))
                            {
                                findItem = item;
                                if (PerfomanceTrace)
                                {
                                    TraceManager.Debug("GenericRepository", "IdentityInList", this.remapTab + "<<<IdentityInList find by compare");
                                }

                                return true;
                            }
                        }
                        else
                        {
                            bool in_list = true;
                            foreach (string unicitykey in subList.UnicityKeys)
                            {
                                PropertyInfo prop = item.GetType().GetProperty(unicitykey);
                                var value1 = prop.GetValue(item);
                                var value2 = newItem.GetType().GetProperty(prop.Name).GetValue(newItem);
                                if (!this.AreValuesEquals(prop, value1, value2, null))
                                {
                                    in_list = false;
                                    break;
                                }
                            }

                            if (in_list)
                            {
                                findItem = item;
                                if (PerfomanceTrace)
                                {
                                    TraceManager.Debug("GenericRepository", "IdentityInList", this.remapTab + "<<<IdentityInList find by subList.UnicityKeys");
                                }

                                return true;
                            }
                        }
                    }
                    else if (this.AreKeysEquals(primaryKeys, keys))
                    {
                        findItem = item;
                        if (PerfomanceTrace)
                        {
                            TraceManager.Debug("GenericRepository", "IdentityInList", this.remapTab + "<<<IdentityInList find by primary key");
                        }

                        return true;
                    }
                }
            }

            findItem = null;
            return false;
        }

        /// <summary>
        /// Determines whether the parent is directly referenced.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="item">The item.</param>
        /// <param name="parentItem">The parent item.</param>
        /// <returns>True if the parent is directly referenced</returns>
        private bool IsParentDirectlyReferenced<T1, T2>(T2 item, T1 parentItem)
            where T1 : ObjectRemap
            where T2 : ObjectRemap
        {
            foreach (PropertyInfo prop in item.GetType().GetProperties())
            {
                var value1 = prop.GetValue(item);
                if (this.IsSameItem(parentItem, value1))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether there are related entities to delete.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>True if there are related entities to delete</returns>
        public static bool IsRelatedEntitiesToDelete(EntityObject entity)
        {
            foreach (var relatedEntity in ((IEntityWithRelationships)entity).RelationshipManager.GetAllRelatedEnds().SelectMany(re => re.CreateSourceQuery().OfType<EntityObject>()).Distinct().ToArray())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Compares the items.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="newItem">The new item.</param>
        /// <param name="item">The item.</param>
        /// <param name="parentItem">The parent item.</param>
        /// <returns>True if item are equals</returns>
        private bool CompareItems<T1, T2>(T2 newItem, T2 item, T1 parentItem)
            where T1 : ObjectRemap
            where T2 : ObjectRemap
        {
            bool in_list = true;
            EntityKeyHelper.KeyProperties[] keys = EntityKeyHelper.GetKeysProperties<T2>(this.Db);
            if (item.GetType().IsAssignableFrom(newItem.GetType()) || newItem.GetType().IsAssignableFrom(item.GetType()))
            {
                foreach (PropertyInfo prop in item.GetType().GetProperties())
                {
                    var value1 = prop.GetValue(item);
                    var value2 = newItem.GetType().GetProperty(prop.Name).GetValue(newItem);
                    if (((value1 == null) && (value2 != null)) || ((value2 == null) && (value1 != null)))
                    {
                        if (this.IsSameItem(parentItem, value1))
                        {
                            continue;
                        }

                        if (this.IsSameItem(parentItem, value2))
                        {
                            continue;
                        }

                        in_list = false;
                        break;
                    }
                    else
                    if ((value1 != null) && (value2 != null))
                    {
                        if (!this.AreValuesEquals(prop, value1, value2, keys))
                        {
                            in_list = false;
                            break;
                        }
                    }
                }

                return in_list;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Ares the values equals.
        /// </summary>
        /// <param name="prop">The property.</param>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <param name="keysToExclude">The keys to exclude.</param>
        /// <returns>True if value are equale</returns>
        private bool AreValuesEquals(PropertyInfo prop, object value1, object value2, EntityKeyHelper.KeyProperties[] keysToExclude)
        {
            bool ret = true;
            if (typeof(ObjectRemap).IsAssignableFrom(prop.PropertyType))
            {
                if ((value1 != null) && (value2 != null))
                {
                    MethodInfo method = this.GetType().GetMethod("GetPrimaryKeys");
                    MethodInfo generic = method.MakeGenericMethod(new[] { prop.PropertyType });
                    object[] key1 = (object[])generic.Invoke(this, new object[] { value1 });
                    object[] key2 = (object[])generic.Invoke(this, new object[] { value2 });

                    if (!this.AreKeysEquals(key1, key2))
                    {
                        ret = false;
                    }
                }
                else if ((value1 != null) || (value2 != null))
                {
                    ret = false;
                }
            }
            else if (prop.PropertyType.Name == "ICollection`1")
            {
                return ret;
            }
            else if (keysToExclude == null || !keysToExclude.Any(r => prop.Name == r.name))
            {
                if (!value1.Equals(value2))
                {
                    ret = false;
                }
            }

            return ret;
        }

        /// <summary>
        /// Determines whether the object is same item.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <param name="parentItem">The parent item.</param>
        /// <param name="value1">The value1.</param>
        /// <returns>True if same item</returns>
        private bool IsSameItem<T1>(T1 parentItem, object value1)
            where T1 : ObjectRemap
        {
            bool isParent = false;
            if ((value1 != null) && (parentItem != null))
            {
                if (typeof(T1).IsAssignableFrom(value1.GetType()))
                {
                    object[] keys1 = this.GetPrimaryKeys((T1)value1);
                    object[] keys2 = this.GetPrimaryKeys(parentItem);
                    if (this.AreKeysEquals(keys1, keys2))
                    {
                        isParent = true;
                    }
                }
            }

            return isParent;
        }

        /// <summary>
        /// Gets the primary keys of entity.
        /// </summary>
        /// <typeparam name="Entity2">The type of the entity.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The primary keys</returns>
        public object[] GetPrimaryKeys<Entity2>(Entity2 value)
            where Entity2 : ObjectRemap
        {
            return EntityKeyHelper.GetKeys(value, this.Db);
        }

        /// <summary>
        /// Gets the primary keys properties.
        /// </summary>
        /// <returns>The primary keys properties</returns>
        public EntityKeyHelper.KeyProperties[] GetPrimaryKeysProperties()
        {
            return EntityKeyHelper.GetKeysProperties<Entity>(this.Db);
        }

        /// <summary>
        /// Ares the keys equals.
        /// </summary>
        /// <param name="keys1">The keys1.</param>
        /// <param name="keys2">The keys2.</param>
        /// <returns>True if keys are equals</returns>
        public bool AreKeysEquals(object[] keys1, object[] keys2)
        {
            for (int i = 0; i < keys1.Length; i++)
            {
                string key1 = "0";
                if (keys1[i] != null)
                {
                    key1 = keys1[i].ToString();
                }

                string key2 = "0";
                if (keys2[i] != null)
                {
                    key2 = keys2[i].ToString();
                }

                if (key1 != key2)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether keys is null.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>True if key is null</returns>
        private static bool IsKeysNull(object key)
        {
            if (key.ToString() == "0")
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Ares the keys null.
        /// </summary>
        /// <param name="keys1">The keys1.</param>
        /// <returns>True if one key is null</returns>
        private static bool AreKeysNull(object[] keys1)
        {
            for (int i = 0; i < keys1.Length; i++)
            {
                if (IsKeysNull(keys1[i]))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Sets the primary key.
        /// </summary>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="key">The key.</param>
        public void SetPrimaryKey<T2>(T2 value, object key)
            where T2 : ObjectRemap
        {
            EntityKeyHelper.SetKeys(value, this.Db, new object[] { key });
        }

        /// <summary>
        /// Return List of Entity. Exemple : Return List object selected in GridView
        /// </summary>
        /// <param name="lstId"> List of objet Ids</param>
        /// <param name="mode">Precise the usage (All/Read/Write)</param>
        /// <returns>Return list of object from Ids</returns>
        public List<Entity> GetObjects(List<int> lstId, AccessMode mode = AccessMode.Read)
        {
            List<Entity> rows = new List<Entity>();
            if (lstId != null)
            {
                for (int i = 0; i < lstId.Count; i++)
                {
                    rows.Add(this.Find(lstId[i], mode));
                }
            }

            return rows;
        }

        /// <summary>
        /// Gets the standard query that return all item depending of the access mode pass in parameter (read by default).
        /// </summary>
        /// <param name="mode">The mode. (Read by default)</param>
        /// <returns>The standard query</returns>
        public virtual IQueryable<Entity> GetStandardQuery(AccessMode mode = AccessMode.Read)
        {
            return this.ContextDbSet(mode);
        }

        /// <summary>
        /// Execute a stored procedure that does not end with a SELECT
        /// </summary>
        /// <param name="storedProcedureParameter"><see cref="StoredProcedureParameter"/></param>
        /// <returns>The result returned by the database after executing the command.</returns>
        public virtual int ExecuteProcedureNonQuery(StoredProcedureParameter storedProcedureParameter)
        {
            return this.DbContainer.ExecuteProcedureNonQuery(storedProcedureParameter);
        }

        /// <summary>
        /// Execute a stored procedure of type SELECT
        /// </summary>
        /// <param name="storedProcedureParameter"><see cref="StoredProcedureParameter"/></param>
        /// <returns>List of Entity</returns>
        public virtual List<Entity> ExecuteProcedureReader(StoredProcedureParameter storedProcedureParameter)
        {
            return this.DbContainer.ExecuteProcedureReader<Entity>(storedProcedureParameter);
        }

        /// <summary>
        /// Execute a stored procedure of type SELECT
        /// </summary>
        /// <typeparam name="T">Entity or EntityDTO</typeparam>
        /// <param name="storedProcedureParameter"><see cref="StoredProcedureParameter"/></param>
        /// <returns>List of Entity or EntityDTO</returns>
        public virtual List<T> ExecuteProcedureReader<T>(StoredProcedureParameter storedProcedureParameter)
        {
            return this.DbContainer.ExecuteProcedureReader<T>(storedProcedureParameter);
        }

        /// <summary>
        /// Gets the standard query without child.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <returns>The standard query without child</returns>
        public virtual IQueryable<Entity> GetStandardQueryWithoutChild(AccessMode mode = AccessMode.Read)
        {
            IQueryable<Entity> query = this.ContextDbSet(mode);

            // Look just for immediate subclasses as that will be enough to remove
            // any generations below
            var subTypes = typeof(Entity).Assembly.GetTypes()
                 .Where(t => t.IsSubclassOf(typeof(Entity)));
            if (subTypes.Count() == 0)
            {
                return query.OfType<Entity>();
            }

            // Start with a parameter of the type of the query
            var parameter = Expression.Parameter(typeof(Entity));

            // Build up an expression excluding all the sub-types
            Expression removeAllSubTypes = null;
            foreach (var subType in subTypes)
            {
                // For each sub-type, add a clause to make sure that the parameter is
                // not of this type
                var removeThisSubType = Expression.Not(Expression
                     .TypeIs(parameter, subType));

                // Merge with the previous expressions
                if (removeAllSubTypes == null)
                {
                    removeAllSubTypes = removeThisSubType;
                }
                else
                {
                    removeAllSubTypes = Expression
                        .AndAlso(removeAllSubTypes, removeThisSubType);
                }
            }

            // Convert to a lambda (actually pass the parameter in)
            var removeAllSubTypesLambda = Expression
                 .Lambda(removeAllSubTypes, parameter);

            // Filter the query
            return query
                .OfType<Entity>()
                .Where(removeAllSubTypesLambda as Expression<Func<Entity, bool>>);
        }
    }
#pragma warning restore SA1202 // Elements must be ordered by access
#pragma warning restore SA1204 // Elements must be ordered by access
#pragma warning restore CS0162 // Unreachable code detected
}
