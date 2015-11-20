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
    public class questionsController : Controller
    {
        private Entities db = new Entities();

        // GET: questions
        public ActionResult Index()
        {
            return View(db.questions.ToList());
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "question")] questions questions)
        {
            if (ModelState.IsValid)
            {
                db.questions.Add(questions);
                db.SaveChanges();
                return RedirectToAction("Index");
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
            db.questions.Remove(questions);
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
