using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Entities
{
    public class User
    {
        public int UserId { get; set; }
        [MaxLength(100)]
        public string FullName { get; set; }
        [MaxLength(50)]
        public string DisplayName { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string Phone { get; set; }
        [MaxLength(10)]
        public string Username { get; set; }
        [MaxLength(10)]
        public string Password { get; set; }
        [MaxLength(50)]
        public string FacebookId { get; set; }

        public bool Enabled { get; set; }

        public UserType UserType { get; set; }
        public Gender? Gender { get; set; }
    }
}
