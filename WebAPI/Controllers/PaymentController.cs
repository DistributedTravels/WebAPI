using Models.Reservations;
using Microsoft.AspNetCore.Mvc;
using MassTransit;
using Models.Payments;
using WebAPI.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        readonly IRequestClient<PaymentInformationForReservationEvent> _client;
        readonly IHubContext<EventHub> _eventHub;
        public PaymentController(IRequestClient<PaymentInformationForReservationEvent> client, IHubContext<EventHub> eventHub)
        {
            _client = client;
            _eventHub = eventHub;
        }

        [HttpPost]
        [Route("SendInformation")]
        public async Task<Guid> SendInformation([FromBody] PaymentInformation paymentInformation)
        {
            var card = new CardCredentials()
            {
                CVV = paymentInformation.CVV,
                Number = paymentInformation.Number,
                ExpDate = paymentInformation.ExpDate,
                FullName = paymentInformation.FullName
            };
            var request = new PaymentInformationForReservationEvent(card, 0)
            {
                CorrelationId = paymentInformation.ReservationId,
                Id = paymentInformation.ReservationId
            };
            var response = await _client.GetResponse<PaymentInformationForReservationReplyEvent>(request);
            return response.Message.CorrelationId;
        }

        [HttpPost]
        [Route("SendEvent")]
        public async Task SendEvent()
        {
            var message = new EventMessage() { Message = "whatever", User = "Ala MaKota" };
            await _eventHub.Clients.All.SendAsync("EventMessage", message);
        }
    }
}
