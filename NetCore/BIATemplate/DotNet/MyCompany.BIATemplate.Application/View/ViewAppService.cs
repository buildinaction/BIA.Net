// <copyright file="ViewAppService.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Application.View
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Authentication;
    using Microsoft.Extensions.Logging;
    using MyCompany.BIATemplate.Application.Bia;
    using MyCompany.BIATemplate.Crosscutting.Common.Exceptions;
    using MyCompany.BIATemplate.Domain.Core;
    using MyCompany.BIATemplate.Domain.Dto.View;
    using MyCompany.BIATemplate.Domain.UserModule.Aggregate;
    using MyCompany.BIATemplate.Domain.ViewModule.Aggregate;

    /// <summary>
    /// The application service used to manage views.
    /// </summary>
    public class ViewAppService : AppServiceBase, IViewAppService
    {
        /// <summary>
        /// The claims principal.
        /// </summary>
        private readonly ClaimsPrincipal principal;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<ViewAppService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        public ViewAppService(IGenericRepository repository, IPrincipal principal, ILogger<ViewAppService> logger)
            : base(repository)
        {
            this.principal = principal as ClaimsPrincipal;
            this.logger = logger;
        }

        public async Task<IEnumerable<ViewDto>> GetAllAsync()
        {
            var currentUserId = this.principal.GetUserId();

            var associatedSites = (await this.Repository.GetByFilterAsync<Member, int>(s => s.SiteId, w => w.UserId == currentUserId)).ToList();

            var views = await this.Repository.GetBySpecAsync(
                ViewMapper.EntityToDto(currentUserId),
                ViewSpecification.SearchGetAll(associatedSites, currentUserId));

            return views.ToList();
        }

        public async Task RemoveUserViewAsync(int id)
        {
            var entity = await this.Repository.GetAsync<View>(id, view => view.ViewUsers);
            if (entity == null)
            {
                throw new ElementNotFoundException();
            }

            if (entity.ViewType != ViewType.User)
            {
                this.logger.LogWarning("Trying to delete the wrong view type: " + entity.ViewType);
                throw new BusinessException("Wrong view type: " + entity.ViewType);
            }

            var currentUserId = this.principal.GetUserId();
            if (entity.ViewUsers.All(a => a.UserId != currentUserId))
            {
                this.logger.LogWarning($"The user {currentUserId} is trying to delete the view {id} of an other user.");
                throw new BusinessException("Can't delete the view of other users !");
            }

            this.Repository.Remove(entity);
            await this.Repository.UnitOfWork.CommitAsync();
        }

        public async Task SetDefaultUserViewAsync(DefaultViewDto dto)
        {
            var entity = await this.Repository.GetAsync<View>(dto.Id, view => view.ViewUsers);
            if (entity == null)
            {
                this.logger.LogWarning($"View with id {dto.Id} not found.");
                throw new ElementNotFoundException();
            }

            var currentUserId = this.principal.GetUserId();

            // if isDefault is true we have to remove others view set as default and update the
            // current one otherwise we remove the current view
            if (dto.IsDefault)
            {
                var formerDefault = (await this.Repository.GetByFilterAsync<View>(
                        w => w.TableId == dto.TableId && w.ViewUsers.Any(a => a.IsDefault && a.UserId == currentUserId), v => v.ViewUsers))
                    .FirstOrDefault();

                var formerViewUser =
                    formerDefault?.ViewUsers.FirstOrDefault(a => a.IsDefault && a.UserId == currentUserId);
                if (formerViewUser != null)
                {
                    if (formerDefault.ViewType == ViewType.User)
                    {
                        formerViewUser.IsDefault = false;
                    }
                    else
                    {
                        formerDefault.ViewUsers.Remove(formerViewUser);
                    }

                    this.Repository.Update(formerDefault);
                }

                if (entity.ViewType == ViewType.User)
                {
                    var viewUser =
                        entity.ViewUsers.FirstOrDefault(f => f.UserId == currentUserId);
                    if (viewUser != null)
                    {
                        viewUser.IsDefault = dto.IsDefault;
                    }
                }
                else
                {
                    entity.ViewUsers.Add(new ViewUser { IsDefault = true, ViewId = entity.Id, UserId = currentUserId });
                }
            }
            else
            {
                var viewUser =
                    entity.ViewUsers.FirstOrDefault(f => f.UserId == currentUserId);
                if (viewUser != null)
                {
                    if (entity.ViewType == ViewType.User)
                    {
                        viewUser.IsDefault = dto.IsDefault;
                    }
                    else
                    {
                        entity.ViewUsers.Remove(viewUser);
                    }
                }
            }

            this.Repository.Update(entity);
            await this.Repository.UnitOfWork.CommitAsync();
        }
    }
}