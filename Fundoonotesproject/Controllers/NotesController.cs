using BusinessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTOs;
using ModelLayer.DTOs.Notes;
using ModelLayer.Entities;
using System.Security.Claims;

namespace Fundoonotesproject.Controllers
{
    [ApiController]
    [Route("api/notes")]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly NoteService _noteService;

        public NotesController(NoteService noteService)
        {
            _noteService = noteService;
        }

        
        private int GetUserId()
        {
            return int.Parse(User.FindFirst("UserId")!.Value);
        }

        
        [HttpGet]
        public IActionResult GetAllNotes()
        {
            int userId = GetUserId();
            var notes = _noteService.GetNotes(userId);
            return Ok(notes);
        }

        
        [HttpGet("{id}")]
        public IActionResult GetNoteById(int id)
        {
            int userId = GetUserId();
            var note = _noteService.GetNoteById(id, userId);

            if (note == null)
                return NotFound("Note not found");

            return Ok(note);
        }

        
        [HttpPost]
        public IActionResult CreateNote(CreateNoteDto dto)
        {
            int userId = GetUserId();
            _noteService.CreateNote(dto, userId);
            return Ok("Note created successfully");
        }

        
        [HttpPut("{id}")]
        public IActionResult UpdateNote(int id, UpdateNoteDto dto)
        {
            int userId = GetUserId();
            bool result = _noteService.UpdateNote(id, userId, dto);

            if (!result)
                return NotFound("Note not found");

            return Ok("Note updated successfully");
        }

        
        [HttpDelete("{id}")]
        public IActionResult DeleteNote(int id)
        {
            int userId = GetUserId();
            bool result = _noteService.DeleteNote(id, userId);

            if (!result)
                return NotFound("Note not found");

            return Ok("Note deleted successfully");
        }
    }
}

