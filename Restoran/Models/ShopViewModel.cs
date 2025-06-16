using Restoran.DataContext.Entities;

namespace Restoran.Models
{
    public class ShopViewModel
    {
        public List<Product> Products { get; set; } = [];
        public List<Slider> Sliders { get; set; } = [];
        public List<ShopCategory> ShopCategories { get; set; } = [];
    }
}
