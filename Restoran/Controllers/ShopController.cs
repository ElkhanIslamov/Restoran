using Microsoft.AspNetCore.Mvc;
using Restoran.DataContext;
using Microsoft.EntityFrameworkCore;
using Restoran.Models;


namespace Restoran.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _dbContext;
        public ShopController(AppDbContext context)
        {
            _dbContext = context;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _dbContext.ShopCategories.ToListAsync();
            var sliders = await _dbContext.Sliders.ToListAsync();   
            var products = await _dbContext.Products
                .Include(p => p.ShopCategory)
                .ToListAsync();

            var model = new ShopViewModel
            {
                ShopCategories = categories,
                Products = products,
                Sliders = sliders

            };

            return View(model);
        }
    }
       
    }


