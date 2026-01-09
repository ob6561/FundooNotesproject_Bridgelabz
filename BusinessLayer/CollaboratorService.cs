using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataLayer.Repositories.Interfaces;
using ModelLayer.DTOs.Collaborators;
using ModelLayer.Entities;

namespace BusinessLayer
{
    public class CollaboratorService
    {
        private readonly ICollaboratorRepository _collabRepo;
        private readonly IUserRepository _userRepo;

        public CollaboratorService(
            ICollaboratorRepository collabRepo,
            IUserRepository userRepo)
        {
            _collabRepo = collabRepo;
            _userRepo = userRepo;
        }

        
        public async Task<List<Collaborator>> GetByNoteAsync(int noteId)
        {
            return await _collabRepo.GetByNoteIdAsync(noteId);
        }

        
        public async Task AddCollaboratorAsync(CreateCollaboratorDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email);
            if (user == null)
                throw new Exception("User not found");

            var collaborator = new Collaborator
            {
                NoteId = dto.NoteId,
                UserId = user.UserId,
                Permission = dto.Permission
            };

            await _collabRepo.AddAsync(collaborator);
        }

        
        public async Task UpdatePermissionAsync(int id, int permission)
        {
            var collab = await _collabRepo.GetByIdAsync(id);
            if (collab == null)
                throw new Exception("Collaborator not found");

            collab.Permission = permission;
            await _collabRepo.UpdateAsync(collab);
        }

        
        public async Task RemoveAsync(int id)
        {
            var collab = await _collabRepo.GetByIdAsync(id);
            if (collab == null)
                throw new Exception("Collaborator not found");

            await _collabRepo.DeleteAsync(collab);
        }
    }
}
