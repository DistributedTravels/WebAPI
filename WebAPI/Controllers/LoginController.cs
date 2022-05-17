using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MassTransit;
using Models.Offers;
using Models.Offers.Dto;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        /*readonly IRequestClient<GetOffersEvent> _client;*/
        private readonly List<User> users = new() { 
            new User { UserId = Guid.Parse("ed35e5bf-c0b7-45db-8312-640e517b2414"), UserName = "Ala", Password = "MaKota"},
            new User { UserId = Guid.Parse("a33daeee-3245-4cae-88ed-e2643c87a482"), UserName = "Kevin", Password = "SamWDomu"},
            new User { UserId = Guid.Parse("21d90ae6-f2f3-4552-85ce-5fbb95d2eee2"), UserName = "User1", Password = "1resU"},
            new User { UserId = Guid.Parse("82041dce-2adf-4af7-968d-011c3c59e5c2"), UserName = "User2", Password = "2resU"},
            new User { UserId = Guid.Parse("3c89c7b5-f834-47c0-8c5e-8fe0c8d222bc"), UserName = "Alicja", Password = "Bogdan"},
            new User { UserId = Guid.Parse("7051c674-adcc-420f-944b-4feda1df0c16"), UserName = "Bogdan", Password = "Alicja"},
            new User { UserId = Guid.Parse("b5642e66-bd5b-40f3-8065-d79fd90d95c4"), UserName = "Fortuna", Password = "KolemSieToczy"},
            new User { UserId = Guid.Parse("14942299-2257-4ff8-b34d-fcdf5ea85479"), UserName = "Zagreus", Password = "Thanatos"},
            new User { UserId = Guid.Parse("d29c03a4-55a5-48f0-92ae-4856ca21b61e"), UserName = "Naru", Password = "Mitsu"},
            new User { UserId = Guid.Parse("70884a6e-48da-46ad-bbbd-b28849de6ea8"), UserName = "SanLang", Password = "XieLian"},
        };


        public LoginController(/*IRequestClient<GetOffersEvent> client*/)
        {
            /*_client = client;*/
        }

        [HttpPost]
        [Route("Auth")]
        public IActionResult Auth(string login, string pw)
        {
            var valid = false;
            login =  HttpUtility.UrlDecode(login, Encoding.UTF8);
            pw = HttpUtility.UrlDecode(pw, Encoding.UTF8);
            // check for valid login credentials
            Guid found = Guid.Parse("00000000-0000-0000-0000-000000000000");
            foreach(var user in users)
            {
                if(user.Password == pw && user.UserName == login)
                {
                    valid = true;
                    found = user.UserId;
                    break;
                }
            }

            if (valid)
            {
                return Ok(new { UserId = found.ToString() });
            }
            else
            {
                return NotFound();
            }
        }

    }

    public class User { 
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
