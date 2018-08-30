// <copyright file="TServiceDTO.cs" company="BIA.NET">
// Copyright (c) BIA.NET. All rights reserved.
// </copyright>

namespace BIA.Net.Business.Services
{
    using BIA.Net.Business.DTO.Infrastructure;
    using BIA.Net.Business.Services;
    using BIA.Net.DataTable.DTO;
    using BIA.Net.Model;
    using BIA.Net.Model.DAL;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using static BIA.Net.Business.Services.AllServicesDTO;

    /// <summary>
    /// Generic service to access Entity and translate them in DTO
    /// </summary>
    /// <typeparam name="DTO">The type of to.</typeparam>
    /// <typeparam name="Entity">The type of the entity.</typeparam>
    /// <typeparam name="DBContext">The Entity framework DB context of the entity.</typeparam>
    public class TServiceDTO<DTO, Entity, DBContext> 
        where Entity : ObjectRemap, new()
        where DBContext : DbContext, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TServiceDTO{DTO, Entity, DBContext}"/> class.
        /// </summary>
        public TServiceDTO()
        {
            Repository = new TGenericRepository<Entity, DBContext>();
        }

        /// <summary>
        /// Gets the repository
        /// </summary>
        protected TGenericRepository<Entity, DBContext> Repository { get; }

        /// <summary>
        /// Translates the access for the repository.
        /// </summary>
        /// <param name="smode">The smode.</param>
        /// <returns>The access translated for the repository</returns>
        public AccessMode TranslateAccess(ServiceAccessMode smode)
        {
            AccessMode mode = AccessMode.All;
            switch (smode)
            {
                case ServiceAccessMode.All:
                    mode = AccessMode.All;
                    break;
                case ServiceAccessMode.Read:
                    mode = AccessMode.Read;
                    break;
                case ServiceAccessMode.Write:
                    mode = AccessMode.Write;
                    break;
                case ServiceAccessMode.Delete:
                    mode = AccessMode.Delete;
                    break;
            }

            return mode;
        }

        /// <summary>
        /// Gets all DTO of a Type corresponding to the acces mode.
        /// </summary>
        /// <param name="smode">The smode.</param>
        /// <returns>all DTO of a Type corresponding to the acces mode</returns>
        public virtual List<DTO> GetAll(ServiceAccessMode smode = ServiceAccessMode.Read)
        {
            IQueryable<Entity> query = Repository.GetStandardQuery(TranslateAccess(smode));
            List<DTO> list = query.Select(GetSelectorExpression()).ToList();
            return list;
        }

        /// <summary>
        /// Gets all DTO filter by condition of a Type corresponding to the acces mode.
        /// </summary>
        /// <param name="where">The filter condition.</param>
        /// <param name="smode">The smode.</param>
        /// <returns>all DTO of a Type corresponding to the acces mode</returns>
        public virtual List<DTO> GetAllWhere(Expression<Func<Entity, bool>> where, ServiceAccessMode smode = ServiceAccessMode.Read)
        {
            IQueryable<Entity> query = Repository.GetStandardQuery(TranslateAccess(smode));
            List<DTO> list = query.Where(where).Select(GetSelectorExpression()).ToList();
            return list;
        }

        /// <summary>
        /// Returns a filter object list to be displayed in a paginated list.
        /// </summary>
        /// <param name="datatableDTO"><see cref="DataTableAjaxPostDTO"/></param>
        /// <param name="filteredResultsCount">filtered results count</param>
        /// <param name="totalResultsCount">total results count</param>
        /// <param name="where">custom filter</param>
        /// <param name="smode"><see cref="ServiceAccessMode"/></param>
        /// <returns>list of objects</returns>
        public virtual List<DTO> GetAllWhereForAjaxDataTable(DataTableAjaxPostDTO datatableDTO, out int filteredResultsCount, out int totalResultsCount, Expression<Func<Entity, bool>> where = default(Expression<Func<Entity, bool>>), ServiceAccessMode smode = ServiceAccessMode.Read)
        {
            if (datatableDTO == null)
            {
                throw new ArgumentNullException("datatableDTO", "datatableDTO is null.");
            }

            if (datatableDTO.Order == null)
            {
                throw new ArgumentNullException("datatableDTO.Order", "datatableDTO.Order is null.");
            }

            IQueryable<Entity> query = Repository.GetStandardQuery(TranslateAccess(smode));
            totalResultsCount = query.Count();

            if (where != default(Expression<Func<Entity, bool>>))
            {
                query = query.Where(where);
                filteredResultsCount = query.Count();
            }
            else
            {
                filteredResultsCount = totalResultsCount;
            }

            string sortExpression = datatableDTO.Columns[datatableDTO.Order.First().Column].Data.Replace("__", ".") + " " + datatableDTO.Order.First().Dir.ToLower();

            List<DTO> result = query.OrderBy(sortExpression)
                .Skip(datatableDTO.Start)
                .Take(datatableDTO.Length)
                .Select(GetSelectorExpression()).ToList();

            return result ?? new List<DTO>();
        }

        /// <summary>
        /// Finds the DTO by specifing key values, acces mode, and add include.
        /// </summary>
        /// <param name="keyValue_s">The key value_s.</param>
        /// <param name="smode">The smode.</param>
        /// <param name="expFieldsToInclude">The exp fields to include.</param>
        /// <param name="sFieldsToInclude">The s fields to include.</param>
        /// <returns> the DTO by specifing key values, acces mode, and add include</returns>
        public virtual DTO Find(object keyValue_s, ServiceAccessMode smode = ServiceAccessMode.Read, List<Expression<Func<Entity, dynamic>>> expFieldsToInclude = null, List<string> sFieldsToInclude = null)
        {
            DTO entity = Repository.GetFindQuery(keyValue_s, TranslateAccess(smode), expFieldsToInclude, sFieldsToInclude).Select(GetSelectorExpression()).SingleOrDefault();
            return entity;
        }

