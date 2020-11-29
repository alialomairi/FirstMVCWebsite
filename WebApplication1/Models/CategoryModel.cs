using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllEngineers.Models
{
    public class CategoryModel
    {
        public string CategoryName { get; set; }
        public string CategoryKey { get; set; }
        public int? ParentId { get; set; }
    }
}