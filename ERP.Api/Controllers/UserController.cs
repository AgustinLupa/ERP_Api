using Microsoft.AspNetCore.Mvc;
using ERP.Api.Entity.Contracts;
using ERP.Api.Entity;
using ERP.Api.Utils;
using ERP.Api.Models.Request;
using ERP.Api.Models.Response;
using Microsoft.AspNetCore.Authorization;
using ERP.Api.Models.Tools;

namespace ERP.Api.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService data;
    private readonly IConfiguration configuration;

    public UserController(IUserService userService, IConfiguration _configuration)
    {
        data = userService;
        configuration = _configuration;
    }


    [HttpGet, Authorize(Roles="Admin")]
    public async Task< IActionResult> GetAll()
    {
        var user = await data.GetAll();
        if (user.Any() == false)
            return StatusCode(204,
                new HttpResult(204, "No se encontraron usuarios."));
        
        return Ok(new HttpResult { Response = user });
    }

    [HttpGet, Authorize(Roles = "Admin")]
    [Route("active")]
    public async Task<IActionResult> GetActive()
    {
        var user = await data.GetActive();
        if (user.Any() == false)  
            return StatusCode(204, 
                new HttpResult (204, "No se encontraron usuarios activos.")); 

        return Ok(new HttpResult { Response= user });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]LoginCredentials credentials)
    {
        User user = credentials.MapToUserDTO();
        
        var result = await data.Login(user);
        if (result.Id == 0) 
            return StatusCode(500, 
                new HttpResult(500, "Ha ocurrido un error inesperado."));

        if(result.Name != "")
        {
            var token = JWTToken.CreateToken(result, configuration);
            var json = new { Token= token, User = result };
            return Ok(new HttpResult { Message = "Exito", Response = json});
        }

        return NotFound(new HttpResult(404, "Credenciales incorrectas."));
    }

    [HttpPost("create"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] SaveUser newUser)
    {
        if (ModelState.IsValid == false) return ValidationProblem(ModelState);
        if (Enum.IsDefined(typeof(Roles), newUser.id_role) == false) 
            return BadRequest(
                new HttpResult(400, "No se ha seleccionado un rol valido.", request: newUser));

        var user = newUser.MapToUserDTO();
        var result = await data.Create(user);

        if (result == 0) 
            return StatusCode(500, 
                new HttpResult(500, "Ha ocurrido un error inesperado.", request: newUser));

        return Ok(new HttpResult(){ Response = result });
    }

    [HttpPut("update/{id}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] EditUser editUser)
    {
        if (ModelState.IsValid == false) return ValidationProblem(ModelState);

        if (Enum.IsDefined(typeof(Roles), editUser.id_role) == false)
            return BadRequest(
                new HttpResult(400, "No se ha seleccionado un rol valido.", request: editUser));

        User user = editUser.MapToUserDTO(id);
        var result = await data.Update(user);

        if (result == 0) 
            return StatusCode(500, 
                new HttpResult(500, "Ha ocurrido un error inesperado.", request: editUser));

        return Ok(new HttpResult() { Response = result });
    }

    [HttpDelete("delete/{id_user}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id_user)
    {            
        var result = await data.Delete(id_user);
        if (result == 0) return NotFound(new HttpResult(404, "No se ha encontrado el usuario."));
        return Ok(new HttpResult());
    }
}
