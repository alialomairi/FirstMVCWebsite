using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MVCWebsite.Models
{
    public class ResetAccountModel
    {
        [Required(ErrorMessage = "Required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Required")]
        [Compare("Password",ErrorMessage = "Required")]
        public string Confirm { get; set; }

    }
}
