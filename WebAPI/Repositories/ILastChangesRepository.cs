using Models.Offers;

namespace WebAPI.Repositories
{
    public interface ILastChangesRepository
    {
        public IEnumerable<ChangedOfferEvent> GetLastChanges(int number);
        public void SaveLastChange(ChangedOfferEvent changedOfferEvent);
    }
}
