using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AllEngineers.Controllers
{
    public class TutorialsController : Controller
    {
        private TeachMeContext ctx = new TeachMeContext();
        protected override void Dispose(bool disposing)
        {
            ctx.Dispose();
            base.Dispose(disposing);
        }
        // GET: Tutorials
        public ActionResult Index(int id)
        {
            Material material = ctx.Materials.Find(id);

            return View(material);
        }
    }
}