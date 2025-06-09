namespace Restoran.Areas.Admin.Data
{
    public class SliderCreateViewModel
    {
        public required string FirstTitle { get; set; }
        public required string SecondTitle { get; set; }
        public required string Description { get; set; }
        public string? CoverImageUrl { get; set; }
        public required IFormFile CoverImageFile { get; set; }
    }
}
