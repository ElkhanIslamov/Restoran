using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restoran.Areas.Admin.Data;
using Restoran.Areas.Admin.Extensions;
using Restoran.Data;
using Restoran.DataContext;
using Restoran.DataContext.Entities;

namespace Restoran.Areas.Admin.Controllers
{
    public class DishController : AdminController
    {
        private readonly AppDbContext _dbContext;
        public DishController(AppDbContext context)
        {
          _dbContext = context;
        }
        public async Task<IActionResult> Index()
        {
          var dishes = await _dbContext.Dishes
                .Include(d => d.Category)
                .ToListAsync();

          return View(dishes);
        }
        public async Task<IActionResult> Details(int id)
        {
          var dish = await _dbContext.Dishes
                .Include(d => d.Category)
                .AsNoTracking()
                .SingleOrDefaultAsync(d => d.Id == id);

          return View(dish);
        }
        public async Task<IActionResult> Create()
        {
            var categories = await _dbContext.Categories.ToListAsync();
            var categoryListItems = categories.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();

            var dishCreateModel = new DishCreateViewModel
            {
                Title = string.Empty,
                CategorySelectListItems = categoryListItems,
                Description = string.Empty,
                ImageUrl = string.Empty,
                ImageFile = null
            };
            return View(dishCreateModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DishCreateViewModel model)
        {
            var categories = await _dbContext.Categories.ToListAsync();
            var categoryListItems = categories.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();

            if (!ModelState.IsValid)
            {
                model.CategorySelectListItems = categoryListItems;
                ModelState.AddModelError("", "Xeta bas verdi, yeniden yoxlayin!");

                return View(model);
            }

            if (!model.ImageFile.IsImage())
            {
                ModelState.AddModelError("ImageFile", "Sekil secilmelidir!");
                model.CategorySelectListItems = categoryListItems;

                return View(model);
            }

            if (!model.ImageFile.IsAllowedSize(1))
            {
                ModelState.AddModelError("ImageFile", "Sekil hecmi 1mb-dan cox ola bilmez");
                model.CategorySelectListItems = categoryListItems;

                return View(model);
            }

            var unicalImageFileName = await model.ImageFile.GenerateFile(FilePathConstants.DishPath);

            var dish = new Dish
            {
                Title = model.Title,
                Description = model.Description,
                Price = model.Price,
                CategoryId = model.CategoryId,
                ImageUrl = unicalImageFileName
            };          

            await _dbContext.Dishes.AddAsync(dish);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete([FromBody] RequestModel requestModel)
        {
            var dish = await _dbContext.Dishes
                .FirstOrDefaultAsync(x => x.Id == requestModel.Id);

            if (dish == null) return NotFound();

            var removedDish = _dbContext.Dishes.Remove(dish);
            await _dbContext.SaveChangesAsync();

            if (removedDish != null)
            {
              System.IO.File.Delete(Path.Combine(FilePathConstants.DishPath, dish.ImageUrl));
            }

            return Json(removedDish.Entity);
        }
        public async Task<IActionResult> Update(int id)
        {
            var dish = _dbContext.Dishes
                .SingleOrDefault(x => x.Id == id);

            var categories = await _dbContext.Categories.ToListAsync();
            var categoryListItems = categories.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();

            if (dish == null)
                return NotFound();

            var dishUpdateModel = new DishUpdateViewModel
            {
                Title = dish.Title,
                Description = dish.Description,
                CategoryId = dish.CategoryId,
                CategorySelectListItems = categoryListItems,
                ImageUrl = dish.ImageUrl,
                Price = dish.Price,
                ImageFile = null
            };
         
            return View(dishUpdateModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, DishUpdateViewModel model)
        {
            var dish = await _dbContext.Dishes.FirstOrDefaultAsync(d => d.Id == id);

            if (dish == null) return NotFound();

            var categories = await _dbContext.Categories.ToListAsync();
            var categoryListItems = categories.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();

            if (!ModelState.IsValid)
            {
                model.CategorySelectListItems = categoryListItems;

                return View(model);
            }

            dish.Title = model.Title;
            dish.Description = model.Description;
            dish.Price = model.Price;
            dish.CategoryId = model.CategoryId;

            if (model.ImageFile != null)
            {
                if (!model.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "Sekil secilmelidir!");
                    model.CategorySelectListItems = categoryListItems;

                    return View(model);
                }

                if (!model.ImageFile.IsAllowedSize(1))
                {
                    ModelState.AddModelError("ImageFile", "Sekil hecmi 1mb-dan cox ola bilmez");
                    model.CategorySelectListItems = categoryListItems;

                    return View(model);
                }

                var unicalImageFileName = await model.ImageFile.GenerateFile(FilePathConstants.DishPath);

                if (dish.ImageUrl != null)
                {
                    System.IO.File.Delete(Path.Combine(FilePathConstants.DishPath, dish.ImageUrl));
                }

                 dish.ImageUrl = unicalImageFileName;
            }    

            _dbContext.Dishes.Update(dish);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}
