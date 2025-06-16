using Microsoft.AspNetCore.Mvc;
using Restoran.Areas.Admin.Data;
using Restoran.Data;
using Restoran.DataContext.Entities;
using Restoran.DataContext;
using Microsoft.EntityFrameworkCore;
using Restoran.Areas.Admin.Extensions;

namespace Restoran.Areas.Admin.Controllers
{
    public class GridController : AdminController
    {
        private readonly AppDbContext _dbContext;
        public GridController(AppDbContext context)
        {
            _dbContext = context;
        }
        public async Task<IActionResult> Index()
        {
            var grids = await _dbContext.Grids.ToListAsync();

            return View(grids);
        }
        public async Task<IActionResult> Details(int id)
        {
            var grid = await _dbContext.Grids.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

            return View(grid);
        }
        public async Task<IActionResult> Create()
        {
            var gridCreateModel = new GridCreateViewModel
            { 
                Description = string.Empty, 
                Name = string.Empty, 
                ImageFile = null,               
            };

            return View(gridCreateModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GridCreateViewModel model)
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

            var unicalCoverImageFileName = await model.ImageFile.GenerateFile(FilePathConstants.GridPath);

            var grid = new Grid
            {
                Description = model.Description,
                Name = model.Name,
                Price = model.Price,
                ImageUrl = unicalCoverImageFileName
            };

            await _dbContext.Grids.AddAsync(grid);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete([FromBody] RequestModel requestModel)
        {
            var grid = await _dbContext.Grids
                .FirstOrDefaultAsync(x => x.Id == requestModel.Id);

            if (grid == null) return NotFound();

            var removedGrid = _dbContext.Grids.Remove(grid);
            await _dbContext.SaveChangesAsync();

            if (grid.ImageUrl != null)
            {
                System.IO.File.Delete(Path.Combine(FilePathConstants.SliderPath, grid.ImageUrl));
            }

            return Json(removedGrid.Entity);
        }
        public IActionResult Update(int id)
        {
            var grid = _dbContext.Grids
                .SingleOrDefault(x => x.Id == id);

            if (grid == null)
                return NotFound();

            var gridUpdateModel = new GridUpdateViewModel
            {
                Name = grid.Name,
                Description = grid.Description,
                ImageUrl = grid.ImageUrl,
                Price = grid.Price,
                ImageFile = null
            };

            return View(gridUpdateModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, GridUpdateViewModel model)
        {
            var grid = await _dbContext.Grids.FirstOrDefaultAsync(s => s.Id == id);

            if (grid == null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            grid.Name = model.Name;
            grid.Description = model.Description;
          
            if (model.ImageFile != null)
            {
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

                var unicalCoverImageFileName = await model.ImageFile.GenerateFile(FilePathConstants.GridPath);

                if (grid.ImageUrl != null)
                {
                    System.IO.File.Delete(Path.Combine(FilePathConstants.GridPath, grid.ImageUrl));
                }
                grid.ImageUrl = unicalCoverImageFileName;
            }

            _dbContext.Grids.Update(grid);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}
