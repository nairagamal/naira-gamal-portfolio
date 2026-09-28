using portfolio.DTOs;

namespace portfolio.Interfaces
{
    public interface IExperienceService
    {
        Task<IEnumerable<ExperienceDto>> GetAllExperiencesAsync();
        Task<ExperienceDto?> GetExperienceByIdAsync(int id);
        Task<ExperienceDto> CreateExperienceAsync(CreateExperienceDto experienceDto);
        Task<ExperienceDto> UpdateExperienceAsync(UpdateExperienceDto experienceDto);
        Task<bool> DeleteExperienceAsync(int id);
    }
}
