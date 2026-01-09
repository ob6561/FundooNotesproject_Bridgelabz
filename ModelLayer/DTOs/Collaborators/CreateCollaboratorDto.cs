using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTOs.Collaborators
{
    public class CreateCollaboratorDto
    {
        public int NoteId {  get; set; }
        public string Email {  get; set; }
        public int Permission {  get; set; }
    }
}
