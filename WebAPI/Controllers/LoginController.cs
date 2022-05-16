using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MassTransit;
using Models.Offers;
using Models.Offers.Dto;
using System.Security.Cryptography;
using System.Text;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        /*readonly IRequestClient<GetOffersEvent> _client;*/

        public LoginController(/*IRequestClient<GetOffersEvent> client*/)
        {
            /*_client = client;*/
        }

        //[HttpGet]
        //[Route("Auth")]
        //public Task<HttpResponseMessage> Auth(string login, string pw_hash)
        //{
        //    var valid = false;
        //    // check for valid login credentials
        //    if (login.Equals("Ala") && pw_hash.Equals("MaKota"))
        //        valid = true;

        //    HttpResponseMessage response;
        //    if (valid)
        //    {
        //        response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        //    }
        //    else
        //    {
        //        response = new HttpResponseMessage(System.Net.HttpStatusCode.NotAcceptable);
        //    }
        //    return response;
        //}

        private string HashValue(string pw)
        {
            var bytes = Encoding.UTF8.GetBytes(pw);
            var hash = MD5.HashData(bytes);
            return BitConverter.ToString(hash);
        }
    }
}
