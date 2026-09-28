using portfolio.Entities;

namespace portfolio.Interfaces
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<IEnumerable<Project>> GetProjectsWithImagesAsync();
    }
}
