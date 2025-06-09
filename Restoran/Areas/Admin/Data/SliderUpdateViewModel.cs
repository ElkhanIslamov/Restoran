namespace Restoran.Areas.Admin.Data
{
    public class SliderUpdateViewModel
    {
        public required string FirstTitle { get; set; }
        public required string SecondTitle { get; set; }
        public required string Description { get; set; }
        public IFormFile? CoverImageFile { get; set; }

    }
}
