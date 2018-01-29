namespace BIA.Net.ImageManager.Controllers
{
    using BIA.Net.MVC.Filter;
    using System;
    using System.IO;
    using System.Net;
    using System.Web.Mvc;
    using ViewModel;

    /// <summary>
    /// Image Controller
    /// </summary>
    public class ImageController : Controller
    {
        /// <summary>
        /// Edit Tool Image
        /// </summary>
        /// <param name="entityName">entity name</param>
        /// <param name="id">entity id</param>
        /// <returns>ActionResult : EditImage View</returns>
        [HttpGet]
        public ActionResult Edit(string entityName, int id)
        {
            if (id > 0 && !string.IsNullOrEmpty(entityName))
            {
                UploadFileVM vm = new UploadFileVM();

                vm.EntityId = id;
                vm.EntityName = entityName;

                return View(vm);
            }
            else
            {
                return RedirectToAction("Index", entityName);
            }
        }

        /// <summary>
        /// Edit Tool Image
        /// </summary>
        /// <param name="vm">view model</param>
        /// <returns>ActionResult : EditImage View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public ActionResult Edit(UploadFileVM vm)
        {
            if (vm.UploadFile != null && vm.UploadFile.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            {
                this.FileUpload(vm);
            }

            return View(vm);
        }

        /// <summary>
        /// Delete Tool Image
        /// </summary>
        /// <param name="vm">view model</param>
        /// <returns>ActionResult : DeleteImage View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public ActionResult Delete(UploadFileVM vm)
        {
            if (vm != null && vm.EntityId > 0)
            {
                Services.ServiceUploadFile.Delete(GetImagePath(vm.EntityName, vm.EntityId));
            }

            return View("Edit", vm);
        }

        /// <summary>
        /// Return PartialView Image
        /// </summary>
        /// <param name="entityName">entity name</param>
        /// <param name="id">entity id</param>
        /// <returns>ActionResult : PartialView Image</returns>
        public ActionResult _Details(string entityName, int id)
        {
            if (id < 1 || string.IsNullOrWhiteSpace(entityName))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DTO.FileDTO image = Services.ServiceUploadFile.GetUploadedFile(GetImagePath(entityName, id));

            return PartialView("_Details", image);
        }

        /// <summary>
        /// Saves an file for an entity.
        /// </summary>
        /// <param name="vm">ViewModel <see cref="UploadFileVM"/></param>
        private void FileUpload(UploadFileVM vm)
        {
            if (vm != null && vm.UploadFile != null && vm.UploadFile.ContentLength > 0)
            {
                byte[] fileData = null;
                using (var binaryReader = new BinaryReader(vm.UploadFile.InputStream))
                {
                    fileData = binaryReader.ReadBytes(vm.UploadFile.ContentLength);
                }

                DTO.FileDTO fileDTO = new DTO.FileDTO();

                fileDTO.Binary = fileData;
                fileDTO.ContentType = vm.UploadFile.ContentType;
                fileDTO.Name = vm.UploadFile.FileName;

                Services.ServiceUploadFile.UploadImage(GetImagePath(vm.EntityName, vm.EntityId), fileDTO);
            }
        }

        /// <summary>
        /// Returns the physical path where the images are stored.
        /// </summary>
        /// <param name="entityName">entity Name</param>
        /// <param name="entityId">entity Id</param>
        /// <returns>the physical path</returns>
        private string GetImagePath(string entityName, int entityId)
        {
            string path = null;

            if (!string.IsNullOrEmpty(Common.AppSettingsReader.ProjectImagesPath))
            {
                path = string.Format("{0}/{1}/{2}", Common.AppSettingsReader.ProjectImagesPath, entityName, entityId);

                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                if (directoryInfo != null && directoryInfo.Exists)
                {
                    path = directoryInfo.FullName;
                }
                else
                {
                    path = Server.MapPath(path);
                }
            }
            else
            {
                throw new Exception("AppSettings 'ProjectImagesPath' is null or empty");
            }

            return path;
        }
    }
}