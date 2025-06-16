namespace Restoran.DataContext.Entities
{
    public class Blog
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime DateTime  { get; set; }= DateTime.UtcNow;
        public required string Description { get; set; }
        public required string Content { get; set; }
        public int? CommentCount { get; set; }
        public required string ImageUrl { get; set; }

    }
}
