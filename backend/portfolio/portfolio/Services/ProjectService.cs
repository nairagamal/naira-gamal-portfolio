using AutoMapper;
using Microsoft.EntityFrameworkCore;
using portfolio.Data;
using portfolio.DTOs;
using portfolio.Entities;
using portfolio.Interfaces;

namespace portfolio.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public ProjectService(
            IProjectRepository projectRepository,
            IFileService fileService,
            IMapper mapper,
            ApplicationDbContext context)
        {
            _projectRepository = projectRepository;
            _fileService = fileService;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
        {
            var projects = await _context.Projects
                .Include(p => p.Skills)
                .Include(p => p.Features)
                .Where(p => !p.IsDeleted)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            var projectDtos = _mapper.Map<IEnumerable<ProjectDto>>(projects);

            foreach (var dto in projectDtos)
            {
                var project = projects.First(p => p.Id == dto.Id);
                if (!string.IsNullOrEmpty(project.ImagePath))
                {
                    try
                    {
                        dto.ImageUrl = _fileService.GetFileUrl(project.ImagePath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error getting image URL: {ex.Message}");
                        dto.ImageUrl = null;
                    }
                }
            }

            return projectDtos;
        }

        public async Task<ProjectDto?> GetProjectByIdAsync(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Skills)
                .Include(p => p.Features)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (project == null)
                return null;

            var projectDto = _mapper.Map<ProjectDto>(project);
            if (!string.IsNullOrEmpty(project.ImagePath))
            {
                try
                {
                    projectDto.ImageUrl = _fileService.GetFileUrl(project.ImagePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting image URL: {ex.Message}");
                    projectDto.ImageUrl = null;
                }
            }

            return projectDto;
        }

        public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto projectDto, IFormFile? image)
        {
            try
            {
                var project = _mapper.Map<Project>(projectDto);
                project.CreatedAt = DateTime.UtcNow;

                // Add skills
                if (projectDto.Skills != null && projectDto.Skills.Any())
                {
                    for (int i = 0; i < projectDto.Skills.Count; i++)
                    {
                        project.Skills.Add(new ProjectSkill
                        {
                            SkillName = projectDto.Skills[i],
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                }

                // Add features
                if (projectDto.Features != null && projectDto.Features.Any())
                {
                    foreach (var feature in projectDto.Features)
                    {
                        project.Features.Add(new ProjectFeature
                        {
                            Feature = feature,
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                }

                if (image != null && image.Length > 0)
                {
                    if (!_fileService.IsImageFile(image))
                        throw new ArgumentException("Only image files are allowed (jpg, jpeg, png, gif, webp)");

                    var imagePath = await _fileService.SaveFileAsync(image, "projects");
                    project.ImagePath = imagePath;
                }

                await _context.Projects.AddAsync(project);
                await _context.SaveChangesAsync();

                var resultDto = _mapper.Map<ProjectDto>(project);
                if (!string.IsNullOrEmpty(project.ImagePath))
                {
                    resultDto.ImageUrl = _fileService.GetFileUrl(project.ImagePath);
                }

                return resultDto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating project: {ex.Message}", ex);
            }
        }

        public async Task<ProjectDto> UpdateProjectAsync(UpdateProjectDto projectDto, IFormFile? image)
        {
            var existingProject = await _context.Projects
                .Include(p => p.Skills)
                .Include(p => p.Features)
                .FirstOrDefaultAsync(p => p.Id == projectDto.Id && !p.IsDeleted);

            if (existingProject == null)
                throw new ArgumentException("Project not found");

            _mapper.Map(projectDto, existingProject);
            existingProject.UpdatedAt = DateTime.UtcNow;

            // Update skills
            existingProject.Skills.Clear();
            if (projectDto.Skills != null && projectDto.Skills.Any())
            {
                foreach (var skill in projectDto.Skills)
                {
                    existingProject.Skills.Add(new ProjectSkill
                    {
                        SkillName = skill,
                        ProjectId = existingProject.Id,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

            // Update features
            existingProject.Features.Clear();
            if (projectDto.Features != null && projectDto.Features.Any())
            {
                foreach (var feature in projectDto.Features)
                {
                    existingProject.Features.Add(new ProjectFeature
                    {
                        Feature = feature,
                        ProjectId = existingProject.Id,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

            if (image != null && image.Length > 0)
            {
                if (!_fileService.IsImageFile(image))
                    throw new ArgumentException("Only image files are allowed");

                if (!string.IsNullOrEmpty(existingProject.ImagePath))
                {
                    await _fileService.DeleteFileAsync(existingProject.ImagePath);
                }

                var imagePath = await _fileService.SaveFileAsync(image, "projects");
                existingProject.ImagePath = imagePath;
            }

            await _context.SaveChangesAsync();

            var resultDto = _mapper.Map<ProjectDto>(existingProject);
            if (!string.IsNullOrEmpty(existingProject.ImagePath))
            {
                resultDto.ImageUrl = _fileService.GetFileUrl(existingProject.ImagePath);
            }

            return resultDto;
        }

        // Other methods remain the same...
        public async Task<bool> DeleteProjectAsync(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
                return false;

            project.IsDeleted = true;
            project.UpdatedAt = DateTime.UtcNow;

            await _projectRepository.UpdateAsync(project);

            if (!string.IsNullOrEmpty(project.ImagePath))
            {
                try
                {
                    await _fileService.DeleteFileAsync(project.ImagePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting image: {ex.Message}");
                }
            }

            return true;
        }
    }
}