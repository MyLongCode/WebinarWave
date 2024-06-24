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

        [Route("/room/create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [Route("/room/create")]
        [HttpPost]
        public IActionResult Create(string roomName)
        {
            var room = new Models.Room
            {
                Name = roomName,
            };
            db.Rooms.Add(room);
            db.SaveChanges();
            return RedirectToAction("JoinRoom", room.Id);

        }
        [Route("/room/{roomId}")]
        [HttpGet]
        public IActionResult JoinRoom(int roomId)
        {
            var messages = db.Messages.Include(u => u.User).Select(m => m.RoomId == roomId).ToList();
            return View(messages);
        }
    }
}
