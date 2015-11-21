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
using ExporterObjects;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using Rotativa;

namespace BPPS.Controllers
{
    public class HomeController : Controller
    {
        private Entities db = new Entities();
        private List<feedbacks> newFeedbacks;

        public ActionResult Index()
        {
            string sessionUserId = User.Identity.GetUserId();
            this.newFeedbacks = db.feedbacks
                .Where(f => f.Id == sessionUserId).
                Where(f => f.received == null)
                .Include(p => p.Projects).ToList();
            ViewBag.hasNewFeedbacks = this.newFeedbacks.Count >= 1 ? true : false;
            ViewBag.newFeedbacks = this.newFeedbacks;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //public virtual ActionResult ExportTo(int id)
        //{
        //    List<Export> list = Export.GetData();
        //    ExportList<Export> exp = new ExportList<Export>();
        //    exp.PathTemplateFolder = Server.MapPath("~/Export");

        //    string filePathExport = Server.MapPath("~/Export/a" + ExportBase.GetFileExtension((ExportToFormat)id));
        //    exp.ExportTo(list, (ExportToFormat)id, filePathExport);
        //    return this.File(filePathExport, "application/octet-stream", System.IO.Path.GetFileName(filePathExport));
        //}
    }
}