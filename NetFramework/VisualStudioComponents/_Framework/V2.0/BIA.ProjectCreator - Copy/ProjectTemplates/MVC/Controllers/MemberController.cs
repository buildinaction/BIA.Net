// <copyright file="MemberController.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace $safeprojectname$.Controllers
{
    using BIA.Net.Authentication.MVC;
    using BIA.Net.Business.Services;
    using BIA.Net.Common.Helpers;
    using BIA.Net.MVC;
    using BIA.Net.MVC.Filter;
    using BIA.Net.MVC.Utility;
    using Business.DTO;
    using Business.Services;
    using Common;
    using MVC.Helpers;
    using MVC.ViewModel.Member;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using static BIA.Net.Business.Services.AllServicesDTO;

    /// <summary>
    /// Controller for Member Pages
    /// </summary>
    /// <seealso cref="$safeprojectname$.Controllers.BaseController" />
    [Authorize(Roles = Constants.RoleAdmin + "," + Constants.RoleSiteAdmin)]
    public class MemberController : BaseController
    {
        /// <summary>
        /// the service member
        /// </summary>
        private ServiceMember serviceMember;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberController"/> class.
        /// </summary>
        public MemberController()
        {
            this.serviceMember = BIAUnity.Resolve<ServiceMember>();
        }

        /// <summary>
        /// Index View : list all Members.
        /// </summary>
        /// <returns>ActionResult : Index View</returns>
        public ActionResult Index()
        {
            MemberIndexVM memberIndexVM = new MemberIndexVM
            {
                ListMemberDTO = serviceMember.GetAll(ServiceAccessMode.Write).OrderBy(s => s.User.DisplayName).ToList()
            };
            PrepareFilterRelatedLink(memberIndexVM);
            return View(memberIndexVM);
        }

        /// <summary>
        /// _List Partial View.
        /// </summary>
        /// <param name="filterMember">members selected in filter</param>
        /// <param name="filterMemberRole">member roles selected in filter</param>
        /// <param name="siteId">The site identifier.</param>
        /// <returns>ActionResult : _List partial view</returns>
        public ActionResult _List(ICollection<string> filterMember, ICollection<string> filterMemberRole, int? siteId)
        {
            IEnumerable<MemberDTO> listMemberDTO = serviceMember.GetAllFromSite(siteId).OrderBy(m => m.DisplayName);
            if (filterMember != null || filterMemberRole != null)
            {
                if (filterMemberRole != null)
                {
                    listMemberDTO = listMemberDTO.Where(m => m.MemberRole.Any(mr => filterMemberRole.Contains(mr.Title)));
                }

                if (filterMember != null)
                {
                    listMemberDTO = listMemberDTO.Where(m => filterMember.Contains(m.DisplayName));
                }
            }

            return PartialView(listMemberDTO);
        }

        /// <summary>
        /// _List Partial View.
        /// </summary>
        /// <returns>ActionResult : _List partial view</returns>
        public ActionResult _Filter()
        {
            MemberIndexVM memberIndexVM = new MemberIndexVM
            {
                ListMemberDTO = serviceMember.GetAll(ServiceAccessMode.Write).OrderBy(s => s.User.DisplayName).ToList()
            };
            PrepareFilterRelatedLink(memberIndexVM);
            return PartialView(memberIndexVM.Filter);
        }

        /// <summary>
        /// Details View.
        /// </summary>
        /// <param name="id">The id of the Member.</param>
        /// <returns>ActionResult : Details View</returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MemberDTO memberDTO = serviceMember.Find(id);
            if (memberDTO == null)
            {
                return HttpNotFound();
            }

            return View(memberDTO);
        }

        /// <summary>
        /// Create View for a Member.
        /// </summary>
        /// <param name="siteId">The site identifier.</param>
        /// <returns>ActionResult : Create View</returns>
        public ActionResult Create(int siteId)
        {
            PrepareRelatedLink(siteId);
            MemberDTO memberDTO = new MemberDTO
            {
                SiteId = siteId
            };
            return View(memberDTO);
        }

        /// <summary>
        /// Creates the specified member.
        /// </summary>
        /// <param name="memberDTO">The member.</param>
        /// <returns>ActionResult : Create View or Site Edit view if success</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public ActionResult Create([Bind(Include = "Id,UserId,MemberRoleIds,SiteId")] MemberDTO memberDTO)
        {
            if (ModelState.IsValid)
            {
                MemberDTO memberCreated = serviceMember.Insert(memberDTO);
                RefreshRolesAnotherConnectedMember(memberCreated);
                return RedirectToAction("CloseDialog", "DialogBasicAction", null).Success("Create was successful");
            }

            PrepareRelatedLink(memberDTO.SiteId);
            return View(memberDTO);
        }

        /// <summary>
        /// Edit View.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult : Edit view</returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MemberDTO memberDTO = serviceMember.Find(id);
            if (memberDTO == null)
            {
                return HttpNotFound();
            }

            PrepareRelatedLink(memberDTO.SiteId, memberDTO.Id);
            return View(memberDTO);
        }

        /// <summary>
        /// Edits the specified member.
        /// </summary>
        /// <param name="memberDTO">The member.</param>
        /// <returns>ActionResult : Edit view or Site Edit if success</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public ActionResult Edit([Bind(Include = "Id,SiteId,UserId,MemberRoleIds")] MemberDTO memberDTO)
        {
            ValidatorUtility.ValidOnly(ModelState, "Id,MemberRole");
            if (ModelState.IsValid)
            {
                MemberDTO memberUpdated = serviceMember.UpdateValues(memberDTO, new List<string>() { nameof(MemberDTO.User), nameof(MemberDTO.MemberRole) });
                RefreshRolesAnotherConnectedMember(memberUpdated);
                return RedirectToAction("CloseDialog", "DialogBasicAction", null).Success("Edit was successful");
            }

            PrepareRelatedLink(null);
            return View(memberDTO);
        }

        /// <summary>
        /// Delete View.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult : Delete view</returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MemberDTO memberDTO = serviceMember.Find(id);
            if (memberDTO == null)
            {
                return HttpNotFound();
            }

            return View(memberDTO);
        }

        /// <summary>
        /// Deletes the specified member.
        /// </summary>
        /// <param name="id">The id of the member.</param>
        /// <returns>ActionResult : Delete view or Site Edit if success</returns>
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public ActionResult DeleteConfirmed(int id)
        {
            MemberDTO memberDTO = serviceMember.Find(id);
            if (memberDTO == null)
            {
                return HttpNotFound();
            }

            RefreshRolesAnotherConnectedMember(memberDTO);
            serviceMember.DeleteById(id);
            return RedirectToAction("CloseDialog", "DialogBasicAction", null).Success("Delete was successful");
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
        /// <param name="excludeSiteId">The exclude site identifier.</param>
        /// <param name="memberId">The id member to include in filter</param>
        private void PrepareRelatedLink(int? excludeSiteId, int? memberId = null)
        {
            if (excludeSiteId != null)
            {
                ViewBag.UserDTOId = new SelectList(
                    ((ServiceUser)AllServicesDTO.GetService<UserDTO>()).GetAllExcludeSite(excludeSiteId, memberId).OrderBy(s => s.DisplayFullName),
                    nameof(UserDTO.Id),
                    nameof(UserDTO.DisplayFullName));
            }

            ViewBag.MemberRoleDTOIds = new MultiSelectList(
                AllServicesDTO.GetAll<MemberRoleDTO>(),
                nameof(MemberRoleDTO.Id),
                nameof(MemberRoleDTO.Title));
        }

        /// <summary>
        /// Call this function if the action of the user impact another member right or properties stocked in UserInfo.
        /// </summary>
        /// <param name="memberCreated">The member.</param>
        private void RefreshRolesAnotherConnectedMember(MemberDTO memberCreated)
        {
            if (memberCreated.User != null)
            {
                BIAAuthorizationFilterMVC<UserInfoMVC, UserDTO>.RefreshUserRoles(memberCreated.User.Login);
            }
            else
            {
                UserDTO userDTO = ((ServiceUser)AllServicesDTO.GetService<UserDTO>()).GetByMemberId(memberCreated.Id);
                BIAAuthorizationFilterMVC<UserInfoMVC, UserDTO>.RefreshUserRoles(userDTO.Login);
            }
        }

        /// <summary>
        /// Prepares the related link.
        /// </summary>
        /// <param name="memberIndexVM">site index VM</param>
        private void PrepareFilterRelatedLink(MemberIndexVM memberIndexVM)
        {
            memberIndexVM.Filter = new MemberFilterVM
            {
                MslMember = new MultiSelectList(
                    memberIndexVM.ListMemberDTO.Select(m => m.DisplayName).Distinct()),

                MslMemberRole = new MultiSelectList(
                    memberIndexVM.ListMemberDTO.SelectMany(m => m.MemberRole.Select(mr => mr.Title)).Distinct().ToList())
            };
        }
    }
}