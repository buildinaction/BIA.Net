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
    }
}