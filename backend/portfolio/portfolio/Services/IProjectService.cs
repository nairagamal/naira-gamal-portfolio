using portfolio.DTOs;

namespace portfolio.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
        Task<ProjectDto?> GetProjectByIdAsync(int id);
        Task<ProjectDto> CreateProjectAsync(CreateProjectDto projectDto, IFormFile? image);
        Task<ProjectDto> UpdateProjectAsync(UpdateProjectDto projectDto, IFormFile? image);
        Task<bool> DeleteProjectAsync(int id);
    }
}
