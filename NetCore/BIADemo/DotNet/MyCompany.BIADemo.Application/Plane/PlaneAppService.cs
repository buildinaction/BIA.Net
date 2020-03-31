// <copyright file="PlaneAppService.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIADemo.Application.Plane
{
    using MyCompany.BIADemo.Application.Bia;
    using MyCompany.BIADemo.Domain.Core;
    using MyCompany.BIADemo.Domain.Dto.Bia;
    using MyCompany.BIADemo.Domain.Dto.Plane;
    using MyCompany.BIADemo.Domain.PlaneModule.Aggregate;

    /// <summary>
    /// The application service used for plane.
    /// </summary>
    public class PlaneAppService : CrudAppServiceBase<PlaneDto, Plane, LazyLoadDto, PlaneMapper>, IPlaneAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public PlaneAppService(IGenericRepository repository)
            : base(repository)
        {
        }
    }
}