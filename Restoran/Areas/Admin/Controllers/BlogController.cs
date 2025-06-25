using Microsoft.AspNetCore.Mvc;
using Restoran.Areas.Admin.Data;
using Restoran.Data;
using Restoran.DataContext.Entities;
using Restoran.DataContext;
using Microsoft.EntityFrameworkCore;
using Restoran.Areas.Admin.Extensions;

namespace Restoran.Areas.Admin.Controllers
{
    public class BlogController : AdminController
    {
        private readonly AppDbContext _dbContext;
        public BlogController(AppDbContext context)
        {
            _dbContext = context;
        }
        public async Task<IActionResult> Index()
        {
            var blogs = await _dbContext.Blogs.ToListAsync();

            return View(blogs);
        }
        public async Task<IActionResult> Details(int id)
        {
            var blog = await _dbContext.Blogs.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

            return View(blog);
        }
        public async Task<IActionResult> Create()
        {
            var blogCreateModel = new BlogCreateViewModel
            {
                Name = string.Empty,
                DateTime = DateTime.UtcNow,
                Description = string.Empty,
                Content = string.Empty,
                ImageFile = null,
                ImageUrl = string.Empty
            };

            return View(blogCreateModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Xeta bas verdi, yeniden yoxlayin!");
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
            var unicalCoverImageFileName = await model.ImageFile.GenerateFile(FilePathConstants.BlogPath);
            var blog = new Blog
            {
                Name = model.Name,
                DateTime = model.DateTime,
                Description = model.Description,
                Content = model.Content,
                ImageUrl = unicalCoverImageFileName,
                
                
            };
            await _dbContext.Blogs.AddAsync(blog);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));


        }
        public async Task<IActionResult> Delete([FromBody] RequestModel requestModel)
        {
            var blog = await _dbContext.Blogs
                .FirstOrDefaultAsync(x => x.Id == requestModel.Id);

            if (blog == null) return NotFound();

            var removedBlogs = _dbContext.Blogs.Remove(blog);
            await _dbContext.SaveChangesAsync();

            if (blog.ImageUrl != null)
            {
                System.IO.File.Delete(Path.Combine(FilePathConstants.BlogPath, blog.ImageUrl));
            }

            return Json(removedBlogs.Entity);
        }
        public IActionResult Update(int id)
        {
            var blog = _dbContext.Blogs
                .SingleOrDefault(x => x.Id == id);

            if (blog == null)
                return NotFound();

            var blogUpdateModel = new BlogUpdateViewModel
            {
                Content = blog.Content,
                DateTime = blog.DateTime,
                Description = blog.Description,
                Name = blog.Name,
                CommentCount = blog.CommentCount,
                ImageUrl = blog.ImageUrl,
                ImageFile = null 
            };

            return View(blogUpdateModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, BlogUpdateViewModel model)
        {
            var blog = await _dbContext.Blogs.FirstOrDefaultAsync(s => s.Id == id);

            if (blog == null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            blog.Name = model.Name;
            blog.DateTime = model.DateTime;
            blog.Description = model.Description;
            blog.Content = model.Content;
            
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

                var unicalCoverImageFileName = await model.ImageFile.GenerateFile(FilePathConstants.BlogPath);

                if (blog.ImageUrl != null)
                {
                    System.IO.File.Delete(Path.Combine(FilePathConstants.SliderPath, blog.ImageUrl));
                }
                blog.ImageUrl = unicalCoverImageFileName;
            }

            _dbContext.Blogs.Update(blog);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
