using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using portfolio.DTOs;
using portfolio.Interfaces;
using portfolio.Services;

namespace portfolio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExperiencesController : ControllerBase
    {
        private readonly IExperienceService _experienceService;

        public ExperiencesController(IExperienceService experienceService)
        {
            _experienceService = experienceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var experiences = await _experienceService.GetAllExperiencesAsync();
            return Ok(experiences);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var experience = await _experienceService.GetExperienceByIdAsync(id);
            if (experience == null)
                return NotFound(new { message = "Experience not found" });
            return Ok(experience);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExperienceDto experienceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var experience = await _experienceService.CreateExperienceAsync(experienceDto);
            return CreatedAtAction(nameof(GetById), new { id = experience.Id }, experience);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateExperienceDto experienceDto)
        {
            if (id != experienceDto.Id)
                return BadRequest(new { message = "ID mismatch" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var experience = await _experienceService.UpdateExperienceAsync(experienceDto);
            return Ok(experience);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _experienceService.DeleteExperienceAsync(id);
            if (!result)
                return NotFound(new { message = "Experience not found" });
            return NoContent();
        }
    }
}