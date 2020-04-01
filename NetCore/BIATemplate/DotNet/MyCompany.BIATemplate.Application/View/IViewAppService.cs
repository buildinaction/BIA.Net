// <copyright file="IViewAppService.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Application.View
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MyCompany.BIATemplate.Domain.Dto.View;

    /// <summary>
    /// The interface defining the view application service.
    /// </summary>
    public interface IViewAppService
    {
        Task<IEnumerable<ViewDto>> GetAllAsync();

        Task RemoveUserViewAsync(int id);

        Task SetDefaultUserViewAsync(DefaultViewDto dto);
    }
}