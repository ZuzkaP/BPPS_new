using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BPPS.Models;
using PagedList;

namespace BPPS.Controllers
{
    public class ProjectsController : Controller
    {
        private Entities db = new Entities();

        // GET: Projects
        public ActionResult Index(string location, string segment, string subSegment, string bpssStatus, string status, int? page)
        {
            var projects = from m in db.Projects.ToList()
                           select m;

            var locationList = new List<string>();
            var locationQry = from d in db.Projects.ToList()
                              orderby d.departments.locations.country
                              select d.departments.locations.country;

            locationList.AddRange(locationQry.Distinct());
            ViewBag.location = new SelectList(locationList);

            var segmentList = new List<string>();
            var segmentQry = from d in db.Projects.ToList()
                             orderby d.departments.name
                             select d.departments.name;

            segmentList.AddRange(segmentQry.Distinct());
            ViewBag.segment = new SelectList(segmentList);

            var subSegmentList = new List<string>();
            var subSegmentQry = from d in db.Projects.ToList()
                                orderby d.departments.name
                                select d.departments.name;

            subSegmentList.AddRange(subSegmentQry.Distinct());
            ViewBag.subSegment = new SelectList(subSegmentList);

            //db.feedbacks.Where(f => f.projects.project_name == 'BPSS').toList()

            var bpssStatusList = new List<string>();
            var bpssStatusQry = from d in db.Projects.ToList()
                                orderby d.status
                                select d.status;

            bpssStatusList.AddRange(bpssStatusQry.Distinct());
            ViewBag.bpssStatus = new SelectList(bpssStatusList);

            var statusList = new List<string>();
            var statusQry = from d in db.Projects.ToList()
                            orderby d.status
                            select d.status;


            statusList.AddRange(statusQry.Distinct());
            ViewBag.status = new SelectList(statusList);

            if (!String.IsNullOrEmpty(location))
            {
                projects = projects.Where(s => s.departments.locations.country.ToUpper() == location.ToUpper());
            }

            if (!String.IsNullOrEmpty(segment))
            {
                projects = projects.Where(s => s.departments.name.ToUpper() == segment.ToUpper());
            }

            if (!String.IsNullOrEmpty(subSegment))
            {
                projects = projects.Where(s => s.departments.name.ToUpper() == subSegment.ToUpper());
            }

            if (!String.IsNullOrEmpty(bpssStatus))
            {
                projects = projects.Where(s => s.status.ToUpper() == bpssStatus.ToUpper());
            }

            if (!String.IsNullOrEmpty(status))
            {
                projects = projects.Where(s => s.status.ToUpper() == status.ToUpper());
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            ViewBag.Count = projects.ToList().Count();

            return View(projects.OrderByDescending(p => p.project_id).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult IndexMy(string location, string segment, string subSegment, string bpssStatus, string status)
        {
            var projects = from m in db.Projects.ToList()
                           select m;

            var locationList = new List<string>();
            var locationQry = from d in db.Projects.ToList()
                              orderby d.departments.locations.country
                              select d.departments.locations.country;

            locationList.AddRange(locationQry.Distinct());
            ViewBag.location = new SelectList(locationList);

            var segmentList = new List<string>();
            var segmentQry = from d in db.Projects.ToList()
                             orderby d.departments.name
                             select d.departments.name;

            segmentList.AddRange(segmentQry.Distinct());
            ViewBag.segment = new SelectList(segmentList);

            var subSegmentList = new List<string>();
            var subSegmentQry = from d in db.Projects.ToList()
                                orderby d.departments.name
                                select d.departments.name;

            subSegmentList.AddRange(subSegmentQry.Distinct());
            ViewBag.subSegment = new SelectList(subSegmentList);

            //db.feedbacks.Where(f => f.projects.project_name == 'BPSS').toList()

            var bpssStatusList = new List<string>();
            var bpssStatusQry = from d in db.Projects.ToList()
                                orderby d.status
                                select d.status;

            bpssStatusList.AddRange(bpssStatusQry.Distinct());
            ViewBag.bpssStatus = new SelectList(bpssStatusList);

            var statusList = new List<string>();
            var statusQry = from d in db.Projects.ToList()
                            orderby d.status
                            select d.status;


            statusList.AddRange(statusQry.Distinct());
            ViewBag.status = new SelectList(statusList);

            if (!String.IsNullOrEmpty(location))
            {
                projects = projects.Where(s => s.departments.locations.country.ToUpper() == location.ToUpper());
            }

            if (!String.IsNullOrEmpty(segment))
            {
                projects = projects.Where(s => s.departments.name.ToUpper() == segment.ToUpper());
            }

            if (!String.IsNullOrEmpty(subSegment))
            {
                projects = projects.Where(s => s.departments.name.ToUpper() == subSegment.ToUpper());
            }

            if (!String.IsNullOrEmpty(bpssStatus))
            {
                projects = projects.Where(s => s.status.ToUpper() == bpssStatus.ToUpper());
            }

            if (!String.IsNullOrEmpty(status))
            {
                projects = projects.Where(s => s.status.ToUpper() == status.ToUpper());
            }


            List<int> user_projects;
            user_projects = db.Users_projects.Where(up => up.project_role != "partner").Select(up => up.project_id).ToList();
            return View(db.Projects.Where(f => user_projects.Any(p => p == f.project_id)).ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "project_id,acronym,name,description,start_date,end_date,status,department_id")] Projects projects)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(projects);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projects);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "project_id,acronym,name,description,start_date,end_date,status,department_id")] Projects projects)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projects).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projects);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Projects projects = db.Projects.Find(id);
            db.Projects.Remove(projects);
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
