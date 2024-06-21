using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var messages = db.Messages.Include(u => u.User).ToList();
            return View(messages);
        }
    }
}
