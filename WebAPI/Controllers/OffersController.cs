﻿using Microsoft.AspNetCore.Mvc;
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
        public async Task<IEnumerable<TripDto>> GetOffers(string startDate, string endDate, string departure, string destination, int adults, int children_under_3, int children_under_10, int children_under_18)
        {
            var beginDat = DateTime.ParseExact(startDate, "MM-dd-yyyy", null);
            var endDat = DateTime.ParseExact(endDate, "MM-dd-yyyy", null);

            var response = await _client.GetResponse<GetOffersReplyEvent>(new GetOffersEvent(destination, beginDat, endDat, adults+children_under_3+children_under_10+children_under_18));
            return response.Message.Trips;
        }
    }
}
