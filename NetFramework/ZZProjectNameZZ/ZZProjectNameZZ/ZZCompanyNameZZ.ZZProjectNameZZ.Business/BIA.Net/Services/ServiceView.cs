// <copyright file="ServiceView.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace BIA.Net.Business.Services
{
#pragma warning disable SA1210 // Using directives must be ordered alphabetically by namespace
    using ZZCompanyNameZZ.ZZProjectNameZZ.Business.DTO;
    using ZZCompanyNameZZ.ZZProjectNameZZ.Business.Services;
    using ZZCompanyNameZZ.ZZProjectNameZZ.Model;
#pragma warning restore SA1210 // Using directives must be ordered alphabetically by namespace

    using BIA.Net.Authentication.Business.Helpers;
    using BIA.Net.Common;
    using BIA.Net.Common.Helpers;
    using Business.DTO;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Service to manipulate Site
    /// </summary>
    public class ServiceView : TServiceDTO<ViewDTO, View, ZZProjectNameZZDBContainer>
    {
        /// <summary>
        /// The service to manage the view for the sites
        /// </summary>
        private readonly ServiceSiteView serviceSiteView;

        /// <summary>
        /// The service to manage the view for the Users
        /// </summary>
        private readonly ServiceUserView serviceUserView;

        /// <summary>
        /// Id of the current User
        /// </summary>
        private readonly int userId = 0;

        /// <summary>
        /// Value of the user
        /// </summary>
        private readonly AUserInfo<UserDTO> userInfo = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceView"/> class.
        /// </summary>
        public ServiceView()
        {
            serviceSiteView = BIAUnity.Resolve<ServiceSiteView>();
            serviceUserView = BIAUnity.Resolve<ServiceUserView>();

            // Retrieve user Id
            userInfo = (AUserInfo<UserDTO>)AUserInfo<UserDTO>.GetCurrentUserInfo();
            userId = userInfo.Properties.Id;
        }

        /// <summary>
        /// Create a view for a specific site
        /// </summary>
        /// <param name="data">data information to create the view</param>
        /// <returns>return the id of the view</returns>
        public int CreateView(SiteViewDTO data)
        {
            try
            {
                SiteViewDTO newView = serviceSiteView.Insert(data);
                return newView.ViewId;
            }
            catch (Exception ex)
            {
                TraceManager.Error($"Problem at the creation of the view {data.View.Name}", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Create a view for a specific user
        /// </summary>
        /// <param name="data">data information to create the view</param>
        /// <returns>return the id of the view</returns>
        public int CreateView(UserViewDTO data)
        {
            try
            {
                UserViewDTO newView = serviceUserView.Insert(data);
                return newView.ViewId;
            }
            catch (Exception ex)
            {
                TraceManager.Error($"Problem at the creation of the view {data.View.Name}", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Retrieve a specific view
        /// </summary>
        /// <param name="viewId">Id of the view</param>
        /// <returns>return the view</returns>
        public ViewDTO GetView(int viewId)
        {
            return Find(viewId);
        }

        /// <summary>
        /// Retrieve a default view for a table
        /// </summary>
        /// <param name="tableId">Id of the table</param>
        /// <returns>return the view</returns>
        public ViewDTO GetDefaultView(string tableId)
        {
            List<int> sitesUserIsMember = BIAUnity.Resolve<ServiceSite>().GetListSiteCurrentUserIsMember().Select(s => s.Id).ToList();

            return Repository.GetStandardQuery().Where(v => v.TableId == tableId &&
            (

                // System default view
                v.ViewType == (int)TypeOfView.SystemDefault
                ||

                // Site views set as default in at less on site of the user
                v.SiteViews.Any(sv => sv.IsDefault && sitesUserIsMember.Contains(sv.SiteId))
                ||

                // User view set as default by the user
                v.UserViews.Any(uv => uv.IsDefault && uv.UserId == userId))).Select(
                    p => new ViewDTO()
                    {
                        Id = p.Id,
                        TableId = p.TableId,
                        Name = p.Name,
                        Description = p.Description,
                        Preference = p.Preference,
                        ViewType = (TypeOfView)p.ViewType,
                    }).OrderByDescending(v => v.ViewType).FirstOrDefault();
        }

        /// <summary>
        /// Retrieve the list of the view for a table id and filter by user and site
        /// </summary>
        /// <param name="tableId">HTML id of the table</param>
        /// <param name="siteId">the site selected</param>
        /// <param name="sitesUserIsMember">list of sites where current user is member</param>
        /// <returns>list of the view</returns>
        public List<ViewDTO> GetSiteViewsIManage(string tableId, int siteId, List<int> sitesUserIsMember)
        {
            try
            {
                List<ViewDTO> views = Repository.GetStandardQuery().Where(v => v.TableId == tableId && v.ViewType == (int)TypeOfView.Site &&
                (
                    v.SiteViews.Count == 0
                    ||
                    v.SiteViews.Any(sv => sitesUserIsMember.Contains(sv.SiteId))))
                .Select(
                        p => new ViewDTO()
                        {
                            Id = p.Id,
                            TableId = p.TableId,
                            Name = p.Name,
                            Description = p.Description,
                            Preference = p.Preference,
                            ViewType = (TypeOfView)p.ViewType,
                            SitesAssignedCount = p.SiteViews.Count(),
                            IsAssignedToThisSite = p.SiteViews.Any(sv => sv.SiteId == siteId),
                            IsDefaultViewForThisSite = p.SiteViews.Any(sv => sv.SiteId == siteId && sv.IsDefault)
                        }).ToList();

                // Return the list
                return views;
            }
            catch (Exception ex)
            {
                TraceManager.Error($"Problem in GetSiteViews", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Retrieve the list of the view for a table id and filter by user and site
        /// </summary>
        /// <param name="tableId">HTML id of the table</param>
        /// <param name="sitesUserIsMember">list of sites where current user is member</param>
        /// <returns>list of the view</returns>
        public List<ViewDTO> GetViewICanSee(string tableId, List<int> sitesUserIsMember)
        {
            try
            {
                // Retrieve sites user Id
                List<ViewDTO> views = Repository.GetStandardQuery().Where(v => v.TableId == tableId &&
                (
                    v.ViewType == (int)TypeOfView.SystemDefault
                    ||
                    v.ViewType == (int)TypeOfView.System
                    ||
                    v.SiteViews.Any(sv => sitesUserIsMember.Contains(sv.SiteId))
                    ||
                    v.UserViews.Any(uv => uv.UserId == userId))).Select(
                        p => new ViewDTO()
                        {
                            Id = p.Id,
                            TableId = p.TableId,
                            Name = p.Name,
                            Description = p.Description,
                            Preference = p.Preference,
                            ViewType = (TypeOfView)p.ViewType,
                            SitesAssignedCount = p.SiteViews.Count(),
                        }).ToList();

                // Return the list
                return views;
            }
            catch (Exception ex)
            {
                TraceManager.Error($"Problem in GetSiteViews", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Set the view like default view for the user
        /// </summary>
        /// <param name="viewId">Id if the view</param>
        /// <param name="active">Define if the default value is to set or to remove</param>
        public void SetUserDefaultView(int viewId, bool active)
        {
            // Apply the default view on the user view
            serviceUserView.UpdateUserDefaultView(userId, viewId, active);
        }

        /// <summary>
        /// Set the view like default view for the site
        /// </summary>
        /// <param name="viewId">Id if the view</param>
        /// <param name="active">Define if the default value is to set or to remove</param>
        /// <param name="siteId">Id of the site</param>
        public void SetSiteDefaultView(int viewId, bool active, int siteId)
        {
            serviceSiteView.UpdateSiteDefaultView(siteId, viewId, active);
        }

        /// <summary>
        /// Assign the view to the site
        /// </summary>
        /// <param name="viewId">Id if the view</param>
        /// <param name="siteId">Id of the site</param>
        public void AssignViewToSite(int viewId, int siteId)
        {
            try
            {
                SiteViewDTO temp = new SiteViewDTO()
                {
                    SiteId = siteId,
                    ViewId = viewId
                };
                SiteViewDTO newView = serviceSiteView.Insert(temp);
            }
            catch (Exception ex)
            {
                TraceManager.Error($"Problem at the assign the view {viewId} to the site {siteId}.", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Unassign the view to the site
        /// </summary>
        /// <param name="viewId">Id if the view</param>
        /// <param name="siteId">Id of the site</param>
        public void UnassignViewToSite(int viewId, int siteId)
        {
            // Delete the relation between the user and the view
            serviceSiteView.DeleteSiteView(new List<int>() { siteId }, viewId);
        }

        /// <summary>
        /// Update the view for a site
        /// </summary>
        /// <param name="data">data information to create the view</param>
        /// <returns>return the id of the view</returns>
        public int UpdateView(SiteViewDTO data)
        {
            try
            {
                serviceSiteView.UpdateValues(data, new List<string> { "IsDefault", "View.Name", "View.Description", "View.Parameters" });
                return data.ViewId;
            }
            catch (Exception ex)
            {
                TraceManager.Error($"Problem to update of the view {data.View.Name}", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Update the view for an user
        /// </summary>
        /// <param name="data">data information to create the view</param>
        /// <returns>return the id of the view</returns>
        public int UpdateView(UserViewDTO data)
        {
            try
            {
                serviceUserView.UpdateValues(data, new List<string> { "IsDefault", "View.Name", "View.Description", "View.Parameters" });
                return data.ViewId;
            }
            catch (Exception ex)
            {
                TraceManager.Error($"Problem to update of the view {data.View.Name}", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Delete the view for an user
        /// </summary>
        /// <param name="viewId">Id of the view</param>
        public void DeleteUserView(int viewId)
        {
            try
            {
                // Delete the relation between the user and the view
                serviceUserView.DeleteUserView(userId, viewId);

                // Delete the view
                View elementView = Repository.Find(viewId, BIA.Net.Model.DAL.AccessMode.Delete);
                Repository.W_Delete(elementView);
                Repository.SaveChange();
            }
            catch (Exception ex)
            {
                TraceManager.Error($"Problem to delete the view with id: {viewId}", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Delete the view for a site
        /// </summary>
        /// <param name="viewId">Id of the view</param>
        public void DeleteSiteView(int viewId)
        {
            try
            {
                // Retrieve sites user Id
                List<int> sitesUserId = BIAUnity.Resolve<ServiceSite>().GetListSiteManagedByCurrentUser().Select(s => s.Id).ToList();

                // Delete the relation between the user and the view
                serviceSiteView.DeleteSiteView(sitesUserId, viewId);

                // Check if this view is use in another site
                if (!serviceSiteView.ViewHasSite(viewId))
                {
                    // Delete the view
                    serviceUserView.DeleteAllUserView(viewId);

                    View elementView = Repository.Find(viewId, BIA.Net.Model.DAL.AccessMode.Delete);
                    Repository.Delete(elementView);
                    Repository.SaveChange();
                }
            }
            catch (Exception ex)
            {
                TraceManager.Error($"Problem to delete the view with id: {viewId}", ex);
                throw ex;
            }
        }
    }
}