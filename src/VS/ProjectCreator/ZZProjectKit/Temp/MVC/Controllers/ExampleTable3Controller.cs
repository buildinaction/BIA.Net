namespace $safeprojectname$.Controllers
{
    using BIA.Net.Business.Services;
    using BIA.Net.MVC.Filter;
    using Business.DTO;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    /// <summary>
    /// Controller for ExampleTable3 Pages
    /// </summary>
    /// <seealso cref="$safeprojectname$.Controllers.BaseController" />
    public class ExampleTable3Controller : BaseController
    {
        /// <summary>
        /// Index View : list all ExampleTable3s.
        /// </summary>
        /// <returns>ActionResult : Index View</returns>
        public ActionResult Index()
        {
            List<ExampleTable3DTO> list = AllServicesDTO.GetAll<ExampleTable3DTO>().OrderBy(m => m.Title).ToList();
            return View(list);
        }

        /// <summary>
        /// _List Partial View.
        /// </summary>
        /// <returns>ActionResult : _List partial View</returns>
        public ActionResult _List()
        {
            return PartialView(AllServicesDTO.GetAll<ExampleTable3DTO>().OrderBy(m => m.Title));
        }

        /// <summary>
        /// Details View for a ExampleTable3. (ex :ExampleTable3/Details/5)
        /// </summary>
        /// <param name="id">The id of the ExampleTable3.</param>
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

            return View(exampleTable3DTO);
        }

        /// <summary>
        /// Create View. (ex :ExampleTable3/Create)
        /// </summary>
        /// <returns>ActionResult : Create View</returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Creates the specified ExampleTable3.
        /// </summary>
        /// <param name="exampleTable3DTO">The ExampleTable3.</param>
        /// <returns>ActionResult : Create View or Index View if success</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Value,Decimal,Double,DateOnly,DateAndTime,TimeOnly,TimeSpan,TimeSpanOver24H")] ExampleTable3DTO exampleTable3DTO)
        {
            if (ModelState.IsValid)
            {
                AllServicesDTO.Insert(exampleTable3DTO);
                return RedirectToAction("Index");
            }

            return View(exampleTable3DTO);
        }

        /// <summary>
        /// Edit View. (ex :ExampleTable3/Edit/5)
        /// </summary>
        /// <param name="id">The identifier of the ExampleTable3.</param>
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

            return View(exampleTable3DTO);
        }

        /// <summary>
        /// Edits the specified ExampleTable3.
        /// </summary>
        /// <param name="exampleTable3DTO">The ExampleTable3.</param>
        /// <returns>ActionResult : Edit View or Index View if success</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Value,Decimal,Double,DateOnly,DateAndTime,TimeOnly,TimeSpan,TimeSpanOver24H")] ExampleTable3DTO exampleTable3DTO)
        {
            if (ModelState.IsValid)
            {
                AllServicesDTO.UpdateValues(exampleTable3DTO, new List<string>() { nameof(ExampleTable3DTO.Title), nameof(ExampleTable3DTO.Description), nameof(ExampleTable3DTO.Value), nameof(ExampleTable3DTO.Decimal), nameof(ExampleTable3DTO.Double), nameof(ExampleTable3DTO.DateOnly), nameof(ExampleTable3DTO.DateAndTime), nameof(ExampleTable3DTO.TimeOnly), nameof(ExampleTable3DTO.TimeSpan), nameof(ExampleTable3DTO.TimeSpanOver24H) });
                return RedirectToAction("Index");
            }

            return View(exampleTable3DTO);
        }

        /// <summary>
        /// Delete View. (ex :ExampleTable3/Delete/5)
        /// </summary>
        /// <param name="id">The identifier of the ExampleTable3.</param>
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

            return View(exampleTable3DTO);
        }

        /// <summary>
        /// Deletes the specified ExampleTable3.
        /// </summary>
        /// <param name="id">The identifier of the ExampleTable3.</param>
        /// <returns>ActionResult : Delete View or Index View if success</returns>
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public ActionResult DeleteConfirmed(int id)
        {
           AllServicesDTO.DeleteById<ExampleTable3DTO>(id);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Releases unmanaged resources and optionally releases managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}