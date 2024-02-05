using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using realTimePolls.Models;

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
            // This newMessage call is what is not being received on the front end
            //await Clients.All.SendAsync("pollHub", "TESTING!!!");

            // This console.WriteLine does print when I bring up the component in the front end.
            //Debug.WriteLine("Test");
            // what needs to happen is when the user is at the home screen:
            // every single user needs to be placed into the same group
            // if they are all in the same group, the server can emit messages to every connected client.

            //The server needs to tell all the clients about the current vote state,
            // This should happen whenever a user votes. Whenever a user votes, they will send a request to the server
            // and then the server will broadcast to the entire group with the new vote state
            // the client will receive that number and display that, not the one from the database

            // you need to make it so that when a user votes, it emits something to the server.
            // then the server emits back to every single user
            //await base.OnConnectedAsync();
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
