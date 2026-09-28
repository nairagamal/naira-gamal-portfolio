using FluentValidation;
using portfolio.DTOs;

namespace portfolio.Validators
{
    public class CreateTestimonialValidator : AbstractValidator<CreateTestimonialDto>
    {
        public CreateTestimonialValidator()
        {
            RuleFor(x => x.ClientName)
                .NotEmpty().WithMessage("Client name is required")
                .MaximumLength(100).WithMessage("Client name must not exceed 100 characters");

            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Testimonial text is required")
                .MaximumLength(1000).WithMessage("Text must not exceed 1000 characters");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5");
        }
    }
}
