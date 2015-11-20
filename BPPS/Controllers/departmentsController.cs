using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BPPS.Models;

namespace BPPS.Controllers
{
    public class departmentsController : Controller
    {
        private Entities db = new Entities();

        // GET: departments
        public ActionResult Index()
        {
            var departments = db.departments.Include(d => d.locations).Include(d => d.departments2);
            return View(departments.ToList());
        }

        // GET: departments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            departments departments = db.departments.Find(id);
            if (departments == null)
            {
                return HttpNotFound();
            }
            return View(departments);
        }

        // GET: departments/Create
        public ActionResult Create()
        {
            ViewBag.location_id = new SelectList(db.locations, "location_id", "town");
            ViewBag.dep_department_id = new SelectList(db.departments, "department_id", "name");
            return View();
        }

        // POST: departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "department_id,location_id,name,dep_department_id")] departments departments)
        {
            if (ModelState.IsValid)
            {
                db.departments.Add(departments);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.location_id = new SelectList(db.locations, "location_id", "town", departments.location_id);
            ViewBag.dep_department_id = new SelectList(db.departments, "department_id", "name", departments.dep_department_id);
            return View(departments);
        }

        // GET: departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            departments departments = db.departments.Find(id);
            if (departments == null)
            {
                return HttpNotFound();
            }
            ViewBag.location_id = new SelectList(db.locations, "location_id", "town", departments.location_id);
            ViewBag.dep_department_id = new SelectList(db.departments, "department_id", "name", departments.dep_department_id);
            return View(departments);
        }

        // POST: departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "department_id,location_id,name,dep_department_id")] departments departments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(departments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.location_id = new SelectList(db.locations, "location_id", "town", departments.location_id);
            ViewBag.dep_department_id = new SelectList(db.departments, "department_id", "name", departments.dep_department_id);
            return View(departments);
        }

        // GET: departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            departments departments = db.departments.Find(id);
            if (departments == null)
            {
                return HttpNotFound();
            }
            return View(departments);
        }

        // POST: departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            departments departments = db.departments.Find(id);
            db.departments.Remove(departments);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
