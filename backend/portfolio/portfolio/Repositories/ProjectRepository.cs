using Microsoft.EntityFrameworkCore;
using portfolio.Data;
using portfolio.Entities;
using portfolio.Interfaces;

namespace portfolio.Repositories
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Project>> GetProjectsWithImagesAsync()
        {
            return await _dbSet
                .Where(p => !p.IsDeleted)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
    }
}
