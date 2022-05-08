using Microsoft.AspNetCore.Mvc;
using MassTransit;
using Models.Offers;
using Models.Offers.Dto;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OffersController : ControllerBase
    {
        readonly IRequestClient<GetOffersEvent> _client;

        public OffersController(IRequestClient<GetOffersEvent> client)
        {
            _client = client;
        }

        [HttpGet]
        [Route("GetOffers")]
        public async Task<IEnumerable<TripDto>> GetOffers(/* parametry */)
        {
            var response = await _client.GetResponse<GetOffersReplyEvent>(new GetOffersEvent("Albania", DateTime.Now, DateTime.Now, 3));
            return response.Message.Trips;
        }
    }
}
