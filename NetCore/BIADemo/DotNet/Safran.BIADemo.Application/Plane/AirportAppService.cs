// BIADemo only
// <copyright file="AirportAppService.cs" company="Safran">
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
    public class AirportAppService : CrudAppServiceBase<AirportDto, Airport, LazyLoadDto, AirportMapper>, IAirportAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AirportAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public AirportAppService(ITGenericRepository<Airport> repository)
            : base(repository)
        {
        }

        /// <summary>
        /// Return options.
        /// </summary>
        /// <returns>List of OptionDto.</returns>
        public Task<IEnumerable<OptionDto>> GetAllOptionsAsync()
        {
            return this.GetAllAsync<OptionDto, AirportOptionMapper>();
        }
    }
}