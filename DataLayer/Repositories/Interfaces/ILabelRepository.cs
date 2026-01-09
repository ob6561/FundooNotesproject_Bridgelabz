using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Entities;

namespace DataLayer.Repositories.Interfaces
{
    public interface ILabelRepository
    {
        Task<List<Label>> GetAllAsync(int userId);
        Task<Label?> GetByIdAsync(int id, int userId);
        Task AddAsync(Label label);
        Task UpdateAsync(Label label);
        Task DeleteAsync(Label label);
    }
}
