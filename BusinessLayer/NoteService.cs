using DataLayer.Repositories.Interfaces;
using ModelLayer.DTOs.Notes;
using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class NoteService
    {
        private readonly INoteRepository _noteRepository;

        public NoteService(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        
        public async Task<List<Note>> GetNotesAsync(int userId)
        {
            return await _noteRepository.GetAllByUserAsync(userId);
        }

        
        public async Task<Note?> GetNoteByIdAsync(int noteId, int userId)
        {
            return await _noteRepository.GetByIdAsync(noteId, userId);
        }

        
        public async Task CreateNoteAsync(CreateNoteDto dto, int userId)
        {
            var note = new Note
            {
                Title = dto.Title,
                Content = dto.Content,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Color = "#ffffff",
                IsPinned = false,
                IsArchived = false,
                IsDeleted = false
            };

            await _noteRepository.AddAsync(note);
        }

        
        public async Task<bool> UpdateNoteAsync(int noteId, int userId, UpdateNoteDto dto)
        {
            var note = await _noteRepository.GetByIdAsync(noteId, userId);
            if (note == null) return false;

            note.Title = dto.Title;
            note.Content = dto.Content;
            note.UpdatedAt = DateTime.UtcNow;

            await _noteRepository.UpdateAsync(note);
            return true;
        }

        
        public async Task<bool> DeleteNoteAsync(int noteId, int userId)
        {
            var note = await _noteRepository.GetByIdAsync(noteId, userId);
            if (note == null) return false;

            await _noteRepository.DeleteAsync(note);
            return true;
        }

        
        public async Task<bool> TogglePinAsync(int noteId, int userId)
        {
            var note = await _noteRepository.GetByIdAsync(noteId, userId);
            if (note == null) return false;

            note.IsPinned = !note.IsPinned;
            note.UpdatedAt = DateTime.UtcNow;

            await _noteRepository.UpdateAsync(note);
            return true;
        }

        
        public async Task<List<Note>> SearchNotesAsync(int userId, string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<Note>();

            return await _noteRepository.SearchNotesAsync(userId, query);
        }

        
        public async Task<bool> ToggleArchiveAsync(int noteId, int userId)
        {
            var note = await _noteRepository.GetByIdAsync(noteId, userId);
            if (note == null) return false;

            note.IsArchived = !note.IsArchived;
            note.UpdatedAt = DateTime.UtcNow;

            await _noteRepository.UpdateAsync(note);
            return true;
        }

        
        public async Task<bool> UpdateColorAsync(int noteId, int userId, string color)
        {
            var note = await _noteRepository.GetByIdAsync(noteId, userId);
            if (note == null) return false;

            note.Color = color;
            note.UpdatedAt = DateTime.UtcNow;

            await _noteRepository.UpdateAsync(note);
            return true;
        }

        
        public async Task BulkDeleteNotesAsync(List<int> noteIds, int userId)
        {
            if (noteIds == null || !noteIds.Any())
                return;

            await _noteRepository.BulkDeleteAsync(noteIds, userId);
        }
    }
}
