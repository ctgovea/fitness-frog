using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Treehouse.FitnessFrog.Data;
using Treehouse.FitnessFrog.Models;

namespace Treehouse.FitnessFrog.Controllers
{
    public class EntriesController : Controller
    {
        private EntriesRepository _entriesRepository = null;

        public EntriesController()
        {
            _entriesRepository = new EntriesRepository();
        }

        public ActionResult Index()
        {
            List<Entry> entries = _entriesRepository.GetEntries();

            // Calculate the total activity.
            double totalActivity = entries
                .Where(e => e.Exclude == false)
                .Sum(e => e.Duration);

            // Determine the number of days that have entries.
            int numberOfActiveDays = entries
                .Select(e => e.Date)
                .Distinct()
                .Count();

            ViewBag.TotalActivity = totalActivity;
            ViewBag.AverageDailyActivity = (totalActivity / (double)numberOfActiveDays);

            return View(entries);
        }

        public ActionResult Add()
        {
            var entry = new Entry()
            {
                Date = DateTime.Today,
            };

            ViewBag.ActivitiesSelectListItems = new SelectList(
                Data.Data.Activities, "Id", "Name");

            return View(entry);
        }

        [HttpPost]
        //public ActionResult Add(DateTime? date, int? activityID, double? duration, Entry.IntensityLevel? intensity, bool? exclude, string notes)
        public ActionResult Add(Entry entry)
        {
            //string date = Request.Form["Date"];

            //DateTime dateValue;
            //DateTime.TryParse(date, out dateValue);

            // We can remove this now that our view uses the HTML helper methods that we're using to render our form
            // field input or text area elements internally use ModelState to get the user's attempted values.

            //ViewBag.date = ModelState["Date"].Value.AttemptedValue;
            //ViewBag.ActivityID = ModelState["ActivityId"].Value.AttemptedValue;
            //ViewBag.Duration = ModelState["Duration"].Value.AttemptedValue;
            //ViewBag.Intensity = ModelState["Intensity"].Value.AttemptedValue;
            //ViewBag.Exclude = ModelState["Exclude"].Value.AttemptedValue;
            //ViewBag.Notes = ModelState["Notes"].Value.AttemptedValue;

            // If there aren't any "Duration field validation errors
            // then make sure that the duration is greater than "0".
            if (ModelState.IsValidField("Duration") && entry.Duration <= 0)
            {
                ModelState.AddModelError("Duration", "The Duration field value must be greater than '0'");
            }

            if (ModelState.IsValid)
            {
                _entriesRepository.AddEntry(entry);

                return RedirectToAction("Index");
            }

            ViewBag.ActivitiesSelectListItems = new SelectList(
                Data.Data.Activities, "Id", "Name");

            return View(entry);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View();
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View();
        }
    }
}