using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BPPS.Models;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using PagedList;


namespace BPPS.Controllers
{
    public class feedbacksController : Controller
    {
        private Entities db = new Entities();

        // GET: feedbacks
        public ActionResult Index(string project_name, int? page)
        {
            var feedbacks = from m in db.feedbacks select m;
            var projects = from m in db.Projects.ToList()
                           select m;

            int[] ids = new int[] { };
       
            if (!String.IsNullOrEmpty(project_name))
            {
                feedbacks = feedbacks.Where(s => projects.Any(p => p.name.ToUpper().Contains(project_name.ToUpper())));
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(db.feedbacks.OrderByDescending(f => f.received).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult IndexOnMy(int? page)
        {
            List<int> user_projects;
            string sessionId = User.Identity.GetUserId();

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            user_projects = db.Users_projects.Where(up => up.Id == sessionId && up.project_role != "partner").Select(up => up.project_id).ToList();
            return View(db.feedbacks.Where(f => user_projects.Any(p => p == f.project_id)).OrderByDescending(p => p.feedback_id).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult IndexMy(int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            string sessionId = User.Identity.GetUserId();

            return View(db.feedbacks.Where(f => f.Id == sessionId).OrderByDescending(p => p.feedback_id).ToPagedList(pageNumber, pageSize));
        }

        // GET: feedbacks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            feedbacks feedbacks = db.feedbacks.Find(id);
            if (feedbacks == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.feedbackResult = db.feedback_questions.Where(f => f.feedback_id == id).ToList();
            }
            return View(feedbacks);
        }

        // GET: feedbacks/Create
        public ActionResult Create()
        {
            ViewBag.project_id = new SelectList(db.Projects, "project_id", "name");
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "LastName");
            return View();
        }

        // POST: feedbacks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "project_id,Id")] feedbacks feedbacks)
        {
            if (ModelState.IsValid)
            {
                feedbacks.initiated = DateTime.Now;
                feedbacks.received = null;
                db.feedbacks.Add(feedbacks);
                db.SaveChanges();
                return RedirectToAction("Create", "feedback_questions", new { feedback_id = feedbacks.feedback_id });
            }
            return View(feedbacks);
        }

        // GET: feedbacks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            feedbacks feedbacks = db.feedbacks.Find(id);
            if (feedbacks == null)
            {
                return HttpNotFound();
            }
            return View(feedbacks);
        }

        // POST: feedbacks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "feedback_id,initiated,received,project_id,Id")] feedbacks feedbacks)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feedbacks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(feedbacks);
        }

        // GET: feedbacks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            feedbacks feedbacks = db.feedbacks.Find(id);
            if (feedbacks == null)
            {
                return HttpNotFound();
            }
            return View(feedbacks);
        }


        // POST: feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            feedbacks feedbacks = db.feedbacks.Find(id);
            db.feedbacks.Remove(feedbacks);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CommentDetail(int feedback_id)
        {
            feedback_questions comment;
            comment = db.feedback_questions.Single(fq => fq.feedback_id == feedback_id);
            return PartialView("_CommentDetail", comment);
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
