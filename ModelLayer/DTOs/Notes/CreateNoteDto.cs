using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTOs.Notes
{
    public class CreateNoteDto
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
    }
}
