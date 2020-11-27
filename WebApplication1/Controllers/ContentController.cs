using Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AllEngineers.Controllers
{
    public class ContentController : Controller
    {
        private readonly TeachMeContext ctx = new TeachMeContext();
        protected override void Dispose(bool disposing)
        {
            ctx.Dispose();
            base.Dispose(disposing);
        }
        //
        // GET: /Content/
        public ActionResult Index(int id)
        {
            ContentPage page = ctx.ContentPages.Find(id);

            ViewBag.Title = page.Title;
            return View(page);
        }

        public JavaScriptResult Extenders()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("(function ($) {");

            foreach (string file in Directory.GetFiles(Server.MapPath("~/Scripts/extenders"), "*.js"))
                builder.Append(System.IO.File.ReadAllText(file));

            builder.Append("})(window.jQuery);");
            return JavaScript(builder.ToString());
        }
        public ContentResult Styles()
        {
            StringBuilder builder = new StringBuilder();

            foreach (string file in Directory.GetFiles(Server.MapPath("~/Css/styles"), "*.css"))
                builder.Append(System.IO.File.ReadAllText(file));

            return Content(builder.ToString(),"text/css");
        }

    }
}
