using System.ComponentModel.DataAnnotations;

namespace portfolio.Entities
{
    public class Testimonial : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string ClientName { get; set; } = string.Empty;

        public string? ClientImagePath { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Text { get; set; } = string.Empty;

        [Range(1, 5)]
        public int Rating { get; set; }
    }
}
