using System.ComponentModel.DataAnnotations.Schema;

namespace Restoran.DataContext.Entities
{
    public class Grid
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public required string ImageUrl { get; set; }
    }
}
