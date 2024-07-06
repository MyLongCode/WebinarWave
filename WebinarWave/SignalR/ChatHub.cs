using Microsoft.AspNetCore.SignalR;
using System.Globalization;
using WebinarWave.Data;
using WebinarWave.Models;

namespace WebinarWave.SignalR
{
    public class ChatHub : Hub
    {
        public ApplicationDbContext db;
        public static Dictionary<string, int> RoomsUsers = new Dictionary<string, int>();
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

        public static int EditUsers(string roomName, int userCount)
        {
            if (RoomsUsers.ContainsKey(roomName)) RoomsUsers[roomName] += userCount;
            else RoomsUsers[roomName] = 1;
            return RoomsUsers[roomName];
        }

        public Task JoinRoom(string roomName)
        {
            Clients.Group(roomName).SendAsync("UserJoinOrLeave", EditUsers(roomName, 1));
            return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public Task LeaveRoom(string roomName)
        {
            Clients.Group(roomName).SendAsync("UserJoinOrLeave", EditUsers(roomName, -1));
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }
    }
}
