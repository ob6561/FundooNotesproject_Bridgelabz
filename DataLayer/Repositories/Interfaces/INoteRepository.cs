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
        Task<List<Note>> GetAllByUserAsync(int userId);
        Task<Note?> GetByIdAsync(int noteId, int userId);
        Task AddAsync(Note note);
        Task UpdateAsync(Note note);
        Task DeleteAsync(Note note);
        Task BulkDeleteAsync(List<int> noteIds, int userId);
        Task<List<Note>> SearchNotesAsync(int userId, string query);
    }
}
