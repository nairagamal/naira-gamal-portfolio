using System.ComponentModel.DataAnnotations;

namespace portfolio.Entities
{
    public class Admin : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]  // Increased length for hash
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
    }
}
