using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restoran.Areas.Admin.Data
{
    public class ProductCreateViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int ShopCategoryId { get; set; }
        public List<SelectListItem> ShopCategorySelectListItems { get; set; } = [];
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [NotMapped]
        public required IFormFile ImageFile { get; set; }
        public string? ImageUrl { get; set; }

    }
}
