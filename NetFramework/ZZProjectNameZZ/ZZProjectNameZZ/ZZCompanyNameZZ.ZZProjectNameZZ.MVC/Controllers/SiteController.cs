// <copyright file="SiteController.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace ZZCompanyNameZZ.ZZProjectNameZZ.MVC.Controllers
{
    using BIA.Net.Business.Services;
    using BIA.Net.Common;
    using BIA.Net.Common.Helpers;
    using BIA.Net.MVC;
    using BIA.Net.MVC.Filter;
    using Business.CTO;
    using Business.DTO;
    using Business.Services;
    using Common.Resources.BIA.Net;
    using MVC.ViewModel.Site;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using static BIA.Net.Business.Services.AllServicesDTO;
    using static Common.Constants;

    /// <summary>
    /// Controller for Site Pages
    /// </summary>
    /// <seealso cref="ZZCompanyNameZZ.ZZProjectNameZZ.MVC.Controllers.BaseController" />
    [Authorize(Roles = RoleAdmin + "," + RoleSiteAdmin)]
    public class SiteController : BaseController
    {
        /// <summary>
        /// the service site
        /// </summary>
        private readonly ServiceSite serviceSite;

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteController"/> class.
        /// </summary>
        public SiteController()
        {
            serviceSite = BIAUnity.Resolve<ServiceSite>();
        }

        /// <summary>
        /// Index View : list all Sites.
        /// </summary>
        /// <returns>ActionResult : Index View</returns>
        [Authorize(Roles = RoleAdmin + "," + RoleSiteAdmin)]
        public ActionResult Index()
        {
            // Return the list of the site with the appropriate filter
            return View(GetListData(null, true));
        }

        /// <summary>
        /// Filter View.
        /// </summary>
        /// <returns>ActionResult : Filter view</returns>
        public ActionResult AdvancedFilter()
        {
            return View(PrepareFilterRelatedLink(serviceSite.GetAll(ServiceAccessMode.Write).OrderBy(s => s.Title).ToList()));
        }

        /// <summary>
        /// _List Partial View.
        /// </summary>
        /// <param name="advancedFilter">Advanced Filter</param>
        /// <returns>ActionResult : _List partial view</returns>
        public ActionResult _List(SiteAdvancedFilterCTO advancedFilter)
        {
            return PartialView(GetListData(advancedFilter));
        }

        /// <summary>
        /// Function to retrieve the site with the filter panel associated
        /// </summary>
        /// <param name="advancedFilter">List of filter</param>
        /// <param name="useDefaultView">boolean to force default view</param>
        /// <returns>Return the list of the sites</returns>
        public IEnumerable<SiteDTO> GetListData(SiteAdvancedFilterCTO advancedFilter = null, bool useDefaultView = false)
        {
            PrepareView("SiteTable", ref advancedFilter, useDefaultView);
            List<SiteDTO> listDTO = AllServicesDTO.GetAdvancedFiltered<SiteDTO, SiteAdvancedFilterCTO>(advancedFilter, ServiceAccessMode.Write);
            return listDTO;
        }

        /// <summary>
        /// Details View.  (ex :Site>/Details/5)
        /// </summary>
        /// <param name="id">The id of the Site.</param>
        /// <returns>ActionResult : Details View</returns>
        [Authorize(Roles = RoleAdmin + "," + RoleSiteAdmin)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SiteDTO siteDTO = serviceSite.Find(id);
            if (siteDTO == null)
            {
                return HttpNotFound();
            }

            return View("Details", BIASettingsReader.GetDialogLayout("PopupInfos"), siteDTO);
        }

        /// <summary>
        /// Create View for a Site  (ex : Site/Create)
        /// </summary>
        /// <returns>ActionResult : Create View</returns>
        [Authorize(Roles = RoleAdmin)]
        public ActionResult Create()
        {
            PrepareRelatedLink();
            return View();
        }

        /// <summary>
        /// Creates the specified site.
        /// </summary>
        /// <param name="siteDTO">The site.</param>
        /// <returns>ActionResult : Create View or Index View if success</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        [Authorize(Roles = RoleAdmin)]
        public ActionResult Create([Bind(Include = "Id,Title,MembersIds")] SiteDTO siteDTO)
        {
            if (ModelState.IsValid)
            {
                serviceSite.Insert(siteDTO);
                return RedirectToAction("Index").Success(TextResources.CreatedSuccessfully);
            }

            PrepareRelatedLink();
            return View(siteDTO);
        }

        /// <summary>
        /// Edit View. (ex : Site/Edit/5)
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult : Edit view</returns>
        [Authorize(Roles = RoleAdmin + "," + RoleSiteAdmin)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SiteDTO siteDTO = serviceSite.Find(id);
            if (siteDTO == null)
            {
                return HttpNotFound();
            }

            PrepareRelatedLink();
            return View("Edit", BIASettingsReader.GetDialogLayout("PopupInfos"), siteDTO);
        }

        /// <summary>
        /// Edits the specified site.
        /// </summary>
        /// <param name="siteDTO">The site.</param>
        /// <returns>ActionResult : Edit view or Index View if success</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        [Authorize(Roles = RoleAdmin + "," + RoleSiteAdmin)]
        public ActionResult Edit([Bind(Include = "Id,Title")] SiteDTO siteDTO)
        {
            if (ModelState.IsValid)
            {
                serviceSite.UpdateValues(siteDTO, new List<string>() { nameof(SiteDTO.Title) });
                return RedirectToAction("Index").Success(TextResources.UpdatedSuccessfully);
            }

            PrepareRelatedLink();
            return View("Edit", BIASettingsReader.GetDialogLayout("PopupInfos"), siteDTO);
        }

        /// <summary>
        /// Delete View. (ex : Site/Delete/5)
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult : Delete view</returns>
        [Authorize(Roles = RoleAdmin)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SiteDTO siteDTO = serviceSite.Find(id);
            if (siteDTO == null)
            {
                return HttpNotFound();
            }

            return View("Delete", BIASettingsReader.GetDialogLayout("PopupInfos"), siteDTO);
        }

        /// <summary>
        /// Deletes the specified site.
        /// </summary>
        /// <param name="id">The id of the site.</param>
        /// <returns>ActionResult : Delete view or Index View if success</returns>
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        [Authorize(Roles = RoleAdmin)]
        public ActionResult DeleteConfirmed(int id)
        {
            serviceSite.DeleteById(id);
            return RedirectToAction("Index").Success(TextResources.DeletedSuccessfully);
        }

        /// <summary>
        /// Releases unmanaged resources and optionally releases managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        /// <summary>
        /// Prepares the related link.
        /// </summary>
        private void PrepareRelatedLink()
        {
        }

        /// <summary>
        /// Prepares the related link.
        /// </summary>
        /// <param name="listSiteDTO">list of site</param>
        /// <returns>the filter view Model</returns>
        private SiteAdvancedFilterVM PrepareFilterRelatedLink(List<SiteDTO> listSiteDTO)
        {
            SiteAdvancedFilterVM filterVM = new SiteAdvancedFilterVM
            {
                MslMember = new MultiSelectList(
                    listSiteDTO.SelectMany(s => s.Members.Select(m => m.DisplayName)).Distinct().ToList()),

                MslTitle = new MultiSelectList(
                    listSiteDTO.Select(s => s.Title).Distinct().ToList()),
            };
            return filterVM;
        }
    }
}