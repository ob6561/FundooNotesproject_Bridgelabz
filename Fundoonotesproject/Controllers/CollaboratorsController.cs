using Microsoft.AspNetCore.Mvc;
using BusinessLayer;
using Microsoft.AspNetCore.Authorization;
using ModelLayer.DTOs.Collaborators;
using System.Threading.Tasks;

namespace Fundoonotesproject.Controllers
{
    [ApiController]
    [Route("api/collaborators")]
    [Authorize]
    public class CollaboratorsController : ControllerBase
    {
        private readonly CollaboratorService _service;

        public CollaboratorsController(CollaboratorService service)
        {
            _service = service;
        }

        
        [HttpGet("note/{noteId}")]
        public async Task<IActionResult> GetByNote(int noteId)
        {
            var collaborators = await _service.GetByNoteAsync(noteId);
            return Ok(collaborators);
        }

        
        [HttpPost]
        public async Task<IActionResult> Add(CreateCollaboratorDto dto)
        {
            await _service.AddCollaboratorAsync(dto);
            return Ok("Collaborator added");
        }

        
        [HttpPatch("{id}/permission")]
        public async Task<IActionResult> UpdatePermission(int id, UpdatePermissionDto dto)
        {
            await _service.UpdatePermissionAsync(id, dto.Permission);
            return Ok("Permission updated");
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.RemoveAsync(id);
            return Ok("Collaborator removed");
        }
    }
}
