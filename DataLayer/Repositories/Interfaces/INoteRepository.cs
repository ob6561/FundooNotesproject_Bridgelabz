using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ModelLayer.Entities;
using System.Collections.Generic;

namespace DataLayer.Repositories.Interfaces
{
    public interface INoteRepository
    {
        IEnumerable<Note> GetAllByUser(int userId);
        Note? GetById(int noteId, int userId);
        void Add(Note note);
        void Update(Note note);
        void Delete(Note note);
    }
}
