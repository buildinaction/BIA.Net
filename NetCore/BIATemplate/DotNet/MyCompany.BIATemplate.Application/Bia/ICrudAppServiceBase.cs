// <copyright file="ICrudAppServiceBase.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Application.Bia
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MyCompany.BIATemplate.Domain.Dto.Bia;

    /// <summary>
    /// The interface defining the CRUD methods.
    /// </summary>
    /// <typeparam name="TDto">The DTO type.</typeparam>
    /// <typeparam name="TFilterDto">The filter DTO type.</typeparam>
    public interface ICrudAppServiceBase<TDto, in TFilterDto>
        where TDto : BaseDto, new()
        where TFilterDto : LazyLoadDto, new()
    {
        /// <summary>
        /// Get the DTO with paging and sorting.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>The list of DTO.</returns>
        Task<(IEnumerable<TDto> Results, int Total)> GetAllAsync(TFilterDto filters);

        /// <summary>
        /// Return a DTO for a given identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The DTO.</returns>
        Task<TDto> GetAsync(int id);

        /// <summary>
        /// Transform the DTO into the corresponding entity and add it to the DB.
        /// </summary>
        /// <param name="dto">The DTO.</param>
        /// <returns>The DTO with id updated.</returns>
        Task<TDto> AddAsync(TDto dto);

        /// <summary>
        /// Update an entity in DB with the DTO values.
        /// </summary>
        /// <param name="dto">The DTO.</param>
        /// <returns>The DTO updated.</returns>
        Task<TDto> UpdateAsync(TDto dto);

        /// <summary>
        /// Remove an entity with its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        Task RemoveAsync(int id);

        /// <summary>
        /// Save the list of DTO in DB regarding to theirs state.
        /// </summary>
        /// <param name="dtos">The list of DTO to save.</param>
        Task SaveAsync(IEnumerable<TDto> dtos);

        /// <summary>
        /// Returns data in csv format.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>Data in csv format.</returns>
        Task<byte[]> GetCsvAsync(TFilterDto filters);
    }
}