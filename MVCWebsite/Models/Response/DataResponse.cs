using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCWebsite.Models.Response
{
    public class DataResponse: JsonResponse
    {
        public object data { get; set; }
    }
}