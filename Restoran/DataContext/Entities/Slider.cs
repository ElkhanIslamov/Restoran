using System.ComponentModel.DataAnnotations.Schema;

namespace Restoran.DataContext.Entities
{
    public class Slider
    {
        public int Id { get; set; }
        public required string  FirstTitle { get; set; }
        public required string  SecondTitle { get; set; }
        public required string Description { get; set; }
        public required string CoverImageUrl { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }


    }
}
