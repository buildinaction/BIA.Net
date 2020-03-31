// <copyright file="SiteController.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace $safeprojectname$.Controllers
{
    using BIA.Net.Common.Helpers;
    using BIA.Net.MVC;
    using BIA.Net.MVC.Filter;
    using Business.DTO;
    using Business.Services;
    using Common;
    using MVC.ViewModel.Site;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using static BIA.Net.Business.Services.AllServicesDTO;

    /// <summary>
    /// Controller for Site Pages
    /// </summary>
    /// <seealso cref="$safeprojectname$.Controllers.BaseController" />
    [Authorize(Roles = Constants.RoleAdmin + "," + Constants.RoleSiteAdmin)]
    public class SiteController : BaseController
    {
        /// <summary>
        /// the service site
        /// </summary>
        private ServiceSite serviceSite;

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteController"/> class.
        /// </summary>
        public SiteController()
        {
            this.serviceSite = BIAUnity.Resolve<ServiceSite>();
        }

        /// <summary>
        /// Index View : list all Sites.
        /// </summary>
        /// <returns>ActionResult : Index View</returns>
        [Authorize(Roles = Constants.RoleAdmin + "," + Constants.RoleSiteAdmin)]
        public ActionResult Index()
        {
            SiteIndexVM siteIndexVM = new SiteIndexVM
            {
                ListSiteDTO = serviceSite.GetAll(ServiceAccessMode.Write).OrderBy(s => s.Title).ToList()
            };
            PrepareFilterRelatedLink(siteIndexVM);
            return View(siteIndexVM);
        }

        /// <summary>
        /// _List Partial View.
        /// </summary>
        /// <param name="filterTitle">sites selected in filter</param>
        /// <param name="filterMember">members selected in filter</param>
        /// <returns>ActionResult : _List partial view</returns>
        public ActionResult _List(ICollection<string> filterTitle, ICollection<string> filterMember)
        {
            IEnumerable<SiteDTO> listSiteDTO = serviceSite.GetAll(ServiceAccessMode.Write).OrderBy(s => s.Title);
            if (filterMember != null || filterTitle != null)
            {
                if (filterMember != null)
                {
                    listSiteDTO = listSiteDTO.Where(s => s.Members.Any(m => filterMember.Contains(m.DisplayName)));
                }

                if (filterTitle != null)
                {
                    listSiteDTO = listSiteDTO.Where(s => filterTitle.Contains(s.Title));
                }
            }

            return PartialView(listSiteDTO);
        }

        /// <summary>
        /// _List Partial View.
        /// </summary>
        /// <returns>ActionResult : _List partial view</returns>
        public ActionResult _Filter()
        {
            SiteIndexVM siteIndexVM = new SiteIndexVM
            {
                ListSiteDTO = serviceSite.GetAll(ServiceAccessMode.Write).OrderBy(s => s.Title).ToList()
            };
            PrepareFilterRelatedLink(siteIndexVM);
            return PartialView(siteIndexVM.Filter);
        }

        /// <summary>
        /// Details View.  (ex :Site>/Details/5)
        /// </summary>
        /// <param name="id">The id of the Site.</param>
        /// <returns>ActionResult : Details View</returns>
        [Authorize(Roles = Constants.RoleAdmin + "," + Constants.RoleSiteAdmin)]
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

            return View("Details", BIA.Net.Common.BIASettingsReader.GetDialogLayout("PopupInfos"), siteDTO);
        }

        /// <summary>
        /// Create View for a Site  (ex : Site/Create)
        /// </summary>
        /// <returns>ActionResult : Create View</returns>
        [Authorize(Roles = Constants.RoleAdmin)]
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
        [Authorize(Roles = Constants.RoleAdmin)]
        public ActionResult Create([Bind(Include = "Id,Title,MembersIds")] SiteDTO siteDTO)
        {
            if (ModelState.IsValid)
            {
                serviceSite.Insert(siteDTO);
                return RedirectToAction("Index").Success("Create was successful");
            }

            PrepareRelatedLink();
            return View(siteDTO);
        }

        /// <summary>
        /// Edit View. (ex : Site/Edit/5)
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult : Edit view</returns>
        [Authorize(Roles = Constants.RoleAdmin + "," + Constants.RoleSiteAdmin)]
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
            return View("Edit", BIA.Net.Common.BIASettingsReader.GetDialogLayout("PopupInfos"), siteDTO);
        }

        /// <summary>
        /// Edits the specified site.
        /// </summary>
        /// <param name="siteDTO">The site.</param>
        /// <returns>ActionResult : Edit view or Index View if success</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        [Authorize(Roles = Constants.RoleAdmin + "," + Constants.RoleSiteAdmin)]
        public ActionResult Edit([Bind(Include = "Id,Title")] SiteDTO siteDTO)
        {
            if (ModelState.IsValid)
            {
                serviceSite.UpdateValues(siteDTO, new List<string>() { nameof(SiteDTO.Title) });
                return RedirectToAction("Index").Success("Edit was successful");
            }

            PrepareRelatedLink();
            return View("Edit", BIA.Net.Common.BIASettingsReader.GetDialogLayout("PopupInfos"), siteDTO);
        }

        /// <summary>
        /// Delete View. (ex : Site/Delete/5)
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult : Delete view</returns>
        [Authorize(Roles = Constants.RoleAdmin)]
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

            return View("Delete", BIA.Net.Common.BIASettingsReader.GetDialogLayout("PopupInfos"), siteDTO);
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
        [Authorize(Roles = Constants.RoleAdmin)]
        public ActionResult DeleteConfirmed(int id)
        {
            serviceSite.DeleteById(id);
            return RedirectToAction("Index").Success("Delete was successful");
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
        /// <param name="siteIndexVM">site index VM</param>
        private void PrepareFilterRelatedLink(SiteIndexVM siteIndexVM)
        {
            siteIndexVM.Filter = new SiteFilterVM
            {
                MslMember = new MultiSelectList(
                    siteIndexVM.ListSiteDTO.SelectMany(s => s.Members.Select(m => m.DisplayName)).Distinct().ToList()),

                MslTitle = new MultiSelectList(
                    siteIndexVM.ListSiteDTO.Select(s => s.Title).Distinct().ToList())
            };
        }
    }
}