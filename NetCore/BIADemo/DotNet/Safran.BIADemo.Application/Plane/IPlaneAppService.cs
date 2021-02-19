// BIADemo only
// <copyright file="IPlaneAppService.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIADemo.Application.Plane
{
    using BIA.Net.Core.Application;
    using BIA.Net.Core.Domain.Dto.Base;
    using Safran.BIADemo.Domain.Dto.Plane;
    using Safran.BIADemo.Domain.PlaneModule.Aggregate;

    /// <summary>
    /// The interface defining the application service for plane.
    /// </summary>
    public interface IPlaneAppService : ICrudAppServiceListAndItemBase<PlaneDto, PlaneListItemDto, Plane, LazyLoadDto>
    {
    }
}