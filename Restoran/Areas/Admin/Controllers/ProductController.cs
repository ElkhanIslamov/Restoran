using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Restoran.Areas.Admin.Data;
using Restoran.Data;
using Restoran.DataContext.Entities;
using Restoran.DataContext;
using Restoran.Areas.Admin.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Restoran.Areas.Admin.Controllers
{
    public class ProductController : AdminController
    {
        private readonly AppDbContext _dbContext;
        public ProductController(AppDbContext context)
        {
            _dbContext = context;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _dbContext.Products
                  .Include(p => p.ShopCategory)
                  .ToListAsync();

            return View(products);
        }
        public async Task<IActionResult> Details(int id)
        {
            var product = await _dbContext.Products
                  .Include(p => p.ShopCategory)
                  .AsNoTracking()
                  .SingleOrDefaultAsync(p => p.Id == id);

            return View(product);
        }
        public async Task<IActionResult> Create()
        {
            var categories = await _dbContext.ShopCategories.ToListAsync();
            var categoryListItems = categories.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();

            var productCreateModel = new ProductCreateViewModel
            {
                Name = string.Empty,
                ShopCategorySelectListItems = categoryListItems,
                ImageUrl = string.Empty,
                ImageFile = null
            };
            return View(productCreateModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            var categories = await _dbContext.ShopCategories.ToListAsync();
            var categoryListItems = categories.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();

            if (!ModelState.IsValid)
            {
                model.ShopCategorySelectListItems = categoryListItems;
                ModelState.AddModelError("", "Xeta bas verdi, yeniden yoxlayin!");

                return View(model);
            }

            if (!model.ImageFile.IsImage())
            {
                ModelState.AddModelError("ImageFile", "Sekil secilmelidir!");
                model.ShopCategorySelectListItems = categoryListItems;

                return View(model);
            }

            if (!model.ImageFile.IsAllowedSize(1))
            {
                ModelState.AddModelError("ImageFile", "Sekil hecmi 1mb-dan cox ola bilmez");
                model.ShopCategorySelectListItems = categoryListItems;

                return View(model);
            }

            var unicalImageFileName = await model.ImageFile.GenerateFile(FilePathConstants.ProductPath);

            var product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                ShopCategoryId = model.ShopCategoryId,
                ImageUrl = unicalImageFileName
            };

            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete([FromBody] RequestModel requestModel)
        {
            var product = await _dbContext.Products
                .FirstOrDefaultAsync(x => x.Id == requestModel.Id);

            if (product == null) return NotFound();

            var removedProduct = _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();

            if (removedProduct != null)
            {
                System.IO.File.Delete(Path.Combine(FilePathConstants.ProductPath, product.ImageUrl));
            }

            return Json(removedProduct.Entity);
        }
        public async Task<IActionResult> Update(int id)
        {
            var product = _dbContext.Products
                .SingleOrDefault(x => x.Id == id);

            var categories = await _dbContext.Categories.ToListAsync();
            var categoryListItems = categories.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();

            if (product == null)
                return NotFound();

            var productUpdateModel = new ProductUpdateViewModel
            {
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                ShopCategoryId = product.ShopCategoryId,
                ShopCategorySelectListItems = categoryListItems,
                ImageFile = null
            };

            return View(productUpdateModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, ProductUpdateViewModel model)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(d => d.Id == id);

            if (product == null) return NotFound();

            var categories = await _dbContext.Categories.ToListAsync();
            var categoryListItems = categories.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();

            if (!ModelState.IsValid)
            {
                model.ShopCategorySelectListItems = categoryListItems;

                return View(model);
            }

            product.Name = model.Name;
            product.Price = model.Price;
            product.ShopCategoryId = model.ShopCategoryId;

            if (model.ImageFile != null)
            {
                if (!model.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "Sekil secilmelidir!");
                    model.ShopCategorySelectListItems = categoryListItems;

                    return View(model);
                }

                if (!model.ImageFile.IsAllowedSize(1))
                {
                    ModelState.AddModelError("ImageFile", "Sekil hecmi 1mb-dan cox ola bilmez");
                    model.ShopCategorySelectListItems = categoryListItems;

                    return View(model);
                }

                var unicalImageFileName = await model.ImageFile.GenerateFile(FilePathConstants.ProductPath);

                if (product.ImageUrl != null)
                {
                    System.IO.File.Delete(Path.Combine(FilePathConstants.ProductPath, product.ImageUrl));
                }

                product.ImageUrl = unicalImageFileName;
            }

            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
