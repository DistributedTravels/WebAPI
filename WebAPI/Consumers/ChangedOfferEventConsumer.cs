using Models.Offers;
using WebAPI.Repositories;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using WebAPI.Hubs;

namespace WebAPI.Consumers
{
    public class ChangedOfferEventConsumer : IConsumer<ChangedOfferEvent>
    {
        private readonly ILastChangesRepository _lastChangesRepository;
        readonly IHubContext<EventHub> _eventHub;
        public ChangedOfferEventConsumer(ILastChangesRepository lastChangesRepository, IHubContext<EventHub> eventHub)
        {
            _lastChangesRepository = lastChangesRepository;
            _eventHub = eventHub;
        }
        public async Task Consume(ConsumeContext<ChangedOfferEvent> context)
        {
            _lastChangesRepository.SaveLastChange(context.Message);
            var eventMessage = new LastOfferChangeMessage()
            {
                NewOffer = context.Message.NewOffer
            };
            await _eventHub.Clients.All.SendAsync("OfferChanged", eventMessage);
        }
    }
}
