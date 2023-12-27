using ERP.Api.Entity.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ERP.Api.Models.Response;
using ERP.Api.Models.Request;
using ERP.Api.Entity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ERP.Api.Controllers;

[Route("api/suppliers")]
[ApiController]
public class SupplierController : ControllerBase
{
    private readonly ISupplierService _service;

    public SupplierController (ISupplierService service)
    {
        _service = service;
    }

    // GET: api/<SupplierController>
    [HttpGet, Authorize]
    public async Task<IActionResult> GetAll()
    {
        var result= await _service.GetAll();
        if (result == null || (result.Any() == false))
            return StatusCode(204, new HttpResult
            {
                StatusCode = 204,
                Message = "No se han encontrado proveedores."
            });
        return Ok(new HttpResult { Response = result});
    }

    [HttpGet, Authorize]
    [Route("active")]
    public async Task<IActionResult> GetActive()
    {
        var result = await _service.GetActiveSupplier();
        if (result == null || (result.Any() == false))
            return StatusCode(204, new HttpResult
            {
                StatusCode = 204,
                Message = "No se han encontrado proveedores activos."
            });
        return Ok(new HttpResult { Response = result });
    }

    // GET api/<SupplierController>/5
    [HttpGet("{id}"), Authorize]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _service.GetById(id);
        if (result == null)
        {
            return NotFound(new HttpResult
            {
                StatusCode = 404,
                Message= "No se ha encontrado el proveedor buscado.",
                Request= id
            });
        }
        return Ok( new HttpResult { Response= result});
    }

    // GET api/<SupplierController>/find/5
    [HttpGet("find/{name}"), Authorize]
    public async Task<IActionResult> GetByName(string name)
    {
        var result = await _service.GetByName(name);
        if (result == null)
        {
            return NotFound(new HttpResult
            {
                StatusCode = 404,
                Message = "No se ha encontrado el proveedor buscado.",
                Request = name
            });
        }
        return Ok(new HttpResult { Response = result });
    }

    // POST api/<SupplierController>
    [HttpPost("create"), Authorize]
    public async Task<IActionResult> Create([FromBody] SaveSupplier newSupplier)
    {
        if (ModelState.IsValid == false) return ValidationProblem(ModelState);
        var supplier = new Supplier {
               Name = newSupplier.Name,
               Adress = newSupplier.Address,
               Phone = newSupplier.Phone,
               State = newSupplier.State,
            };
        var result = await _service.CreateSupplier(supplier);
        if (result == 0) return StatusCode(500, new HttpResult
            {
                StatusCode= 500,
                Message= "Ha ocurrido un error inesperado al intentar crear el proveedor.",
                Request= newSupplier
            });
        return Ok(new HttpResult { Message = "Proveedor creado con éxito."});
    }

    // PUT api/<SupplierController>/5
    [HttpPut("{id}"), Authorize]
    public async Task<IActionResult> Put(int id, [FromBody] SaveSupplier editSupplier)
    {
        if (ModelState.IsValid == false) return ValidationProblem(ModelState);
        var supplier = new Supplier
        {
            Id = id,
            Name = editSupplier.Name,
            Adress = editSupplier.Address,
            Phone = editSupplier.Phone,
            State = editSupplier.State,
        };
        var result = await _service.UpdateSupplier(supplier);
        if (result == false) return NotFound(new HttpResult
        {
            StatusCode = 404,
            Message = "No se ha encontrado al proveedor.",
            Request = editSupplier
        });
        return Ok(new HttpResult { Message = "Proveedor actualizado con éxito." });
    }

    // DELETE api/<SupplierController>/5
    [HttpDelete("{id}"), Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteSupplier(id);
        if (result == false)
            return NotFound(new HttpResult { Message= "No se ha encontrado al proveedor." });
        return Ok(new HttpResult { Message= "Proveedor eliminado con exito." });
    }
}
