using SmartBus.Application.Dtos.AuthenticationDtos;
using SmartBus.Application.Dtos.UserDtos;
using SmartBus.Application.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.IServices
{
    public interface IAuthenticationServices
    {
        public Task<CustomResult<RegisterResultDto>> Register(RegisterDto dto);
        public Task<CustomResult<LoginResultDto>> Login(LoginDto dto);
        public Task<CustomResult<LoginResultDto>> RefreshToken(string userId, string refreshToken); 
        public Task<CustomResult> Logout(string userId);
        public Task<CustomResult> ChangePassword(string userId, ChangePasswordRequest requestDto);
        public Task<CustomResult> ForgetPassword(string email);
        public Task<CustomResult<FrogetPasswordResponseDto>> CheckOTPValid(string email, string OTP);
        public Task<CustomResult>  ResetPassword(ResetPasswordAfterForgetRequest dto);
        public Task<CustomResult> ConfirmMail(string email, string token);
        public Task<CustomResult<UserDto>> UserInfo(string userId);
    }
}
