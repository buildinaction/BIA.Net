namespace BIA.Net.MVC.Controllers.View
{
#pragma warning disable SA1210 // Using directives must be ordered alphabetically by namespace
    using ZZCompanyNameZZ.ZZProjectNameZZ.Business.DTO;
    using ZZCompanyNameZZ.ZZProjectNameZZ.Business.Helpers;
    using ZZCompanyNameZZ.ZZProjectNameZZ.Business.Services;
    using ZZCompanyNameZZ.ZZProjectNameZZ.Common;
    using ZZCompanyNameZZ.ZZProjectNameZZ.Common.Resources.BIA.Net;
    using ZZCompanyNameZZ.ZZProjectNameZZ.MVC.Controllers;
#pragma warning restore SA1210 // Using directives must be ordered alphabetically by namespace

    using BIA.Net.Business.DTO;
    using BIA.Net.Business.Services;
    using BIA.Net.Common.Helpers;
    using BIA.Net.MVC.ViewModel.View;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Controller for view page
    /// </summary>
    /// <seealso cref="BaseController" />
    public class ViewController : BaseController
    {
        /// <summary>
        /// The service view
        /// </summary>
        private readonly ServiceView serviceView;

        /// <summary>
        /// The service site
        /// </summary>
        private readonly ServiceSite serviceSite;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewController"/> class.
        /// </summary>
        public ViewController()
        {
            // Init the services
            serviceView = BIAUnity.Resolve<ServiceView>();
            serviceSite = BIAUnity.Resolve<ServiceSite>();
       }

        /// <summary>
        /// Function to display the popup of the view
        /// </summary>
        /// <param name="tableId">If of the table</param>
        /// <param name="currentViewId">Id of the current view</param>
        /// <returns>Return the partial of the popup view generated</returns>
        public ActionResult ShowPopup(string tableId, int currentViewId = 0)
        {
            ViewPopupVM temp = GetVMPopup(tableId, currentViewId, 0);

            // Generate the view from the partial and the ViewModel
            return PartialView("~/Views/Shared/BIA.Net/Views/ViewsPopup.cshtml", temp);
        }

        /// <summary>
        /// Return the list of site views
        /// </summary>
        /// <param name="tableId">the table id</param>
        /// <param name="siteId">the site Id</param>
        /// <returns>the list of site views</returns>
        public ActionResult _ListSiteViews(string tableId, int siteId = 0)
        {
            // Generate the view from the partial and the ViewModel
            ViewPopupVM temp = GetVMPopup(tableId, 0, siteId);
            return PartialView("~/Views/Shared/BIA.Net/Views/_ListSiteViews.cshtml", temp);
        }

        /// <summary>
        /// Function to Delete an site/user view
        /// </summary>
        /// <param name="tableId">Id of the table</param>
        /// <param name="viewId">Id of the view</param>
        /// <param name="isSite">Define if the deletion for a view from site</param>
        /// <param name="siteId">Id of the site</param>
        /// <returns>Return the partial of the viewpopup updated</returns>
        public ActionResult DeleteView(string tableId, int viewId, bool isSite, int siteId = 0)
        {
            if (isSite)
            {
                // Call the service to delete the site view
                serviceView.DeleteSiteView(viewId);
            }
            else
            {
                // Call the service to delete the user view
                serviceView.DeleteUserView(viewId);
            }

            // Return the partial of the viewpopup updated
            return GetListsMultiContents(tableId, siteId);
        }

        /// <summary>
        /// Function to set a view to a user/site like default view at the first loading for a new sesssion
        /// </summary>
        /// <param name="tableId">Id of the table</param>
        /// <param name="viewId">Id of view</param>
        /// <param name="active">Define if the default value is to set or to remove</param>
        /// <param name="isSite">Define if the view to set by default is for a site or not</param>
        /// <param name="siteId">Id of the site</param>
        /// <returns>Return the partial of the viewpopup updated</returns>
        public ActionResult SetDefaultView(string tableId, int viewId, bool active, bool isSite, int siteId = 0)
        {
            if (isSite)
            {
                // Call the service to set the site view like default view
                serviceView.SetSiteDefaultView(viewId, active, siteId);
            }
            else
            {
                // Call the service to set the user view like default view
                serviceView.SetUserDefaultView(viewId, active);
            }

            return GetListsMultiContents(tableId, siteId);
        }

        /// <summary>
        /// Function to set a view to a user like default view at the first loading for a new sesssion
        /// </summary>
        /// <param name="tableId">Id of the table</param>
        /// <param name="viewId">Id of view</param>
        /// <param name="add">Define if the action is to add or delete</param>
        /// <param name="siteId">Id of the site</param>
        /// <returns>Return the partial of the viewpopup updated</returns>
        public ActionResult AssignViewToSite(string tableId, int viewId, bool add, int siteId)
        {
            // Call the service to add or delete the view from a site
            if (add)
            {
                serviceView.AssignViewToSite(viewId, siteId);
            }
            else
            {
                serviceView.UnassignViewToSite(viewId, siteId);
            }

            // Return the partial of the viewpopup updated
            return GetListsMultiContents(tableId, siteId);
        }

        #region Update and Create the view

        /// <summary>
        /// Function to update a view
        /// </summary>
        /// <param name="tableId">Id of the table</param>
        /// <param name="viewId">Id of the view</param>
        /// <param name="preference">Table options to update</param>
        /// <param name="siteId">Id of the site</param>
        /// <returns>Return the partial of the viewpopup updated</returns>
        public ActionResult UpdateView(string tableId, int viewId, string preference, int siteId = 0)
        {
            ViewDTO currentView = serviceView.GetView(viewId);

            // Changing preference of the table
            currentView.Preference = preference;

            // Update the view
            serviceView.UpdateValues(currentView, new List<string>() { nameof(ViewDTO.Preference) });

            // Return the partial of the viewpopup updated
            return GetListsMultiContents(tableId, siteId);
        }

        /// <summary>
        /// Function to create a view
        /// </summary>
        /// <param name="tableId">Id of the table</param>
        /// <param name="name">Name of the view</param>
        /// <param name="description">Description of the view</param>
        /// <param name="preference">Options of the table</param>
        /// <param name="isSiteView">Define of the view is a site view</param>
        /// <param name="siteId">Id of the site to associate to the view</param>
        /// <returns>Return the partial of the viewpopup updated</returns>
        public ActionResult CreateView(string tableId, string name, string description, string preference, bool isSiteView, int siteId = 0)
        {
            // Check if the name is define
            if (!string.IsNullOrEmpty(name))
            {
                // check if the view is for a site and sites are defined
                if (isSiteView)
                {
                    // Parse each site to create a relation with the view and the site
                    SiteViewDTO newView = new SiteViewDTO()
                    {
                        SiteId = siteId,
                        View = new ViewDTO()
                        {
                            Name = name,
                            Description = description,
                            Preference = preference,
                            TableId = tableId,
                            ViewType = TypeOfView.Site
                        }
                    };

                    // Create the view for this site
                    serviceView.CreateView(newView);
                }
                else
                {
                    UserViewDTO newView = new UserViewDTO()
                    {
                        UserId = ((UserInfo)User).Properties.Id,
                        View = new ViewDTO()
                        {
                            Name = name,
                            Description = description,
                            Preference = preference,
                            TableId = tableId,
                            ViewType = TypeOfView.User
                        }
                    };

                    // Create the view for an user
                    serviceView.CreateView(newView);
                }
            }

            // Return the partial of the viewpopup updated
            return GetListsMultiContents(tableId, siteId);
        }

        #endregion

        /// <summary>
        /// retrun the view model used to display ViewsPopup
        /// </summary>
        /// <param name="tableId">the table id</param>
        /// <param name="currentViewId">the current view id</param>
        /// <param name="siteId">the site Id</param>
        /// <returns>the view model used to display ViewsPopup</returns>
        private ViewPopupVM GetVMPopup(string tableId, int currentViewId, int siteId)
        {
            bool isAdmin = User.IsInRole(Constants.RoleSiteAdmin) || User.IsInRole(Constants.RoleAdmin);

            ViewDTO defaultView = serviceView.GetDefaultView(tableId);
            int defaultViewId = defaultView != null ? defaultView.Id : 0;

            List<int> sitesUserIsMember = serviceSite.GetListSiteCurrentUserIsMember().Select(s => s.Id).ToList();

            List<ViewDTO> viewsDTOICanSee = serviceView.GetViewICanSee(tableId, sitesUserIsMember);
            List<ViewVM> viewsICanSee = viewsDTOICanSee.Select(v => new ViewVM()
            {
                Id = v.Id,
                Name = v.Name,
                Description = v.Description,
                IsReference = v.ViewType == TypeOfView.SystemDefault || v.ViewType == TypeOfView.Site || v.ViewType == TypeOfView.System,
                SitesAssignedCount = v.SitesAssignedCount,
                IsAssignedToThisSite = v.IsAssignedToThisSite,
                IsDefaultView = defaultViewId == v.Id,
                IsCurrentView = currentViewId == v.Id,
            }).ToList();

            // Add default view
            viewsICanSee.Add(new ViewVM()
            {
                Id = 0,
                Name = TextResources.View_DefaultView,
                Description = TextResources.View_DefaultViewDescription,
                IsDefaultView = defaultViewId == 0,
                IsReference = true,
                IsCurrentView = currentViewId == 0,
            });

            // Retrieve user Id and the site associated

            // Create the popup view model
            ViewPopupVM vm = new ViewPopupVM()
            {
                ViewsICanSee = viewsICanSee.OrderByDescending(f => f.IsReference).ThenBy(f => f.Name).ToList(),
                UserViewsUpdatable = new SelectList(viewsICanSee.Where(f => !f.IsReference).OrderByDescending(f => f.Name), nameof(ViewVM.Id), nameof(ViewVM.Name)),
                TableId = tableId,
                IsAdmin = isAdmin,
            };

            if (isAdmin)
            {
                // Retrieve all sites to manage site views
                List<SiteDTO> listSite = BIAUnity.Resolve<ServiceSite>().GetListSiteManagedByCurrentUser();

                // Define the site selected to manage the views
                SiteDTO site = listSite.FirstOrDefault(x => x.Id == siteId);
                if (site == null)
                {
                    site = listSite.FirstOrDefault();
                }

                // List all the view for this user and table
                List<ViewDTO> siteViewsDTOIManage = serviceView.GetSiteViewsIManage(tableId, site.Id, sitesUserIsMember);

                List<ViewVM> siteViewsIManage = siteViewsDTOIManage.Select(v => new ViewVM()
                {
                    Id = v.Id,
                    Name = v.Name,
                    Description = v.Description,
                    IsReference = true,
                    SitesAssignedCount = v.SitesAssignedCount,
                    IsAssignedToThisSite = v.IsAssignedToThisSite,
                    IsDefaultView = v.IsDefaultViewForThisSite,
                    IsCurrentView = currentViewId == v.Id,
                }).ToList();

                // List of the site to manage
                vm.SitesPossible = new SelectList(listSite.OrderBy(f => f.Title), "Id", "Title", site.Id);

                // Views associed to the site yet
                vm.SiteViewsIManage = siteViewsIManage.OrderByDescending(f => f.Name).ToList();

                vm.SitesViewsUpdatable = new SelectList(siteViewsIManage.OrderByDescending(f => f.Name), nameof(ViewVM.Id), nameof(ViewVM.Name));
            }

            return vm;
        }

        /// <summary>
        /// Retrun the html for the 3 lists
        /// </summary>
        /// <param name="tableId">the table Id</param>
        /// <param name="siteId">the site If</param>
        /// <returns>the html for the 3 lists</returns>
        private ActionResult GetListsMultiContents(string tableId, int siteId)
        {
            ViewPopupVM temp = GetVMPopup(tableId, 0, siteId);
            return Json(
                new
                {
                    ListSiteViews = temp.IsAdmin ? RenderPartialToString("~/Views/Shared/BIA.Net/Views/_ListSiteViews.cshtml", temp) : string.Empty,
                    ListMyViews = RenderPartialToString("~/Views/Shared/BIA.Net/Views/_ListMyViews.cshtml", temp),
                    ListManageMyViews = RenderPartialToString("~/Views/Shared/BIA.Net/Views/_ListManageMyViews.cshtml", temp),
                },
                JsonRequestBehavior.AllowGet);
        }
    }
}