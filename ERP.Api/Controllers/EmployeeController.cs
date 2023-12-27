using ERP.Api.Entity.Contracts;
using ERP.Api.Models.Request;
using ERP.Api.Models.Response;
using ERP.Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Net;
using ERP.Api.Entity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ERP.Api.Controllers
{
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
            if (employees == null || (employees.Any() == false))
            {
                return StatusCode(204,
                new HttpResult { StatusCode = 204, Message = "No se encontraron Empleados." });
            };
            return Ok(new HttpResult { Response = employees });
        }

        [HttpGet, Authorize]
        [Route("active")]
        public async Task<IActionResult> GetActive()
        {
            var employees = await data.GetActiveEmployee();
            if (employees == null || (employees.Any() == false))
            {
                return StatusCode(204,
                new HttpResult { StatusCode = 204, Message = "No se encontraron Empleados." });
            };
            return Ok(new HttpResult { Response = employees });
        }


        // GET api/<EmployeeController>/5        
        [HttpGet("{code_employee}"), Authorize]
        public async Task<IActionResult> GetByCode(int code_employee)
        {
            var employees = await data.GetByCode(code_employee);
            if (employees == null)
            {
                return StatusCode(204,
                new HttpResult { StatusCode = 404, Message = "No se encontraron usuarios." });
            };
            return Ok(new HttpResult { Response = employees });
        }

        // POST api/<EmployeeController>
        [HttpPost, Authorize]
        public async Task<IActionResult> Create([FromBody] SaveEmployee emp)
        {
            if (ModelState.IsValid == false) return ValidationProblem(ModelState);
            var employee = new Employee()
            {
                Id = emp.Id,
                Name = emp.Name,
                Code_Employee = emp.Code_Employee,
                Dni = emp.Dni,
                LastName = emp.LastName,
                State = emp.State
            };

            var result = await data.CreateEmployee(employee);
            if (result == null) return StatusCode(500, new HttpResult
            {
                StatusCode = 500,
                Message = "Ha ocurrido un error inesperado.",
                Request = emp
            });
            if (result >= 1)
            {                
                var json = new { StatusCode = 200};
                return Ok(new HttpResult { Message = "Exito", Response = json });
            }
            return Ok(new HttpResult() { Response = result });
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] SaveEmployee emp)
        {
            if (ModelState.IsValid == false) return ValidationProblem(ModelState);
            var employee = new Employee()
            {
                Id = id,
                Name = emp.Name,
                Code_Employee = emp.Code_Employee,
                Dni = emp.Dni,
                LastName = emp.LastName,
                State = emp.State
            };

            var result = await data.UpdateEmployee(employee);
            if (result == null) return StatusCode(500, new HttpResult
            {
                StatusCode = 500,
                Message = "Ha ocurrido un error inesperado.",
                Request = emp
            });
            if (result >= 1)
            {
                var json = new { StatusCode = 200 };
                return Ok(new HttpResult { Message = "Exito al modificar", Response = json });
            }
            return Ok(new HttpResult() { Response = result });
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{code_employee}"), Authorize]
        public async Task<IActionResult> Delete(int code_employee)
        {
            var result = await data.DeleteEmployee(code_employee);
            if (result == 0) return NotFound(new HttpResult { StatusCode = 404, Message = "No se ha encontrado el usuario." });
            return Ok(new HttpResult{ Message = "Exito al Eliminar" });
        }


        [HttpGet("{name}"), Authorize]
        [Route("find")]
        public async Task<IActionResult> GetByName(string name)
        {
            if(string.IsNullOrEmpty(name)){
                var json = new { StatusCode = 404 };
                return BadRequest(new HttpResult { Message = "Error al leer el nombre", Response = json });
            }
            var result = await data.GetByName(name);
            if (result == null)
            {
                return StatusCode(204,
                new HttpResult { StatusCode = 404, Message = "No se encontraron usuarios." });
            };
            return Ok(new HttpResult { Response = result });

        }
    }
}
