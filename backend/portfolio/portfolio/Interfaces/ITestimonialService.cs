using portfolio.DTOs;

namespace portfolio.Interfaces
{
    public interface ITestimonialService
    {
        Task<IEnumerable<TestimonialDto>> GetAllTestimonialsAsync();
        Task<TestimonialDto?> GetTestimonialByIdAsync(int id);
        Task<TestimonialDto> CreateTestimonialAsync(CreateTestimonialDto testimonialDto, IFormFile? image);
        Task<TestimonialDto> UpdateTestimonialAsync(UpdateTestimonialDto testimonialDto, IFormFile? image);
        Task<bool> DeleteTestimonialAsync(int id);
    }
}
