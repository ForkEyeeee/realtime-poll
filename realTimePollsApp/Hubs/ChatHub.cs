using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using realTimePolls.Models;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public Task JoinGroup(string groupName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public override async Task OnConnectedAsync()
        {
            // This newMessage call is what is not being received on the front end
            await Clients.All.SendAsync("chatHub", "TESTING!!!");

            // This console.WriteLine does print when I bring up the component in the front end.
            Debug.WriteLine("Test");

            await base.OnConnectedAsync();
        }
    }
}
