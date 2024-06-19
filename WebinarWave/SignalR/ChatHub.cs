using Microsoft.AspNetCore.SignalR;

namespace WebinarWave.SignalR
{
    public class ChatHub : Hub
    {
        public async Task Message(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
