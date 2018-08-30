// <copyright file="AllServicesDTO.cs" company="BIA.NET">
// Copyright (c) BIA.NET. All rights reserved.
// </copyright>

namespace BIA.Net.Business.Services
{
    using BIA.Net.Common.Helpers;
    using BIA.Net.DataTable.DTO;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Unity;

    /// <summary>
    /// Containner for all service
    /// </summary>
    public static class AllServicesDTO
    {
        /// <summary>
        /// The service container
        /// </summary>
        private static Dictionary<Type, Type> MappingDTOtoIService = new Dictionary<Type, Type>();


        /// <summary>
        /// Enum for Acces mode
        /// </summary>
        public enum ServiceAccessMode
        {
            /// <summary>
            /// All
            /// </summary>
            All = 0,

            /// <summary>
            /// The read
            /// </summary>
            Read = 1,

            /// <summary>
            /// The write
            /// </summary>
            Write = 2,

            /// <summary>
            /// The delete
            /// </summary>
            Delete = 3
        }

        /// <summary>
        /// Gets the service corresponding to the DTO.
        /// </summary>
        /// <typeparam name="DTO">The type of to.</typeparam>
        /// <returns>the service corresponding to the DTO</returns>
        public static object GetService<DTO>()
        {
            MapperServiceDTO.TypeMapper mapperType = MapperServiceDTO.ServiceMapping[typeof(DTO)];
            return BIAUnity.RootContainer.Resolve(mapperType.ServiceType);

        }

        /// <summary>
        /// Gets all dto of a type based on acces right.
        /// </summary>
        /// <typeparam name="DTO">The type of to.</typeparam>
        /// <param name="smode">The smode.</param>
        /// <returns>all dto of a type based on acces right</returns>
        public static List<DTO> GetAll<DTO>(ServiceAccessMode smode = ServiceAccessMode.Read)
        {
            return ((dynamic)GetService<DTO>()).GetAll(smode);
        }

        /// <summary>
        /// Gets all DTO filter by condition of a Type corresponding to the acces mode.
        /// </summary>
        /// <typeparam name="DTO">The type of the dto.</typeparam>
        /// <typeparam name="Entity">The type of the entity.</typeparam>
        /// <param name="where">The filter condition.</param>
        /// <param name="smode">The smode.</param>
        /// <returns>all DTO of a Type corresponding to the acces mode</returns>
        public static List<DTO> GetAllWhere<DTO, Entity>(Expression<Func<Entity, bool>> where, ServiceAccessMode smode = ServiceAccessMode.Read)
        {
            return ((dynamic)GetService<DTO>()).GetAllWhere(where, smode);
        }

        /// <summary>
        /// Returns a filter object list to be displayed in a paginated list.
        /// </summary>
        /// <typeparam name="DTO">The type of the dto.</typeparam>
        /// <typeparam name="Entity">The type of the entity.</typeparam>
        /// <param name="datatableDTO"><see cref="DataTableAjaxPostDTO"/> </param>
        /// <param name="filteredResultsCount">filtered results count</param>
        /// <param name="totalResultsCount">total results count</param>
        /// <param name="where">custom filter</param>
        /// <param name="smode"><see cref="ServiceAccessMode"/></param>
        /// <returns>list of objects</returns>
        public static List<DTO> GetAllWhereForAjaxDataTable<DTO, Entity>(DataTableAjaxPostDTO datatableDTO, out int filteredResultsCount, out int totalResultsCount, Expression<Func<Entity, bool>> where = default(Expression<Func<Entity, bool>>), ServiceAccessMode smode = ServiceAccessMode.Read)
        {
            return ((dynamic)GetService<DTO>()).GetAllWhereForAjaxDataTable(datatableDTO, out filteredResultsCount, out totalResultsCount, where, smode);
        }

        /// <summary>
        /// Finds the DTO by specifing key values, acces mode, and add include.
        /// </summary>
        /// <typeparam name="DTO">The type of to.</typeparam>
        /// <param name="keyValue_s">The key value_s.</param>
        /// <param name="smode">The smode.</param>
        /// <param name="sFieldsToInclude">The s fields to include.</param>
        /// <returns>the DTO</returns>
        public static DTO Find<DTO>(object keyValue_s, ServiceAccessMode smode = ServiceAccessMode.Read, List<string> sFieldsToInclude = null)
        {
            return ((dynamic)GetService<DTO>()).Find(keyValue_s, smode, sFieldsToInclude: sFieldsToInclude);
        }

