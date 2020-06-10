using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AllEngineers
{
    public class ViewEngineConfig
    {
        public static void RegisterViewLocation(ViewEngineCollection viewEngines)
        {
            RazorViewEngine razorEngine = ViewEngines.Engines.OfType<RazorViewEngine>().FirstOrDefault();
            // Add /MyVeryOwn/ folder to the default location scheme for STANDARD Views
            razorEngine.AreaViewLocationFormats =
                razorEngine.ViewLocationFormats.Concat(new string[] { 
                "~/Views/{2}/{1}/{0}.cshtml"
            }).ToArray();

            // Add /MyVeryOwnPartialFolder/ folder to the default location scheme for PARTIAL Views
            razorEngine.AreaPartialViewLocationFormats =
                razorEngine.AreaPartialViewLocationFormats.Concat(new string[] { 
                "~/Views/{2}/ctrl/{0}.cshtml"
            }).ToArray();
        }
    }
}