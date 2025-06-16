using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restoran.Areas.Admin.Data
{
    public class ProductUpdateViewModel
    {
        public required string Name { get; set; }
        public int ShopCategoryId { get; set; }
        public List<SelectListItem> ShopCategorySelectListItems { get; set; } = [];
        public string? ImageUrl { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
