using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Tsp;
using SmartBus.Application.Dtos.AuthenticationDtos;
using SmartBus.Application.IServices;
using System.Security.Claims;

namespace SmartBus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationServices _authenticationServices;
        public AuthenticationController(IAuthenticationServices authenticationServices)
        {
            _authenticationServices = authenticationServices;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _authenticationServices.Register(dto);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authenticationServices.Login(dto);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("RefreshToken")]
        [Authorize]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _authenticationServices.RefreshToken(userId!, dto.RefreshToken);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _authenticationServices.Logout(userId!);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("Change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _authenticationServices.ChangePassword(userId!, dto);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordRequestDto dto)
        {
            var result = await _authenticationServices.ForgetPassword(dto.Email);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("CheckOTP")]
        public async Task<IActionResult> CheckOTP(CheckOTPRequestDto dto)
        {
            var result = await _authenticationServices.CheckOTPValid(dto.Email,dto.OTP);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);

        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordAfterForgetRequest dto)
        {
            var result = await _authenticationServices.ResetPassword(dto);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("Me")]
        [Authorize]
        public async Task<IActionResult> UserInfo()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _authenticationServices.UserInfo(userId!);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);

        }
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmMail(string email , string  token)
        {
            var result = await _authenticationServices.ConfirmMail(email, token);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        
    }
}
