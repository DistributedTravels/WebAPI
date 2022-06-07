using Models.Offers;
using WebAPI.Repositories;
using MassTransit;

namespace WebAPI.Consumers
{
    public class ChangedOfferEventConsumer : IConsumer<ChangedOfferEvent>
    {
        private readonly ILastChangesRepository _lastChangesRepository;
        public ChangedOfferEventConsumer(ILastChangesRepository lastChangesRepository)
        {
            _lastChangesRepository = lastChangesRepository;
        }
        public async Task Consume(ConsumeContext<ChangedOfferEvent> context)
        {
            _lastChangesRepository.SaveLastChange(context.Message);
        }
    }
}
