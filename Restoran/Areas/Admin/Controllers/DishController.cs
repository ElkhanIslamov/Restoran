using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restoran.DataContext;

namespace Restoran.Areas.Admin.Controllers
{
    public class DishController : AdminController
    {
        private readonly AppDbContext _dbContext;

        public DishController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var dishes = await _dbContext.Dishes.ToListAsync();

            return View(dishes);
        }
    }
}
