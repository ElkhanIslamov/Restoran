using Microsoft.AspNetCore.Mvc;

namespace Restoran.Areas.Admin.Controllers
{
    public class DashboardController : AdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
