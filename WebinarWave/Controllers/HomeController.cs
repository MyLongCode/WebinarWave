using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebinarWave.Data;
using WebinarWave.Models;

namespace WebinarWave.Controllers
{
    public class HomeController : Controller
    {
        public ApplicationDbContext db;
        public HomeController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}