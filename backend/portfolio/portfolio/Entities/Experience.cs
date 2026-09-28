// portfolio/Entities/Experience.cs
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace portfolio.Entities
{
    public class Experience : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string JobTitle { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Company { get; set; } = string.Empty;

        [Required]
        public string Date { get; set; } = string.Empty;

        // Collection of bullet points (like ProjectSkill)
        public virtual ICollection<ExperienceBulletPoint> BulletPoints { get; set; } = new List<ExperienceBulletPoint>();
    }

    public class ExperienceBulletPoint : BaseEntity
    {
        [Required]
        [MaxLength(500)]
        public string Point { get; set; } = string.Empty;

        public int Order { get; set; } // For ordering the bullet points

        public int ExperienceId { get; set; }

        [JsonIgnore]
        public virtual Experience? Experience { get; set; }
    }
}