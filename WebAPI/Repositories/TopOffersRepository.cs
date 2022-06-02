using WebAPI.Hubs;

namespace WebAPI.Repositories
{
    public class TopOffersRepository : ITopOffersRepository
    {
        private Dictionary<TopOffer, int> _offers;
        public TopOffersRepository()
        {
            _offers = new Dictionary<TopOffer, int>();
        }
        public IEnumerable<TopOffer> GetTopOffers(int number)
        {
            var actualNumber = number;
            if (number > _offers.Count)
            {
                actualNumber = _offers.Count;
            }
            var topOffers = _offers.OrderByDescending(x => x.Value).Select(x => x.Key).Take(actualNumber).ToList();
            for(int i = actualNumber; i < number; i++)
            {
                topOffers.Add(new TopOffer() { HotelName = "", PrefersBigRooms = false, HasOwnTransport = false });
            }
            return topOffers;
        }
        public void AddOffer(TopOffer offer)
        {
            bool containsOffer = false;
            foreach(var knownOffer in _offers)
            {
                if(knownOffer.Key.HotelName.Equals(offer.HotelName) && knownOffer.Key.PrefersBigRooms == offer.PrefersBigRooms
                    && knownOffer.Key.HasOwnTransport == offer.HasOwnTransport)
                {
                    containsOffer = true;
                    _offers[knownOffer.Key] += 1;
                }
            }
            if(!containsOffer)
            {
                _offers.Add(offer, 1);
            }
        }
    }
}
