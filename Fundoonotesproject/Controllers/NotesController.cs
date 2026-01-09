using BusinessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTOs.Notes;
using System.Linq;
using System.Threading.Tasks;

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

        
        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            int userId = GetUserId();
            var notes = await _noteService.GetNotesAsync(userId);
            return Ok(notes);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNoteById(int id)
        {
            int userId = GetUserId();
            var note = await _noteService.GetNoteByIdAsync(id, userId);

            if (note == null)
                return NotFound("Note not found");

            return Ok(note);
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] CreateNoteDto dto)
        {
            int userId = GetUserId();
            await _noteService.CreateNoteAsync(dto, userId);
            return Ok("Note created successfully");
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(int id, [FromBody] UpdateNoteDto dto)
        {
            int userId = GetUserId();
            bool updated = await _noteService.UpdateNoteAsync(id, userId, dto);

            if (!updated)
                return NotFound("Note not found");

            return Ok("Note updated successfully");
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            int userId = GetUserId();
            bool deleted = await _noteService.DeleteNoteAsync(id, userId);

            if (!deleted)
                return NotFound("Note not found");

            return Ok("Note deleted successfully");
        }

        
        [HttpPatch("{id}/pin")]
        public async Task<IActionResult> TogglePin(int id)
        {
            int userId = GetUserId();
            bool result = await _noteService.TogglePinAsync(id, userId);

            if (!result)
                return NotFound("Note not found");

            return Ok("Pin status toggled");
        }

        
        [HttpGet("search")]
        public async Task<IActionResult> SearchNotes([FromQuery] string query)
        {
            int userId = GetUserId();
            var notes = await _noteService.SearchNotesAsync(userId, query);
            return Ok(notes);
        }

        
        [HttpPatch("{id}/archive")]
        public async Task<IActionResult> ToggleArchive(int id)
        {
            int userId = GetUserId();
            bool result = await _noteService.ToggleArchiveAsync(id, userId);

            if (!result)
                return NotFound("Note not found");

            return Ok("Archive status toggled");
        }

        
        [HttpPatch("{id}/color")]
        public async Task<IActionResult> UpdateColor(int id, UpdateNoteColorDto dto)
        {
            int userId = GetUserId();
            bool result = await _noteService.UpdateColorAsync(id, userId, dto.Color);

            if (!result)
                return NotFound("Note not found");

            return Ok("Color updated successfully");
        }

        
        [HttpDelete("bulk")]
        public async Task<IActionResult> BulkDelete(BulkDeleteNotesDto dto)
        {
            int userId = GetUserId();
            await _noteService.BulkDeleteNotesAsync(dto.NoteIds, userId);
            return Ok("Notes deleted successfully");
        }

        
        [HttpGet("debug-auth")]
        public IActionResult DebugAuth()
        {
            return Ok(
                User.Claims.Select(c => new { c.Type, c.Value })
            );
        }
    }
}
