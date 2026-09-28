namespace portfolio.DTOs
{
    public class TestimonialDto
    {
        public int Id { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string? ClientImageUrl { get; set; }
        public string Text { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateTestimonialDto
    {
        public string ClientName { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public int Rating { get; set; }
    }

    public class UpdateTestimonialDto
    {
        public int Id { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public int Rating { get; set; }
    }
}
