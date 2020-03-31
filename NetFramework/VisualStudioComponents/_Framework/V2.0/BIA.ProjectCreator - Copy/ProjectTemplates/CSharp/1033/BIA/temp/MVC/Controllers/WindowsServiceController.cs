namespace $safeprojectname$.Controllers
{
    using BIA.Net.Common;
    using BIA.Net.Common.Helpers;
    using BIA.Net.MVC.Filter;
    using Common;
    using System;
    using System.Reflection;
    using System.Web.Mvc;

    /// <summary>
    /// WindowsService Controller
    /// </summary>
    [Authorize(Roles = Constants.RoleAdmin)]
    public class WindowsServiceController : BaseController
    {
        /// <summary>
        /// Gets class Name
        /// </summary>
        private static string ClassName
        {
            get { return typeof(WindowsServiceController).Name; }
        }

        /// <summary>
        /// return view index
        /// </summary>
        /// <returns>view index</returns>
        public ActionResult Index()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            ViewModel.WindowsService.IndexVM vm = new ViewModel.WindowsService.IndexVM();

            try
            {
                vm.Status = WindowsServiceHelper.GetStatus(AppSettingsReader.WindowsServiceName);
            }
            catch (Exception ex)
            {
                string errorMsg = "Error retrieving service status.";
                TraceManager.Error(ClassName, methodName, errorMsg, ex);
                vm.Status = errorMsg;
            }

            return View(vm);
        }

        /// <summary>
        /// Stop the Windows Service
        /// </summary>
        /// <returns>view index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public ActionResult StopService()
        {
            WindowsServiceHelper.Stop(AppSettingsReader.WindowsServiceName);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Start the Windows Service
        /// </summary>
        /// <returns>view index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public ActionResult StartService()
        {
            WindowsServiceHelper.Start(AppSettingsReader.WindowsServiceName);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Restart the Windows Service
        /// </summary>
        /// <returns>view index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public ActionResult RestartService()
        {
            WindowsServiceHelper.Restart(AppSettingsReader.WindowsServiceName);
            return RedirectToAction("Index");
        }
    }
}