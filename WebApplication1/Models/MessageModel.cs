using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace AllEngineers.Models
{
    public class MessageModel
    {
        [Required(ErrorMessage = "Required")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Text { get; set; }
        [Required(ErrorMessage = "Required")]
        public byte Type { get; set; }
        [Required(ErrorMessage = "Required")]
        public int? ParentId { get; set; }

    }
}
