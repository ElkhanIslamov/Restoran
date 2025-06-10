using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pb304PetShop.Areas.Admin.Data;
using Pb304PetShop.Areas.Admin.Extensions;
using Restoran.Areas.Admin.Data;
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
            var dishes = await _dbContext.Dishes.ToListAsync();

            return View(dishes);
        }
        public async Task<IActionResult> Details(int id)
        {
            var dish = await _dbContext.Dishes.AsNoTracking().SingleOrDefaultAsync(d => d.Id == id);

            return View(dish);
        }
        public async Task<IActionResult> Create()
        {
            var dishCreateModel = new DishCreateViewModel
            {
                Title = string.Empty,
                Description = string.Empty,
                ImageUrl = string.Empty,
                Price = 0,
                ImageFile = null
            };
            return View(dishCreateModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DishCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!model.ImageFile.IsImage())
            {
                ModelState.AddModelError("ImageFile", "Sekil secilmelidir!");

                return View(model);
            }

            if (!model.ImageFile.IsAllowedSize(1))
            {
                ModelState.AddModelError("ImageFile", "Sekil hecmi 1mb-dan cox ola bilmez");

                return View(model);
            }


            var unicalCoverImageFileName = await model.ImageFile.GenerateFile(FilePathConstants.DishPath);

            var dish = new Dish
            {
                Title = model.Title,
                Description = model.Description,
                ImageUrl = unicalCoverImageFileName,
                Price = model.Price
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

            var removedSlider = _dbContext.Dishes.Remove(dish);
            await _dbContext.SaveChangesAsync();

            return Json(removedSlider.Entity);
        }
        public IActionResult Update(int id)
        {
            var dish = _dbContext.Dishes
                .SingleOrDefault(x => x.Id == id);

            if (dish == null)
                return NotFound();

            var dishUpdateModel = new DishUpdateViewModel
            {
                Title = dish.Title,
                Description = dish.Description,
                ImageUrl = dish.ImageUrl,
                Price = dish.Price,
                ImageFile = null
            };
         
            return View(dishUpdateModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, SliderUpdateViewModel model)
        {
            var slider = await _dbContext.Sliders.FirstOrDefaultAsync(s => s.Id == id);

            if (slider == null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            slider.FirstTitle = model.FirstTitle;
            slider.SecondTitle = model.SecondTitle;
            slider.Description = model.Description;
            slider.ImageFile = model.CoverImageFile;

            if (!model.CoverImageFile.IsAllowedSize(1))
            {
                ModelState.AddModelError("ImageFile", "Sekil hecmi 1mb-dan cox ola bilmez");

                return View(model);
            }

            var unicalCoverImageFileName = await model.CoverImageFile.GenerateFile(FilePathConstants.SliderPath);

            if (slider.CoverImageUrl != null)
            {
                System.IO.File.Delete(Path.Combine(FilePathConstants.SliderPath, slider.CoverImageUrl));
            }

            slider.CoverImageUrl = unicalCoverImageFileName;

            _dbContext.Sliders.Update(slider);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}
