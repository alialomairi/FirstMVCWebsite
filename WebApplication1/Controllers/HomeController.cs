using Entities;
using AllEngineers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AllEngineers.Controllers
{
    public class HomeController : Controller
    {
        private readonly TeachMeContext ctx = new TeachMeContext();
        protected override void Dispose(bool disposing)
        {
            ctx.Dispose();
            base.Dispose(disposing);
        }
        //
        // GET: /Home/

        public ActionResult Index()
        {
            // take the latest tutorials
            ViewBag.Tutorials = ctx.Materials.OrderByDescending(x => x.CreationDate).Take(3).ToList();

            return View();
        }
        public ActionResult ContactUs()
        {
            return View();
        }
        public ActionResult Category(int id)
        {
            ViewData.Model = ctx.Categories.Find(id);
            return View();
        }
        public ActionResult Tutorial(int id, bool v)
        {
            Material tutorial = ctx.Materials.Find(id);
            ViewData.Model = tutorial;

            ViewBag.CurrentSubject = null;
            ViewBag.ShowVideo = v;
            return View();
        }
        public ActionResult Subject(int id)
        {
            Subject subject = ctx.Subjects.Find(id);
            Material tutorial = subject.Tutorial;

            ViewData.Model = tutorial;
            ViewBag.CurrentSubject = subject;
            ViewBag.ShowVideo = false;
            return View("Tutorial");
        }

    }
}
