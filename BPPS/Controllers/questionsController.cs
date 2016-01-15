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
    //[Authorize(Roles = "admin")]
    public class questionsController : Controller
    {
        private Entities db = new Entities();

        // GET: questions
        public ActionResult IndexPartner()
        {
            List<int> partnerFeedback;

            partnerFeedback = db.questions.Where(up => up.for_project_role == "partner").Select(up => up.question_id).ToList();
            return View(db.questions.Where(f => partnerFeedback.Any(p => p == f.question_id)).OrderByDescending(p => p.question_id));
        }
        public ActionResult IndexSiemens()
        {
            List<int> siemensFeedback;

            siemensFeedback = db.questions.Where(up => up.for_project_role == "siemens").Select(up => up.question_id).ToList();
            return View(db.questions.Where(f => siemensFeedback.Any(p => p == f.question_id)).OrderByDescending(p => p.question_id));            
        }
        // GET: questions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            questions questions = db.questions.Find(id);
            if (questions == null)
            {
                return HttpNotFound();
            }
            return View(questions);
        }

        // GET: questions/Create
        public ActionResult CreateForSiemens()
        {
            return PartialView();
        }
        public ActionResult CreateForPartner()
        {
            return PartialView();
        }
        // POST: questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateForSiemens([Bind(Include = "question")] questions questions)
        {
            if (ModelState.IsValid)
            {
                questions.for_project_role = "siemens";
                questions.deprecated = "n";
                db.questions.Add(questions);
                db.SaveChanges();
                return RedirectToAction("IndexSiemens");
            }

            return View(questions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateForPartner([Bind(Include = "question")] questions questions)
        {
            if (ModelState.IsValid)
            {
                questions.for_project_role = "partner";
                questions.deprecated = "n";
                db.questions.Add(questions);
                db.SaveChanges();
                return RedirectToAction("IndexPartner");
            }

            return View(questions);
        }

        // GET: questions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            questions questions = db.questions.Find(id);
            if (questions == null)
            {
                return HttpNotFound();
            }
            return View(questions);
        }

        // POST: questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "question_id,question")] questions questions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(questions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(questions);
        }

        // GET: questions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            questions questions = db.questions.Find(id);
            if (questions == null)
            {
                return HttpNotFound();
            }
            return View(questions);
        }

        // POST: questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            questions questions = db.questions.Find(id);
            if (TryUpdateModel(questions))
            {
                questions.deprecated = "y";
                db.SaveChanges();
            }
            //db.questions.Remove(questions);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Remove(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            questions questions = db.questions.Find(id);
            if (TryUpdateModel(questions))
            {
                questions.deprecated = "y";
                db.SaveChanges();
            }
            //db.questions.Remove(questions);
            //db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult Add(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            questions questions = db.questions.Find(id);
            if (TryUpdateModel(questions))
            {
                questions.deprecated = "n";
                db.SaveChanges();
            }
            //db.questions.Remove(questions);
            //db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
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
