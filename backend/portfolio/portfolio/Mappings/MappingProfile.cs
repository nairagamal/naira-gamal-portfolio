using AutoMapper;
using portfolio.DTOs;
using portfolio.Entities;

namespace portfolio.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Project mappings
            CreateMap<Project, ProjectDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.Skills.Select(s => s.SkillName).ToList()))
                .ForMember(dest => dest.Features, opt => opt.MapFrom(src => src.Features.Select(f => f.Feature).ToList()));

            CreateMap<CreateProjectDto, Project>()
                .ForMember(dest => dest.Skills, opt => opt.Ignore())
                .ForMember(dest => dest.Features, opt => opt.Ignore());

            CreateMap<UpdateProjectDto, Project>()
                .ForMember(dest => dest.Skills, opt => opt.Ignore())
                .ForMember(dest => dest.Features, opt => opt.Ignore());

            // Experience mappings
            // In MappingProfile.cs
            CreateMap<Experience, ExperienceDto>()
                .ForMember(dest => dest.BulletPoints,
                    opt => opt.MapFrom(src => src.BulletPoints
                        .OrderBy(bp => bp.Order)
                        .Select(bp => bp.Point)
                        .ToList()));

            CreateMap<CreateExperienceDto, Experience>()
                .ForMember(dest => dest.BulletPoints, opt => opt.Ignore());

            CreateMap<UpdateExperienceDto, Experience>()
                .ForMember(dest => dest.BulletPoints, opt => opt.Ignore());

            // Testimonial mappings
            CreateMap<Testimonial, TestimonialDto>()
                .ForMember(dest => dest.ClientImageUrl, opt => opt.Ignore());
            CreateMap<CreateTestimonialDto, Testimonial>();
            CreateMap<UpdateTestimonialDto, Testimonial>();

            // ContactMessage mappings
            CreateMap<ContactMessage, ContactMessageDto>();
            CreateMap<CreateContactMessageDto, ContactMessage>();
        }
    }
}