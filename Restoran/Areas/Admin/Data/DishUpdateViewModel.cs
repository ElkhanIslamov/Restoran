using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Restoran.Areas.Admin.Data
{
    public class DishUpdateViewModel
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public int CategoryId { get; set; }
        public List<SelectListItem> CategorySelectListItems { get; set; } = [];
        public  string? ImageUrl { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }      
        public IFormFile? ImageFile { get; set; }
    }
}
