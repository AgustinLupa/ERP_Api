using ERP.Api.Entity;
using ERP.Api.Entity.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers
{
    [Route("api/role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        IRoleService data;

        public RoleController(IRoleService roleService)
        {
            data = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await data.GetAll();
            return Ok(users);
        }

        [HttpGet]
        [Route("active")]
        public async Task<IActionResult> GetActiveRole()
        {
            var rsp= await data.GetActiveRole();
            return Ok(rsp);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var rsp = await data.GetById(id);
            return Ok(rsp);
        }
    }
}
