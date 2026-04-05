using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBus.Application.Dtos.CompanyDtos;
using SmartBus.Application.IServices;

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
        [HttpPost("AddingCompany")]
        public async Task<IActionResult> AddingCompany([FromBody] CreateCompanyDto dto)
        {
            var result = await _services.AddCompany(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("Company")]
        public async Task<IActionResult> Company(Guid id)
        {
            var result = await _services.GetCompanyById(id);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("TestFile")]
        public Task<IActionResult> TestFile(IFormFile file)
        {
            return Task.FromResult<IActionResult>(Ok(file));
        }
    }
}