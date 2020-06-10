using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AllEngineers.Controllers
{
    public class categoriesController : Controller
    {
        private readonly TeachMeContext ctx = new TeachMeContext();
        protected override void Dispose(bool disposing)
        {
            ctx.Dispose();
            base.Dispose(disposing);
        }
        // GET: categories
        public ActionResult Index(int id)
        {
            Category category = ctx.Categories.Find(id);
            ViewBag.Title = category.CategoryName;
            return View(category);
        }
    }
}