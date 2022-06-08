using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Models.Reservations;
using WebAPI.Hubs;

namespace WebAPI.Consumers
{
    public class ChangedReservationEventConsumer : IConsumer<ChangedReservationEvent>
    {
        readonly IHubContext<EventHub> _eventHub;
        public ChangedReservationEventConsumer(IHubContext<EventHub> eventHub)
        {
            _eventHub = eventHub;
        }
        public async Task Consume(ConsumeContext<ChangedReservationEvent> context)
        {
            var message = new LastReservationChangeMessage()
            {
                NewReservation = context.Message
            };
            await _eventHub.Clients.All.SendAsync("ReservationChange", message);
        }
    }
}
