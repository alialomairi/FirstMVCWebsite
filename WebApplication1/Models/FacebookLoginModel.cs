using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace AllEngineers.Models
{
    public class FacebookLoginModel
    {
        [Required]
        public string id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string first_name { get; set; }
        [Required]
        public string picture { get; set; }
        [Required]
        public string email { get; set; }

    }
}
