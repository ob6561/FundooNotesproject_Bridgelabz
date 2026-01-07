using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTOs.Notes
{
    public class BulkDeleteNotesDto
    {
        public List<int> NoteIds {  get; set; }
    }
}
