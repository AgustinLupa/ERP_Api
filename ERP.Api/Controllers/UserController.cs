using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ERP.Api.Service;
using ERP.Api.Entity.Contracts;
using ERP.Api.Entity;
using System.Xml.Linq;
using ERP.Api.Utils;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using ERP.Api.Models.Request;
using ERP.Api.Models.Response;

namespace ERP.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService data;
        private readonly IConfiguration configuration;

        public UserController(IUserService userService, IConfiguration _configuration)
        {
            data = userService;
            this.configuration = _configuration;
        }


        [HttpGet]
        public async Task< IActionResult> GetAll()
        {
            var user = await data.GetAll();
            if (user == null || (user.Any() == false))
            {
                return StatusCode(204,
                new HttpResult { StatusCode = 204, Message = "No se encontraron usuarios." });
            };
            return Ok(new HttpResult { Response = user });
        }

        [HttpGet]
        [Route("active")]
        public async Task<IActionResult> GetActive()
        {
            var user = await data.GetActive();
            if (user == null || (user.Any() == false)) { return StatusCode(204, 
                new HttpResult { StatusCode= 204, Message= "No se encontraron usuarios activos."}); };
            return Ok(new HttpResult { Response= user });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginCredentials credentials)
        {
            User user = new User()
            {
                Name = credentials.username,
                Password = KeySha256.CalculateSHA256(credentials.password)
            };
            
            var result = await data.Login(user);
            if (result == null) return StatusCode(500, new HttpResult
            {
                StatusCode = 500,
                Message = "Ha ocurrido un error inesperado.",
                Request = credentials
            });
            if(result.Name != "")
            {
                var token = JWTToken.CreateToken(result, configuration);
                var json = new { Token= token, User = result };
                return Ok(new HttpResult { Message = "Exito", Response = json});
            }
            return NotFound(new HttpResult { StatusCode = 404, Message = "Credenciales incorrectas."});
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] SaveUser newUser)
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
        [Route("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SaveUser editUser)
        {
            if (ModelState.IsValid == false) return ValidationProblem(ModelState);
            if (Enum.IsDefined(typeof(Roles), editUser.id_role) == false)
                return BadRequest(new HttpResult
                {
                    StatusCode = 400,
                    Message = "No se ha seleccionado un rol valido.",
                    Request = editUser
                });
            User user = new()
            {
                Id = id,
                Name = editUser.username,
                Password = KeySha256.CalculateSHA256(editUser.password),
                Id_Role = (int)editUser.id_role,
                State = editUser.state
            };
            var result = await data.Update(user);
            if (result == 0) return StatusCode(500, new HttpResult
            {
                StatusCode = 500,
                Message = "Ha ocurrido un error inesperado.",
                Request = editUser
            });
            return Ok(new HttpResult() { Response = result });
        }

        [HttpDelete]
        [Route("delete/{id_user}")]
        public async Task<IActionResult> Delete(int id_user)
        {            
            var result = await data.Delete(id_user);
            if (result == 0) return NotFound(new HttpResult { StatusCode = 404, Message = "No se ha encontrado el usuario."});
            return Ok(new HttpResult());
        }
    }
}
