using System.ComponentModel.DataAnnotations.Schema;

namespace Restoran.Areas.Admin.Data
{
    public class GridUpdateViewModel
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public required IFormFile ImageFile { get; set; }
    }
}
