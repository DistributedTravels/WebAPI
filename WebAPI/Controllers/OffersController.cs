using Microsoft.AspNetCore.Mvc;
using MassTransit;
using Models.Offers;

namespace WebAPI.Controllers
{
    public class OffersController : ControllerBase
    {
        readonly IRequestClient<GetOffersEvent> _client;

        public OffersController(IRequestClient<GetOffersEvent> client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<ActionResult<GetOffersReplyEvent>> Offers(/* parametry */)
        {
            var response = await _client.GetResponse<GetOffersReplyEvent>(new GetOffersEvent("Albania", DateTime.Now, DateTime.Now, 3));
            return response.Message;
        }
    }
}
