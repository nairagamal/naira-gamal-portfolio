using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using portfolio.DTOs;
using portfolio.Services;

namespace portfolio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var projects = await _projectService.GetAllProjectsAsync();
                return Ok(projects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error retrieving projects: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var project = await _projectService.GetProjectByIdAsync(id);
                if (project == null)
                    return NotFound(new { message = "Project not found" });
                return Ok(project);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error retrieving project: {ex.Message}" });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProjectDto projectDto, IFormFile? image)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Validate at least one skill is provided
                if (projectDto.Skills == null || !projectDto.Skills.Any())
                    return BadRequest(new { message = "At least one skill is required" });

                var project = await _projectService.CreateProjectAsync(projectDto, image);
                return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error creating project: {ex.Message}" });
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateProjectDto projectDto, IFormFile? image)
        {
            try
            {
                if (id != projectDto.Id)
                    return BadRequest(new { message = "ID mismatch" });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Validate at least one skill is provided
                if (projectDto.Skills == null || !projectDto.Skills.Any())
                    return BadRequest(new { message = "At least one skill is required" });

                var project = await _projectService.UpdateProjectAsync(projectDto, image);
                return Ok(project);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error updating project: {ex.Message}" });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _projectService.DeleteProjectAsync(id);
                if (!result)
                    return NotFound(new { message = "Project not found" });
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error deleting project: {ex.Message}" });
            }
        }
    }
}