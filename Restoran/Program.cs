using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restoran.DataContext.Entities;
using Restoran.DataContext;
using Restoran.Areas.Admin.Data;

namespace Restoran
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });

            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequiredLength = 5;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;

                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.AllowedForNewUsers = true;
            })
             .AddEntityFrameworkStores<AppDbContext>()
             .AddDefaultTokenProviders();

            builder.Services.AddScoped<DataInitializer>();

            FilePathConstants.SliderPath = Path.Combine(builder.Environment.WebRootPath, "images", "slider");
            FilePathConstants.DishPath = Path.Combine(builder.Environment.WebRootPath, "images", "dish");
            FilePathConstants.ProductPath = Path.Combine(builder.Environment.WebRootPath, "images", "product");
            FilePathConstants.GridPath = Path.Combine(builder.Environment.WebRootPath, "images", "grid");
            FilePathConstants.BlogPath = Path.Combine(builder.Environment.WebRootPath, "images", "blog");

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
           

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
