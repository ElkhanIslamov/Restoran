using System.ComponentModel.DataAnnotations.Schema;

namespace Restoran.DataContext.Entities
{
    public class Dish
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string ImageUrl { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
