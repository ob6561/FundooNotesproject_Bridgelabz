using BusinessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTOs.Notes;
using System.Linq;

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
            var userIdClaim = User.FindFirst("UserId")?.Value;
            return int.Parse(userIdClaim!);
        }

        // api/notes
        [HttpGet]
        public IActionResult GetAllNotes()
        {
            int userId = GetUserId();
            var notes = _noteService.GetNotes(userId);
            return Ok(notes);
        }

        // notes id
        [HttpGet("{id}")]
        public IActionResult GetNoteById(int id)
        {
            int userId = GetUserId();
            var note = _noteService.GetNoteById(id, userId);

            if (note == null)
                return NotFound("Note not found");

            return Ok(note);
        }

        // POST notes
        [HttpPost]
        public IActionResult CreateNote([FromBody] CreateNoteDto dto)
        {
            int userId = GetUserId();
            _noteService.CreateNote(dto, userId);
            return Ok("Note created successfully");
        }

        // PUT notes id
        [HttpPut("{id}")]
        public IActionResult UpdateNote(int id, [FromBody] UpdateNoteDto dto)
        {
            int userId = GetUserId();
            bool updated = _noteService.UpdateNote(id, userId, dto);

            if (!updated)
                return NotFound("Note not found");

            return Ok("Note updated successfully");
        }

        // DELETE notes id
        [HttpDelete("{id}")]
        public IActionResult DeleteNote(int id)
        {
            int userId = GetUserId();
            bool deleted = _noteService.DeleteNote(id, userId);

            if (!deleted)
                return NotFound("Note not found");

            return Ok("Note deleted successfully");
        }

        // patch id
        [HttpPatch("{id}/pin")]
        public IActionResult TogglePin(int id)
        {
            int userId = GetUserId();
            bool result = _noteService.TogglePin(id, userId);

            if (!result)
                return NotFound("Note not found");

            return Ok("Pin status toggled");
        }

        // debug checking jwt
        [HttpGet("debug-auth")]
        public IActionResult DebugAuth()
        {
            return Ok(
                User.Claims.Select(c => new { c.Type, c.Value })
            );
        }

        // notes searching
        [HttpGet("search")]
        public IActionResult SearchNotes([FromQuery] string query)
        {
            int userId = GetUserId();
            var notes = _noteService.SearchNotes(userId, query);
            return Ok(notes);
        }

        [HttpPatch("{id}/archive")]
        public IActionResult ToggleArchive(int id)
        {
            int userId = GetUserId();
            bool result = _noteService.ToggleArchive(id, userId);

            if (!result)
                return NotFound("Note not found");

            return Ok("Archive status toggled");
        }

        [HttpPatch("{id}/color")]
        public IActionResult UpdateColor(int id, UpdateNoteColorDto dto)
        {
            int userId = GetUserId();
            bool result = _noteService.UpdateColor(id, userId, dto.Color);

            if (!result)
                return NotFound("Note not found");

            return Ok("Color updated successfully");
        }

        [HttpDelete("bulk")]
        public IActionResult BulkDelete(BulkDeleteNotesDto dto)
        {
            int userId = GetUserId();
            _noteService.BulkDeleteNotes(dto.NoteIds, userId);
            return Ok("Notes deleted successfully");
        }

    }
}
