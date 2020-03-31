namespace ZZCompanyNameZZ.ZZProjectNameZZ.MVC.Controllers
{
    using BIA.Net.Business.Services;
    using BIA.Net.Common.Helpers;
    using BIA.Net.DataTable.DTO;
    using BIA.Net.Helpers;
    using BIA.Net.MVC;
    using BIA.Net.MVC.Filter;
    using Business.CTO;
    using Business.DTO;
    using Common.Resources.BIA.Net;
    using DocumentFormat.OpenXml.Spreadsheet;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using ViewModel.ExampleTable2FACompCol;

    /// <summary>
    /// Controller for ExampleTable2FACompCol Pages
    /// </summary>
    /// <seealso cref="ZZCompanyNameZZ.ZZProjectNameZZ.MVC.Controllers.BaseController" />
    public class ExampleTable2FACompColController : BaseController
    {
        /// <summary>
        /// Index View : list all ExampleTable2FACompCols.
        /// </summary>
        /// <returns>ActionResult : Index View</returns>
        public ActionResult Index()
        {
            PrepareDefaultView<ExampleTable2FACompColAdvancedFilterVM>("ExampleTable2FACompColTable");
            return View();
        }

        /// <summary>
        /// Filter View.
        /// </summary>
        /// <returns>ActionResult : Filter view</returns>
        public ActionResult AdvancedFilter()
        {
            return View(PrepareFilterRelatedLink(AllServicesDTO.GetAll<ExampleTable2CompColDTO>().OrderBy(m => m.Title).ToList()));
        }

        /// <summary>
        /// Return a list of employees in json format
        /// </summary>
        /// <param name="param">DataTableAjaxPost object contains filter and pagging</param>
        /// <returns>list of ExampleTable2CompColDTO in json format</returns>
        [HttpPost]
        public ActionResult GetListData(DataTableAjaxPost<ExampleTable2FACompColAdvancedFilterCTO> param)
        {
            BigJsonResult result = default;

            if (param != null)
            {
                List<ExampleTable2CompColDTO> objs = GetFilteredForAjaxDataTable(param, out int filteredResultsCount, out int totalResultsCount);

                var jsonData = new List<object>();
                objs.ForEach(x => jsonData.Add(new
                {
                x.Id,
                Title = x.Title,
                Description = x.Description,
                Site__Title = x.Site?.Title,
                ComputedCol = x.ComputedCol,
                Links = $"<div class=\"\"><a BIADialogLink = \"Type:Modal\" class=\"listAction fas fa-pencil-alt\" data-placement=\"left\" data-toggle=\"tooltip\" title=\"{TextResources.Edit}\" style=\"cursor:pointer\" href=\"{Url.Action("Edit", "ExampleTable2FACompCol", new { Id = x.Id })}\"></a><a biadialoglink = \"Type:Modal\" class=\"listAction fas fa-list-ul\" data-placement=\"left\" data-toggle=\"tooltip\" title=\"{TextResources.Details}\" style=\"cursor:pointer\" href=\"{Url.Action("Details", "ExampleTable2FACompCol", new { Id = x.Id })}\"></a><a biadialoglink = \"Type:Modal\" class=\"listAction fas fa-trash-alt\" data-placement=\"left\" data-toggle=\"tooltip\" title=\"{TextResources.Delete}\" style=\"cursor:pointer\" href=\"{Url.Action("Delete", "ExampleTable2FACompCol", new { Id = x.Id })}\"></a></div>"
            }));

                result = new BigJsonResult(new
                {
                    draw = param.Draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = jsonData
                });
            }

            return result;
        }

        /// <summary>
        /// Returns an excel file containing the data displayed in the table.
        /// </summary>Return an export
        /// <param name="param">The different filter parameters in json format</param>
        /// <returns>An excel file</returns>
        [HttpPost]
        public FileResult GetExcel(DataTableAjaxPost<ExampleTable2FACompColAdvancedFilterCTO> param)
        {
            FileResult file = default;

            if (param != null)
            {
                // Remove Pagination
                param.Start = default;
                param.Length = int.MaxValue;

                List<ExampleTable2CompColDTO> objs = GetFilteredForAjaxDataTable(param, out _, out _);
                file = GetExportFile(objs);
            }

            return file;
        }

        /// <summary>
        /// Details View for a ExampleTable2FACompCol. (ex :ExampleTable2FACompCol/Details/5)
        /// </summary>
        /// <param name="id">The id of the ExampleTable2FACompCol.</param>
        /// <returns>ActionResult : Details View</returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ExampleTable2CompColDTO exampleTable2CompColDTO = AllServicesDTO.Find<ExampleTable2CompColDTO>(id);
            if (exampleTable2CompColDTO == null)
            {
                return HttpNotFound();
            }

            PrepareRelatedLink();
            return View(exampleTable2CompColDTO);
        }

        /// <summary>
        /// Create View. (ex :ExampleTable2FACompCol/Create)
        /// </summary>
        /// <returns>ActionResult : Create View</returns>
        public ActionResult Create()
        {
            PrepareRelatedLink();
            return View();
        }

        /// <summary>
        /// Creates the specified ExampleTable2FACompCol.
        /// </summary>
        /// <param name="exampleTable2CompColDTO">The ExampleTable2FACompCol.</param>
        /// <returns>ActionResult : Create View or Index View if success</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public ActionResult Create([Bind(Include = "Id,Title,Description,SiteId")] ExampleTable2CompColDTO exampleTable2CompColDTO)
        {
            if (ModelState.IsValid)
            {
                AllServicesDTO.Insert(exampleTable2CompColDTO);
                return RedirectToAction("CloseDialog", "DialogBasicAction", null).Success(TextResources.CreatedSuccessfully);
            }

            PrepareRelatedLink();
            return View(exampleTable2CompColDTO);
        }

        /// <summary>
        /// Edit View. (ex :ExampleTable2FACompCol/Edit/5)
        /// </summary>
        /// <param name="id">The identifier of the ExampleTable2FACompCol.</param>
        /// <returns>ActionResult : Edit View</returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ExampleTable2CompColDTO exampleTable2CompColDTO = AllServicesDTO.Find<ExampleTable2CompColDTO>(id);
            if (exampleTable2CompColDTO == null)
            {
                return HttpNotFound();
            }

            PrepareRelatedLink();
            return View(exampleTable2CompColDTO);
        }

        /// <summary>
        /// Edits the specified ExampleTable2FACompCol.
        /// </summary>
        /// <param name="exampleTable2CompColDTO">The ExampleTable2FACompCol.</param>
        /// <returns>ActionResult : Edit View or Index View if success</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,SiteId")] ExampleTable2CompColDTO exampleTable2CompColDTO)
        {
            if (ModelState.IsValid)
            {
                AllServicesDTO.UpdateValues(exampleTable2CompColDTO, new List<string>() { nameof(ExampleTable2CompColDTO.Title), nameof(ExampleTable2CompColDTO.Description), nameof(ExampleTable2CompColDTO.Site) });
               return RedirectToAction("CloseDialog", "DialogBasicAction", null).Success(TextResources.UpdatedSuccessfully);
            }

            PrepareRelatedLink();
            return View(exampleTable2CompColDTO);
        }

        /// <summary>
        /// Delete View. (ex :ExampleTable2FACompCol/Delete/5)
        /// </summary>
        /// <param name="id">The identifier of the ExampleTable2FACompCol.</param>
        /// <returns>ActionResult : Delete View</returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ExampleTable2CompColDTO exampleTable2CompColDTO = AllServicesDTO.Find<ExampleTable2CompColDTO>(id);
            if (exampleTable2CompColDTO == null)
            {
                return HttpNotFound();
            }

            PrepareRelatedLink();
            return View(exampleTable2CompColDTO);
        }

        /// <summary>
        /// Deletes the specified ExampleTable2FACompCol.
        /// </summary>
        /// <param name="id">The identifier of the ExampleTable2FACompCol.</param>
        /// <returns>ActionResult : Delete View or Index View if success</returns>
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public ActionResult DeleteConfirmed(int id)
        {
           AllServicesDTO.DeleteById<ExampleTable2CompColDTO>(id);
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
        /// <param name="listExampleTable2CompColDTO">list of ExampleTable2CompColDTOs</param>
        /// <returns>the filter view Model</returns>
        private ExampleTable2FACompColAdvancedFilterVM PrepareFilterRelatedLink(List<ExampleTable2CompColDTO> listExampleTable2CompColDTO)
        {
            ExampleTable2FACompColAdvancedFilterVM filterVM = new ExampleTable2FACompColAdvancedFilterVM
            {
                MslTitle = new MultiSelectList(listExampleTable2CompColDTO.Select(dto => dto.Title).Distinct().ToList()),
                MslDescription = new MultiSelectList(listExampleTable2CompColDTO.Select(dto => dto.Description).Distinct().ToList()),
                MslSite = new MultiSelectList(listExampleTable2CompColDTO.Select(dto => dto.Site?.Title).Distinct().ToList()),
            };
            return filterVM;
        }

        /// <summary>
        /// Returns the filtered data TODO move this function in the filter
        /// </summary>
        /// <param name="datatableDTO"><see cref="DataTableAjaxPost{FilterAdvanced}"/></param>
        /// <param name="filteredResultsCount">filtered results count</param>
        /// <param name="totalResultsCount">total results count</param>
        /// <returns>list of <see cref="ExampleTable2CompColDTO"/></returns>
        private List<ExampleTable2CompColDTO> GetFilteredForAjaxDataTable(DataTableAjaxPost<ExampleTable2FACompColAdvancedFilterCTO> datatableDTO, out int filteredResultsCount, out int totalResultsCount)
        {
            // There is an advanced filter => use GetAdvancedFilteredForAjaxDataTable<DTO,CTO> => else you should use GetFilteredForAjaxDataTable<DTO>
            return AllServicesDTO.GetAdvancedFilteredForAjaxDataTable<ExampleTable2CompColDTO, ExampleTable2FACompColAdvancedFilterCTO>(datatableDTO, out filteredResultsCount, out totalResultsCount);
        }

        /// <summary>
        /// Return export file
        /// </summary>
        /// <param name="objs">list of <see cref="ExampleTable2CompColDTO"/></param>
        /// <returns>export file</returns>
        private FileResult GetExportFile(List<ExampleTable2CompColDTO> objs)
        {
            FileResult file = default;

            if (objs != null && objs.Any())
            {
                List<Row> rows = new List<Row>();

                foreach (var obj in objs)
                {
                    List<Cell> cells = new List<Cell>();
                    if (objs.First() == obj)
                    {
                        // Add Header
                        cells.Add(OpenXmlExcelHelper.GetCell(HtmlHelpersTranslate.TranslateStringOrOriginal(nameof(obj.Title))));
                        cells.Add(OpenXmlExcelHelper.GetCell(HtmlHelpersTranslate.TranslateStringOrOriginal(nameof(obj.Description))));
                        cells.Add(OpenXmlExcelHelper.GetCell(HtmlHelpersTranslate.TranslateStringOrOriginal(nameof(obj.Site.Title))));

                        rows.Add(new Row(cells));

                        cells.Clear();
                    }

                    cells.Add(OpenXmlExcelHelper.GetCell(obj.Title));
                    cells.Add(OpenXmlExcelHelper.GetCell(obj.Description));
                    cells.Add(OpenXmlExcelHelper.GetCell(obj.Site.Title.ToString()));

                    rows.Add(new Row(cells));
                }

                IDictionary<string, List<Row>> listSheets = new Dictionary<string, List<Row>>
                {
                    { "ExampleTable2FACompCol", rows }
                };

                file = File(OpenXmlExcelHelper.CreateWorkBook(listSheets), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", string.Format("{0} - {1}.xlsx", HtmlHelpersTranslate.TranslateStringOrOriginal(this.GetType().Name.Replace("Controller", string.Empty)), Common.AppSettingsReader.ProjectTitle));
            }

            return file;
        }
    }
}