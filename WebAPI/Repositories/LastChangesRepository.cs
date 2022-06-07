using Models.Offers;
using Models.Offers.Dto;

namespace WebAPI.Repositories
{
    public class LastChangesRepository : ILastChangesRepository
    {
        private List<ChangedOfferEvent> _events;
        public LastChangesRepository()
        {
            _events = new List<ChangedOfferEvent>();
        }
        public IEnumerable<ChangedOfferEvent> GetLastChanges(int number)
        {
            var actualNumber = number;
            if(number > _events.Count())
            {
                actualNumber = _events.Count();
            }
            var lastChanges = _events.Select(e => e).Take(actualNumber).ToList();
            for(var i = actualNumber; i < number ; i++)
            {
                lastChanges.Add(new ChangedOfferEvent()
                {
                    oldOffer = new TripDto(),
                    newOffer = new TripDto()
                });
            }
            return lastChanges;
        }
        public void SaveLastChange(ChangedOfferEvent changedOfferEvent)
        {
            _events.Add(changedOfferEvent);
        }
    }
}
