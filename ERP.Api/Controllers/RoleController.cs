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
    }
}
