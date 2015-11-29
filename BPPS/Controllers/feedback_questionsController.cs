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
    public class feedback_questionsController : Controller
    {
        private Entities db = new Entities();

        // GET: feedback_questions
        public ActionResult Index()
        {
            return View(db.feedback_questions.ToList());
        }

        // GET: feedback_questions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            feedback_questions feedback_questions = db.feedback_questions.Find(id);
            if (feedback_questions == null)
            {
                return HttpNotFound();
            }
            return View(feedback_questions);
        }

        // GET: feedback_questions/Create/5
        public ActionResult Create(int? feedback_id)
        {
            ViewBag.questions = db.questions.Select(q => q.question).ToList();
            ViewBag.question_id = new SelectList(db.questions, "question_id", "question");
            ViewBag.feedback_id = new SelectList(db.feedbacks.Where(f => f.feedback_id == feedback_id), "feedback_id", "feedback_id");                
            return View();
        }

        // POST: feedback_questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "question_id,feedback_id")] feedback_questions feedback_questions)
        {
            List<questions> question;

            if (ModelState.IsValid)

            {
                question = db.questions.Where(q => q.question_id == feedback_questions.question_id).ToList();
                foreach (var q in question)
                {
                    TempData["message"] = "Question: <b><i>" + q.question + "</i></b> was added to feedback. ";
                }
                db.feedback_questions.Add(feedback_questions);
                db.SaveChanges();
                return RedirectToAction("Create", new { feedback_id = feedback_questions.feedback_id});
            }

            return View(feedback_questions);
        }

        // GET: feedback_questions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            feedback_questions feedback_questions = db.feedback_questions.Find(id);
            if (feedback_questions == null)
            {
                return HttpNotFound();
            }
            return View(feedback_questions);

        }

        public ActionResult FeedbackForm(int feedback_id)
        {
            ViewBag.feedback_id = feedback_id;
            ViewBag.feedback = db.feedbacks.Find(feedback_id); 
            return View(db.feedback_questions.Include(q => q.questions).Where(f => f.feedback_id == feedback_id).ToList());
        }

        [HttpPost]
        public ActionResult FeedbackForm(FormCollection question)
        {
            int iterator = 0;
            int i, ia;
            feedback_questions instance;
            feedbacks feedback;

            i = Int32.Parse(question["feedback_id"]);

            foreach (var key in question.AllKeys)
            {   if (iterator >= 1 && key!="feedback_id" && !key.Contains("_comment"))
                {
                        ia = Int32.Parse(key);
                        instance = db.feedback_questions.Single(fq => fq.feedback_id == i &&
                            fq.question_id == ia);
                        instance.result = Int32.Parse(question[key]);
                        instance.comment = question[key + "_comment"];
                        db.Entry(instance).State = EntityState.Modified;
                        db.SaveChanges();
                }
                iterator++;
            }
            feedback = db.feedbacks.Single(f => f.feedback_id == i);
            feedback.received = DateTime.Now;
            db.Entry(feedback).State = EntityState.Modified;
            db.SaveChanges();
            TempData["message"] = "You have successfully submitted feedback";
            return RedirectToAction("Index", "Home");
        }

        // POST: feedback_questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "question_id,feedback_id,result,comment")] feedback_questions feedback_questions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feedback_questions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(feedback_questions);
        }

        // GET: feedback_questions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            feedback_questions feedback_questions = db.feedback_questions.Find(id);
            if (feedback_questions == null)
            {
                return HttpNotFound();
            }
            return View(feedback_questions);
        }

        // POST: feedback_questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            feedback_questions feedback_questions = db.feedback_questions.Find(id);
            db.feedback_questions.Remove(feedback_questions);
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
