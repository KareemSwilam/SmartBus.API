using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBus.Application.Dtos.CompanyDtos;
using SmartBus.Application.Dtos.UserDtos;
using SmartBus.Application.IServices;
using System.Security.Claims;

namespace SmartBus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IComapnyServices _services;
        public CompanyController(IComapnyServices services)
        {
            _services = services;
        }
        [HttpGet("Company")]
        public async Task<IActionResult> Company(Guid id)
        {
            var result = await _services.GetCompanyById(id);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("Companies")]
        public async Task<IActionResult> Companies()
        {
            var result = await _services.Companies();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("BlockedCompanies")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BlockedCompanies()
        {
            var result = await _services.BlockedCompanies();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("AddingCompany")]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> AddingCompany([FromBody] CreateCompanyDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _services.AddCompany(userId!, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("UnBlockCompany")]
        [Authorize("Admin")]
        public async Task<IActionResult> UnBlockCompany([FromBody] Guid CompanyId)
        {
            var result = await _services.UnBlockCompany(CompanyId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("BlockCompany")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BlockCompany([FromBody] Guid CompanyId)
        {
            var result = await _services.BlockCompany(CompanyId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpDelete("Company")]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> DeleteCompany([FromBody] Guid CompanyId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _services.DeleteCompany(userId!,CompanyId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


    }
}