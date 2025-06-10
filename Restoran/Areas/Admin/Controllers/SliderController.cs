using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pb304PetShop.Areas.Admin.Data;
using Pb304PetShop.Areas.Admin.Extensions;
using Restoran.Areas.Admin.Data;
using Restoran.Data;
using Restoran.DataContext;
using Restoran.DataContext.Entities;

namespace Restoran.Areas.Admin.Controllers
{
    public class SliderController : AdminController
    {
        private readonly AppDbContext _dbContext;
        public SliderController(AppDbContext context)
        {
            _dbContext = context;
        }
        public async Task<IActionResult> Index()
        {
            var sliders = await _dbContext.Sliders.ToListAsync();

            return View(sliders);
        }
        public async Task<IActionResult> Details(int id)
        {
            var slider = await _dbContext.Sliders.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

            return View(slider);
        }
        public async Task<IActionResult> Create()
        {
            var sliderCreateModel = new SliderCreateViewModel
            {
                FirstTitle = string.Empty,
                SecondTitle = string.Empty,
                Description = string.Empty,
                CoverImageFile = null
            };
            return View(sliderCreateModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateViewModel model)
        {            
            if (!ModelState.IsValid)
            {
               return View(model);
            }

            if (!model.CoverImageFile.IsImage())
            {
                ModelState.AddModelError("ImageFile", "Sekil secilmelidir!");
                
                return View(model);
            }

            if (!model.CoverImageFile.IsAllowedSize(1))
            {
                ModelState.AddModelError("ImageFile", "Sekil hecmi 1mb-dan cox ola bilmez");
               
                return View(model);
            }

            
            var unicalCoverImageFileName = await model.CoverImageFile.GenerateFile(FilePathConstants.SliderPath);

            var slider = new Slider
            {
                FirstTitle = model.FirstTitle,
                SecondTitle = model.SecondTitle,
                Description = model.Description,
                CoverImageUrl = unicalCoverImageFileName
            };         

            await _dbContext.Sliders.AddAsync(slider);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete([FromBody] RequestModel requestModel)
        {
            var slider = await _dbContext.Sliders
                .FirstOrDefaultAsync(x => x.Id == requestModel.Id);

            if (slider == null) return NotFound();

            var removedSlider = _dbContext.Sliders.Remove(slider);
            await _dbContext.SaveChangesAsync();

            return Json(removedSlider.Entity);
        }
        public IActionResult Update(int id)
        {
            var slider = _dbContext.Sliders
                .SingleOrDefault(x => x.Id == id);

            if (slider == null)
                return NotFound();           

            var sliderUpdateModel = new SliderUpdateViewModel
            {
                FirstTitle = slider.FirstTitle,
                SecondTitle = slider.SecondTitle,
                Description = slider.Description,
                CoverImageFile = slider.ImageFile
            };

            return View(sliderUpdateModel);
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

