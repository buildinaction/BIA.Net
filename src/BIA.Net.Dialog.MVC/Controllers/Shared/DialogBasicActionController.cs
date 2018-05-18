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
            return SendEvent("BIA.Net.Dialog.Close", null);
        }
        // GET: Dialog
        public ActionResult CloseParentDialog()
        {
            return SendEvent("BIA.Net.Dialog.CloseParent", null);
        }

        private class DialogEventContainer
        {
            public bool IsBiaNetDialogEvent = true;
            public string EventName;
            public object EventData;
        }

        public JsonResult SendEvent(string eventName, object eventData)
        {
            DialogEventContainer evtContainer = new DialogEventContainer
            {
                EventName = eventName,
                EventData = eventData
            };
            return Json(evtContainer, JsonRequestBehavior.AllowGet);
        }
    }
}