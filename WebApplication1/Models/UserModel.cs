using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace AllEngineers.Models
{
    public class UserModel
    {
        
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
        public string Password { get; set; }
        [Compare("Password",ErrorMessage = "Dosen't Match")]
        public string Confirm { get; set; }
        public byte Role { get; set; }

    }
}
