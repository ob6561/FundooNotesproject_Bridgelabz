using DataLayer.Repositories.Interfaces;
using ModelLayer.DTOs;
using ModelLayer.DTOs.Notes;
using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public IEnumerable<Note> GetNotes(int userId)
        {
            return _noteRepository.GetAllByUser(userId);
        }

        public Note? GetNoteById(int noteId, int userId)
        {
            return _noteRepository.GetById(noteId, userId);
        }

        public void CreateNote(CreateNoteDto dto, int userId)
        {
            var note = new Note
            {
                Title = dto.Title,
                Content = dto.Content,
                UserId = userId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Color = "#ffffff",
                IsPinned = false,
                IsArchived = false,
                IsDeleted = false
            };

            _noteRepository.Add(note);
            
        }

        public bool UpdateNote(int noteId, int userId, UpdateNoteDto dto)
        {
            var note = _noteRepository.GetById(noteId, userId);
            if (note == null) return false;

            note.Title = dto.Title;
            note.Content = dto.Content;
            note.UpdatedAt = DateTime.Now;

            _noteRepository.Update(note);
            return true;
        }

        public bool DeleteNote(int noteId, int userId)
        {
            var note = _noteRepository.GetById(noteId, userId);
            if (note == null) return false;

            _noteRepository.Delete(note);
            return true;
        }

        public bool TogglePin(int noteId, int userId)
        {
            var note = _noteRepository.GetById(noteId,userId);
            if(note == null) return false;
            note.IsPinned = !note.IsPinned;
            note.UpdatedAt = DateTime.Now;
            _noteRepository.Update(note);
            return true;
        }

        public List<Note> SearchNotes(int userId, string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<Note>();

            return _noteRepository.SearchNotes(userId, query);
        }

        public bool ToggleArchive(int noteId, int userId)
        {
            var note = _noteRepository.GetById(noteId, userId);
            if (note == null) return false;

            note.IsArchived = !note.IsArchived;
            note.UpdatedAt = DateTime.Now;

            _noteRepository.Update(note);
            return true;
        }

        public bool UpdateColor(int noteId, int userId, string color)
        {
            var note = _noteRepository.GetById(noteId, userId);
            if (note == null) return false;

            note.Color = color;
            note.UpdatedAt = DateTime.Now;

            _noteRepository.Update(note);
            return true;
        }

        public void BulkDeleteNotes(List<int> noteIds, int userId)
        {
            if (noteIds == null || !noteIds.Any())
                return;

            _noteRepository.BulkDelete(noteIds, userId);
        }

    }
}


