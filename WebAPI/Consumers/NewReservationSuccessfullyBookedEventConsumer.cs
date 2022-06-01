using Models.Reservations;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using WebAPI.Hubs;
using WebAPI.Repositories;

namespace WebAPI.Consumers
{
    public class NewReservationSuccessfullyBookedEventConsumer : IConsumer<NewReservationSuccessfullyBookedEvent>
    {
        readonly IHubContext<EventHub> _eventHub;
        private readonly ITopDestinationsRepository _destinationsRepository;
        public NewReservationSuccessfullyBookedEventConsumer(IHubContext<EventHub> eventHub, ITopDestinationsRepository destinationsRepository)
        {
            _eventHub = eventHub;
            _destinationsRepository = destinationsRepository;
        }

        public async Task Consume(ConsumeContext<NewReservationSuccessfullyBookedEvent> context)
        {
            var user = context.Message.User;
            var destination = context.Message.Destination;
            var hotelName = context.Message.HotelName;
            var smallRooms = context.Message.SmallRooms;
            var bigRooms = context.Message.BigRooms;
            var hasOwnTransport = context.Message.HasOwnTransport;
            var message = new EventMessage() { User = user, Destination = destination, HotelName = hotelName };
            await _eventHub.Clients.All.SendAsync("EventMessage", message);
            var oldTopDestinations = _destinationsRepository.GetTopDestinations(3);
            _destinationsRepository.AddDestination(destination);
            var newTopDestinations = _destinationsRepository.GetTopDestinations(3);
            if(!oldTopDestinations.SequenceEqual(newTopDestinations))
            {
                var topDestinationsMessage = new TopDestinationsMessage() { TopDestinations = newTopDestinations };
                await _eventHub.Clients.All.SendAsync("TopDestinationsMessage", topDestinationsMessage);
            }
        }
    }
}
