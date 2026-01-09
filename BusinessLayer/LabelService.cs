using System.Collections.Generic;
using System.Threading.Tasks;
using DataLayer.Repositories.Interfaces;
using ModelLayer.DTOs.Labels;
using ModelLayer.Entities;

namespace BusinessLayer
{
    public class LabelService
    {
        private readonly ILabelRepository _labelRepository;

        public LabelService(ILabelRepository labelRepository)
        {
            _labelRepository = labelRepository;
        }

        
        public async Task<List<Label>> GetLabelsAsync(int userId)
        {
            return await _labelRepository.GetAllAsync(userId);
        }

        
        public async Task CreateLabelAsync(CreateLabelDto dto, int userId)
        {
            var label = new Label
            {
                Name = dto.Name,
                UserId = userId
            };

            await _labelRepository.AddAsync(label);
        }

        
        public async Task<bool> UpdateLabelAsync(int id, int userId, UpdateLabelDto dto)
        {
            var label = await _labelRepository.GetByIdAsync(id, userId);
            if (label == null) return false;

            label.Name = dto.Name;
            await _labelRepository.UpdateAsync(label);
            return true;
        }

        
        public async Task<bool> DeleteLabelAsync(int id, int userId)
        {
            var label = await _labelRepository.GetByIdAsync(id, userId);
            if (label == null) return false;

            await _labelRepository.DeleteAsync(label);
            return true;
        }
    }
}
