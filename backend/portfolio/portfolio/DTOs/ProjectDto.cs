
namespace portfolio.DTOs
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public List<string> Skills { get; set; } = new List<string>(); // Changed to list
        public List<string> Features { get; set; } = new List<string>(); // New property
        public string Description { get; set; } = string.Empty;
        public string? GithubLink { get; set; }
        public string? LiveDemoLink { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateProjectDto
    {
        public string Name { get; set; } = string.Empty;
        public List<string> Skills { get; set; } = new List<string>(); // Changed to list
        public List<string> Features { get; set; } = new List<string>(); // New property (optional)
        public string Description { get; set; } = string.Empty;
        public string? GithubLink { get; set; } // Optional
        public string? LiveDemoLink { get; set; } // Optional
    }

    public class UpdateProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<string> Skills { get; set; } = new List<string>(); // Changed to list
        public List<string> Features { get; set; } = new List<string>(); // New property (optional)
        public string Description { get; set; } = string.Empty;
        public string? GithubLink { get; set; } // Optional
        public string? LiveDemoLink { get; set; } // Optional
    }
}