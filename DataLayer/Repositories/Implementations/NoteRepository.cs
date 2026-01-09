using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Context;
using DataLayer.Repositories.Interfaces;
using ModelLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories.Implementations
{
    public class NoteRepository : INoteRepository
    {
        private readonly FundooContext _context;

        public NoteRepository(FundooContext context)
        {
            _context = context;
        }

        public async Task<List<Note>> GetAllByUserAsync(int userId)
        {
            return await _context.Notes
                                 .Where(n => n.UserId == userId)
                                 .ToListAsync();
        }

        public async Task<Note?> GetByIdAsync(int noteId, int userId)
        {
            return await _context.Notes
                                 .FirstOrDefaultAsync(n =>
                                     n.NoteId == noteId &&
                                     n.UserId == userId);
        }

        public async Task AddAsync(Note note)
        {
            await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Note note)
        {
            _context.Notes.Update(note);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Note note)
        {
            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Note>> SearchNotesAsync(int userId, string query)
        {
            return await _context.Notes
                .Where(n =>
                    n.UserId == userId &&
                    !n.IsDeleted &&
                    (n.Title.Contains(query) || n.Content.Contains(query))
                )
                .ToListAsync();
        }

        public async Task BulkDeleteAsync(List<int> noteIds, int userId)
        {
            var notes = await _context.Notes
                .Where(n => noteIds.Contains(n.NoteId) && n.UserId == userId)
                .ToListAsync();

            _context.Notes.RemoveRange(notes);
            await _context.SaveChangesAsync();
        }
    }
}

