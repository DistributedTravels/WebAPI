using Microsoft.AspNetCore.Mvc;
using MassTransit;
using Models.Offers;
using Models.Offers.Dto;
using Models.Reservations;
using Models.Reservations.Dto;
using WebAPI.Repositories;
using WebAPI.Hubs;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        readonly IRequestClient<ReserveOfferEvent> _reservationClient;
        readonly IRequestClient<AskForReservationStatusEvent> _reservationStatusClient;
        readonly IRequestClient<GetReservationsFromDatabaseEvent> _getReservationsClient;
        readonly ITopDestinationsRepository _topDestinationsRepository;

        public ReservationController(IRequestClient<ReserveOfferEvent> reservationClient, IRequestClient<AskForReservationStatusEvent> reservationStatusClient,
            IRequestClient<GetReservationsFromDatabaseEvent> getReservationsClient, ITopDestinationsRepository topDestinationsRepository)
        {
            _reservationClient = reservationClient;
            _reservationStatusClient = reservationStatusClient;
            _getReservationsClient = getReservationsClient;
            _topDestinationsRepository = topDestinationsRepository;
        }

        [HttpPost]
        [Route("Reserve")]
        public async Task<ReserveOfferReplyEvent> Reserve([FromBody] ReservationInformation reservationInformation)
        {
            var userId = Guid.Parse(reservationInformation.UserId);
            var hasInternet = reservationInformation.Wifi.Equals("Tak");
            var hasBreakfast = reservationInformation.Breakfast.Equals("Tak");
            var hasOwnTransport = !reservationInformation.Transport.Equals("Samolot");
            var beginDateDate = DateTime.ParseExact(reservationInformation.StartDate, "MM-dd-yyyy", null);
            var endDateDate = DateTime.ParseExact(reservationInformation.EndDate, "MM-dd-yyyy", null);
            var hasPromotionCode = reservationInformation.PromotionCode.Equals("abcd");
            var reservationId = Guid.NewGuid();
            var request = new ReserveOfferEvent()
            {
                Id = reservationId,
                OfferId = reservationId,
                CorrelationId = reservationId,
                ReservationId = reservationId,
                UserId = userId,
                TransportId = reservationInformation.TransportId,
                HotelId = reservationInformation.HotelId,
                HotelName = reservationInformation.HotelName,
                Destination = reservationInformation.Destination,
                Departure = reservationInformation.Departure,
                BeginDate = beginDateDate,
                EndDate = endDateDate,
                Adults = reservationInformation.Adults,
                ChildrenUnder3 = reservationInformation.Children_under_3,
                ChildrenUnder10 = reservationInformation.Children_under_10,
                ChildrenUnder18 = reservationInformation.Children_under_18,
                BigRooms = reservationInformation.Number_of_apartments,
                SmallRooms = reservationInformation.Number_of_2_room,
                HasInternet = hasInternet,
                HasBreakfast = hasBreakfast,
                HasOwnTransport = hasOwnTransport,
                NumberOfPeople = reservationInformation.Adults + reservationInformation.Children_under_3 
                    + reservationInformation.Children_under_10 + reservationInformation.Children_under_18,
                HasPromotionCode = hasPromotionCode
            };
            var response = await _reservationClient.GetResponse<ReserveOfferReplyEvent>(request);
            return response.Message;
        }

        [HttpGet]
        [Route("CheckReservationStatus")]
        public async Task<AskForReservationStatusReplyEvent> CheckReservationStatus(Guid reservationId)
        {
            var request = new AskForReservationStatusEvent()
            {
                Id = reservationId,
                CorrelationId = reservationId,
                ReservationId = reservationId
            };
            var response = await _reservationStatusClient.GetResponse<AskForReservationStatusReplyEvent>(request);
            return response.Message;
        }
        [HttpGet]
        [Route("GetReservations")]
        public async Task<IEnumerable<ReservationDto>> GetReservations(string userId)
        {
            var userGuid = Guid.Parse(userId);
            var response = await _getReservationsClient.GetResponse<GetReservationsFromDatabaseReplyEvent>(new GetReservationsFromDatabaseEvent() { CorrelationId = Guid.NewGuid(), UserId = userGuid });
            var reservations = response.Message.Reservations;
            return reservations;
        }

        [HttpGet]
        [Route("TopDestinations")]
        public async Task<TopDestinationsMessage> TopDestinations()
        {
            var topDestinations = _topDestinationsRepository.GetTopDestinations(3);
            return new TopDestinationsMessage() { TopDestinations = topDestinations };
        }
    }
}