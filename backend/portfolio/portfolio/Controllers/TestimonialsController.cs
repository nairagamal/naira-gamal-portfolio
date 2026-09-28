using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using portfolio.DTOs;
using portfolio.Interfaces;
using portfolio.Services;

namespace portfolio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestimonialsController : ControllerBase
    {
        private readonly ITestimonialService _testimonialService;

        public TestimonialsController(ITestimonialService testimonialService)
        {
            _testimonialService = testimonialService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var testimonials = await _testimonialService.GetAllTestimonialsAsync();
            return Ok(testimonials);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var testimonial = await _testimonialService.GetTestimonialByIdAsync(id);
            if (testimonial == null)
                return NotFound(new { message = "Testimonial not found" });
            return Ok(testimonial);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateTestimonialDto testimonialDto, IFormFile? image)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var testimonial = await _testimonialService.CreateTestimonialAsync(testimonialDto, image);
            return CreatedAtAction(nameof(GetById), new { id = testimonial.Id }, testimonial);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateTestimonialDto testimonialDto, IFormFile? image)
        {
            if (id != testimonialDto.Id)
                return BadRequest(new { message = "ID mismatch" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var testimonial = await _testimonialService.UpdateTestimonialAsync(testimonialDto, image);
            return Ok(testimonial);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _testimonialService.DeleteTestimonialAsync(id);
            if (!result)
                return NotFound(new { message = "Testimonial not found" });
            return NoContent();
        }
    }
}