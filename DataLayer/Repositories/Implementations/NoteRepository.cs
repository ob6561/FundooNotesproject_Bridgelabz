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

        public IEnumerable<Note> GetAllByUser(int userId)
        {
            return _context.Notes
                           .Where(n => n.UserId == userId)
                           .ToList();
        }

        public Note? GetById(int noteId, int userId)
        {
            return _context.Notes
                           .FirstOrDefault(n => n.NoteId == noteId && n.UserId == userId);
        }

        public void Add(Note note)
        {
            _context.Notes.Add(note);
            _context.SaveChanges();
        }

        public void Update(Note note)
        {
            _context.Notes.Update(note);
            _context.SaveChanges();
        }

        public void Delete(Note note)
        {
            _context.Notes.Remove(note);
            _context.SaveChanges();
        }
    }
}

