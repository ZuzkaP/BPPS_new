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
    }
}