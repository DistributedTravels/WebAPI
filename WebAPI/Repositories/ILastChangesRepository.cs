using Models.Offers;

namespace WebAPI.Repositories
{
    public interface ILastChangesRepository
    {
        public IEnumerable<ChangedOfferEvent> GetLastChanges();
        public void SaveLastChange(ChangedOfferEvent changedOfferEvent);
    }
}
