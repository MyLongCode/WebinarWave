using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebinarWave.Controllers.Room;
using WebinarWave.Data;
using WebinarWave.ViewModels;

namespace WebinarWave.Controllers
{
    public class ChatController : Controller
    {
        public ApplicationDbContext db;
        public ChatController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [Route("/room/create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [Route("/room/create")]
        [HttpPost]
        public IActionResult Create([FromBody]CreateRoomRequest dto)
        {
            var room = new Models.Room
            {
                Name = dto.Name,
            };
            db.Rooms.Add(room);
            db.SaveChanges();
            return RedirectToAction("JoinRoom", room.Id);

        }
        [Route("/room/join")]
        [HttpGet]
        public IActionResult Join()
        {
            return View();
        }
        [Route("/room/{roomId}")]
        [HttpGet]
        public IActionResult JoinRoom(int roomId)
        {
            var room = db.Rooms.Find(roomId);
            if (room == null) return BadRequest("room is not defined");
            room.Messages = db.Messages.Include(u => u.User).Where(m => m.RoomId == roomId).ToArray();
            return View("Room", new RoomViewModel
            {
                Messages = room.Messages,
                Name = room.Name,
                Id = roomId
            });
        }
    }
}
