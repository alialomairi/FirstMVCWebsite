using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MVCWebsite.Models
{
    public class SignUpModel
    {
        public string size { get; set; }
        public string position { get; set; }
        public float zoom { get; set; }
        [Required(ErrorMessage = "Required")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Required")]
        public string DisplayName { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Required")]
        [Compare("Password",ErrorMessage = "Dosen't Match")]
        public string Confirm { get; set; }

    }
}
