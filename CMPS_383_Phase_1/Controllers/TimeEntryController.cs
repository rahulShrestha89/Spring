using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using CMPS_383_Phase_1.Models;
using CMPS_383_Phase_1.Helpers;

namespace CMPS_383_Phase_1.Controllers
{

    [AuthorizeUser]
    [Authorize]
    public class TimeEntryController : Controller
    {
        private TimeClockApplicationDbContext db = new TimeClockApplicationDbContext();

        [HttpGet]
        public ActionResult Index()
        {

            AccountHelper ahelper = new AccountHelper();
            Users currentUser = db.User.Find(ahelper.getUserId(User.Identity.Name));

            ViewBag.ClockedStatus = currentUser.ClockedIn;
            return View("Index");

        }

        public ActionResult TimeSummary()
        {
            return View(db.TimeEntry.ToList());
        }

        [HttpPost]
        public ActionResult ClockIn()
        {

            AccountHelper ahelper = new AccountHelper();
            Users currentUser = db.User.Find(ahelper.getUserId(User.Identity.Name));

            TimeEntry entry = new Models.TimeEntry { TimeIn = DateTime.UtcNow, UserId = ahelper.getUserId(User.Identity.Name) };
            db.TimeEntry.Add(entry);
            db.SaveChanges();

            currentUser.ClockedIn = true;
            ViewBag.ClockedStatus = true;
            currentUser.TimeEntryId = entry.TimeEntryId;

            db.Entry(currentUser).State = EntityState.Modified;
            db.SaveChanges();

            ViewBag.Message = "You have clocked in";
            return View("Index");
        }

        [HttpPost]
        public ActionResult ClockOut()
        {
            AccountHelper ahelper = new AccountHelper();
            Users currentUser = db.User.Find(ahelper.getUserId(User.Identity.Name));

            TimeEntry entry = db.TimeEntry.Find(currentUser.TimeEntryId);
            entry.TimeOut = DateTime.UtcNow;
            db.Entry(entry).State = EntityState.Modified;
            db.SaveChanges();

            currentUser.ClockedIn = false;
            ViewBag.ClockedStatus = false;
            db.Entry(currentUser).State = EntityState.Modified;
            db.SaveChanges();

            ViewBag.Message = "You have clocked out";
            return View("Index");
        }


        [AuthorizeUser(Roles = "1")]
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.User, "UserId", "UserName");
            return View();
        }

        //Post: TimeEntry Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(Roles = "1")]
        public ActionResult Create([Bind(Include = "TimeEntryId,TimeIn,TimeOut,UserId")] TimeEntry timeentry)
        {
            if (ModelState.IsValid)
            {
                db.TimeEntry.Add(timeentry);
                db.SaveChanges();
                return RedirectToAction("TimeSummary", "TimeEntry");
            }
            ViewBag.UserId = new SelectList(db.User, "UserId", "UserName", timeentry.UserId);
            return View(timeentry);
        }
        // GET: TimeEntry Edit
        [AuthorizeUser(Roles = "1")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeEntry timeentry = db.TimeEntry.Find(id);
            if (timeentry == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.User, "UserId", "UserName", timeentry.UserId);
            return View(timeentry);
        }
        // POST: TimeEntry Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(Roles = "1")]
        public ActionResult Edit([Bind(Include = "TimeEntryId,TimeIn,TimeOut,UserId")]TimeEntry timeentry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(timeentry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("TimeSummary", "TimeEntry");
            }
            ViewBag.UserId = new SelectList(db.User, "UserId", "UserName", timeentry.UserId);
            return View(timeentry);
        }
        // GET: TimeEntry Delete
        [AuthorizeUser(Roles = "1")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeEntry timeentry = db.TimeEntry.Find(id);
            if (timeentry == null)
            {
                return HttpNotFound();
            }
            return View(timeentry);
        }
        // POST: TimeEntry Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(Roles = "1")]
        public ActionResult DeleteConfirmed(int id)
        {
            TimeEntry timeentry = db.TimeEntry.Find(id);
            db.TimeEntry.Remove(timeentry);
            db.SaveChanges();
            return RedirectToAction("TimeSummary", "TimeEntry");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [AuthorizeUser]
        public ActionResult BackEnd()
        {
            return View(db.TimeEntry.ToList());
        }


    }
}