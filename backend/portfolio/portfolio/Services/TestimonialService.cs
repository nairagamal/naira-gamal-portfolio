using AutoMapper;
using portfolio.DTOs;
using portfolio.Entities;
using portfolio.Interfaces;

namespace portfolio.Services
{
    public class TestimonialService : ITestimonialService
    {
        private readonly IGenericRepository<Testimonial> _testimonialRepository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public TestimonialService(
            IGenericRepository<Testimonial> testimonialRepository,
            IFileService fileService,
            IMapper mapper)
        {
            _testimonialRepository = testimonialRepository;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TestimonialDto>> GetAllTestimonialsAsync()
        {
            var testimonials = await _testimonialRepository.GetAllAsync();
            var testimonialDtos = _mapper.Map<IEnumerable<TestimonialDto>>(testimonials.OrderByDescending(t => t.CreatedAt));

            foreach (var dto in testimonialDtos)
            {
                var testimonial = testimonials.First(t => t.Id == dto.Id);
                if (!string.IsNullOrEmpty(testimonial.ClientImagePath))
                {
                    dto.ClientImageUrl = _fileService.GetFileUrl(testimonial.ClientImagePath);
                }
            }

            return testimonialDtos;
        }

        public async Task<TestimonialDto?> GetTestimonialByIdAsync(int id)
        {
            var testimonial = await _testimonialRepository.GetByIdAsync(id);
            if (testimonial == null)
                return null;

            var dto = _mapper.Map<TestimonialDto>(testimonial);
            if (!string.IsNullOrEmpty(testimonial.ClientImagePath))
            {
                dto.ClientImageUrl = _fileService.GetFileUrl(testimonial.ClientImagePath);
            }

            return dto;
        }

        public async Task<TestimonialDto> CreateTestimonialAsync(CreateTestimonialDto testimonialDto, IFormFile? image)
        {
            var testimonial = _mapper.Map<Testimonial>(testimonialDto);
            testimonial.CreatedAt = DateTime.UtcNow;

            if (image != null)
            {
                if (!_fileService.IsImageFile(image))
                    throw new ArgumentException("Only image files are allowed");

                var imagePath = await _fileService.SaveFileAsync(image, "testimonials");
                testimonial.ClientImagePath = imagePath;
            }

            var created = await _testimonialRepository.AddAsync(testimonial);
            var dto = _mapper.Map<TestimonialDto>(created);

            if (!string.IsNullOrEmpty(created.ClientImagePath))
            {
                dto.ClientImageUrl = _fileService.GetFileUrl(created.ClientImagePath);
            }

            return dto;
        }

        public async Task<TestimonialDto> UpdateTestimonialAsync(UpdateTestimonialDto testimonialDto, IFormFile? image)
        {
            var existing = await _testimonialRepository.GetByIdAsync(testimonialDto.Id);
            if (existing == null)
                throw new ArgumentException("Testimonial not found");

            _mapper.Map(testimonialDto, existing);
            existing.UpdatedAt = DateTime.UtcNow;

            if (image != null)
            {
                if (!_fileService.IsImageFile(image))
                    throw new ArgumentException("Only image files are allowed");

                if (!string.IsNullOrEmpty(existing.ClientImagePath))
                {
                    await _fileService.DeleteFileAsync(existing.ClientImagePath);
                }

                var imagePath = await _fileService.SaveFileAsync(image, "testimonials");
                existing.ClientImagePath = imagePath;
            }

            await _testimonialRepository.UpdateAsync(existing);

            var dto = _mapper.Map<TestimonialDto>(existing);
            if (!string.IsNullOrEmpty(existing.ClientImagePath))
            {
                dto.ClientImageUrl = _fileService.GetFileUrl(existing.ClientImagePath);
            }

            return dto;
        }

        public async Task<bool> DeleteTestimonialAsync(int id)
        {
            var testimonial = await _testimonialRepository.GetByIdAsync(id);
            if (testimonial == null)
                return false;

            testimonial.IsDeleted = true;
            testimonial.UpdatedAt = DateTime.UtcNow;

            await _testimonialRepository.UpdateAsync(testimonial);

            if (!string.IsNullOrEmpty(testimonial.ClientImagePath))
            {
                await _fileService.DeleteFileAsync(testimonial.ClientImagePath);
            }

            return true;
        }
    }
}
