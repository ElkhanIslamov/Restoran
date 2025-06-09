using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Restoran.Models;

namespace Restoran.Controllers
{
    public class HomeController : Controller
    {     
        public IActionResult Index()
        {
            return View();
        }      
    }
}
