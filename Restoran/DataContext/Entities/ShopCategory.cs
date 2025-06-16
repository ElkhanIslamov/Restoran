namespace Restoran.DataContext.Entities
{
    public class ShopCategory
    {
        public int Id { get; set; }
        public required  string Name { get; set; }
        public List<Product> Products { get; set; } = [];
    }
}
