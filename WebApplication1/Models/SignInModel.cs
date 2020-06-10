using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AllEngineers.Models
{
    public class SignInModel
    {
        [Required(ErrorMessage="Required")]
        public string Username { get; set; }
        [Required(ErrorMessage="Required")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}