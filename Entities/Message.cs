using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Entities
{
    public class Message
    {
        public int MessageId { get; set; }
        public MessageType MessageType { get; set; }
        public string MessageSubject { get; set; }
        public string MessageText { get; set; }

        [ForeignKey("Parent")]
        public int? ParentId { get; set; }
        public virtual Message Parent { get; set; }

        public virtual ICollection<Message> Childs { get; set; }
        public bool HasChilds { get { return Childs!=null&&Childs.Count > 0; } }
    }
}
