// portfolio/Validators/CreateProjectValidator.cs
using FluentValidation;
using portfolio.DTOs;

namespace portfolio.Validators
{
    public class CreateProjectValidator : AbstractValidator<CreateProjectDto>
    {
        public CreateProjectValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Project name is required")
                .MaximumLength(100).WithMessage("Project name must not exceed 100 characters");

            // Updated: Validate Skills list instead of SkillsUsed string
            RuleFor(x => x.Skills)
                .NotEmpty().WithMessage("At least one skill is required")
                .Must(skills => skills != null && skills.Any()).WithMessage("At least one skill is required");

            RuleForEach(x => x.Skills)
                .NotEmpty().WithMessage("Skill cannot be empty")
                .MaximumLength(100).WithMessage("Each skill must not exceed 100 characters");

            // Features are optional, but validate if provided
            RuleForEach(x => x.Features)
                .MaximumLength(200).WithMessage("Each feature must not exceed 200 characters")
                .When(x => x.Features != null);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");

            // Links remain optional, but validate URL format if provided
            RuleFor(x => x.GithubLink)
                .Must(uri => string.IsNullOrEmpty(uri) || Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("Github link must be a valid URL");

            RuleFor(x => x.LiveDemoLink)
                .Must(uri => string.IsNullOrEmpty(uri) || Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("Live demo link must be a valid URL");
        }
    }
}