using Microsoft.AspNetCore.SignalR;
using WebinarWave.Data;

namespace WebinarWave.SignalR
{
    public class ChatHub : Hub
    {
        public ApplicationDbContext db;
        public ChatHub(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task Message(int roomId,string username, string message)
        {
            var user = db.Users.FirstOrDefault(u => u.Username == username);
            db.Messages.Add(new Models.Message
            {
                User = user,
                Text = message,
                RoomId = roomId
            });
            db.SaveChanges();
            await Clients.All.SendAsync("ReceiveMessage", username, message);
        }
    }
}
