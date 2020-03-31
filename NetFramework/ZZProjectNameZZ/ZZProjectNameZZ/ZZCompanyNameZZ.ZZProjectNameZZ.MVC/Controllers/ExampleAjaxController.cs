namespace ZZCompanyNameZZ.ZZProjectNameZZ.MVC.Controllers
{
    using BIA.Net.Business.Services;
    using BIA.Net.MVC;
    using BIA.Net.MVC.Filter;
    using Business.CTO;
    using Business.DTO;
    using Common.Resources.BIA.Net;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using ViewModel.ExampleAjax;

    /// <summary>
    /// Controller for ExampleAjax Pages
    /// </summary>
    /// <seealso cref="ZZCompanyNameZZ.ZZProjectNameZZ.MVC.Controllers.BaseController" />
    public class ExampleAjaxController : BaseController
    {
        /// <summary>
        /// Index View : list all ExampleAjaxs.
        /// </summary>
        /// <returns>ActionResult : Index View</returns>
        public ActionResult Index()
        {
            PrepareDefaultView<ExampleAjaxAdvancedFilterVM>("ExampleAjaxTable");
            return View();
        }

        /// <summary>
        /// Filter View.
        /// </summary>
        /// <returns>ActionResult : Filter view</returns>
        public ActionResult AdvancedFilter()
        {
            return View(PrepareFilterRelatedLink(AllServicesDTO.GetAll<ExampleTable3DTO>().OrderBy(m => m.Title).ToList()));
        }

        /// <summary>
        /// Return a list of ExampleAjax in json format
        /// </summary>
        /// <param name="advancedFilter">the filter value</param>
        /// <returns>list of employees in json format</returns>
        [HttpPost]
        public ActionResult GetListData(ExampleAjaxAdvancedFilterCTO advancedFilter)
        {
            // There is an advanced filter => use GetAdvancedFiltered<DTO,CTO> => else you should use GetAll<DTO>
            List<ExampleTable3DTO> listDTO = AllServicesDTO.GetAdvancedFiltered<ExampleTable3DTO, ExampleAjaxAdvancedFilterCTO>(advancedFilter);
            var jsonData = new List<object>();
            listDTO.ForEach(x => jsonData.Add(new
            {
                x.Id,
                x.Title,
                x.Description,
                Value = x.Value.ToString(),
                MyDecimal = x.MyDecimal.ToString(),
                MyDouble = x.MyDouble.ToString(),
                DateOnly = x.DateOnly?.ToString("d"),
                DateAndTime = x.DateAndTime?.ToString("g"),
                TimeOnly = x.TimeOnly?.ToString("t"),
                MyTimeSpan = x.MyTimeSpan?.ToString("t"),
                TimeSpanOver24H = x.TimeSpanOver24H != null ? new TimeSpan(x.TimeSpanOver24H.Value).ToString("t") : string.Empty,
                Site__Title = x.Site?.Title,
                Links = $"<div class=\"\"><a BIADialogLink = \"Type:Modal\" class=\"listAction fas fa-pencil-alt\" data-placement=\"left\" data-toggle=\"tooltip\" title=\"{TextResources.Edit}\" style=\"cursor:pointer\" href=\"{Url.Action("Edit", "ExampleAjax", new { x.Id })}\"></a><a biadialoglink = \"Type:Modal\" class=\"listAction fas fa-list-ul\" data-placement=\"left\" data-toggle=\"tooltip\" title=\"{TextResources.Details}\" style=\"cursor:pointer\" href=\"{Url.Action("Details", "ExampleAjax", new { x.Id })}\"></a><a biadialoglink = \"Type:Modal\" class=\"listAction fas fa-trash-alt\" data-placement=\"left\" data-toggle=\"tooltip\" title=\"{TextResources.Delete}\" style=\"cursor:pointer\" href=\"{Url.Action("Delete", "ExampleAjax", new { x.Id })}\"></a></div>"
            }));

            BigJsonResult jsonResult = new BigJsonResult(new { data = jsonData });
            return jsonResult;
        }

        /// <summary>
        /// Details View for a ExampleAjax. (ex :ExampleAjax/Details/5)
        /// </summary>
        /// <param name="id">The id of the ExampleAjax.</param>
        /// <returns>ActionResult : Details View</returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ExampleTable3DTO exampleTable3DTO = AllServicesDTO.Find<ExampleTable3DTO>(id);
            if (exampleTable3DTO == null)
            {
                return HttpNotFound();
            }

            PrepareRelatedLink();
            return View(exampleTable3DTO);
        }

        /// <summary>
        /// Create View. (ex :ExampleAjax/Create)
        /// </summary>
        /// <returns>ActionResult : Create View</returns>
        public ActionResult Create()
        {
            PrepareRelatedLink();
            return View();
        }

        /// <summary>
        /// Creates the specified ExampleAjax.
        /// </summary>
        /// <param name="exampleTable3DTO">The ExampleAjax.</param>
        /// <returns>ActionResult : Create View or Index View if success</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Value,MyDecimal,MyDouble,DateOnly,DateAndTime,TimeOnly,MyTimeSpan,TimeSpanOver24H,SiteId")] ExampleTable3DTO exampleTable3DTO)
        {
            if (ModelState.IsValid)
            {
                AllServicesDTO.Insert(exampleTable3DTO);
                return RedirectToAction("CloseDialog", "DialogBasicAction", null).Success(TextResources.CreatedSuccessfully);
            }

            PrepareRelatedLink();
            return View(exampleTable3DTO);
        }

        /// <summary>
        /// Edit View. (ex :ExampleAjax/Edit/5)
        /// </summary>
        /// <param name="id">The identifier of the ExampleAjax.</param>
        /// <returns>ActionResult : Edit View</returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ExampleTable3DTO exampleTable3DTO = AllServicesDTO.Find<ExampleTable3DTO>(id);
            if (exampleTable3DTO == null)
            {
                return HttpNotFound();
            }

            PrepareRelatedLink();
            return View(exampleTable3DTO);
        }

        /// <summary>
        /// Edits the specified ExampleAjax.
        /// </summary>
        /// <param name="exampleTable3DTO">The ExampleAjax.</param>
        /// <returns>ActionResult : Edit View or Index View if success</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Value,MyDecimal,MyDouble,DateOnly,DateAndTime,TimeOnly,MyTimeSpan,TimeSpanOver24H,SiteId")] ExampleTable3DTO exampleTable3DTO)
        {
            if (ModelState.IsValid)
            {
                AllServicesDTO.UpdateValues(exampleTable3DTO, new List<string>() { nameof(ExampleTable3DTO.Title), nameof(ExampleTable3DTO.Description), nameof(ExampleTable3DTO.Value), nameof(ExampleTable3DTO.MyDecimal), nameof(ExampleTable3DTO.MyDouble), nameof(ExampleTable3DTO.DateOnly), nameof(ExampleTable3DTO.DateAndTime), nameof(ExampleTable3DTO.TimeOnly), nameof(ExampleTable3DTO.MyTimeSpan), nameof(ExampleTable3DTO.TimeSpanOver24H), nameof(ExampleTable3DTO.Site) });
               return RedirectToAction("CloseDialog", "DialogBasicAction", null).Success(TextResources.UpdatedSuccessfully);
            }

            PrepareRelatedLink();
            return View(exampleTable3DTO);
        }

        /// <summary>
        /// Delete View. (ex :ExampleAjax/Delete/5)
        /// </summary>
        /// <param name="id">The identifier of the ExampleAjax.</param>
        /// <returns>ActionResult : Delete View</returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ExampleTable3DTO exampleTable3DTO = AllServicesDTO.Find<ExampleTable3DTO>(id);
            if (exampleTable3DTO == null)
            {
                return HttpNotFound();
            }

            PrepareRelatedLink();
            return View(exampleTable3DTO);
        }

        /// <summary>
        /// Deletes the specified ExampleAjax.
        /// </summary>
        /// <param name="id">The identifier of the ExampleAjax.</param>
        /// <returns>ActionResult : Delete View or Index View if success</returns>
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public ActionResult DeleteConfirmed(int id)
        {
           AllServicesDTO.DeleteById<ExampleTable3DTO>(id);
            return RedirectToAction("CloseDialog", "DialogBasicAction", null).Success(TextResources.DeletedSuccessfully);
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
            ViewBag.SiteDTOId = new SelectList(
                AllServicesDTO.GetAll<SiteDTO>().OrderBy(p => p.Title),
                nameof(SiteDTO.Id),
                nameof(SiteDTO.Title));
        }

        /// <summary>
        /// Prepares the related link.
        /// </summary>
        /// <param name="listExampleAjaxDTO">liste of ExampleAjaxs</param>
        /// <returns>the filter view Model</returns>
        private ExampleAjaxAdvancedFilterVM PrepareFilterRelatedLink(List<ExampleTable3DTO> listExampleAjaxDTO)
        {
            ExampleAjaxAdvancedFilterVM filterVM = new ExampleAjaxAdvancedFilterVM
            {
                MslTitle = new MultiSelectList(listExampleAjaxDTO.Select(dto => dto.Title).Distinct().ToList()),
                MslDescription = new MultiSelectList(listExampleAjaxDTO.Select(dto => dto.Description).Distinct().ToList()),
                MslValue = new MultiSelectList(listExampleAjaxDTO.Select(dto => dto.Value).Distinct().ToList()),
                MslMyDecimal = new MultiSelectList(listExampleAjaxDTO.Select(dto => dto.MyDecimal).Distinct().ToList()),
                MslMyDouble = new MultiSelectList(listExampleAjaxDTO.Select(dto => dto.MyDouble).Distinct().ToList()),
                MslDateOnly = new MultiSelectList(listExampleAjaxDTO.Select(dto => dto.DateOnly).Distinct().ToList()),
                MslDateAndTime = new MultiSelectList(listExampleAjaxDTO.Select(dto => dto.DateAndTime).Distinct().ToList()),
                MslTimeOnly = new MultiSelectList(listExampleAjaxDTO.Select(dto => dto.TimeOnly).Distinct().ToList()),
                MslMyTimeSpan = new MultiSelectList(listExampleAjaxDTO.Select(dto => dto.MyTimeSpan).Distinct().ToList()),
                MslTimeSpanOver24H = new MultiSelectList(listExampleAjaxDTO.Select(dto => dto.TimeSpanOver24H).Distinct().ToList()),
                MslSite = new MultiSelectList(listExampleAjaxDTO.Select(dto => dto.Site?.Title).Distinct().ToList()),
            };
            return filterVM;
        }
    }
}