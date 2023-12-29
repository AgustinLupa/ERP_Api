using ERP.Api.Entity.Contracts;
using ERP.Api.Models.Request;
using ERP.Api.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ERP.Api.Models.Tools;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ERP.Api.Controllers;

[Route("api/employee")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService data;

    public EmployeeController(IEmployeeService employeeService)
    {
        data = employeeService;
    }

    // GET: api/<EmployeeController>
    [HttpGet, Authorize]
    public async Task<IActionResult> GetAll()
    {
        var employees = await data.GetAll();
        if (employees.Any() == false)
            return StatusCode(204,
                new HttpResult(204, "No se encontraron Empleados."));
        return Ok(new HttpResult { Response = employees });
    }

    [HttpGet("active"), Authorize]
    public async Task<IActionResult> GetActive()
    {
        var employees = await data.GetActiveEmployee();
        if (employees.Any() == false) 
            return StatusCode(204,
                new HttpResult (204, "No se encontraron Empleados activos."));
        return Ok(new HttpResult { Response = employees });
    }


    // GET api/<EmployeeController>/5        
    [HttpGet("{code_employee}"), Authorize]
    public async Task<IActionResult> GetByCode(int code_employee)
    {
        var employees = await data.GetByCode(code_employee);
        if (employees == null)
            return StatusCode(204,
                new HttpResult (404, "No se encontraron usuarios."));
        return Ok(new HttpResult { Response = employees });
    }

    // POST api/<EmployeeController>
    [HttpPost, Authorize]
    public async Task<IActionResult> Create([FromBody] SaveEmployee emp)
    {
        if (ModelState.IsValid == false) return ValidationProblem(ModelState);
        var employee = emp.MapToEmployeeDTO();

        var result = await data.CreateEmployee(employee);
        if (result == 0) 
            return StatusCode(500, 
                new HttpResult(500, "Ha ocurrido un error inesperado.", emp));
        if (result >= 1)
            return Ok(new HttpResult { Message = "Exito" });            

        return Ok(new HttpResult() { Response = result });
    }

    // PUT api/<EmployeeController>/5
    [HttpPut("{id}"), Authorize]
    public async Task<IActionResult> UpdateEmployee(int id, [FromBody] SaveEmployee emp)
    {
        if (ModelState.IsValid == false) return ValidationProblem(ModelState);
        var employee = emp.MapToEmployeeDTO(id);

        var result = await data.UpdateEmployee(employee);
        if (result == 0) 
            return StatusCode(500, 
                new HttpResult(500, "Ha ocurrido un error inesperado.", request: emp));
        if (result >= 1)
            return Ok(new HttpResult { Message = "Exito al modificar"});

        return Ok(new HttpResult() { Response = result });
    }

    // DELETE api/<EmployeeController>/5
    [HttpDelete("{code_employee}"), Authorize]
    public async Task<IActionResult> Delete(int code_employee)
    {
        var result = await data.DeleteEmployee(code_employee);
        if (result == 0) return NotFound(new HttpResult(404, "No se ha encontrado el usuario."));
        return Ok(new HttpResult{ Message = "Exito al Eliminar" });
    }

    [HttpGet("find/{name}"), Authorize]
    public async Task<IActionResult> GetByName(string name)
    {
        if(string.IsNullOrEmpty(name))
            return BadRequest(new HttpResult { Message = "Error al leer el nombre" });
        
        var result = await data.GetByName(name);
        if (result.Any() == false)
            return StatusCode(204,
                new HttpResult(204, "No se encontraron usuarios."));

        return Ok(new HttpResult { Response = result });
    }
}
