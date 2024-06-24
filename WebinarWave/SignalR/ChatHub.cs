using Microsoft.AspNetCore.SignalR;
using System.Globalization;
using WebinarWave.Data;
using WebinarWave.Models;

namespace WebinarWave.SignalR
{
    public class ChatHub : Hub
    {
        public ApplicationDbContext db;
        public ChatHub(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task SendMessage(string roomName,string username, string message)
        {
            var user = db.Users.FirstOrDefault(u => u.Username == username);
            db.Messages.Add(new Models.Message
            {
                User = user,
                Text = message,
                RoomId = db.Rooms.FirstOrDefault(u => u.Name == roomName).Id
            });
            db.SaveChanges();
            await Clients.Group(roomName).SendAsync("ReceiveMessage", username, message);
        }

        public Task JoinRoom(string roomName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public Task LeaveRoom(string roomName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }
    }
}
