using Microsoft.AspNetCore.SignalR;

namespace WebAPI.Hubs
{
    public class EventHub : Hub
    {
        public async Task SendMessage(EventMessage message)
        {
            await Clients.All.SendAsync("EventMessage", message);
        }
    }
}
