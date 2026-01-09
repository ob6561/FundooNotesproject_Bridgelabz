using Microsoft.AspNetCore.Mvc;
using BusinessLayer;
using Microsoft.AspNetCore.Authorization;
using ModelLayer.DTOs.Labels;
using System.Threading.Tasks;

namespace Fundoonotesproject.Controllers
{
    [ApiController]
    [Route("api/labels")]
    [Authorize]
    public class LabelsController : ControllerBase
    {
        private readonly LabelService _labelService;

        public LabelsController(LabelService labelService)
        {
            _labelService = labelService;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst("UserId")!.Value);
        }

        
        [HttpGet]
        public async Task<IActionResult> GetLabels()
        {
            var labels = await _labelService.GetLabelsAsync(GetUserId());
            return Ok(labels);
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateLabel(CreateLabelDto dto)
        {
            await _labelService.CreateLabelAsync(dto, GetUserId());
            return Ok("Label created");
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLabel(int id, UpdateLabelDto dto)
        {
            var updated = await _labelService.UpdateLabelAsync(id, GetUserId(), dto);
            if (!updated)
                return NotFound("Label not found");

            return Ok("Label updated");
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLabel(int id)
        {
            var deleted = await _labelService.DeleteLabelAsync(id, GetUserId());
            if (!deleted)
                return NotFound("Label not found");

            return Ok("Label deleted");
        }
    }
}
