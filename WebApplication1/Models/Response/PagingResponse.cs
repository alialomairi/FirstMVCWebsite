using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllEngineers.Models.Response
{
    public class PagingResponse:HtmlResponse
    {
        public int totalCount { get; set; }
    }
}