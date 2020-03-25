// <copyright file="ServiceView.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace BIA.Net.Business.Services
{
    using BIA.Net.Authentication.Business;
    using BIA.Net.Authentication.Business.Helpers;
    using BIA.Net.Business.Interface;
    using BIA.Net.Business.Services;
    using BIA.Net.Common;
    using BIA.Net.Common.Helpers;
    using Business.DTO;
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using static BIA.Net.Business.DTO.ViewDTO;

    /// <summary>
    /// Service to manipulate Site
    /// </summary>
    public class ServiceView<View,DBContainer, TUserProperties> : TServiceDTO<ViewDTO, View, DBContainer>, IServiceView
                where View : ObjectRemap, new()
                where DBContainer : DbContext, new()
                where TUserProperties : IUserProperties, new()
    {
        /// <summary>
        /// The service to manage the view for the sites
        /// </summary>
        private ServiceSiteView serviceSiteView;

        /// <summary>
        /// The service to manage the view for the Users
        /// </summary>
        private ServiceUserView serviceUserView;

        /// <summary>
        /// Id of the current User
        /// </summary>
        private int userId = 0;

        /// <summary>
        /// Value of the user
        /// </summary>
        private AUserInfo<TUserProperties> userInfo = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceView"/> class.
        /// </summary>
        public ServiceView()
        {
            serviceSiteView = BIAUnity.Resolve<ServiceSiteView>();
            serviceUserView = BIAUnity.Resolve<ServiceUserView>();

            // Retrieve user Id
            userInfo = (AUserInfo<TUserProperties>)AUserInfo<TUserProperties>.GetCurrentUserInfo();
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
        /// Retrieve a specific view
        /// </summary>
        /// <param name="tableId">Id of the table</param>
        /// <param name="viewId">Id of the view</param>
        /// <returns>return the view</returns>
        public ViewDTO GetView(string tableId, int viewId)
        {
            // Return the default view
            return new ViewDTO()
            {
                Id = 0,
                IsDefaultView = true,
                ViewType = ViewTypeEnum.SystemDefault,
                Preference = string.Empty,
                Name = Common.Resources.BIA.Net.TextResources.View_DefaultView,
                TableId = tableId.ToString(),
                Comment = string.Empty,
                Description = string.Empty
            };
        }

        /// <summary>
        /// Retrieve a default view for a table
        /// </summary>
        /// <param name="tableId">Id of the table</param>
        /// <returns>return the view</returns>
        public ViewDTO GetDefaultView(string tableId)
        {
            // Retrieve the views
            List<ViewDTO> listViews = GetViews(tableId);

            // Return the view with a default status at true
            return listViews.FirstOrDefault(x => x.IsDefaultView == true);
        }

        /// <summary>
        /// Retrieve the list of the view for a table id and filter by user and site
        /// </summary>
        /// <param name="tableId">HTML id of the table</param>
        /// <returns>list of the view</returns>
        public List<ViewDTO> GetViews(string tableId)
        {
            try
            {
                // Retrieve sites user Id
                List<int> sitesUser = userInfo.Properties.Members?.Select(x => x.SiteId).ToList() ?? new List<int>();

                // Retrieve views parameters from sites and user
                List<SiteViewDTO> siteViews = serviceSiteView.GetAllWhere(x => x.View.TableId.Equals(tableId.ToString()) && sitesUser.Contains(x.SiteId));
                List<UserViewDTO> userViews = serviceUserView.GetAllWhere(x => x.View.TableId.Equals(tableId.ToString()) && x.UserId == userId);

                // Set the comment for all Site view to specify how many site use this view
                List<ViewDTO> siteViewsAssign = new List<ViewDTO>();
                foreach (IGrouping<int, SiteViewDTO> currentView in siteViews.GroupBy(x => x.ViewId))
                {
                    if (!siteViewsAssign.Any(x => x.Id == currentView.Key))
                    {
                        SiteViewDTO element = currentView.FirstOrDefault();
                        if (element != null)
                        {
                            // Set the defult value from the site admin system
                            element.View.IsDefaultView = currentView.Any(x => x.View.IsDefaultView);

                            // Define the list of the site assign to this view
                            element.View.SitesAssigned = serviceSiteView.ListSiteForAView(currentView.Key);

                            // Define the list of the site where this view is define like default
                            element.View.SitesIsDefault = serviceSiteView.ListSiteForAViewLikeDefault(currentView.Key);

                            // Set the appropriated comment
                            int nbSite = element.View.SitesAssigned.Count();
                            switch (nbSite)
                            {
                                case 1:
                                    element.View.Comment = string.Format(Common.Resources.BIA.Net.TextResources.View_SiteUsed, nbSite);
                                    break;
                                case int n when n > 1:
                                    element.View.Comment = string.Format(Common.Resources.BIA.Net.TextResources.View_SitesUsed, nbSite);
                                    break;
                            }

                            // Add the view in the list of the view managed
                            siteViewsAssign.Add(element.View);
                        }
                    }
                }

                // Set to empty the assign site for the user views
                userViews.ForEach(x => x.View.SitesAssigned = new List<int>());

                // Check the Default value
                if (userViews.Any(x => x.IsDefault == true))
                {
                    siteViewsAssign.ForEach(x => x.IsDefaultView = false);
                    if (userViews.Any(x => x.IsDefault == true && x.View.ViewType == ViewType.Site))
                    {
                        siteViewsAssign.Where(x => userViews.Select(y => y.ViewId).Distinct().Contains(x.Id)).ToList().ForEach(x => x.IsDefaultView = true);
                    }
                }

                // Retrieve the views for the sites not assign
                List<int> listSiteViewId = siteViewsAssign.Select(x => x.Id).ToList();
                List<ViewDTO> siteViewsUnassign = GetAllWhere(x => x.TableId.Equals(tableId.ToString()) && !listSiteViewId.Contains(x.Id) && x.ViewType == (int)ViewType.Site);
                siteViewsUnassign.ForEach(x => x.IsDefaultView = userViews.Any(y => y.ViewId == x.Id && y.IsDefault == true));
                siteViewsUnassign.ForEach(x => x.Comment = string.Format(Common.Resources.BIA.Net.TextResources.View_SiteUsed, 0));

                // Delete the site view define in the user list
                userViews.RemoveAll(x => x.View.ViewType == ViewTypeEnum.Site);

                // Merge the list of the views
                List<ViewDTO> listViews = siteViewsAssign.Union(userViews.Select(x => x.View)).Union(siteViewsUnassign).ToList();

                // Add default view
                listViews.Add(new ViewDTO()
                {
                    Id = 0,
                    IsDefaultView = !listViews.Any(x => x.IsDefaultView == true),
                    ViewType = ViewTypeEnum.SystemDefault,
                    Preference = string.Empty,
                    Name = Common.Resources.BIA.Net.TextResources.View_DefaultView,
                    Description = Common.Resources.BIA.Net.TextResources.View_DefaultViewDescription,
                    TableId = tableId.ToString()
                });

                // Return the list
                return listViews;
            }
            catch (Exception ex)
            {
                TraceManager.Error($"Problem to manage the popup", ex);
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
            // Retrieve the current view
            ViewDTO currentView = Find(viewId);

            // Apply the default view on the user view
            serviceUserView.UpdateUserDefaultView(userId, viewId, active);

            if (currentView.ViewType == ViewTypeEnum.Site)
            {
                try
                {
                    // Delete All association between site's views and the user
                    serviceUserView.DeleteSiteViewToUser(userId, viewId);

                    // Add relation between the site's view and the user to set in default this view
                    UserViewDTO newView = new UserViewDTO()
                    {
                        UserId = userId,
                        ViewId = viewId,
                        IsDefault = true
                    };
                    serviceUserView.Insert(newView);
                }
                catch (Exception ex)
                {
                    TraceManager.Error($"Problem to {(active ? "associate" : "unlink")} of the site view {viewId} to an user", ex);
                    throw ex;
                }
            }
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
                List<int> sitesUserId = userInfo.Properties.Members?.Select(x => x.SiteId).ToList() ?? new List<int>();

                // Delete the relation between the user and the view
                serviceSiteView.DeleteSiteView(sitesUserId, viewId);

                // Check if this view is use in another site
                if (!serviceSiteView.ViewHasSite(viewId))
                {
                    // Delete the view
                    View elementView = Repository.Find(viewId, BIA.Net.Model.DAL.AccessMode.Delete);
                    Repository.W_Delete(elementView);
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