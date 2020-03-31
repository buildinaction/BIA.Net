// <copyright file="HomeController.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace ZZCompanyNameZZ.ZZProjectNameZZ.MVC.Controllers
{
    using BIA.Net.Business.Services;
    using BIA.Net.Common;
    using BIA.Net.Common.Helpers;
    using BIA.Net.Helpers;
    using BIA.Net.MVC;
    using BIA.Net.MVC.Filter;
    using Business.DTO;
    using Business.Services;
    using Model;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Controller for Home Pages
    /// </summary>
    /// <seealso cref="ZZCompanyNameZZ.ZZProjectNameZZ.MVC.Controllers.BaseController" />
    public class ExampleController : BaseController
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>The index ActionResut</returns>
        public ActionResult Index()
        {
            // GetExampleCallStoredProcedure();
            return View();
        }

        /// <summary>
        /// Example page.
        /// </summary>
        /// <returns>The Example page ActionResut</returns>
        public ActionResult ExamplePage()
        {
            TraceManager.Debug("ExamplePage opened");
            ViewBag.MembersIds = new MultiSelectList(
                AllServicesDTO.GetAll<MemberDTO>(),
                nameof(MemberDTO.Id),
                nameof(MemberDTO.DisplayName));
            return View(AllServicesDTO.GetAllWhere<SiteDTO, Site>(s => s.Id == 1).FirstOrDefault()).Warning("Your are on an Example Page");
        }

        /// <summary>
        /// Example page.
        /// </summary>
        /// <returns>The Example page ActionResut</returns>
        public ActionResult ExamplePageReadOnly()
        {
            TraceManager.Debug("ExamplePageReadOnly opened");
            ViewBag.MembersIds = new MultiSelectList(
                AllServicesDTO.GetAll<MemberDTO>(),
                nameof(MemberDTO.Id),
                nameof(MemberDTO.DisplayName));
            return View(AllServicesDTO.GetAllWhere<SiteDTO, Site>(s => s.Id == 1).FirstOrDefault()).Danger("Example of error message");
        }

        /// <summary>
        /// Example page.
        /// </summary>
        /// <returns>The Example page ActionResut</returns>
        public ActionResult ExamplePage2()
        {
            TraceManager.Debug("ExamplePage2 opened");
            ViewBag.MembersIds = new MultiSelectList(
                AllServicesDTO.GetAll<MemberDTO>(),
                nameof(MemberDTO.Id),
                nameof(MemberDTO.DisplayName));
            return View(AllServicesDTO.GetAllWhere<SiteDTO, Site>(s => s.Id == 1).FirstOrDefault()).Information("Your are on a second example Page");
        }

        /// <summary>
        /// Tabs page.
        /// </summary>
        /// <returns>The Example page ActionResut</returns>
        public ActionResult Tabs()
        {
            ViewBag.MembersIds = new MultiSelectList(
                AllServicesDTO.GetAll<MemberDTO>(),
                nameof(MemberDTO.Id),
                nameof(MemberDTO.DisplayName));
            return View(AllServicesDTO.GetAllWhere<SiteDTO, Site>(s => s.Id == 1).FirstOrDefault()).Information("Your are on the tabs example Page");
        }

        /// <summary>
        /// Tabs Ajax page.
        /// </summary>
        /// <returns>The Example page ActionResut</returns>
        public ActionResult TabsAjax()
        {
            return View();
        }

        /// <summary>
        /// Tabs Ajax page.
        /// </summary>
        /// <returns>The Example page ActionResut</returns>
        public ActionResult _Tab1()
        {
            return View().Information("Tab 1 loaded");
        }

        /// <summary>
        /// Tabs Ajax page.
        /// </summary>
        /// <returns>The Example page ActionResut</returns>
        public ActionResult _Tab2()
        {
            ViewBag.MembersIds = new MultiSelectList(
                AllServicesDTO.GetAll<MemberDTO>(),
                nameof(MemberDTO.Id),
                nameof(MemberDTO.DisplayName));
            return View(AllServicesDTO.GetAllWhere<SiteDTO, Site>(s => s.Id == 1).FirstOrDefault()).Information("Tab 2 loaded");
        }

        /// <summary>
        /// Example page Cascading
        /// </summary>
        /// <returns>The Example page Cascading ActionResut</returns>
        public ActionResult ExamplePageCascading()
        {
            ViewBag.SiteId = new SelectList(
                AllServicesDTO.GetAll<SiteDTO>().OrderBy(x => x.Title).ToList(),
                nameof(SiteDTO.Id),
                nameof(SiteDTO.Title));

            ViewBag.MemberId = AllServicesDTO.GetAll<MemberDTO>().OrderBy(x => x.DisplayName).ToList()
                .Select(m => new CascadingSelectListItem(
                    m.Id.ToString(), // change here the key value to use in child list
                    m.DisplayName, // change here the display value to use in child list
                    new Dictionary<string, List<string>>() { { nameof(m.SiteId), new List<string>() { m.SiteId.ToString() } } })).ToList();

            return View(new MemberDTO());
        }


        /// <summary>
        /// Example page Cascading
        /// </summary>
        /// <returns>The Example page Cascading ActionResut</returns>
        public ActionResult ExamplePageCascadingReadOnly()
        {
            ViewBag.SiteId = new SelectList(
                AllServicesDTO.GetAll<SiteDTO>().OrderBy(x => x.Title).ToList(),
                nameof(SiteDTO.Id),
                nameof(SiteDTO.Title));

            ViewBag.MemberId = AllServicesDTO.GetAll<MemberDTO>().OrderBy(x => x.DisplayName).ToList()
                .Select(m => new CascadingSelectListItem(
                    m.Id.ToString(), // change here the key value to use in child list
                    m.DisplayName, // change here the display value to use in child list
                    new Dictionary<string, List<string>>() { { nameof(m.SiteId), new List<string>() { m.SiteId.ToString() } } })).ToList();

            return View(new MemberDTO());
        }

        /// <summary>
        /// Example page.
        /// </summary>
        /// <returns>The Example page ActionResut</returns>
        public ActionResult ExamplePageIconBefore()
        {
            TraceManager.Debug("ExamplePageIconBefore opened");
            ViewBag.MembersIds = new MultiSelectList(
                AllServicesDTO.GetAll<MemberDTO>(),
                nameof(MemberDTO.Id),
                nameof(MemberDTO.DisplayName));
            return View(AllServicesDTO.GetAllWhere<SiteDTO, Site>(s => s.Id == 1).FirstOrDefault());
        }

        /// <summary>
        /// Example page.
        /// </summary>
        /// <returns>The Example page ActionResut</returns>
        public ActionResult FullScreen()
        {
            TraceManager.Debug("ExamplePage in fullScreen");
            ViewBag.MembersIds = new MultiSelectList(
                AllServicesDTO.GetAll<MemberDTO>(),
                nameof(MemberDTO.Id),
                nameof(MemberDTO.DisplayName));
            return View(AllServicesDTO.GetAllWhere<SiteDTO, Site>(s => s.Id == 1).FirstOrDefault());
        }

        /// <summary>
        /// Example page.
        /// </summary>
        /// <returns>The Example page ActionResut</returns>
        public ActionResult Error()
        {
            MemberDTO memberNull = null;
            memberNull.Id = 2;
            throw new Exception("Error in example controller.");
        }

        /// <summary>
        /// Example to upload a PrintScreen
        /// </summary>
        /// <returns>View UploadScreenCapture</returns>
        [HttpGet]
        public ActionResult UploadScreenCapture()
        {
            return View();
        }

        /// <summary>
        /// Example post PrintScreen
        /// </summary>
        /// <param name="printScreenData">printScreen data base64</param>
        /// <returns>View UploadScreenCapture</returns>
        [HttpPost]
        public ActionResult UploadScreenCapture(string printScreenData)
        {
            /* This code is not secure
            if (!string.IsNullOrWhiteSpace(printScreenData))
            {
                string[] imageTab = printScreenData.Split(',');

                if (imageTab != null && imageTab.Length == 2)
                {
                    byte[] imageBytes = Convert.FromBase64String(imageTab[1]);

                    if (imageBytes != null)
                    {
                        using (FileStream fileStream = System.IO.File.Create(@"D:\ScreenCapture.png"))
                        {
                            fileStream.Write(imageBytes, 0, imageBytes.Length);
                            ViewBag.message = "Capture uploaded. It is on the serveur at path D:\\ScreenCapture.png";
                        }
                    }
                }
            }
            */

            return View();
        }

        /// <summary>
        /// Example Dialog
        /// </summary>
        /// <returns>View Dialog</returns>
        [HttpGet]
        public ActionResult Dialog()
        {
            return View();
        }

        /// <summary>
        /// Example Dialog parent Popup
        /// </summary>
        /// <returns>View Dialog Parent popup</returns>
        [HttpGet]
        public ActionResult DialogParentPopup()
        {
            return View("DialogParentPopup", BIA.Net.Common.BIASettingsReader.GetDialogLayout("PopupInfos"));
        }

        /// <summary>
        /// Example Dialog parent Popup
        /// </summary>
        /// <param name="actionJS">action name to send to js event</param>
        /// <returns>Close the dialog after the submit of the form</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public ActionResult DialogParentPopup(string actionJS)
        {
            System.Threading.Thread.Sleep(1000);
            return RedirectToAction("CloseDialog", "DialogBasicAction");
        }

        /// <summary>
        /// Example Dialog child Popup
        /// </summary>
        /// <returns>View Dialog Child popup</returns>
        [HttpGet]
        public ActionResult DialogChildPopup()
        {
            return View();
        }

        /// <summary>
        /// Example Dialog child Popup
        /// </summary>
        /// <param name="action">action name to send to js event</param>
        /// <returns>View Dialog Child popup</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public ActionResult DialogChildPopup(string action)
        {
            if (action == "SubmitAndClose")
            {
                return RedirectToAction("CloseDialog", "DialogBasicAction", null);
            }

            if (action == "SubmitAndCloseParent" || action == "SubmitAndSubmitParent" || action == "SubmitAndActionParent")
            {
                return RedirectToAction("SendEvent", "DialogBasicAction", new { eventName = "DialogChildPopupAction", eventData = action });
            }

            ModelState.AddModelError("action", "Action " + HttpUtility.HtmlEncode(action) + " not implemented");
            return View();
        }

        /// <summary>
        /// Example to upload a simple file
        /// </summary>
        /// <returns>the view</returns>
        [HttpGet]
        public ActionResult BasicFileUpload()
        {
            return View();
        }

        /// <summary>
        /// Example to upload a simple file
        /// </summary>
        /// <param name="file">file to save</param>
        /// <returns>the view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public ActionResult BasicFileUpload(HttpPostedFileBase file)
        {
            string error;
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    string path = Path.Combine(
                        Server.MapPath("~/Images"),
                        Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    return View().Success("File uploaded successfully");
                }
                catch (Exception ex)
                {
                    error = "ERROR:" + ex.Message.ToString();
                }
            }
            else
            {
                error = "You have not specified a file.";
            }

            return View().Danger(error);
        }

        /// <summary>
        /// Example to upload several files
        /// </summary>
        /// <returns>the view</returns>
        [HttpGet]
        public ActionResult MultiFilesUpload()
        {
            return View();
        }

        /// <summary>
        /// Example to upload several files
        /// </summary>
        /// <param name="files">the files to save</param>
        /// <returns>the view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public ActionResult MultiFilesUpload(IEnumerable<HttpPostedFileBase> files)
        {
            string error = string.Empty;
            string filesUploaded = string.Empty;
            if (files != null && files.Count() > 0)
            {
                foreach (HttpPostedFileBase file in files.Where(x => x != null).ToList())
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        try
                        {
                            string path = Path.Combine(
                                Server.MapPath("~/Images"),
                                Path.GetFileName(file.FileName));
                            file.SaveAs(path);
                            if (!string.IsNullOrEmpty(filesUploaded))
                            {
                                filesUploaded += ", ";
                            }

                            filesUploaded += Path.GetFileName(file.FileName);
                        }
                        catch (Exception ex)
                        {
                            error = "ERROR:" + ex.Message.ToString();
                        }
                    }
                }

                if (string.IsNullOrEmpty(error))
                {
                    View().Success("Files " + filesUploaded + " uploaded successfully");
                }
            }
            else
            {
                error = "You have not specified a file.";
            }

            return View().Danger(error);
        }

        /// <summary>
        /// Example call stored procedure
        /// </summary>
        /// <returns>view list</returns>
        [HttpGet]
        public ActionResult CallStoredProcedure()
        {
            ServiceExampleForStoredProcedure srv = BIAUnity.Resolve<ServiceExampleForStoredProcedure>();
            List<UserDTO> users = srv.GetExampleUsersByCompany("LABINAL");

            return View(users);
        }
    }
}