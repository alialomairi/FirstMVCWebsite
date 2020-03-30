using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MVCWebsite.Controllers
{
    public class ContentController : Controller
    {
        //
        // GET: /Content/

        public JavaScriptResult Extenders()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("(function ($) {");

            foreach (string file in Directory.GetFiles(Server.MapPath("~/js/extenders"), "*.js"))
                builder.Append(System.IO.File.ReadAllText(file));

            builder.Append("})(window.jQuery);");
            return JavaScript(builder.ToString());
        }
        public ContentResult Styles()
        {
            StringBuilder builder = new StringBuilder();

            foreach (string file in Directory.GetFiles(Server.MapPath("~/css/styles"), "*.css"))
                builder.Append(System.IO.File.ReadAllText(file));

            return Content(builder.ToString(),"text/css");
        }

    }
}
