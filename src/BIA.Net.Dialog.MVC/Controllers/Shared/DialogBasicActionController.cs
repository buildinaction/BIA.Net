using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BIA.Net.Dialog.MVC.Controllers
{
    public class DialogBasicActionController : Controller
    {
        // GET: Dialog
        public ActionResult CloseDialog()
        {
            return Content("Close Dialog");
        }
        // GET: Dialog
        public ActionResult CloseParentDialog()
        {
            return Content("Close Parent Dialog");
        }
        // GET: Dialog
        public ActionResult ActionDialog(string action)
        {
            return Content("Action Dialog:" + action);
        }
    }
}