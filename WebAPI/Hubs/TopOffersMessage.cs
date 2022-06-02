namespace WebAPI.Hubs
{
    public class TopOffersMessage
    {
        public IEnumerable<(string HotelName, bool PrefersBigRooms, bool HasOwnTransport)> TopOffers;
    }
}
