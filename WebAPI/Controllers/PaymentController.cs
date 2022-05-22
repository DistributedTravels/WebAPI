using Models.Reservations;
using Microsoft.AspNetCore.Mvc;
using MassTransit;
using Models.Payments;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        readonly IRequestClient<PaymentInformationForReservationEvent> _client;
        public PaymentController(IRequestClient<PaymentInformationForReservationEvent> client)
        {
            _client = client;
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
            var request = new PaymentInformationForReservationEvent(card, 0);
            var response = await _client.GetResponse<PaymentInformationForReservationReplyEvent>(request);
            return response.Message.CorrelationId;
        }
    }
}
