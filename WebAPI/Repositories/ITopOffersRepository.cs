using WebAPI.Hubs;

namespace WebAPI.Repositories
{
    public interface ITopOffersRepository
    {
        public IEnumerable<TopOffer> GetTopOffers(int number);
        public void AddOffer(TopOffer offer);
    }
}
