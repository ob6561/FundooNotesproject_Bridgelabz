using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Entities
{
    public class Collaborator
    {
        [Key]
        public int CollaboratorId { get; set; }

        public int NoteId { get; set; }
        public Note Note { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int Permission { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
