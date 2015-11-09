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
    public class Users_projectsController : Controller
    {
        private Entities db = new Entities();

        // GET: Users_projects
        public ActionResult Index()
        {
            var users_projects = db.Users_projects.Include(u => u.AspNetUsers);
            return View(users_projects.ToList());
        }

        // GET: Users_projects/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users_projects users_projects = db.Users_projects.Find(id);
            if (users_projects == null)
            {
                return HttpNotFound();
            }
            return View(users_projects);
        }

        // GET: Users_projects/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Users_projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,project_id,project_role")] Users_projects users_projects)
        {
            if (ModelState.IsValid)
            {
                db.Users_projects.Add(users_projects);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", users_projects.Id);
            return View(users_projects);
        }

        // GET: Users_projects/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users_projects users_projects = db.Users_projects.Find(id);
            if (users_projects == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", users_projects.Id);
            return View(users_projects);
        }

        // POST: Users_projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,project_id,project_role")] Users_projects users_projects)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users_projects).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", users_projects.Id);
            return View(users_projects);
        }

        // GET: Users_projects/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users_projects users_projects = db.Users_projects.Find(id);
            if (users_projects == null)
            {
                return HttpNotFound();
            }
            return View(users_projects);
        }

        // POST: Users_projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Users_projects users_projects = db.Users_projects.Find(id);
            db.Users_projects.Remove(users_projects);
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
