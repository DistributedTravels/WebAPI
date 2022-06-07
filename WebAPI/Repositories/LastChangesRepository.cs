using Models.Offers;

namespace WebAPI.Repositories
{
    public class LastChangesRepository : ILastChangesRepository
    {
        private List<ChangedOfferEvent> _events;
        public LastChangesRepository()
        {
            _events = new List<ChangedOfferEvent>();
        }
        public IEnumerable<ChangedOfferEvent> GetLastChanges()
        {
            return _events.Select(e => e).Take(10).ToList();
        }
        public void SaveLastChange(ChangedOfferEvent changedOfferEvent)
        {
            _events.Add(changedOfferEvent);
        }
    }
}
