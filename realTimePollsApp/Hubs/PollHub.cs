using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs
{
    public class PollHub : Hub
    {
        public Task JoinGroup(string groupName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync($"Connected {Context.ConnectionId}");
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
