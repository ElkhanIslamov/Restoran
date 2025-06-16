using Restoran.DataContext.Entities;

namespace Restoran.Models
{
    public class HomeViewModel
    {
        public List<Slider> Sliders { get; set; } = [];
        public List<Dish> Dishes{ get; set; } = [];
        public List<Category> Categories { get; set; } = [];
        public List<Grid> Grids { get; set; } = [];
        public List<Blog> Blogs { get; set; } = [];
    }
}
