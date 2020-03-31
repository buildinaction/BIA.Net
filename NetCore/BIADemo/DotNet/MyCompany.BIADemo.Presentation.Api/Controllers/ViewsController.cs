// <copyright file="ViewsController.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIADemo.Presentation.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using MyCompany.BIADemo.Application.View;
    using MyCompany.BIADemo.Crosscutting.Common;
    using MyCompany.BIADemo.Crosscutting.Common.Exceptions;
    using MyCompany.BIADemo.Domain.Dto.View;
    using MyCompany.BIADemo.Presentation.Api.Controllers.Bia;

    /// <summary>
    /// The API controller used to manage views.
    /// </summary>
    public class ViewsController : BiaControllerBase
    {
        /// <summary>
        /// The service role.
        /// </summary>
        private readonly IViewAppService viewService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewsController"/> class.
        /// </summary>
        /// <param name="viewService">The view service.</param>
        public ViewsController(IViewAppService viewService)
        {
            this.viewService = viewService;
        }

        /// <summary>
        /// Gets all views that I can see.
        /// </summary>
        /// <returns>The list of views.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = Rights.Views.List)]
        public async Task<IActionResult> GetAll()
        {
            var results = await this.viewService.GetAllAsync();

            return this.Ok(results);
        }

        /// <summary>
        /// Remove a view.
        /// </summary>
        /// <param name="id">The view identifier.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete("userView/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Views.DeleteUserView)]
        public async Task<IActionResult> RemoveUserView(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                await this.viewService.RemoveUserViewAsync(id);
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
            catch (BusinessException)
            {
                return this.BadRequest();
            }
            catch (Exception)
            {
                return this.StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Set a view as the default one for the current user.
        /// </summary>
        /// <param name="id">The view identifier.</param>
        /// <returns>The result of the action.</returns>
        [HttpPut("setDefaultUserView")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Views.SetDefaultUserView)]
        public async Task<IActionResult> SetDefaultUserView([FromBody]DefaultViewDto dto)
        {
            if (dto == null)
            {
                return this.BadRequest();
            }

            try
            {
                await this.viewService.SetDefaultUserViewAsync(dto);
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
            catch (Exception)
            {
                return this.StatusCode(500, "Internal server error");
            }
        }
    }
}