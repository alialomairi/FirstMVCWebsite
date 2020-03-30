using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCWebsite.Models.Response
{
    public class RedirectResponse: JsonResponse
    {
        public string redirectUrl { get; set; }
    }
}