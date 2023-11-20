using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebMvcProject.Data;
using WebMvcProject.Models;

namespace WebMvcProject.Controllers
{
    public class HomeController : Controller
    {

        private readonly ProjectDbContext db;
        public HomeController(ProjectDbContext _db)
        {
            db = _db;
        }

        public IActionResult Index(string userName)
        {
            ViewBag.IsRegisterVisible = true;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}