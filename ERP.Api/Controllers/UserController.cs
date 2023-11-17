using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ERP.Api.Service;
using ERP.Api.Entity.Contracts;
using ERP.Api.Entity;
using System.Xml.Linq;
using ERP.Api.Utils;
using Newtonsoft.Json.Linq;
using ERP.Api.Models.Request;
using ERP.Api.Models.Response;

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
            if(result.Name != "")
            {
                return Ok(json);
            }
            return NotFound();
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(NewUser newUser)
        {
            if (ModelState.IsValid == false) return ValidationProblem(ModelState);
            if (Enum.IsDefined(typeof(Roles), newUser.id_role) == false) 
                return BadRequest(new HttpResult
                    {
                        StatusCode = 400,
                        Message = "No se ha seleccionado un rol valido.",
                        Request= newUser
                    });
            var user = new User
            {
                Name = newUser.username,
                Password = KeySha256.CalculateSHA256(newUser.password),
                Id_Role = (int)newUser.id_role,
            };
            var result = await data.Create(user);
            if (result == 0) return StatusCode(500, new HttpResult
                {
                    StatusCode = 500,
                    Message = "Ha ocurrido un error inesperado.",
                    Request= newUser
                });
            return Ok(new HttpResult(){ Response = result });
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
