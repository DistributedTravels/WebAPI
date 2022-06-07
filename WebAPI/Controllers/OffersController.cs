using Microsoft.AspNetCore.Mvc;
using MassTransit;
using Models.Offers;
using Models.Offers.Dto;
using WebAPI.Repositories;
using Models.Hotels;
using Models.Transport;
using Models.WebAPI;


namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OffersController : ControllerBase
    {
        readonly IRequestClient<GetOffersEvent> _getOffersClient;
        readonly IRequestClient<GetInfoFromHotelEvent> _hotelsClient;
        readonly IRequestClient<GetAvailableTravelsEvent> _travelsClient;
        readonly IRequestClient<ChangesInOffersEvent> _changesInOffersClient;
        readonly ILastChangesRepository _lastChangesRepository;

        public OffersController(IRequestClient<GetOffersEvent> getOffersClient, IRequestClient<GetInfoFromHotelEvent> hotelsClient,
            IRequestClient<GetAvailableTravelsEvent> travelsClient, ILastChangesRepository lastChangesRepository, IRequestClient<ChangesInOffersEvent> changesInOffersClient)
        {
            _getOffersClient = getOffersClient;
            _hotelsClient = hotelsClient;
            _travelsClient = travelsClient;
            _lastChangesRepository = lastChangesRepository;
            _changesInOffersClient = changesInOffersClient;
        }

        [HttpGet]
        [Route("GetOffers")]
        public async Task<IEnumerable<TripDto>> GetOffers(string startDate, string endDate, string departure, string destination, int adults, int children_under_3, int children_under_10, int children_under_18)
        {
            var beginDat = DateTime.ParseExact(startDate, "MM-dd-yyyy", null);
            var endDat = DateTime.ParseExact(endDate, "MM-dd-yyyy", null);
            var request = new GetOffersEvent(destination, departure, beginDat, endDat, adults + children_under_3 + children_under_10 + children_under_18, adults, children_under_3, children_under_10, children_under_18);
            var response = await _getOffersClient.GetResponse<GetOffersReplyEvent>(request);
            return response.Message.Trips;
        }

        [HttpGet]
        [Route("SayHello")]
        public async Task<String> SayHello()
        {
            return "Hello";
        }

        [HttpGet]
        [Route("CheckOfferAvailability")]
        public async Task<CheckOfferAvailabilityResponse> CheckOfferAvailability(string startDate, string endDate, int hotelId, int transportId, 
            int adults, int children_under_3, int children_under_10, int children_under_18, int number_of_2_room, 
            int number_of_apartments, string transport, string breakfast, string wifi)
        {
            int numberOfPeople = adults + children_under_3 + children_under_10 + children_under_18;
            var hotelsAskId = Guid.NewGuid();
            var travelsAskId = Guid.NewGuid();
            var beginDateDate = DateTime.ParseExact(startDate, "MM-dd-yyyy", null);
            var endDateDate = DateTime.ParseExact(endDate, "MM-dd-yyyy", null);
            var hasOwnTransport = !transport.Equals("Samolot");
            var hasBreakfast = breakfast.Equals("Tak");
            var hasInternet = wifi.Equals("Tak");
            double hotelPrice = 0.0;
            double travelPrice = 0.0;
            var hotelsAvailable = false;
            var transportAvailable = false;
            var hotelsRequest = new GetInfoFromHotelEvent(hotelId: hotelId,
                beginDate: beginDateDate, endDate: endDateDate, appartmentsAmount: number_of_apartments, 
                casualRoomAmount: number_of_2_room, breakfast: hasBreakfast, wifi: hasInternet)
            {
                Id = hotelsAskId,
                CorrelationId = hotelsAskId
            };
            var hotelsResponse = await _hotelsClient.GetResponse<GetInfoFromHotelEventReply>(hotelsRequest);
            hotelsAvailable = hotelsResponse.Message.Answer == GetInfoFromHotelEventReply.State.CAN_BE_RESERVED;
            if (hotelsAvailable)
            {
                hotelPrice = hotelsResponse.Message.Price;
            }
            if (!hasOwnTransport)
            {
                var travelsRequest = new GetAvailableTravelsEvent() { TravelId = transportId, Id = travelsAskId, CorrelationId = travelsAskId, FreeSeats = numberOfPeople };
                var travelsResponse = await _travelsClient.GetResponse<GetAvailableTravelsReplyEvent>(travelsRequest);
                transportAvailable = travelsResponse.Message.TravelItems.Count() > 0;
                if (transportAvailable)
                {
                    var travelReceived = travelsResponse.Message.TravelItems.ToArray()[0];
                    transportAvailable = travelReceived.AvailableSeats >= numberOfPeople;
                    travelPrice = travelReceived.Price * numberOfPeople;
                }
            }
            else
            {
                transportAvailable = true;
            }
            bool offerAvailable = transportAvailable && hotelsAvailable;
            // TODO check if price not too high
            double totalPrice = (hotelPrice + travelPrice) * 1.5;
            return new CheckOfferAvailabilityResponse()
            {
                Answer = offerAvailable,
                Price = Math.Round(totalPrice, 2)
            };
        }
        [HttpGet]
        [Route("GetLastChanges")]
        public async Task<IEnumerable<ChangedOfferEvent>> GetLastChanges()
        {
            return _lastChangesRepository.GetLastChanges(10);
        }
        [HttpPost]
        [Route("AddChanges")]
        public async Task AddChanges([FromBody] ChangesInOffersEvent change)
        {
            _changesInOffersClient.GetResponse<ChangesInOffersReplyEvent>(change);
        }
    }
}
