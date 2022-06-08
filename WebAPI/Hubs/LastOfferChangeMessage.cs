using Models.Offers.Dto;

namespace WebAPI.Hubs
{
    public class LastOfferChangeMessage
    {
        public TripDto NewOffer { get; set; }
    }
}
