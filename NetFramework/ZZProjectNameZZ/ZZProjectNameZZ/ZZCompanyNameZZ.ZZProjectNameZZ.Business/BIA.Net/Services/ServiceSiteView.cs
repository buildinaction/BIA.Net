// <copyright file="ServiceSiteView.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace BIA.Net.Business.Services
{
#pragma warning disable SA1210 // Using directives must be ordered alphabetically by namespace
    using ZZCompanyNameZZ.ZZProjectNameZZ.Model;
#pragma warning restore SA1210 // Using directives must be ordered alphabetically by namespace

    using Business.DTO;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Service to manipulate Site View
    /// </summary>
    public class ServiceSiteView : TServiceDTO<SiteViewDTO, SiteView, ZZProjectNameZZDBContainer>
    {
        /// <summary>
        /// Method to delete the view for a list of sites
        /// </summary>
        /// <param name="listSiteId">List of the id of the sites</param>
        /// <param name="viewId">Id of the view</param>
        public void DeleteSiteView(List<int> listSiteId, int viewId)
        {
            // Retrieve the site view for a site
            List<SiteView> elementSiteView = Repository.GetStandardQuery(BIA.Net.Model.DAL.AccessMode.Delete).Where(x => listSiteId.Contains(x.SiteId) && x.ViewId == viewId).ToList();

            // Parse the liste to delete the site view
            foreach (SiteView currentSiteView in elementSiteView)
            {
                Repository.Delete(currentSiteView);
            }

            // Save the Deletion for EF
            Repository.SaveChange();
        }

        /// <summary>
        /// Method to update the default site view for a view
        /// </summary>
        /// <param name="siteId">Id of the sites</param>
        /// <param name="viewId">Id of the view</param>
        /// <param name="active">Define if the default value is to set or to remove</param>
        public void UpdateSiteDefaultView(int siteId, int viewId, bool active)
        {
            List<SiteView> elementSiteView = new List<SiteView>();
            if (active)
            {
                ViewDTO view = AllServicesDTO.Find<ViewDTO>(viewId);
                if (view != null)
                {
                    // Retrieve the list of the site views to update
                    elementSiteView = Repository.GetStandardQuery(BIA.Net.Model.DAL.AccessMode.Write).Where(x => x.SiteId == siteId && x.View.TableId == view.TableId).ToList();

                    // Set all site views with a isdefault at false and set to true the appropriated view
                    elementSiteView.ForEach(x => x.IsDefault = false);
                    elementSiteView.Where(x => x.ViewId == viewId).ToList().ForEach(x => x.IsDefault = true);
                }
            }
            else
            {
                // Retrieve the list of the site views to update
                elementSiteView = Repository.GetStandardQuery(BIA.Net.Model.DAL.AccessMode.Write).Where(x => x.ViewId == viewId && x.SiteId == siteId && x.IsDefault == true).ToList();
                elementSiteView.ForEach(x => x.IsDefault = false);
            }

            // Parse the list to save the modification
            foreach (SiteView item in elementSiteView)
            {
                Repository.Update(item);
            }

            // Save the Deletion for EF
            Repository.SaveChange();
        }

        /// <summary>
        /// Function to know if the view is associated to a site
        /// </summary>
        /// <param name="viewId">Id of the view</param>
        /// <returns>Return the status</returns>
        public bool ViewHasSite(int viewId)
        {
            return Repository.GetStandardQuery(BIA.Net.Model.DAL.AccessMode.All).Any(x => x.ViewId == viewId);
        }

        /// <summary>
        /// Function to retrieve the list of sites associated to a view
        /// </summary>
        /// <param name="viewId">Id of the view</param>
        /// <returns>Return the number of sites associated to the view</returns>
        public List<int> ListSiteForAView(int viewId)
        {
            return Repository.GetStandardQuery(BIA.Net.Model.DAL.AccessMode.All).Where(x => x.ViewId == viewId).Select(x => x.SiteId).Distinct().ToList();
        }

        /// <summary>
        /// Function to retrieve the list of sites associated to a view like is Default
        /// </summary>
        /// <param name="viewId">Id of the view</param>
        /// <returns>Return the number of sites associated to the view</returns>
        public List<int> ListSiteForAViewLikeDefault(int viewId)
        {
            return Repository.GetStandardQuery(BIA.Net.Model.DAL.AccessMode.All).Where(x => x.ViewId == viewId && x.IsDefault).Select(x => x.SiteId).Distinct().ToList();
        }
    }
}