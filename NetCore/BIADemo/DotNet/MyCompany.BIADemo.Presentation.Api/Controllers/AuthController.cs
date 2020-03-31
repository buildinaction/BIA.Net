// <copyright file="AuthController.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIADemo.Presentation.Api.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using MyCompany.BIADemo.Application.User;
    using MyCompany.BIADemo.Crosscutting.Common;
    using MyCompany.BIADemo.Domain.Dto.User;

    /// <summary>
    /// The API controller used to authenticate users.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// The JWT factory.
        /// </summary>
        private readonly IJwtFactory jwtFactory;

        /// <summary>
        /// The user application service.
        /// </summary>
        private readonly IUserAppService userAppService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="jwtFactory">The JWT factory.</param>
        /// <param name="userAppService">The user application service.</param>
        public AuthController(IJwtFactory jwtFactory, IUserAppService userAppService)
        {
            this.jwtFactory = jwtFactory;
            this.userAppService = userAppService;
        }

        /// <summary>
        /// The login action.
        /// </summary>
        /// <returns>The JWT if authenticated.</returns>
        [HttpGet("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login()
        {
            var identity = this.User.Identity;
            if (identity == null || !identity.IsAuthenticated)
            {
                return this.Unauthorized();
            }

            var login = identity.Name.Split('\\').LastOrDefault();
            if (string.IsNullOrEmpty(login))
            {
                return this.BadRequest("Incorrect login");
            }

            // get rights
            var userRights = await this.userAppService.GetRightsForUserAsync(login);

            if (userRights == null || !userRights.Rights.Any())
            {
                return this.Unauthorized("No rights found");
            }

            var userInfo = await this.userAppService.GetUserInfoAsync(login) ?? new UserInfoDto { Login = login, Language = Constants.DefaultValues.Language };

            var claimsIdentity = await Task.FromResult(this.jwtFactory.GenerateClaimsIdentity(login, userInfo.Id, userRights.Rights));
            if (claimsIdentity == null)
            {
                return this.Unauthorized("No rights found");
            }

            var userProfile = await this.userAppService.GetUserProfileAsync(login) ?? new UserProfileDto { Theme = Constants.DefaultValues.Theme };

            var token = await this.jwtFactory.GenerateJwtAsync(claimsIdentity, userInfo, userProfile);

            return this.Ok(token);
        }
    }
}