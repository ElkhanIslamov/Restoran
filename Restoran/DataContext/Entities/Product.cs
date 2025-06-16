using System.ComponentModel.DataAnnotations.Schema;

namespace Restoran.DataContext.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string ImageUrl { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int ShopCategoryId { get; set; }
        public ShopCategory? ShopCategory { get; set; }  
    }
}
