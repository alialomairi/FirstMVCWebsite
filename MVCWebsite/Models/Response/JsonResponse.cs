﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCWebsite.Models.Response
{
    public class JsonResponse
    {
        public bool success { get; set; }
        public string error { get; set; }
    }
}