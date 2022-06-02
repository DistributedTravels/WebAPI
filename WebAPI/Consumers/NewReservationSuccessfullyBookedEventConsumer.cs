﻿using Models.Reservations;
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
        private readonly ITopOffersRepository _offersRepository;
        public NewReservationSuccessfullyBookedEventConsumer(IHubContext<EventHub> eventHub, ITopDestinationsRepository destinationsRepository,
            ITopOffersRepository topOffersRepository)
        {
            _eventHub = eventHub;
            _destinationsRepository = destinationsRepository;
            _offersRepository = topOffersRepository;
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
            var oldTopOffers = _offersRepository.GetTopOffers(3);
            _offersRepository.AddOffer(new TopOffer() { HotelName = hotelName, PrefersBigRooms = bigRooms > smallRooms, HasOwnTransport = hasOwnTransport });
            var newTopOffers = _offersRepository.GetTopOffers(3);
            if(!oldTopOffers.SequenceEqual(newTopOffers))
            {
                var topOffersMessage = new TopOffersMessage() { TopOffers = newTopOffers };
                await _eventHub.Clients.All.SendAsync("TopOffersMessage", topOffersMessage);
            }
        }
    }
}
