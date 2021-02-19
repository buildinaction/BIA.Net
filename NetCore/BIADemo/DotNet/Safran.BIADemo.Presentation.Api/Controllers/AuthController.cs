// <copyright file="AuthController.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIADemo.Presentation.Api.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Presentation.Api.Authentication;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Safran.BIADemo.Application.Site;
    using Safran.BIADemo.Application.User;
    using Safran.BIADemo.Crosscutting.Common;
    using Safran.BIADemo.Domain.Dto.User;

    /// <summary>
    /// The API controller used to authenticate users.
    /// </summary>
    public class AuthController : BiaControllerBaseNoToken
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
        /// The site application service.
        /// </summary>
        private readonly ISiteAppService siteAppService;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<AuthController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="jwtFactory">The JWT factory.</param>
        /// <param name="userAppService">The user application service.</param>
        /// <param name="siteAppService">The site application service.</param>
        /// <param name="logger">The logger.</param>
        public AuthController(IJwtFactory jwtFactory, IUserAppService userAppService, ISiteAppService siteAppService, ILogger<AuthController> logger)
        {
            this.jwtFactory = jwtFactory;
            this.userAppService = userAppService;
            this.siteAppService = siteAppService;
            this.logger = logger;
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
            return await this.LoginOnSite(0);
        }

        /// <summary>
        /// The login action.
        /// </summary>
        /// <param name="siteId">The site identifier.</param>
        /// <returns>
        /// The JWT if authenticated.
        /// </returns>
        [HttpGet("login/site/{siteId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LoginOnSite(int siteId)
        {
            var identity = this.User.Identity;
            if (identity == null || !identity.IsAuthenticated)
            {
                if (identity == null)
                {
                    this.logger.LogInformation("Unauthorized because identity is null");
                }
                else if (!identity.IsAuthenticated)
                {
                    this.logger.LogInformation("Unauthorized because not authenticated");
                }

                return this.Unauthorized();
            }

            var login = identity.Name.Split('\\').LastOrDefault();
            var sid = ((System.Security.Principal.WindowsIdentity)identity).User.Value;

            if (string.IsNullOrEmpty(login))
            {
                this.logger.LogWarning("Unauthorized because bad login");
                return this.BadRequest("Incorrect login");
            }

            if (string.IsNullOrEmpty(sid))
            {
                this.logger.LogWarning("Unauthorized because bad sid");
                return this.BadRequest("Incorrect sid");
            }

            // parallel launch the get user profile
            var userProfileTask = this.userAppService.GetUserProfileAsync(login);

            // get roles
            var userRolesFromUserDirectory = await this.userAppService.GetUserDirectoryRolesAsync(sid);

            if (userRolesFromUserDirectory == null || !userRolesFromUserDirectory.Any())
            {
                this.logger.LogInformation("Unauthorized because No roles found");
                return this.Forbid("No roles found");
            }

            // get user info
            UserInfoDto userInfo = null;
            if (userRolesFromUserDirectory.Contains(Constants.Role.User))
            {
                userInfo = await this.userAppService.GetCreateUserInfoAsync(sid);
                await this.userAppService.UpdateLastLoginDateAndActivate(userInfo.Id);
            }
            else
            {
                userInfo = new UserInfoDto { Login = login, Language = Constants.DefaultValues.Language };
            }

            // get user rights
            List<string> userRights = null;
            if (userRolesFromUserDirectory.Contains(Constants.Role.User))
            {
                if (siteId < 1)
                {
                    var userMainRights = this.userAppService.TranslateRolesInRights(userRolesFromUserDirectory);
                    var sites = await this.siteAppService.GetAllAsync(userInfo.Id, userMainRights);
                    var site = sites?.OrderByDescending(x => x.IsDefault).FirstOrDefault();

                    if (site != null)
                    {
                        siteId = site.Id;
                    }
                }

                if (siteId > 0)
                {
                    userRights = await this.userAppService.GetRightsForUserAsync(userRolesFromUserDirectory, sid, siteId);
                    if (userRights == null || !userRights.Any())
                    {
                        this.logger.LogInformation("Unauthorized because no user rights for site : " + siteId);
                        return this.Unauthorized("You don't have access to this site");
                    }
                }
            }

            // For admin and non user
            if (userRights == null)
            {
                userRights = await this.userAppService.GetRightsForUserAsync(userRolesFromUserDirectory, sid);
            }

            if (userRights == null || !userRights.Any())
            {
                this.logger.LogInformation("Unauthorized because No rights found");
                return this.Unauthorized("No rights found");
            }

            // user data
            UserDataDto userData = null;
            if (siteId > 0)
            {
                userData = new UserDataDto() { CurrentSiteId = siteId };
            }

            var claimsIdentity = await Task.FromResult(this.jwtFactory.GenerateClaimsIdentity(login, userInfo.Id, userRights, userData));
            if (claimsIdentity == null)
            {
                this.logger.LogInformation("Unauthorized because claimsIdentity is null");
                return this.Unauthorized("No rights found");
            }

            var userProfile = userProfileTask.Result ?? new UserProfileDto { Theme = Constants.DefaultValues.Theme };

            var token = await this.jwtFactory.GenerateJwtAsync(claimsIdentity, new { UserInfo = userInfo, UserProfile = userProfile, UserData = userData });

            return this.Ok(token);
        }

        /// <summary>
        /// Gets the front end version.
        /// </summary>
        /// <returns>The front end version.</returns>
        [HttpGet("frontEndVersion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetFrontEndVersion()
        {
            return this.Ok(Constants.Application.FrontEndVersion);
        }
    }
}