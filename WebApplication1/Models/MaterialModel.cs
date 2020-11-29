using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AllEngineers.Models
{
    public class MaterialModel
    {
        public string TutorialName { get; set; }
        public string TutorialKey { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public int? TrainerId { get; set; }
        public int? CategoryId { get; set; }

    }
}