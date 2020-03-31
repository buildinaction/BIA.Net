// <copyright file="IPlaneAppService.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIADemo.Application.Plane
{
    using MyCompany.BIADemo.Application.Bia;
    using MyCompany.BIADemo.Domain.Dto.Bia;
    using MyCompany.BIADemo.Domain.Dto.Plane;

    /// <summary>
    /// The interface defining the application service for plane.
    /// </summary>
    public interface IPlaneAppService : ICrudAppServiceBase<PlaneDto, LazyLoadDto>
    {
    }
}