// BIADemo only
// <copyright file="IPlaneTypeAppService.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIADemo.Application.Plane
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application;
    using BIA.Net.Core.Domain.Dto.Base;
    using Safran.BIADemo.Domain.Dto.Option;
    using Safran.BIADemo.Domain.Dto.Plane;
    using Safran.BIADemo.Domain.PlaneModule.Aggregate;

    /// <summary>
    /// The interface defining the application service for plane.
    /// </summary>
    public interface IPlaneTypeAppService : ICrudAppServiceBase<PlaneTypeDto, PlaneType, LazyLoadDto>
    {
        /// <summary>
        /// Return options.
        /// </summary>
        /// <returns>List of OptionDto.</returns>
        Task<IEnumerable<OptionDto>> GetAllOptionsAsync();
    }
}