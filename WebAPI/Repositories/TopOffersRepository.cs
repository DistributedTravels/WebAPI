namespace WebAPI.Repositories
{
    public class TopOffersRepository : ITopOffersRepository
    {
        private Dictionary<(string HotelName, bool PrefersBigRooms, bool HasOwnTransport), int> _offers;
        public TopOffersRepository()
        {
            _offers = new Dictionary<(string, bool, bool), int>();
        }
        public IEnumerable<(string HotelName, bool PrefersBigRooms, bool HasOwnTransport)> GetTopOffers(int number)
        {
            var actualNumber = number;
            if (number > _offers.Count)
            {
                actualNumber = _offers.Count;
            }
            var topOffers = _offers.OrderByDescending(x => x.Value).Select(x => x.Key).Take(actualNumber).ToList();
            for(int i = actualNumber; i < number; i++)
            {
                topOffers.Add(("", false, false));
            }
            return topOffers;
        }
        public void AddOffer((string HotelName, bool PrefersBigRooms, bool HasOwnTransport) offer)
        {
            if(_offers.ContainsKey(offer))
            {
                _offers[offer] += 1;
            }
            else
            {
                _offers.Add(offer, 1);
            }
        }
    }
}
