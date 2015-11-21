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
using System.Web.UI.WebControls;
using System.IO;
using Rotativa;
using System.Web.UI;

namespace BPPS.Controllers
{
    [Authorize(Roles = "admin, siemens")]
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

        [AllowAnonymous]
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

        public ActionResult SendFeedbackEmail(int? id)
        {
            feedbacks feedback = db.feedbacks.Find(id);
            feedback.initiated = DateTime.Now;
            db.Entry(feedback).State = EntityState.Modified;
            db.SaveChanges();
            
            TempData["initiated_feedback"] = "Email was send to partner.";
            return RedirectToAction("IndexOnMy", "feedbacks");
        }


        // GET: feedbacks/Create
        public ActionResult Create(int? project_id)
        {
            Users_projects user_id;
            List<feedback_questions> feedback;

            if(project_id != null)
            {
                ViewBag.project_id = new SelectList(db.Projects.Where(p => p.project_id == project_id), "project_id", "name");
                ViewBag.Id = new SelectList(db.Users_projects.Where(p => p.project_id == project_id && p.project_role == "partner").Select(up => new { up.AspNetUsers.LastName, up.AspNetUsers.Id }), "Id", "LastName");
                user_id = db.Users_projects.Single(p => p.project_id == project_id && p.project_role == "partner");
                feedback = db.feedback_questions.Where(fq => fq.feedbacks.Projects.project_id == project_id && fq.feedbacks.Id == user_id.Id).ToList();
                if (db.feedbacks.Where(f => f.project_id == project_id && f.Id == user_id.Id).ToList().Count() >= 1)
                {
                    return RedirectToAction("Details", "feedbacks", new { id = feedback[0].feedback_id });
                }
            }
            else
            {
                ViewBag.project_id = new SelectList(db.Projects, "project_id", "name");
                ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "LastName");
            }
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
                feedbacks.initiated = null;
                feedbacks.received = null;
                db.feedbacks.Add(feedbacks);
                db.SaveChanges();

                foreach (var question in db.questions.ToList())
                {
                    feedback_questions feedback_question = new feedback_questions();
                    feedback_question.feedback_id = feedbacks.feedback_id;
                    feedback_question.question_id = question.question_id;

                    db.feedback_questions.Add(feedback_question);
                    db.SaveChanges();
                }

                TempData["successCreatedFeedback"] = "Úspešne ste vytvorili feedback";
                return RedirectToAction("IndexMy", "feedbacks");

                //return RedirectToAction("Create", "feedback_questions", new { feedback_id = feedbacks.feedback_id });
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

        public ActionResult ExportData(int? id)
        {
            var datasource = db.feedback_questions.Where(f => f.feedback_id == id).Select(f => new { f.questions.question, f.result, f.comment }).ToList();

            GridView gv = new GridView();
            gv.DataSource = datasource;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Report.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return Json("Success");
        }

        public ActionResult PrintPDF(int? id)
        {
            return new ActionAsPdf("Details", new { id = id })
            {
                FileName = "Report.pdf"
            };
        }
    }
}
