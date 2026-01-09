using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Entities;

namespace DataLayer.Repositories.Interfaces
{
    public interface ICollaboratorRepository
    {
        Task<List<Collaborator>> GetByNoteIdAsync(int noteId);
        Task<Collaborator?> GetByIdAsync(int id);
        Task AddAsync(Collaborator collaborator);
        Task UpdateAsync(Collaborator collaborator);
        Task DeleteAsync(Collaborator collaborator);
    }
}
