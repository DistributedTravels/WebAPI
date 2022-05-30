using Models.Reservations;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using WebAPI.Hubs;

namespace WebAPI.Consumers
{
    public class NewReservationSuccessfullyBookedEventConsumer : IConsumer<NewReservationSuccessfullyBookedEvent>
    {
        readonly IHubContext<EventHub> _eventHub;
        public NewReservationSuccessfullyBookedEventConsumer(IHubContext<EventHub> eventHub)
        {
            _eventHub = eventHub;
        }

        public async Task Consume(ConsumeContext<NewReservationSuccessfullyBookedEvent> context)
        {
            var user = context.Message.User;
            var destination = context.Message.Destination;
            var hotelName = context.Message.HotelName;
            var message = new EventMessage() { User = user, Destination = destination, HotelName = hotelName };
            await _eventHub.Clients.All.SendAsync("EventMessage", message);
        }
    }
}
