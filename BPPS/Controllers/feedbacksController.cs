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
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace BPPS.Controllers
{
    [Authorize(Roles = "admin, siemens, partner")]
    public class feedbacksController : Controller
    {
        private Entities db = new Entities();
        DateTime result;
        DateTime initiated;
        DataTable dtPDF = new DataTable();
        [Authorize(Roles = "admin, siemens")]
        // GET: feedbacks
        public ActionResult Index(string project_name, string buisness_partner, int? page, string Feedback_initialized, string Feedback_received)
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

            if (!String.IsNullOrEmpty(Feedback_initialized))
            {
                result = DateTime.ParseExact(Feedback_initialized, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                
                feedback = feedback.Where(fe => fe.initiated.Value.Date.ToShortDateString() == result.Date.ToShortDateString()).ToList();
                
            }

            if (!String.IsNullOrEmpty(Feedback_received))
            {
                result = DateTime.ParseExact(Feedback_received, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                feedback = feedback.Where(fe => fe.received.Value.Date.ToShortDateString() == result.Date.ToShortDateString()).ToList();
               
            }

            if (!String.IsNullOrEmpty(buisness_partner) || !String.IsNullOrEmpty(project_name))
            {
                Session["feedbackList"] = feedback;
                return View(feedback.ToPagedList(pageNumber, pageSize));
            }

            Session["feedbackList"] = feedback;
            return View(feedback.OrderByDescending(f => f.received).ToPagedList(pageNumber, pageSize));
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

        [Authorize(Roles = "admin, siemens,partner")]
        public ActionResult IndexForMe(int? page)
        {
            List<int> user_projects;
            string sessionId = User.Identity.GetUserId();

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            user_projects = db.Users_projects.Where(up => up.Id == sessionId && up.project_role == "partner").Select(up => up.project_id).ToList();
            return View(db.feedbacks.Where(f => user_projects.Any(p => p == f.project_id)).OrderByDescending(p => p.feedback_id).ToPagedList(pageNumber, pageSize));
        }


        [Authorize(Roles = "admin, siemens, partner")]
        public ActionResult IndexMy(int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            string sessionId = User.Identity.GetUserId();

            return View(db.feedbacks.Where(f => f.Id == sessionId && f.initiated != null).OrderByDescending(p => p.feedback_id).ToPagedList(pageNumber, pageSize));
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
                List<string> zoznam = new List<string>();
                zoznam.Add("Daco");
                zoznam.Add("Zaco");
                zoznam.Add("Nieco");

                ViewBag.zoznam = zoznam;
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

        [Authorize(Roles = "admin, siemens, partner")]
        public ActionResult SendFeedbackEmail2(int? id)
        {
            feedbacks feedback = db.feedbacks.Find(id);
            feedback.initiated = DateTime.Now;
            db.Entry(feedback).State = EntityState.Modified;
            db.SaveChanges();

            dynamic email = new Email("EmailExample");
            email.to = feedback.AspNetUsers.Email;
            email.Send();

            TempData["initiated_feedback"] = "Email was sent to siemens member";
            return RedirectToAction("IndexForMe", "feedbacks");
        }

        [Authorize(Roles = "admin, siemens")]
        // GET: feedbacks/Create
        public ActionResult Create(int? project_id)
        {
            List<Users_projects> user_id;
            List<string> Email;
            List<feedbacks> fe;
            int i = 0;

            if (project_id != null)
            {
                ViewBag.project_id = new SelectList(db.Projects.Where(p => p.project_id == project_id), "project_id", "name");
                //      ViewBag.Id = new SelectList(db.Users_projects.Where(p => p.project_id == project_id && p.project_role == "partner").Select(up => new { up.AspNetUsers.FirstName,up.AspNetUsers.LastName, up.AspNetUsers.Id }), "Id", "LastName", "FirstName");
                Email = db.Users_projects.Where(p => p.project_id == project_id && p.project_role == "partner").Select(up => up.AspNetUsers.Email).ToList();
                ViewBag.Email = Email;
                ViewBag.FirstName = db.Users_projects.Where(p => p.project_id == project_id && p.project_role == "partner").Select(up => up.AspNetUsers.FirstName).ToList();
                ViewBag.LastName = db.Users_projects.Where(p => p.project_id == project_id && p.project_role == "partner").Select(up => up.AspNetUsers.LastName).ToList();

                user_id = db.Users_projects.Where(p => p.project_id == project_id && p.project_role == "partner").ToList();

                fe = db.feedbacks.Where(f => f.project_id == project_id).ToList();

                string[] emails = new string[Email.Count];

                foreach (var fee in fe)
                {
                    emails[i] = fee.AspNetUsers.Email;
                    i++;
                }

                ViewBag.hasFeedback = emails;


                //feedback = db.feedback_questions.Where(fq => fq.feedbacks.Projects.project_id == project_id && fq.feedbacks.Id == user_id.Id).ToList();
                /*if (feedback[0].feedbacks.initiated != null)
                {
                    ViewBag["infoF"] = "Feedback is already initiated and sent to partner!";
                    return View();
                }*/
              /*  if (db.feedbacks.Where(f => f.project_id == project_id && f.Id == user_id.Id).ToList().Count() >= 1)
                {
                    return RedirectToAction("Edit", "feedback_questions", new { id = feedback[0].feedback_id });
                }*/
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
        public ActionResult Create([Bind(Include = "project_id")] feedbacks feedbacks, List<string> partners)
        {

            int a = partners.Count();

            string[] partnerIDs = new string[a];

            AspNetUsers anu;

            int inc = 0;

             foreach (var partner in partners)
             {
                
            anu = db.AspNetUsers.Single(asn => asn.Email == partner);
            partnerIDs[inc] = anu.Id;
                inc++;
            }


            inc = 0;
            int iterations = 0;
            int count = partnerIDs.Count();
            iterations = count / 2;
            int[] b = new int[a];

            if (ModelState.IsValid)
            {
                foreach (var id in partnerIDs)
                {
                    // return RedirectToAction("IndexMy", "feedbacks", new { perkelt = partnerIDs.Count()});

                    feedbacks.Id = id;
                    feedbacks.initiated = null;
                    feedbacks.received = null;
                    db.feedbacks.Add(feedbacks);
                    db.SaveChanges();
                    b[inc] = feedbacks.feedback_id;
                    inc++;

                }

                inc = 0;
                
                foreach (var c in b) { 

                foreach (var question in db.questions.Where(q => q.deprecated == "n" && q.for_project_role == "partner").ToList())
                {
                    feedback_questions feedback_question = new feedback_questions();
                    feedback_question.feedback_id = c;
                    feedback_question.question_id = question.question_id;

                    db.feedback_questions.Add(feedback_question);
                    db.SaveChanges();
                }
            }

                TempData["successCreatedFeedback"] = "Úspešne ste vytvorili feedback";
                      return RedirectToAction("IndexOnMy", "feedbacks");

                      //return RedirectToAction("Create", "feedback_questions", new { feedback_id = feedbacks.feedback_id });
                  }
                  
            return RedirectToAction("Index", "locations");
        }

        [Authorize(Roles = "admin, siemens, partner")]
        // GET: feedbacks/Create
        public ActionResult CreateToSiemens(int? project_id)
        {
            List<Users_projects> user_id;
            List<string> Email;
            List<feedbacks> fe;
            int i = 0;

            if (project_id != null)
            {
                ViewBag.project_id = new SelectList(db.Projects.Where(p => p.project_id == project_id), "project_id", "name");
                //      ViewBag.Id = new SelectList(db.Users_projects.Where(p => p.project_id == project_id && p.project_role == "partner").Select(up => new { up.AspNetUsers.FirstName,up.AspNetUsers.LastName, up.AspNetUsers.Id }), "Id", "LastName", "FirstName");
                Email = db.Users_projects.Where(p => p.project_id == project_id && p.project_role != "partner").Select(up => up.AspNetUsers.Email).ToList();
                ViewBag.Email = Email;
                ViewBag.FirstName = db.Users_projects.Where(p => p.project_id == project_id && p.project_role != "partner").Select(up => up.AspNetUsers.FirstName).ToList();
                ViewBag.LastName = db.Users_projects.Where(p => p.project_id == project_id && p.project_role != "partner").Select(up => up.AspNetUsers.LastName).ToList();

                user_id = db.Users_projects.Where(p => p.project_id == project_id && p.project_role != "partner").ToList();

                fe = db.feedbacks.Where(f => f.project_id == project_id).ToList();

                string[] emails = new string[Email.Count];

                foreach (var fee in fe)
                {
                    emails[i] = fee.AspNetUsers.Email;
                    i++;
                }

                ViewBag.hasFeedback = emails;


                //feedback = db.feedback_questions.Where(fq => fq.feedbacks.Projects.project_id == project_id && fq.feedbacks.Id == user_id.Id).ToList();
                /*if (feedback[0].feedbacks.initiated != null)
                {
                    ViewBag["infoF"] = "Feedback is already initiated and sent to partner!";
                    return View();
                }*/
                /*  if (db.feedbacks.Where(f => f.project_id == project_id && f.Id == user_id.Id).ToList().Count() >= 1)
                  {
                      return RedirectToAction("Edit", "feedback_questions", new { id = feedback[0].feedback_id });
                  }*/
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
        [Authorize(Roles = "admin, siemens, partner")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateToSiemens([Bind(Include = "project_id")] feedbacks feedbacks, List<string> partners)
        {

            int a = partners.Count();

            string[] partnerIDs = new string[a];

            AspNetUsers anu;

            int inc = 0;

            foreach (var partner in partners)
            {

                anu = db.AspNetUsers.Single(asn => asn.Email == partner);
                partnerIDs[inc] = anu.Id;
                inc++;
            }


            inc = 0;
            int iterations = 0;
            int count = partnerIDs.Count();
            iterations = count / 2;
            int[] b = new int[a];

            if (ModelState.IsValid)
            {
                foreach (var id in partnerIDs)
                {
                    // return RedirectToAction("IndexMy", "feedbacks", new { perkelt = partnerIDs.Count()});

                    feedbacks.Id = id;
                    feedbacks.initiated = null;
                    feedbacks.received = null;
                    db.feedbacks.Add(feedbacks);
                    db.SaveChanges();
                    b[inc] = feedbacks.feedback_id;
                    inc++;

                }

                inc = 0;

                foreach (var c in b)
                {

                    foreach (var question in db.questions.Where(q => q.deprecated == "n" && q.for_project_role == "siemens").ToList())
                    {
                        feedback_questions feedback_question = new feedback_questions();
                        feedback_question.feedback_id = c;
                        feedback_question.question_id = question.question_id;

                        db.feedback_questions.Add(feedback_question);
                        db.SaveChanges();
                    }
                }

                TempData["successCreatedFeedback"] = "Úspešne ste vytvorili feedback";
                return RedirectToAction("IndexForMe", "feedbacks");

                //return RedirectToAction("Create", "feedback_questions", new { feedback_id = feedbacks.feedback_id });
            }
            return RedirectToAction("Index", "locations");
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
            var datasource2 = (List<feedbacks>)Session["feedbackList"];
            GridView gv = new GridView();
            gv.DataSource = datasource2;
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
      
        //protected void btnPDF_Click(object sender, ImageClickEventArgs e)
        //{
        //    DataTable dtn = new DataTable();
        //    dtn = GetDataTable();
        //    dtPDF = dtn.Copy();
        //    for (int i = 0; i <= dtn.Rows.Count - 1; i++)
        //    {
        //        ExportToPdf(dtPDF);
        //    }
        //}

        //public void ExportToPdf(DataTable myDataTable)
        //{
        //    Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
        //    try
        //    {
        //        PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);
        //        pdfDoc.Open();
        //        Chunk c = new Chunk("" + System.Web.HttpContext.Current.Session["CompanyName"] + "", FontFactory.GetFont("Verdana", 11));
        //        Paragraph p = new Paragraph();
        //        p.Alignment = Element.ALIGN_CENTER;
        //        p.Add(c);
        //        pdfDoc.Add(p);
        //        string clientLogo = Server.MapPath(".") + "/logo/tpglogo.jpg";
        //        string imageFilePath = Server.MapPath(".") + "/logo/tpglogo.jpg";
        //        iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageFilePath);
        //        //Resize image depend upon your need   
        //        jpg.ScaleToFit(80f, 60f);
        //        //Give space before image   
        //        jpg.SpacingBefore = 0f;
        //        //Give some space after the image   
        //        jpg.SpacingAfter = 1f;
        //        jpg.Alignment = Element.HEADER;
        //        pdfDoc.Add(jpg);
        //        Font font8 = FontFactory.GetFont("ARIAL", 7);
        //        DataTable dt = myDataTable;
        //        if (dt != null)
        //        {
        //            //Craete instance of the pdf table and set the number of column in that table  
        //            PdfPTable PdfTable = new PdfPTable(dt.Columns.Count);
        //            PdfPCell PdfPCell = null;
        //            for (int rows = 0; rows < dt.Rows.Count; rows++)
        //            {
        //                for (int column = 0; column < dt.Columns.Count; column++)
        //                {
        //                    PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows][column].ToString(), font8)));
        //                    PdfTable.AddCell(PdfPCell);
        //                }
        //            }
        //            //PdfTable.SpacingBefore = 15f; // Give some space after the text or it may overlap the table            
        //            pdfDoc.Add(PdfTable); // add pdf table to the document   
        //        }
        //        pdfDoc.Close();
        //        Response.ContentType = "application/pdf";
        //        Response.AddHeader("content-disposition", "attachment; filename= SampleExport.pdf");
        //        System.Web.HttpContext.Current.Response.Write(pdfDoc);
        //        Response.Flush();
        //        Response.End();
        //        //HttpContext.Current.ApplicationInstance.CompleteRequest();  
        //    }
        //    catch (DocumentException de)
        //    {
        //        System.Web.HttpContext.Current.Response.Write(de.Message);
        //    }
        //    catch (IOException ioEx)
        //    {
        //        System.Web.HttpContext.Current.Response.Write(ioEx.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Web.HttpContext.Current.Response.Write(ex.Message);
        //    }
        //}
    }
}
