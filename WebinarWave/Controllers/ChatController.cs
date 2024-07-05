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
            var roomRepeat = db.Rooms.FirstOrDefault(r => r.Name == dto.Name);
            if (roomRepeat != null) return BadRequest();
            var room = new Models.Room
            {
                Name = dto.Name,
                Password = "",
                IsPrivate = false,
            };
            if (dto.Password != "")
            {
                room.IsPrivate = true;
                room.Password = dto.Password;
            }
            db.Rooms.Add(room);
            db.SaveChanges();
            return Ok("room created");

        }
        [Route("/room/join")]
        [HttpGet]
        public IActionResult Join()
        {
            return View();
        }
        [Route("/room/{roomName}")]
        [HttpGet]
        public IActionResult JoinRoom(string roomName)
        {
            var room = db.Rooms.FirstOrDefault(r => r.Name == roomName);
            if (room == null) return BadRequest("room is not defined");
            if (room.IsPrivate == true) 
                return View("JoinWithPassword", new JoinWithPasswordViewModel
                {
                    Name = roomName,
                });
            room.Messages = db.Messages.Include(u => u.User).Where(m => m.RoomId == room.Id).ToArray();
            return View("Room", new RoomViewModel
            {
                Messages = room.Messages,
                Name = room.Name,
                Id = room.Id
            });
        }
        [Route("/room/private/{roomName}")]
        [HttpGet]
        public IActionResult JoinRoom(string roomName, string password)
        {
            var room = db.Rooms.FirstOrDefault(r => r.Name == roomName);
            if (room == null) return BadRequest("room is not defined");
            if (room.Password != password) return BadRequest("Password uncorrect");
            room.Messages = db.Messages.Include(u => u.User).Where(m => m.RoomId == room.Id).ToArray();
            return View("Room", new RoomViewModel
            {
                Messages = room.Messages,
                Name = room.Name,
                Id = room.Id
            });
        }
    }
}
