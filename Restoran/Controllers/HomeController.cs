using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restoran.DataContext;
using Restoran.Models;

namespace Restoran.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var sliders = await _dbContext.Sliders.ToListAsync();
            var dishes = await _dbContext.Dishes.Take(6).ToListAsync();
            var categories = await _dbContext.Categories.ToListAsync();
            var grids = await _dbContext.Grids.ToListAsync();

            var model = new HomeViewModel
            {
                Sliders = sliders,
                Dishes = dishes,
                Categories = categories,
                Grids = grids
            };

            return View(model);
        }


    }
}
