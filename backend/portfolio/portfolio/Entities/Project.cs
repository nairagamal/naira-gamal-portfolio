using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace portfolio.Entities
{
    public class Project : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? ImagePath { get; set; }

        // Changed from single string to collection
        public virtual ICollection<ProjectSkill> Skills { get; set; } = new List<ProjectSkill>();

        // New: Optional features/advantages
        public virtual ICollection<ProjectFeature> Features { get; set; } = new List<ProjectFeature>();

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Url]
        public string? GithubLink { get; set; }

        [Url]
        public string? LiveDemoLink { get; set; }
    }

    // New entity for project skills
    public class ProjectSkill : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string SkillName { get; set; } = string.Empty;

        public int ProjectId { get; set; }

        [JsonIgnore]
        public virtual Project? Project { get; set; }
    }

    // New entity for project features/advantages
    public class ProjectFeature : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Feature { get; set; } = string.Empty;

        public int ProjectId { get; set; }

        [JsonIgnore]
        public virtual Project? Project { get; set; }
    }
}