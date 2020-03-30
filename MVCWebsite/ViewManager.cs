using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCWebsite
{
    public static class ViewManager
    {
        public static string RenderView(this Controller controller, string viewname, object model)
        {
            string html = null;

            controller.ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewname);
                ViewContext viewcontext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewcontext, sw);

                html = sw.GetStringBuilder().ToString();
            }

            return html;
        }
    }
}