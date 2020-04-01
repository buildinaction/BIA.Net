// <copyright file="RolesController.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Presentation.Api.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using MyCompany.BIATemplate.Application.User;
    using MyCompany.BIATemplate.Crosscutting.Common;
    using MyCompany.BIATemplate.Presentation.Api.Controllers.Bia;

    /// <summary>
    /// The API controller used to manage roles.
    /// </summary>
    public class RolesController : BiaControllerBase
    {
        /// <summary>
        /// The service role.
        /// </summary>
        private readonly IRoleAppService roleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RolesController"/> class.
        /// </summary>
        /// <param name="roleService">The role service.</param>
        public RolesController(IRoleAppService roleService)
        {
            this.roleService = roleService;
        }

        /// <summary>
        /// Gets all existing roles.
        /// </summary>
        /// <returns>The list of roles.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = Rights.Roles.List)]
        public async Task<IActionResult> GetAll()
        {
            var results = await this.roleService.GetAllAsync();

            this.HttpContext.Response.Headers.Add(Constants.HttpHeaders.TotalCount, results.Count().ToString());

            return this.Ok(results);
        }
    }
}