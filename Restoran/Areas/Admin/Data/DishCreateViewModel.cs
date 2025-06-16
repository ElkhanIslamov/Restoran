using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Restoran.Areas.Admin.Data
{
    public class DishCreateViewModel
    {
        public required string Title { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public List<SelectListItem> CategorySelectListItems { get; set; } = [];
        public decimal Price { get; set; }
        [NotMapped]
        public required IFormFile ImageFile { get; set; }
        public string? ImageUrl { get; set; }
    }
}
