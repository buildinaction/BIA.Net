// <copyright file="UsersController.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Presentation.Api.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using MyCompany.BIATemplate.Application.User;
    using MyCompany.BIATemplate.Crosscutting.Common;
    using MyCompany.BIATemplate.Domain.Dto.Bia;
    using MyCompany.BIATemplate.Domain.Dto.User;
    using MyCompany.BIATemplate.Presentation.Api.Controllers.Bia;

    /// <summary>
    /// The API controller used to manage users.
    /// </summary>
    public class UsersController : BiaControllerBase
    {
        /// <summary>
        /// The service user.
        /// </summary>
        private readonly IUserAppService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        public UsersController(IUserAppService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Gets all users using the filter.
        /// </summary>
        /// <param name="filter">Used to filter on lastname, firstname or login.</param>
        /// <returns>The list of users.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = Rights.Users.List)]
        public async Task<IActionResult> GetAll(string filter)
        {
            var results = await this.userService.GetAllAsync(filter);

            this.HttpContext.Response.Headers.Add(Constants.HttpHeaders.TotalCount, results.Count().ToString());

            return this.Ok(results);
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>The list of user DTO.</returns>
        [HttpPost("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = Rights.Users.ListAccess)]
        public async Task<IActionResult> GetAll([FromBody]LazyLoadDto filters)
        {
            var results = await this.userService.GetAllAsync(filters);

            this.HttpContext.Response.Headers.Add(Constants.HttpHeaders.TotalCount, results.Total.ToString());

            return this.Ok(results.Users);
        }

        /// <summary>
        /// Gets all users in AD using the filter.
        /// </summary>
        /// <param name="filter">Used to filter on lastname, firstname or login.</param>
        /// <returns>The list of users.</returns>
        [HttpGet("fromAD")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = Rights.Users.ListAD)]
        public async Task<IActionResult> GetAllFromAD(string filter)
        {
            var results = await this.userService.GetAllADUserAsync(filter);

            this.HttpContext.Response.Headers.Add(Constants.HttpHeaders.TotalCount, results.Count().ToString());

            return this.Ok(results);
        }

        /// <summary>
        /// Add some users in a group.
        /// </summary>
        /// <param name="users">The list of user.</param>
        /// <returns>The result code.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = Rights.Users.Add)]
        public async Task<IActionResult> AddInGroup([FromBody]IEnumerable<UserADDto> users)
        {
            await this.userService.AddInGroupAsync(users);
            return this.Ok();
        }

        /// <summary>
        /// Remove some users in a group.
        /// </summary>
        /// <param name="id">The identifier of the user.</param>
        /// <returns>The result code.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = Rights.Users.Delete)]
        public async Task<IActionResult> RemoveInGroup(int id)
        {
            await this.userService.RemoveInGroupAsync(id);
            return this.Ok();
        }

        /// <summary>
        /// Synchronize all the users with the AD.
        /// </summary>
        /// <returns>The OK result.</returns>
        [HttpGet("synchronize")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = Rights.Users.Sync)]
        public async Task<IActionResult> Synchronize()
        {
            await this.userService.SynchronizeWithADAsync();

            return this.Ok();
        }
    }
}