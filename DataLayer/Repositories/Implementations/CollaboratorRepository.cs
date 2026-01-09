using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataLayer.Context;
using DataLayer.Repositories.Interfaces;
using ModelLayer.Entities;

namespace DataLayer.Repositories.Implementations
{
    public class CollaboratorRepository : ICollaboratorRepository
    {
        private readonly FundooContext _context;

        public CollaboratorRepository(FundooContext context)
        {
            _context = context;
        }

        public async Task<List<Collaborator>> GetByNoteIdAsync(int noteId)
        {
            return await _context.Collaborators
                                 .Where(c => c.NoteId == noteId)
                                 .ToListAsync();
        }

        public async Task<Collaborator?> GetByIdAsync(int id)
        {
            return await _context.Collaborators
                                 .FirstOrDefaultAsync(c => c.CollaboratorId == id);
        }

        public async Task AddAsync(Collaborator collaborator)
        {
            await _context.Collaborators.AddAsync(collaborator);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Collaborator collaborator)
        {
            _context.Collaborators.Update(collaborator);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Collaborator collaborator)
        {
            _context.Collaborators.Remove(collaborator);
            await _context.SaveChangesAsync();
        }
    }
}
