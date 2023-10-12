using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ERP.Api.Service;
using ERP.Api.Entity.Contracts;
using ERP.Api.Entity;
using System.Xml.Linq;
using ERP.Api.Utils;
using Newtonsoft.Json.Linq;

namespace ERP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService data;

        public UserController(IUserService userService)
        {
            data = userService;
        }


        [HttpGet]
        public async Task< IActionResult> GetAll()
        {
            var users = await data.GetAll();
            return Ok(users);
        }

        [HttpGet]
        [Route("/active")]
        public async Task<IActionResult> GetActive()
        {
            var user = await data.GetActive();
            return Ok(user);
        }

        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            User user = new User()
            {
                Name = username,
                Password = KeySha256.CalculateSHA256(password)
            };
            var json = "";
            var result = await data.Login(user);
            if(result.Name == "")
            {
                json = "No existe";
            }
            else
            {
                json = "Existe";
            }
            
            return Ok(JObject.Parse(json));
        }
    }
}
