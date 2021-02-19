// BIADemo only
// <copyright file="PlaneTypeAppService.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIADemo.Application.Plane
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.RepoContract;
    using Safran.BIADemo.Domain.Dto.Option;
    using Safran.BIADemo.Domain.Dto.Plane;
    using Safran.BIADemo.Domain.PlaneModule.Aggregate;

    /// <summary>
    /// The application service used for plane.
    /// </summary>
    public class PlaneTypeAppService : CrudAppServiceBase<PlaneTypeDto, PlaneType, LazyLoadDto, PlaneTypeMapper>, IPlaneTypeAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneTypeAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public PlaneTypeAppService(ITGenericRepository<PlaneType> repository)
            : base(repository)
        {
        }

        /// <summary>
        /// Return options.
        /// </summary>
        /// <returns>List of OptionDto.</returns>
        public Task<IEnumerable<OptionDto>> GetAllOptionsAsync()
        {
            return this.GetAllAsync<OptionDto, PlaneTypeOptionMapper>();
        }
    }
}