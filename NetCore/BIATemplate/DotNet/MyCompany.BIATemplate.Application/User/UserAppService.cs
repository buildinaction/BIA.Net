// <copyright file="UserAppService.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Application.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.ActiveDirectory;
    using BIA.Net.QueryOrder;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using MyCompany.BIATemplate.Application.Bia;
    using MyCompany.BIATemplate.Crosscutting.Common.Configuration.BiaNet;
    using MyCompany.BIATemplate.Crosscutting.Common.Helpers;
    using MyCompany.BIATemplate.Domain.Core;
    using MyCompany.BIATemplate.Domain.Dto.Bia;
    using MyCompany.BIATemplate.Domain.Dto.User;
    using MyCompany.BIATemplate.Domain.UserModule.Aggregate;
    using MyCompany.BIATemplate.Domain.UserModule.Service;

    /// <summary>
    /// The application service used for user.
    /// </summary>
    public class UserAppService : AppServiceBase, IUserAppService
    {
        /// <summary>
        /// The user right domain service.
        /// </summary>
        private readonly IUserRightDomainService userRightDomainService;

        /// <summary>
        /// The user synchronize domain service.
        /// </summary>
        private readonly IUserSynchronizeDomainService userSynchronizeDomainService;

        /// <summary>
        /// The configuration of the BiaNet section.
        /// </summary>
        private readonly BiaNetSection configuration;

        /// <summary>
        /// The helper used for AD.
        /// </summary>
        private readonly IADHelper adHelper;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<UserAppService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="userRightDomainService">The user right domain service.</param>
        /// <param name="userSynchronizeDomainService">The user synchronize domain service.</param>
        /// <param name="configuration">The configuration of the BiaNet section.</param>
        /// <param name="adHelper">The AD helper.</param>
        /// <param name="logger">The logger.</param>
        public UserAppService(
            IGenericRepository repository,
            IUserRightDomainService userRightDomainService,
            IUserSynchronizeDomainService userSynchronizeDomainService,
            IOptions<BiaNetSection> configuration,
            IADHelper adHelper,
            ILogger<UserAppService> logger)
            : base(repository)
        {
            this.userRightDomainService = userRightDomainService;
            this.userSynchronizeDomainService = userSynchronizeDomainService;
            this.configuration = configuration.Value;
            this.adHelper = adHelper;
            this.logger = logger;
        }

        /// <inheritdoc cref="IUserAppService.GetAllAsync(string)"/>
        public async Task<IEnumerable<UserDto>> GetAllAsync(string filter)
        {
            var specification = UserSpecification.Search(filter);
            var result = await this.Repository.GetBySpecAsync(UserSelectBuilder.EntityToDto(), specification);
            return result.ToList();
        }

        /// <inheritdoc cref="IUserAppService.GetAllAsync(MyCompany.BIATemplate.Domain.Dto.Bia.LazyLoadDto)"/>
        public async Task<(IEnumerable<UserDto> Users, int Total)> GetAllAsync(LazyLoadDto filters)
        {
            var mapper = new UserMapper();

            var specifications = SpecificationHelper.GetLazyLoad(
                UserSpecification.SearchActive(),
                mapper.ExpressionCollection,
                filters);

            var queryOrder = this.GetQueryOrder(mapper.ExpressionCollection, filters?.SortField, filters?.SortOrder == 1);

            var results = await this.Repository.GetBySpecAndCountAsync(
                mapper.EntityToDto(),
                specifications,
                queryOrder,
                filters?.First ?? 0,
                filters?.Rows ?? 0);

            return (results.Item1.ToList(), results.Item2);
        }

        /// <inheritdoc cref="IUserAppService.GetRightsForUserAsync"/>
        public async Task<UserRightDto> GetRightsForUserAsync(string login)
        {
            return await this.userRightDomainService.GetRightsForUserAsync(login);
        }

        /// <inheritdoc cref="IUserAppService.GetUserInfoAsync"/>
        public async Task<UserInfoDto> GetUserInfoAsync(string login)
        {
            var userInfo =
                (await this.Repository.GetByFilterAsync(UserSelectBuilder.SelectUserInfo(), user => user.Login == login))
                .FirstOrDefault();

            // if user is not found in DB, try to synchronize from AD.
            if (userInfo != null)
            {
                userInfo.Language = this.configuration.Languages.Where(w => w.Country == userInfo.Country)
                    .Select(s => s.Code)
                    .FirstOrDefault();
                return userInfo;
            }

            var domain = this.configuration.Authentication.ADDomain;
            var userAD = this.adHelper.SearchUsers(login, domain).FirstOrDefault();
            if (userAD != null)
            {
                var user = new User
                {
                    Guid = userAD.Guid,
                    Login = userAD.Login,
                    FirstName = userAD.FirstName?.Length > 50 ? userAD.FirstName?.Substring(0, 50) : userAD.FirstName ?? string.Empty,
                    LastName = userAD.LastName?.Length > 50 ? userAD.LastName?.Substring(0, 50) : userAD.LastName ?? string.Empty,
                    IsActive = true,
                    Country = userAD.Country?.Length > 10 ? userAD.Country?.Substring(0, 10) : userAD.Country ?? string.Empty,
                    Department = userAD.Department?.Length > 50 ? userAD.Department?.Substring(0, 50) : userAD.Department ?? string.Empty,
                    DistinguishedName = userAD.DistinguishedName?.Length > 250 ? userAD.DistinguishedName?.Substring(0, 250) : userAD.DistinguishedName,
                    Manager = userAD.Manager?.Length > 250 ? userAD.Manager?.Substring(0, 250) : userAD.Manager,
                    Email = userAD.Email?.Length > 256 ? userAD.Email?.Substring(0, 256) : userAD.Email ?? string.Empty,
                    ExternalCompany = userAD.ExternalCompany?.Length > 50 ? userAD.ExternalCompany?.Substring(0, 50) : userAD.ExternalCompany,
                    IsEmployee = userAD.IsEmployee,
                    IsExternal = userAD.IsExternal,
                    Company = userAD.Company?.Length > 50 ? userAD.Company?.Substring(0, 50) : userAD.Company,
                    DaiDate = DateTime.Now,
                    Office = userAD.Office?.Length > 20 ? userAD.Office?.Substring(0, 20) : userAD.Office ?? string.Empty,
                    Site = userAD.Site?.Length > 50 ? userAD.Site?.Substring(0, 50) : userAD.Site,
                    SubDepartment = userAD.SubDepartment?.Length > 50 ? userAD.SubDepartment?.Substring(0, 50) : userAD.SubDepartment,
                };

                this.Repository.Add(user);
                await this.Repository.UnitOfWork.CommitAsync();

                userInfo = new UserInfoDto
                {
                    Login = user.Login,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Country = user.Country,
                };
                userInfo.Language = this.configuration.Languages.Where(w => w.Country == userInfo.Country)
                    .Select(s => s.Code)
                    .FirstOrDefault();
            }

            return userInfo;
        }

        /// <inheritdoc cref="IUserAppService.GetUserProfileAsync"/>
        public async Task<UserProfileDto> GetUserProfileAsync(string login)
        {
            var url = this.configuration.UserProfile.Url;
            var parameters = new Dictionary<string, string> { { "login", login } };
            Dictionary<string, string> result;

            try
            {
                result = await RequestHelper.GetAsync<Dictionary<string, string>>(url, parameters);
            }
            catch (Exception exception)
            {
                result = new Dictionary<string, string>();
                this.logger.LogError(exception, "An error occured while getting the user profile.");
            }

            var profile = new UserProfileDto();
            foreach (var item in result)
            {
                typeof(UserProfileDto).GetProperty(item.Key)?.SetValue(profile, item.Value);
            }

            return profile;
        }

        /// <inheritdoc cref="IUserAppService.GetAllADUserAsync"/>
        public async Task<IEnumerable<UserADDto>> GetAllADUserAsync(string filter)
        {
            var domain = this.configuration.Authentication.ADDomain;
            var users = this.adHelper.SearchUsers(filter, domain);

            return await Task.FromResult(users.OrderBy(o => o.LastName).ThenBy(o => o.FirstName)
                .Select(s => new UserADDto { FirstName = s.FirstName, LastName = s.LastName, Login = s.Login, Guid = s.Guid })
                .ToList());
        }

        /// <inheritdoc cref="IUserAppService.AddInGroupAsync"/>
        public async Task AddInGroupAsync(IEnumerable<UserADDto> users)
        {
            var domain = this.configuration.Authentication.ADDomain;
            var group = this.configuration.Roles.Where(w => w.Type == "AD" && w.Label == "User").Select(s => s.Value)
                .FirstOrDefault();

            this.adHelper.AddUsersInGroup(users.Select(s => s.Guid).ToList(), group, domain);

            await this.SynchronizeWithADAsync();
        }

        /// <inheritdoc cref="IUserAppService.RemoveInGroupAsync"/>
        public async Task RemoveInGroupAsync(int id)
        {
            var user = await this.Repository.GetAsync<User>(id);
            var domain = this.configuration.Authentication.ADDomain;
            var group = this.configuration.Roles.Where(w => w.Type == "AD" && w.Label == "User").Select(s => s.Value)
                .FirstOrDefault();

            if (user == null || string.IsNullOrWhiteSpace(domain) || string.IsNullOrWhiteSpace(group))
            {
                return;
            }

            this.adHelper.RemoveUsersInGroup(new List<Guid> { user.Guid }, group, domain);

            await this.SynchronizeWithADAsync();
        }

        /// <inheritdoc cref="IUserAppService.SynchronizeWithADAsync"/>
        public async Task SynchronizeWithADAsync()
        {
            await this.userSynchronizeDomainService.SynchronizeFromADGroupAsync();
        }

        /// <summary>
        /// Get the paging order.
        /// </summary>
        /// <param name="collection">The expression collection of entity.</param>
        /// <param name="orderMember">The order member.</param>
        /// <param name="ascending">If set to <c>true</c> [ascending].</param>
        /// <returns>The paging order.</returns>
        private QueryOrder<User> GetQueryOrder(ExpressionCollection<User> collection, string orderMember, bool ascending)
        {
            if (string.IsNullOrWhiteSpace(orderMember) || !collection.ContainsKey(orderMember))
            {
                return new QueryOrder<User>().OrderBy(entity => entity.Id);
            }

            var order = new QueryOrder<User>();
            order.GetByExpression(collection[orderMember], ascending);
            return order;
        }
    }
}