// portfolio/Validators/CreateExperienceValidator.cs
using FluentValidation;
using portfolio.DTOs;

namespace portfolio.Validators
{
    public class CreateExperienceValidator : AbstractValidator<CreateExperienceDto>
    {
        public CreateExperienceValidator()
        {
            RuleFor(x => x.JobTitle)
                .NotEmpty().WithMessage("Job title is required")
                .MaximumLength(100).WithMessage("Job title must not exceed 100 characters");

            RuleFor(x => x.Company)
                .NotEmpty().WithMessage("Company is required")
                .MaximumLength(100).WithMessage("Company must not exceed 100 characters");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required");

            // Validate bullet points (like skills in projects)
            RuleFor(x => x.BulletPoints)
                .NotEmpty().WithMessage("At least one bullet point is required")
                .Must(points => points != null && points.Any())
                .WithMessage("At least one bullet point is required");

            RuleForEach(x => x.BulletPoints)
                .NotEmpty().WithMessage("Bullet point cannot be empty")
                .MaximumLength(500).WithMessage("Each bullet point must not exceed 500 characters");
        }
    }
}