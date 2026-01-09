using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataLayer.Context;
using DataLayer.Repositories.Interfaces;
using ModelLayer.Entities;

namespace DataLayer.Repositories.Implementations
{
    public class LabelRepository : ILabelRepository
    {
        private readonly FundooContext _context;

        public LabelRepository(FundooContext context)
        {
            _context = context;
        }

        public async Task<List<Label>> GetAllAsync(int userId)
        {
            return await _context.Labels
                                 .Where(l => l.UserId == userId)
                                 .ToListAsync();
        }

        public async Task<Label?> GetByIdAsync(int id, int userId)
        {
            return await _context.Labels
                                 .FirstOrDefaultAsync(l =>
                                     l.LabelId == id &&
                                     l.UserId == userId);
        }

        public async Task AddAsync(Label label)
        {
            await _context.Labels.AddAsync(label);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Label label)
        {
            _context.Labels.Update(label);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Label label)
        {
            _context.Labels.Remove(label);
            await _context.SaveChangesAsync();
        }
    }
}
