// portfolio/Services/ExperienceService.cs
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using portfolio.Data;
using portfolio.DTOs;
using portfolio.Entities;
using portfolio.Interfaces;

namespace portfolio.Services
{
    public class ExperienceService : IExperienceService
    {
        private readonly IGenericRepository<Experience> _experienceRepository;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ExperienceService(
            IGenericRepository<Experience> experienceRepository,
            IMapper mapper,
            ApplicationDbContext context)
        {
            _experienceRepository = experienceRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<ExperienceDto>> GetAllExperiencesAsync()
        {
            var experiences = await _context.Experiences
                .Include(e => e.BulletPoints)
                .Where(e => !e.IsDeleted)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ExperienceDto>>(experiences);
        }

        public async Task<ExperienceDto?> GetExperienceByIdAsync(int id)
        {
            var experience = await _context.Experiences
                .Include(e => e.BulletPoints)
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

            return experience == null ? null : _mapper.Map<ExperienceDto>(experience);
        }

        public async Task<ExperienceDto> CreateExperienceAsync(CreateExperienceDto experienceDto)
        {
            var experience = _mapper.Map<Experience>(experienceDto);
            experience.CreatedAt = DateTime.UtcNow;

            // Add bullet points with ordering (similar to how Projects handle skills)
            if (experienceDto.BulletPoints != null && experienceDto.BulletPoints.Any())
            {
                for (int i = 0; i < experienceDto.BulletPoints.Count; i++)
                {
                    experience.BulletPoints.Add(new ExperienceBulletPoint
                    {
                        Point = experienceDto.BulletPoints[i],
                        Order = i,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

            var created = await _experienceRepository.AddAsync(experience);
            return _mapper.Map<ExperienceDto>(created);
        }

        public async Task<ExperienceDto> UpdateExperienceAsync(UpdateExperienceDto experienceDto)
        {
            var existing = await _context.Experiences
                .Include(e => e.BulletPoints)
                .FirstOrDefaultAsync(e => e.Id == experienceDto.Id && !e.IsDeleted);

            if (existing == null)
                throw new ArgumentException("Experience not found");

            // Update basic properties
            _mapper.Map(experienceDto, existing);
            existing.UpdatedAt = DateTime.UtcNow;

            // Update bullet points (clear and recreate - similar to Projects)
            // Remove existing bullet points
            _context.ExperienceBulletPoints.RemoveRange(existing.BulletPoints);
            existing.BulletPoints.Clear();

            // Add new bullet points with ordering
            if (experienceDto.BulletPoints != null && experienceDto.BulletPoints.Any())
            {
                for (int i = 0; i < experienceDto.BulletPoints.Count; i++)
                {
                    existing.BulletPoints.Add(new ExperienceBulletPoint
                    {
                        Point = experienceDto.BulletPoints[i],
                        Order = i,
                        ExperienceId = existing.Id,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

            await _context.SaveChangesAsync();
            return _mapper.Map<ExperienceDto>(existing);
        }

        public async Task<bool> DeleteExperienceAsync(int id)
        {
            var experience = await _experienceRepository.GetByIdAsync(id);
            if (experience == null)
                return false;

            experience.IsDeleted = true;
            experience.UpdatedAt = DateTime.UtcNow;

            await _experienceRepository.UpdateAsync(experience);
            return true;
        }
    }
}