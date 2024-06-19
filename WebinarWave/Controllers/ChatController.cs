using Microsoft.AspNetCore.Mvc;
using WebinarWave.Data;

namespace WebinarWave.Controllers
{
    public class ChatController : Controller
    {
        public ApplicationDbContext db;
        public ChatController(ApplicationDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
