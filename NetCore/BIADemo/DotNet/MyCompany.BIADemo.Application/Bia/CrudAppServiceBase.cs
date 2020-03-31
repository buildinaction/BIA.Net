// <copyright file="CrudAppServiceBase.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIADemo.Application.Bia
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Transactions;
    using BIA.Net.QueryOrder;
    using BIA.Net.Specification;
    using MyCompany.BIADemo.Crosscutting.Common;
    using MyCompany.BIADemo.Crosscutting.Common.Exceptions;
    using MyCompany.BIADemo.Domain.Core;
    using MyCompany.BIADemo.Domain.Dto;
    using MyCompany.BIADemo.Domain.Dto.Bia;

    /// <summary>
    /// The base class for all CRUD application service.
    /// </summary>
    /// <typeparam name="TDto">The DTO type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TFilterDto">The filter DTO type.</typeparam>
    /// <typeparam name="TMapper">The mapper used between entity and DTO.</typeparam>
    public abstract class CrudAppServiceBase<TDto, TEntity, TFilterDto, TMapper> : AppServiceBase, ICrudAppServiceBase<TDto, TFilterDto>
        where TDto : BaseDto, new()
        where TEntity : class, IEntity, new()
        where TFilterDto : LazyLoadDto, new()
        where TMapper : BaseMapper<TDto, TEntity>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="CrudAppServiceBase{TDto,TEntity,TFilterDto,TMapper}"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        protected CrudAppServiceBase(IGenericRepository repository)
            : base(repository)
        {
        }

        /// <inheritdoc cref="ICrudAppServiceBase{TDto,TFilterDto}.GetAllAsync"/>
        public virtual async Task<(IEnumerable<TDto> Results, int Total)> GetAllAsync(TFilterDto filters)
        {
            return await this.GetAllAsync<TMapper>(filters);
        }

        /// <inheritdoc cref="ICrudAppServiceBase{TDto,TFilterDto}.GetAsync"/>
        public virtual async Task<TDto> GetAsync(int id)
        {
            return await this.GetAsync<TMapper>(id);
        }

        /// <inheritdoc cref="ICrudAppServiceBase{TDto,TFilterDto}.AddAsync"/>
        public virtual async Task<TDto> AddAsync(TDto dto)
        {
            return await this.AddAsync<TMapper>(dto);
        }

        /// <inheritdoc cref="ICrudAppServiceBase{TDto,TFilterDto}.UpdateAsync"/>
        public virtual async Task<TDto> UpdateAsync(TDto dto)
        {
            return await this.UpdateAsync<TMapper>(dto);
        }

        /// <inheritdoc cref="ICrudAppServiceBase{TDto,TFilterDto}.RemoveAsync"/>
        public virtual async Task RemoveAsync(int id)
        {
            var entity = await this.Repository.GetAsync<TEntity>(id);
            if (entity == null)
            {
                throw new ElementNotFoundException();
            }

            this.Repository.Remove(entity);
            await this.Repository.UnitOfWork.CommitAsync();
        }

        /// <inheritdoc cref="ICrudAppServiceBase{TDto,TFilterDto}.SaveAsync"/>
        public virtual async Task SaveAsync(IEnumerable<TDto> dtos)
        {
            var dtoList = dtos.ToList();
            if (!dtoList.Any())
            {
                return;
            }

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                foreach (var dto in dtoList)
                {
                    await this.SaveAsync(dto);
                }

                transaction.Complete();
            }
        }

        /// <inheritdoc cref="ICrudAppServiceBase{TDto,TFilterDto}.GetCsvAsync"/>
        public virtual async Task<byte[]> GetCsvAsync(TFilterDto filters)
        {
            return await this.GetCsvAsync<TMapper>(filters);
        }

        /// <inheritdoc cref="ICrudAppServiceBase{TDto,TFilterDto}.GetAllAsync"/>
        protected virtual async Task<(IEnumerable<TDto> results, int total)> GetAllAsync<TOtherMapper>(TFilterDto filters)
            where TOtherMapper : BaseMapper<TDto, TEntity>, new()
        {
            var mapper = new TOtherMapper();

            var specifications = SpecificationHelper.GetLazyLoad(
                new TrueSpecification<TEntity>(),
                mapper.ExpressionCollection,
                filters);

            var queryOrder = this.GetQueryOrder(mapper.ExpressionCollection, filters?.SortField, filters?.SortOrder == 1);

            var results = await this.Repository.GetBySpecAndCountAsync(
                mapper.EntityToDto(),
                specifications,
                queryOrder,
                filters?.First ?? 0,
                filters?.Rows ?? 0);

            return (results.Item1.ToList(), results.Item2);
        }

        /// <inheritdoc cref="ICrudAppServiceBase{TDto,TFilterDto}.GetAsync"/>
        protected virtual async Task<TDto> GetAsync<TOtherMapper>(int id)
            where TOtherMapper : BaseMapper<TDto, TEntity>, new()
        {
            var mapper = new TOtherMapper();
            var result = (await this.Repository.GetByFilterAsync(mapper.EntityToDto(), x => x.Id == id))
                .FirstOrDefault();
            if (result == null)
            {
                throw new ElementNotFoundException();
            }

            return result;
        }

        /// <inheritdoc cref="ICrudAppServiceBase{TDto,TFilterDto}.AddAsync"/>
        protected virtual async Task<TDto> AddAsync<TOtherMapper>(TDto dto)
            where TOtherMapper : BaseMapper<TDto, TEntity>, new()
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var entity = new TEntity();
            new TOtherMapper().DtoToEntity(dto, entity);
            this.Repository.Add(entity);
            await this.Repository.UnitOfWork.CommitAsync();
            dto.Id = entity.Id;
            return dto;
        }

        /// <inheritdoc cref="ICrudAppServiceBase{TDto,TFilterDto}.UpdateAsync"/>
        protected virtual async Task<TDto> UpdateAsync<TOtherMapper>(TDto dto)
            where TOtherMapper : BaseMapper<TDto, TEntity>, new()
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var mapper = new TOtherMapper();

            var entity = await this.Repository.GetAsync(dto.Id, mapper.IncludesForUpdate());
            if (entity == null)
            {
                throw new ElementNotFoundException();
            }

            mapper.DtoToEntity(dto, entity);
            this.Repository.Update(entity);
            await this.Repository.UnitOfWork.CommitAsync();
            dto.DtoState = DtoState.Unchanged;
            return dto;
        }

        /// <inheritdoc cref="ICrudAppServiceBase{TDto,TFilterDto}.GetCsvAsync"/>
        protected virtual async Task<byte[]> GetCsvAsync<TOtherMapper>(TFilterDto filters)
            where TOtherMapper : BaseMapper<TDto, TEntity>, new()
        {
            List<string> columnHeaders = null;
            if (filters is FileFiltersDto fileFilters)
            {
                columnHeaders = fileFilters.Columns.Select(x => x.Value).ToList();
            }

            // We reset these parameters, used for paging, in order to recover the totality of the data.
            filters.First = 0;
            filters.Rows = 0;

            IEnumerable<TDto> results = (await this.GetAllAsync(filters)).Results;

            List<object[]> records = results.Select(new TOtherMapper().DtoToRecord()).ToList();

            StringBuilder csv = new StringBuilder();
            records.ForEach(line =>
            {
                csv.AppendLine(string.Join(Constants.Csv.Separator, line));
            });

            return Encoding.GetEncoding("iso-8859-1").GetBytes($"{string.Join(Constants.Csv.Separator, columnHeaders ?? new List<string>())}\r\n{csv.ToString()}");
        }

        /// <summary>
        /// Save a DTO in DB regarding to its state.
        /// </summary>
        /// <param name="dto">The DTO to save.</param>
        protected async Task SaveAsync(TDto dto)
        {
            switch (dto.DtoState)
            {
                case DtoState.Added:
                    await this.AddAsync(dto);
                    break;

                case DtoState.Modified:
                    await this.UpdateAsync(dto);
                    break;

                case DtoState.Deleted:
                    await this.RemoveAsync(dto.Id);
                    break;

                default:
                    return;
            }
        }

        /// <summary>
        /// Get the paging order.
        /// </summary>
        /// <param name="collection">The expression collection of entity.</param>
        /// <param name="orderMember">The order member.</param>
        /// <param name="ascending">If set to <c>true</c> [ascending].</param>
        /// <returns>The paging order.</returns>
        protected virtual QueryOrder<TEntity> GetQueryOrder(ExpressionCollection<TEntity> collection, string orderMember, bool ascending)
        {
            if (string.IsNullOrWhiteSpace(orderMember) || !collection.ContainsKey(orderMember))
            {
                return new QueryOrder<TEntity>().OrderBy(entity => entity.Id);
            }

            var order = new QueryOrder<TEntity>();
            order.GetByExpression(collection[orderMember], ascending);
            return order;
        }
    }
}