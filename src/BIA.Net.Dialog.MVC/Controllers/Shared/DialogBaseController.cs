using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BIA.Net.Dialog.MVC.Controllers
{
    /*public class PartialViewConverter : ViewResult
    {
        public ViewResultBase Res { get; set; }
        public PartialViewConverter(ViewResultBase res) { Res = res; }
        public override void ExecuteResult(ControllerContext context)
        {
            Res.ExecuteResult(context);
        }
        public static ViewResult Convert(ViewResultBase res)
        {
            return new PartialViewConverter(res);
        }
    }*/
    public abstract class DialogBaseContoller : Controller
    {
        public enum DisplayFlag
        {
            None = 0x0,
            Popup = 0x1,
        };

        private string UniformizeUrl(string url)
        {
            Regex rgx = new Regex("/$");
            return rgx.Replace(url.ToLower().Replace("#", ""),"");
        }

        protected override RedirectToRouteResult RedirectToAction(string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            DisplayFlag displayFlag = 0;
            if (HttpContext.Request["displayFlag"] != null)
                displayFlag = (DisplayFlag)int.Parse(HttpContext.Request["displayFlag"]);
            if (displayFlag != 0)
            {
                //Test if we are returned to parent Page
                string dialogUrlParent = null;
                if (HttpContext.Request["dialogUrlParent"] != null)
                    dialogUrlParent = ((string) HttpContext.Request["dialogUrlParent"]).ToLower();
                if (!string.IsNullOrEmpty(dialogUrlParent))
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = UniformizeUrl(u.Action(actionName, controllerName, routeValues));
                    string full_url = UniformizeUrl(HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority + u.Action(actionName, controllerName, routeValues));

                    string[] dialogUrlsParent = dialogUrlParent.Split(';');
                    foreach (string urlPa in dialogUrlsParent)
                    {
                        string uniformizedUrlParent = UniformizeUrl(urlPa);
                        if (url.Equals(uniformizedUrlParent) || full_url.Equals(uniformizedUrlParent))
                        //if (urlPa.Contains(url))
                        {
                            return base.RedirectToAction("CloseDialog", "DialogBasicAction", null);
                        }
                    }
                }

                //Not parent page continue
                if (routeValues == null)
                    routeValues = new RouteValueDictionary();
                routeValues.Add("displayFlag", (int)displayFlag);
                routeValues.Add("BIARedirectedDialogUrl", Url.Action(actionName, routeValues));
            }
            return base.RedirectToAction(actionName, controllerName, routeValues);
        }

        protected override ViewResult View(string viewName, string masterName, object model)
        {
            DisplayFlag displayFlag = 0;
            if (HttpContext.Request["displayFlag"] != null)
            {
                displayFlag = (DisplayFlag)int.Parse(HttpContext.Request["displayFlag"]);
            }
            if(HttpContext.Request["BIARedirectedDialogUrl"] != null)
            {
                HttpContext.Response.AddHeader("BIARedirectedDialogUrl", HttpContext.Request["BIARedirectedDialogUrl"]);
            }
           /* if (HttpContext.Request["DialogRefreshPartialView"] != null)
            {
                viewName = HttpContext.Request["DialogRefreshPartialView"];
                return PartialViewConverter.Convert(base.PartialView(viewName, model));
            }*/
            if ((displayFlag & DisplayFlag.Popup) == DisplayFlag.Popup)
            {
                ViewResult myView = base.View(viewName, masterName, model);
                myView.MasterName = "~/Views/Shared/_Layout_Dialog.cshtml";
                //Request.Add("BIACurrentDialogUrl", Url.Action(viewName));
                return myView;
            }
            return base.View(viewName, masterName, model);
        }
    }
}