        /// <summary>
        /// Inserts the specified item.
        /// </summary>
        /// <typeparam name="DTO">The type of to.</typeparam>
        /// <param name="itemToInsert">The item to insert.</param>
        /// <returns>the DTO fo the item inserted</returns>
        public static DTO Insert<DTO>(DTO itemToInsert)
        {
            return ((dynamic)GetService<DTO>()).Insert(itemToInsert);
        }

        /// <summary>
        /// Updates the values of the item.
        /// </summary>
        /// <typeparam name="DTO">The type of to.</typeparam>
        /// <param name="itemToUpdate">The item to update.</param>
        /// <param name="values">The values.</param>
        /// <returns>the DTO of the itme updated</returns>
        public static DTO UpdateValues<DTO>(DTO itemToUpdate, List<string> values)
        {
            return ((dynamic)GetService<DTO>()).UpdateValues(itemToUpdate, values);
        }

        /// <summary>
        /// Deletes the DTO in database by identifier.
        /// </summary>
        /// <typeparam name="DTO">The type of to.</typeparam>
        /// <param name="keyValues">The key values.</param>
        /// <returns>0 if OK, -1 if KO</returns>
        public static int DeleteById<DTO>(params object[] keyValues)
        {
            return ((dynamic)GetService<DTO>()).DeleteById(keyValues);
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <typeparam name="DTO">The type of to.</typeparam>
        /// <param name="smode">The smode.</param>
        /// <returns>List of all DTO</returns>
        public static async Task<List<DTO>> GetAllAsync<DTO>(ServiceAccessMode smode = ServiceAccessMode.Read)
        {
            return await ((dynamic)GetService<DTO>()).GetAllAsync(smode);
        }

        /// <summary>
        /// Finds the asynchronous.
        /// </summary>
        /// <typeparam name="DTO">The type of to.</typeparam>
        /// <param name="keyValue_s">The key value_s.</param>
        /// <param name="smode">The smode.</param>
        /// <param name="sFieldsToInclude">The s fields to include.</param>
        /// <returns>DTO fund</returns>
        public static async Task<DTO> FindAsync<DTO>(object keyValue_s, ServiceAccessMode smode = ServiceAccessMode.Read, List<string> sFieldsToInclude = null)
        {
            return await ((dynamic)GetService<DTO>()).FindAsync(keyValue_s, smode, sFieldsToInclude: sFieldsToInclude);
        }

        /// <summary>
        /// Inserts the asynchronous.
        /// </summary>
        /// <typeparam name="DTO">The type of to.</typeparam>
        /// <param name="itemToInsert">The item to insert.</param>
        /// <returns>The DTO inserted</returns>
        public static async Task<DTO> InsertAsync<DTO>(DTO itemToInsert)
        {
            return await ((dynamic)GetService<DTO>()).InsertAsync(itemToInsert);
        }

        /// <summary>
        /// Updates the values asynchronous.
        /// </summary>
        /// <typeparam name="DTO">The type of to.</typeparam>
        /// <param name="itemToUpdate">The item to update.</param>
        /// <param name="values">The values.</param>
        /// <returns>The DTO updated</returns>
        public static async Task<DTO> UpdateValuesAsync<DTO>(DTO itemToUpdate, List<string> values)
        {
            return await ((dynamic)GetService<DTO>()).UpdateValuesAsync(itemToUpdate, values);
        }

        /// <summary>
        /// Deletes the by identifier asynchronous.
        /// </summary>
        /// <typeparam name="DTO">The type of to.</typeparam>
        /// <param name="keyValues">The key values.</param>
        /// <returns>0 if OK, -1 if KO</returns>
        public static async Task<int> DeleteByIdAsync<DTO>(params object[] keyValues)
        {
            return await ((dynamic)GetService<DTO>()).DeleteByIdAsync(keyValues);
        }
    }
}