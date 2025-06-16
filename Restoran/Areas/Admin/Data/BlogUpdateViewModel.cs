namespace Restoran.Areas.Admin.Data
{
    public class BlogUpdateViewModel
    {
        public required string Name { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public required string Description { get; set; }
        public required string Content { get; set; }
        public int? CommentCount { get; set; }
        public string? ImageUrl { get; set; }
        public required IFormFile ImageFile { get; set; }
    }
}
