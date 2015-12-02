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
using Postal;

namespace BPPS.Controllers
{
    [Authorize(Roles = "admin, siemens, partner")]
    public class feedbacksController : Controller
    {
        private Entities db = new Entities();

        [Authorize(Roles = "admin, siemens")]
        // GET: feedbacks
        public ActionResult Index(string project_name, string buisness_partner, int? page)
        {
            /*var feedbacks = from m in db.feedbacks select m;
            var projects = from m in db.Projects.ToList()
                           select m;

            int[] ids = new int[] { };
            */
            int pageSize = 5;
            int pageNumber = (page ?? 1);


            ViewBag.buisness_partner = new SelectList(db.Users_projects.Where(up => up.project_role == "partner").Select(up => up.AspNetUsers.FirstName).Distinct().ToList());

            List<feedbacks> feedback = db.feedbacks.ToList();
            if (!String.IsNullOrEmpty(project_name))
            {
                feedback = feedback.Where(f => f.Projects.name.ToUpper().Contains(project_name.ToUpper())).ToList();
            }

            if (!String.IsNullOrEmpty(buisness_partner))
            {
                feedback = feedback.Where(fe => fe.AspNetUsers.FirstName.ToUpper() == buisness_partner.ToUpper()).ToList();
            }
            if (!String.IsNullOrEmpty(buisness_partner) || !String.IsNullOrEmpty(project_name))
            {
                return View(feedback.ToPagedList(pageNumber, pageSize));
            }

            return View(db.feedbacks.OrderByDescending(f => f.received).ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "admin, siemens")]
        public ActionResult IndexOnMy(int? page)
        {
            List<int> user_projects;
            string sessionId = User.Identity.GetUserId();

            int pageSize = 5;
            int pageNumber = (page ?? 1);

          user_projects = db.Users_projects.Where(up => up.Id == sessionId && up.project_role != "partner").Select(up => up.project_id).ToList();
            return View(db.feedbacks.Where(f => user_projects.Any(p => p == f.project_id)).OrderByDescending(p => p.feedback_id).ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "admin, siemens, partner")]
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


        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    feedbacks feedbacks = db.feedbacks.Find(id);
        //    if (feedbacks == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    else
        //    {
        //        db.feedbacks.Remove(feedbacks);
        //        db.SaveChanges();
        //        TempData["successDeletedFeedback"] = "Feedback  was deleted.";
        //    }
        //    return View(feedbacks);
        //}

        [Authorize(Roles = "admin, siemens")]
        public ActionResult SendFeedbackEmail(int? id)
        {
            feedbacks feedback = db.feedbacks.Find(id);
            feedback.initiated = DateTime.Now;
            db.Entry(feedback).State = EntityState.Modified;
            db.SaveChanges();

            dynamic email = new Email("EmailExample");
            email.to = feedback.AspNetUsers.Email;
            email.Send();

            TempData["initiated_feedback"] = "Email was sent to partner.";
            return RedirectToAction("IndexOnMy", "feedbacks");
        }

        [Authorize(Roles = "admin, siemens")]
        // GET: feedbacks/Create
        public ActionResult Create(int? project_id)
        {
            Users_projects user_id;
            List<feedback_questions> feedback;

            if (project_id != null)
            {
                ViewBag.project_id = new SelectList(db.Projects.Where(p => p.project_id == project_id), "project_id", "name");
                ViewBag.Id = new SelectList(db.Users_projects.Where(p => p.project_id == project_id && p.project_role == "partner").Select(up => new { up.AspNetUsers.LastName, up.AspNetUsers.Id }), "Id", "LastName");
                user_id = db.Users_projects.Single(p => p.project_id == project_id && p.project_role == "partner");
                feedback = db.feedback_questions.Where(fq => fq.feedbacks.Projects.project_id == project_id && fq.feedbacks.Id == user_id.Id).ToList();
                /*if (feedback[0].feedbacks.initiated != null)
                {
                    ViewBag["infoF"] = "Feedback is already initiated and sent to partner!";
                    return View();
                }*/
                if (db.feedbacks.Where(f => f.project_id == project_id && f.Id == user_id.Id).ToList().Count() >= 1)
                {
                    return RedirectToAction("Edit", "feedback_questions", new { id = feedback[0].feedback_id });
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
        [Authorize(Roles = "admin, siemens")]
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

                TempData["successCreatedFeedback"] = "Feedback successfuly created.";
                return RedirectToAction("IndexOnMy", "feedbacks");

                //return RedirectToAction("Create", "feedback_questions", new { feedback_id = feedbacks.feedback_id });
            }
            return View(feedbacks);
        }

        [Authorize(Roles = "admin, siemens")]
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
        [Authorize(Roles = "admin, siemens")]
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

        [Authorize(Roles = "admin, siemens")]
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

        [Authorize(Roles = "admin, siemens")]
        // POST: feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            List<feedback_questions> feedbacks_question = db.feedback_questions.Where(f => f.feedback_id == id).ToList();
            feedbacks feedbacks = db.feedbacks.Find(id);
            db.feedback_questions.RemoveRange(feedbacks_question);
            db.feedbacks.Remove(feedbacks);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin, siemens")]
        public ActionResult CommentDetail(int feedback_id)
        {
            feedback_questions comment;
            comment = db.feedback_questions.Single(fq => fq.feedback_id == feedback_id);
            return PartialView("_CommentDetail", comment);
        }

        [Authorize(Roles = "admin, siemens")]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize(Roles = "admin, siemens")]
        public ActionResult ExportData(int? id)
        {
            var datasource = db.feedback_questions.Where(f => f.feedback_id == id).Select(f => new { f.questions.question, f.result, f.comment }).ToList();

            string FilePath = Server.MapPath("~/Temp/");
            //string FileName = "test.txt";

            GridView gv = new GridView();
            gv.DataSource = datasource;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Report_" + id + ".xls");
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

        [Authorize(Roles = "admin, siemens")]
        public ActionResult ExportData2()
        {
            var datasource = db.feedback_questions.OrderBy(f => f.feedbacks.Projects.name).ThenBy(f => f.feedbacks.AspNetUsers.Email).Select(f => new { f.feedbacks.Projects.name, f.feedbacks.AspNetUsers.Email, f.questions.question, f.result, f.comment }).ToList();

            GridView gv = new GridView();
            gv.DataSource = datasource;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Report_all.xls");
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

        [Authorize(Roles = "admin, siemens")]
        public ActionResult PrintPDF(int? id)
        {
            return new ActionAsPdf("Details", new { id = id })
            {
                FileName = "Report_" + id + ".pdf"
            };
        }

        [Authorize(Roles = "admin, siemens")]
        public ActionResult PrintPDF2()
        {
            return new ActionAsPdf("../feedback_questions/All")
            {
                FileName = "Report_all.pdf"
            };
        }
    }
}
