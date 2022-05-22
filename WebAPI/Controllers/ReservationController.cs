using Microsoft.AspNetCore.Mvc;
using MassTransit;
using Models.Offers;
using Models.Offers.Dto;
using Models.Reservations;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        readonly IRequestClient<ReserveOfferEvent> _reservationClient;
        readonly IRequestClient<AskForReservationStatusEvent> _reservationStatusClient;

        public ReservationController(IRequestClient<ReserveOfferEvent> reservationClient, IRequestClient<AskForReservationStatusEvent> reservationStatusClient)
        {
            _reservationClient = reservationClient;
            _reservationStatusClient = reservationStatusClient;
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
                    + reservationInformation.Children_under_10 + reservationInformation.Children_under_18
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
    }
}