using Microsoft.AspNetCore.Mvc;
using SmartBus.Application.IServices;

namespace SmartBus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleServices _roleServices;
        public RoleController(IRoleServices roleServices)
        {
            _roleServices = roleServices;
        }
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var result = await _roleServices.CreateRole(roleName);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete("DeleteRole")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var result = await _roleServices.DeleteRole(roleName);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(string userId, string roleName)
        {
            var result = await _roleServices.AddUserToRole(userId, roleName);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("RemoveUserFromRole")]
        public async Task<IActionResult> RemoveUserFromRole(string userId, string roleName)
        {
            var result = await _roleServices.RemoveUserFromRole(userId, roleName);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("GetUserRoles")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var result = await _roleServices.GetUserRoles(userId);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _roleServices.GetAllRoles();
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("GetUsersInRole")]
        public async Task<IActionResult> GetUsersInRole(string roleName)
        {
            var result = await _roleServices.GetUsersInRole(roleName);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
