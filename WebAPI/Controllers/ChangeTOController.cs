using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Models.Transport;
using Models.Transport.Dto;
using Models.Hotels;
using Models.WebAPI;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChangeTOController : ControllerBase
    {
        private IPublishEndpoint _endpoint;
        public ChangeTOController(IPublishEndpoint endpoint)
        {
            _endpoint = endpoint;
        }
        [HttpPost]
        [Route("ChangeFlight")]
        public async Task ChangeFlight([FromBody] ChangeFlightModel change)
        {
            var travelChangeDto = new TravelChangeDto()
            {
                Id = change.Id,
                AvailableSeats = change.FreeSeats
            };
            var travelChangeEvent = new UpdateTransportTOEvent()
            {
                Action = UpdateTransportTOEvent.Actions.UPDATE,
                Table = UpdateTransportTOEvent.Tables.TRAVEL,
                TravelDetails = travelChangeDto
            };
            await _endpoint.Publish<UpdateTransportTOEvent>(travelChangeEvent);
        }
        [HttpPost]
        [Route("ChangeHotel")]
        public async Task ChangeHotel([FromBody] ChangeHotelModel change)
        {
            if(change.NewPrice < 0)
            {
                var deleteHotel = new DeleteHotelEvent()
                {
                    HotelId = change.Id
                };
                await _endpoint.Publish<DeleteHotelEvent>(deleteHotel);
            }
            else
            {
                var updatePrice = new ChangeBasePriceEvent()
                {
                    HotelId = change.Id,
                    NewPrice = change.NewPrice
                };
                await _endpoint.Publish<ChangeBasePriceEvent>(updatePrice);
            }
        }
    }
}
