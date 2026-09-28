// portfolio/DTOs/ExperienceDto.cs
namespace portfolio.DTOs
{
    public class ExperienceDto
    {
        public int Id { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public List<string> BulletPoints { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; }
    }

    public class CreateExperienceDto
    {
        public string JobTitle { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public List<string> BulletPoints { get; set; } = new List<string>();
    }

    public class UpdateExperienceDto
    {
        public int Id { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public List<string> BulletPoints { get; set; } = new List<string>();
    }
}