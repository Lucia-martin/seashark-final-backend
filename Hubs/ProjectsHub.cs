using Microsoft.AspNetCore.SignalR;
using MyApp.Models;
namespace MyApp.Hubs
{
	public class ProjectsHub : Hub
	{

        public async Task AddTodo(string todo, string groupId, string username)
        {
            //await Clients.All.SendAsync("ReceiveMessage", _botUser, message);
            await Clients.Group(groupId).SendAsync("ReceiveTodo", username, todo);

        }

        public async Task AddToGroup(string groupId, string groupName, string username)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);

            //await Clients.Group(groupId).SendAsync("ReceiveMessageBot", _botUser, $"{username} has joined the group {groupName}.");
        }

        public async Task RemoveFromGroup(string groupId, string groupName, string username)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);

            //await Clients.Group(groupId).SendAsync("ReceiveMessageBot", _botUser, $"{username} has left the group {groupName}.");
        }

    }
}

