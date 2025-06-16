using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restoran.DataContext;
using Restoran.Models;

namespace Restoran.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _dbContext;

        public BlogController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var blogs = await _dbContext.Blogs.ToListAsync();
            var sliders = await _dbContext.Sliders.ToListAsync();
            var dishes = await _dbContext.Dishes.Take(6).ToListAsync();
            var categories = await _dbContext.Categories.ToListAsync();
            var grids = await _dbContext.Grids.ToListAsync();

            var model = new HomeViewModel
            {
                Sliders = sliders,
                Dishes = dishes,
                Categories = categories,
                Grids = grids,
                Blogs = blogs
            };

            return View(model);
        }

    }
}