        /// <summary>
        /// Inserts the specified item to insert.
        /// </summary>
        /// <param name="itemToInsert">The item to insert.</param>
        /// <returns>the DTO inserted.</returns>
        public virtual DTO Insert(DTO itemToInsert)
        {
            Entity entity = new Entity();
            GetMapper().MapToModel(itemToInsert, entity);
            Entity entityInsered = Repository.Insert(entity);
            System.Diagnostics.Debug.Assert(entityInsered != null, "Cannot insert value");
            return ToDTO(entityInsered);
        }

        /// <summary>
        /// Updates the values.
        /// </summary>
        /// <param name="itemToUpdate">The item to insert.</param>
        /// <param name="values">The values.</param>
        /// <returns>The DTO updated</returns>
        public virtual DTO UpdateValues(DTO itemToUpdate, List<string> values)
        {
            Entity entity = new Entity();
            GetMapper().MapToModel(itemToUpdate, entity);
            Entity entityUpdated = Repository.UpdateValues(entity, values);
            System.Diagnostics.Debug.Assert(entityUpdated != null, "Cannot update value");
            return ToDTO(entityUpdated);
        }

        /// <summary>
        /// Deletes DTO by identifier.
        /// </summary>
        /// <param name="keyValues">The key values.</param>
        /// <returns>0 if OK, -1 if KO</returns>
        public virtual int DeleteById(params object[] keyValues)
        {
            int ret = Repository.DeleteById(keyValues);
            System.Diagnostics.Debug.Assert(ret == 0, "Cannot delete value");
            return ret;
        }

        /// <summary>
        /// Gets all DTO asynchronous.
        /// </summary>
        /// <param name="smode">The smode.</param>
        /// <returns>all DTO</returns>
        public async Task<List<DTO>> GetAllAsync(ServiceAccessMode smode = ServiceAccessMode.Read)
        {
            IQueryable<Entity> query = Repository.GetStandardQuery(TranslateAccess(smode));
            return await query.Select(GetSelectorExpression()).ToListAsync();
        }

/*
        /// <summary>
        /// Finds the DTO asynchronous.
        /// </summary>
        /// <param name="keyValue_s">The key value_s.</param>
        /// <param name="smode">The smode.</param>
        /// <param name="expFieldsToInclude">The exp fields to include.</param>
        /// <param name="sFieldsToInclude">The s fields to include.</param>
        /// <returns>the DTO</returns>
        public async Task<DTO> FindAsync(object keyValue_s, ServiceAccessMode smode = ServiceAccessMode.Read, List<Expression<Func<Entity, dynamic>>> expFieldsToInclude = null, List<string> sFieldsToInclude = null)
        {
            return Find(keyValue_s, smode, expFieldsToInclude, sFieldsToInclude);
        }

        /// <summary>
        /// Inserts the DTO asynchronous.
        /// </summary>
        /// <param name="itemToInsert">The item to insert.</param>
        /// <returns>the DTO inserted</returns>
        public async Task<DTO> InsertAsync(DTO itemToInsert)
        {
            Entity entity = new Entity();
            GetMapper().MapToModel(itemToInsert, entity);
            return ToDTO(Repository.Insert(entity));
        }

        /// <summary>
        /// Updates the values asynchronous.
        /// </summary>
        /// <param name="itemToInsert">The item to insert.</param>
        /// <param name="values">The values.</param>
        /// <returns>the DTO updated</returns>
        public async Task<DTO> UpdateValuesAsync(DTO itemToInsert, List<string> values)
        {
            Entity entity = new Entity();
            GetMapper().MapToModel(itemToInsert, entity);
            return ToDTO(Repository.UpdateValues(entity, values));
        }

        /// <summary>
        /// Deletes the by identifier asynchronous.
        /// </summary>
        /// <param name="keyValues">The key values.</param>
        /// <returns>0 if OK, -1 if KO</returns>
        public async Task<int> DeleteByIdAsync(params object[] keyValues)
        {
            return Repository.DeleteById(keyValues);
        }
*/

        /// <summary>
        /// Gets the selector expression.
        /// </summary>
        /// <returns>the selector expression</returns>
        protected static Expression<Func<Entity, DTO>> GetSelectorExpression()
        {
            return GetMapper().SelectorExpression;
        }

        /// <summary>
        /// Gets the mapper.
        /// </summary>
        /// <returns>the mapper</returns>
        protected static MapperBase<Entity, DTO> GetMapper()
        {
            return MapperServiceDTO.GetMapper<Entity, DTO>();
        }

        /// <summary>
        /// Gets the standard query retrieving all item corresponding to the user right.
        /// </summary>
        /// <param name="smode">The smode.</param>
        /// <returns>the standard query retrieving all item corresponding to the user right</returns>
        protected IQueryable<Entity> GetQuery(ServiceAccessMode smode = ServiceAccessMode.Read)
        {
            return this.Repository.GetStandardQuery(TranslateAccess(smode));
        }

        /// <summary>
        /// Translate List of Entity to List of dto.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns>List of dto.</returns>
        protected IQueryable<DTO> ToListDTO(IQueryable<Entity> entities)
        {
            if (entities == null)
            {
                return default(IQueryable<DTO>);
            }

            return entities.ToListDTO(GetSelectorExpression());
        }

        /// <summary>
        /// Translate Entity to dto.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The DTO full with entity fileds values</returns>
        private DTO ToDTO(Entity entity)
        {
            if (entity == null)
            {
                return default(DTO);
            }

            return entity.ToDTO<Entity, DTO>(GetSelectorExpression());
        }
    }
}