namespace WebAPI.Repositories
{
    public interface ITopOffersRepository
    {
        public IEnumerable<(string HotelName, bool PrefersBigRooms, bool HasOwnTransport)> GetTopOffers(int number);
        public void AddOffer((string HotelName, bool PrefersBigRooms, bool HasOwnTransport) offer);
    }
}
