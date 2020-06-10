using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllEngineers.Models.Response
{
    public class HtmlResponse: JsonResponse
    {
        public string html { get; set; }
    }
}