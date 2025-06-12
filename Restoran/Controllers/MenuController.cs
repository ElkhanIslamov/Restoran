using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restoran.DataContext;
using Restoran.DataContext.Entities;
using Restoran.Models;

namespace Restoran.Controllers
{
    public class MenuController : Controller
    {
        private readonly AppDbContext _dbContext;

        public MenuController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _dbContext.Categories.ToListAsync();
            var dishes = await _dbContext.Dishes
                .Include(d => d.Category)
                .ToListAsync();

            var model = new HomeViewModel
            {
                Categories = categories,
                Dishes = dishes

            };            

            return View(model);
        }
    }
}
