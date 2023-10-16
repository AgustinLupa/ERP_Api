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
    [Route("api/users")]
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
        [Route("active")]
        public async Task<IActionResult> GetActive()
        {
            var user = await data.GetActive();
            return Ok(user);
        }

        [HttpPost]
        [Route("login")]
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
            
            return Ok(json);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(string username, string password, int id_rol)
        {
            User user = new()
            {
                Name = username,
                Password = KeySha256.CalculateSHA256(password),
                Id_Role = id_rol
            };
            var result = await data.Create(user);
            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update(int id_user, string username, string password, int id_role, int state)
        {
            User user = new()
            {
                Id = id_user,
                Name = username,
                Password = KeySha256.CalculateSHA256(password) ,
                Id_Role = id_role,
                State = state               
            };
            var result = await data.Update(user);
            return Ok(result);
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete(int id_user)
        {            
            var result = await data.Delete(id_user);
            return Ok(result);
        }
    }
}
