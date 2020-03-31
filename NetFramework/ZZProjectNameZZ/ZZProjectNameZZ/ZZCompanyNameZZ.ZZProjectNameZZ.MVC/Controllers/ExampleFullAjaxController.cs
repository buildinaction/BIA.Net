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
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using ViewModel.ExampleFullAjax;

    /// <summary>
    /// Controller for ExampleFullAjax Pages
    /// </summary>
    /// <seealso cref="ZZCompanyNameZZ.ZZProjectNameZZ.MVC.Controllers.BaseController" />
    public class ExampleFullAjaxController : BaseController
    {
        /// <summary>
        /// Index View : list all ExampleFullAjaxs.
        /// </summary>
        /// <returns>ActionResult : Index View</returns>
        public ActionResult Index()
        {
            PrepareDefaultView<ExampleFullAjaxAdvancedFilterVM>("ExampleFullAjaxTable");
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
        /// Return a list of employees in json format
        /// </summary>
        /// <param name="param">The different filter parameters</param>
        /// <returns>list of employees in json format</returns>
        [HttpPost]
        public ActionResult GetListData(DataTableAjaxPost<ExampleFullAjaxAdvancedFilterCTO> param)
        {
            BigJsonResult result = default;

            if (param != null)
            {
                List<ExampleTable3DTO> objs = GetFilteredForAjaxDataTable(param, out int filteredResultsCount, out int totalResultsCount);

                List<object> jsonData = new List<object>();
                objs.ForEach(x => jsonData.Add(new
                {
                    x.Id,
                    x.Title,
                    x.Description,
                    Value = x.Value.ToString(),
                    MyDecimal = x.MyDecimal?.ToString("N4", CultureInfo.CurrentCulture.NumberFormat),
                    MyDouble = x.MyDouble?.ToString().Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator),
                    DateOnly = x.DateOnly?.ToString("d"),
                    DateAndTime = x.DateAndTime?.ToString("g"),
                    TimeOnly = x.TimeOnly?.ToString("t"),
                    MyTimeSpan = x.MyTimeSpan?.ToString("t"),
                    TimeSpanOver24H = x.TimeSpanOver24H != null ? new TimeSpan(x.TimeSpanOver24H.Value).ToString("t") : string.Empty,
                    Site__Title = x.Site?.Title,
                    Links = $"<div class=\"\"><a BIADialogLink = \"Type:Modal\" class=\"listAction fas fa-pencil-alt\" data-placement=\"left\" data-toggle=\"tooltip\" title=\"{TextResources.Edit}\" style=\"cursor:pointer\" href=\"{Url.Action("Edit", "ExampleFullAjax", new { x.Id })}\"></a><a biadialoglink = \"Type:Modal\" class=\"listAction fas fa-list-ul\" data-placement=\"left\" data-toggle=\"tooltip\" title=\"{TextResources.Details}\" style=\"cursor:pointer\" href=\"{Url.Action("Details", "ExampleFullAjax", new { x.Id })}\"></a><a biadialoglink = \"Type:Modal\" class=\"listAction fas fa-trash-alt\" data-placement=\"left\" data-toggle=\"tooltip\" title=\"{TextResources.Delete}\" style=\"cursor:pointer\" href=\"{Url.Action("Delete", "ExampleFullAjax", new { x.Id })}\"></a></div>"
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
        /// <param name="param">The different filter parameters</param>
        /// <returns>An excel file</returns>
        [HttpPost]
        public FileResult GetExcel(DataTableAjaxPost<ExampleFullAjaxAdvancedFilterCTO> param)
        {
            FileResult file = default;
            if (param != null)
            {
                // Remove Pagination
                param.Start = default;
                param.Length = int.MaxValue;
                List<ExampleTable3DTO> objs = GetFilteredForAjaxDataTable(param, out _, out _);
                file = GetExportFile(objs);
            }

            return file;
        }

        /// <summary>
        /// Details View for a ExampleFullAjax. (ex :ExampleFullAjax/Details/5)
        /// </summary>
        /// <param name="id">The id of the ExampleFullAjax.</param>
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
        /// Create View. (ex :ExampleFullAjax/Create)
        /// </summary>
        /// <returns>ActionResult : Create View</returns>
        public ActionResult Create()
        {
            PrepareRelatedLink();
            return View();
        }

        /// <summary>
        /// Creates the specified ExampleFullAjax.
        /// </summary>
        /// <param name="exampleTable3DTO">The ExampleFullAjax.</param>
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
        /// Edit View. (ex :ExampleFullAjax/Edit/5)
        /// </summary>
        /// <param name="id">The identifier of the ExampleFullAjax.</param>
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
        /// Edits the specified ExampleFullAjax.
        /// </summary>
        /// <param name="exampleTable3DTO">The ExampleFullAjax.</param>
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
        /// Delete View. (ex :ExampleFullAjax/Delete/5)
        /// </summary>
        /// <param name="id">The identifier of the ExampleFullAjax.</param>
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
        /// Deletes the specified ExampleFullAjax.
        /// </summary>
        /// <param name="id">The identifier of the ExampleFullAjax.</param>
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
        /// <param name="listExampleFullAjaxDTO">liste of ExampleFullAjaxs</param>
        /// <returns>the filter view Model</returns>
        private ExampleFullAjaxAdvancedFilterVM PrepareFilterRelatedLink(List<ExampleTable3DTO> listExampleFullAjaxDTO)
        {
            ExampleFullAjaxAdvancedFilterVM filterVM = new ExampleFullAjaxAdvancedFilterVM
            {
                MslTitle = new MultiSelectList(listExampleFullAjaxDTO.Select(dto => dto.Title).Distinct().ToList()),
                MslDescription = new MultiSelectList(listExampleFullAjaxDTO.Select(dto => dto.Description).Distinct().ToList()),
                MslValue = new MultiSelectList(listExampleFullAjaxDTO.Select(dto => dto.Value).Distinct().ToList()),
                MslMyDecimal = new MultiSelectList(listExampleFullAjaxDTO.Select(dto => dto.MyDecimal).Distinct().ToList()),
                MslMyDouble = new MultiSelectList(listExampleFullAjaxDTO.Select(dto => dto.MyDouble).Distinct().ToList()),
                MslDateOnly = new MultiSelectList(listExampleFullAjaxDTO.Select(dto => dto.DateOnly).Distinct().ToList()),
                MslDateAndTime = new MultiSelectList(listExampleFullAjaxDTO.Select(dto => dto.DateAndTime).Distinct().ToList()),
                MslTimeOnly = new MultiSelectList(listExampleFullAjaxDTO.Select(dto => dto.TimeOnly).Distinct().ToList()),
                MslMyTimeSpan = new MultiSelectList(listExampleFullAjaxDTO.Select(dto => dto.MyTimeSpan).Distinct().ToList()),
                MslTimeSpanOver24H = new MultiSelectList(listExampleFullAjaxDTO.Select(dto => dto.TimeSpanOver24H).Distinct().ToList()),
                MslSite = new MultiSelectList(listExampleFullAjaxDTO.Select(dto => dto.Site?.Title).Distinct().ToList()),
            };
            return filterVM;
        }

        /// <summary>
        /// Returns the filtered data TODO move this function in the filter
        /// </summary>
        /// <param name="datatableDTO"><see cref="DataTableAjaxPost{FilterAdvanced}"/></param>
        /// <param name="filteredResultsCount">filtered results count</param>
        /// <param name="totalResultsCount">total results count</param>
        /// <returns>list of <see cref="ExampleTable3DTO"/></returns>
        private List<ExampleTable3DTO> GetFilteredForAjaxDataTable(DataTableAjaxPost<ExampleFullAjaxAdvancedFilterCTO> datatableDTO, out int filteredResultsCount, out int totalResultsCount)
        {
            // There is an advanced filter => use GetAdvancedFilteredForAjaxDataTable<DTO,CTO> => else you should use GetFilteredForAjaxDataTable<DTO>
            return AllServicesDTO.GetAdvancedFilteredForAjaxDataTable<ExampleTable3DTO, ExampleFullAjaxAdvancedFilterCTO>(datatableDTO, out filteredResultsCount, out totalResultsCount);
        }

        /// <summary>
        /// Return export file
        /// </summary>
        /// <param name="objs">list of <see cref="ExampleTable3DTO"/></param>
        /// <returns>export file</returns>
        private FileResult GetExportFile(List<ExampleTable3DTO> objs)
        {
            FileResult file = default;

            if (objs != null && objs.Any())
            {
                List<Row> rows = new List<Row>();

                foreach (ExampleTable3DTO obj in objs)
                {
                    List<Cell> cells = new List<Cell>();
                    if (objs.First() == obj)
                    {
                        // Add Header
                        cells.Add(OpenXmlExcelHelper.GetCell(HtmlHelpersTranslate.TranslateStringOrOriginal(nameof(obj.Title))));
                        cells.Add(OpenXmlExcelHelper.GetCell(HtmlHelpersTranslate.TranslateStringOrOriginal(nameof(obj.Description))));
                        cells.Add(OpenXmlExcelHelper.GetCell(HtmlHelpersTranslate.TranslateStringOrOriginal(nameof(obj.Value))));
                        cells.Add(OpenXmlExcelHelper.GetCell(HtmlHelpersTranslate.TranslateStringOrOriginal(nameof(obj.MyDecimal))));
                        cells.Add(OpenXmlExcelHelper.GetCell(HtmlHelpersTranslate.TranslateStringOrOriginal(nameof(obj.MyDouble))));
                        cells.Add(OpenXmlExcelHelper.GetCell(HtmlHelpersTranslate.TranslateStringOrOriginal(nameof(obj.DateOnly))));
                        cells.Add(OpenXmlExcelHelper.GetCell(HtmlHelpersTranslate.TranslateStringOrOriginal(nameof(obj.DateAndTime))));
                        cells.Add(OpenXmlExcelHelper.GetCell(HtmlHelpersTranslate.TranslateStringOrOriginal(nameof(obj.TimeOnly))));
                        cells.Add(OpenXmlExcelHelper.GetCell(HtmlHelpersTranslate.TranslateStringOrOriginal(nameof(obj.MyTimeSpan))));
                        cells.Add(OpenXmlExcelHelper.GetCell(HtmlHelpersTranslate.TranslateStringOrOriginal(nameof(obj.TimeSpanOver24H))));
                        cells.Add(OpenXmlExcelHelper.GetCell(HtmlHelpersTranslate.TranslateStringOrOriginal(nameof(obj.Site.Title))));

                        rows.Add(new Row(cells));

                        cells.Clear();
                    }

                    cells.Add(OpenXmlExcelHelper.GetCell(obj.Title));
                    cells.Add(OpenXmlExcelHelper.GetCell(obj.Description));
                    cells.Add(OpenXmlExcelHelper.GetCell(obj.Value.ToString()));
                    cells.Add(OpenXmlExcelHelper.GetCell(obj.MyDecimal.ToString()));
                    cells.Add(OpenXmlExcelHelper.GetCell(obj.MyDouble.ToString()));
                    cells.Add(OpenXmlExcelHelper.GetCell(obj.DateOnly?.ToString("D")));
                    cells.Add(OpenXmlExcelHelper.GetCell(obj.DateAndTime?.ToString("D")));
                    cells.Add(OpenXmlExcelHelper.GetCell(obj.TimeOnly?.ToString("D")));
                    cells.Add(OpenXmlExcelHelper.GetCell(obj.MyTimeSpan.ToString()));
                    cells.Add(OpenXmlExcelHelper.GetCell(obj.TimeSpanOver24H.ToString()));
                    cells.Add(OpenXmlExcelHelper.GetCell(obj.Site.Title.ToString()));

                    rows.Add(new Row(cells));
                }

                IDictionary<string, List<Row>> listSheets = new Dictionary<string, List<Row>>
                {
                    { "ExampleFullAjax", rows }
                };

                file = File(OpenXmlExcelHelper.CreateWorkBook(listSheets), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", string.Format("{0} - {1}.xlsx", HtmlHelpersTranslate.TranslateStringOrOriginal(GetType().Name.Replace("Controller", string.Empty)), Common.AppSettingsReader.ProjectTitle));
            }

            return file;
        }
    }
}