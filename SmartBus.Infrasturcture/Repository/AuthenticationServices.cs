using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartBus.Application.Dtos.AuthenticationDtos;
using SmartBus.Application.Dtos.UserDtos;
using SmartBus.Application.HelperMethod.EmailBodyBuilder;
using SmartBus.Application.IExternalServices;
using SmartBus.Application.IServices;
using SmartBus.Application.Result;
using SmartBus.Domain.Models;
using SmartBus.Infrasturcture.Identity;
using SmartBus.Infrasturcture.Persistence;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace SmartBus.Infrasturcture.Repository
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly UserManager<ApplicationUser> _identity;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationContext _context;
        private readonly IConfiguration _configuration;
        private readonly ISendingEmailService _sendingMail;
        public AuthenticationServices(UserManager<ApplicationUser> identity,
                                      RoleManager<IdentityRole> roleManager,
                                      IConfiguration configuration,
                                      ApplicationContext context,
                                      ISendingEmailService sendingMail)
        {
            _identity = identity;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _sendingMail = sendingMail;
            
        }

        public async Task<CustomResult<LoginResultDto>> Login(LoginDto dto)
        {
            var userExist = _identity.Users.FirstOrDefault(u => u.Email == dto.Email);
            if (userExist == null)
                return CustomResult<LoginResultDto>.Failure(CustomError.InvalidInput("Invalid email or password"));
            var passwordValid = await _identity.CheckPasswordAsync(userExist, dto.Password);
            if (!passwordValid)
                return CustomResult<LoginResultDto>.Failure(CustomError.InvalidInput("Invalid email or password"));
            var resultDto = new LoginResultDto
            {
                Token = await createToken(userExist),
                TokenExpiration = DateTime.Now.AddMinutes(_configuration.GetValue<TimeSpan>("JWTConfig:ExpireTime").TotalMinutes),
                RefreshToken = await GenerateRefreshToken(userExist, 64),
                RefreshTokenExpiration = DateTime.Now.AddMinutes(_configuration.GetValue<TimeSpan>("JWTConfig:RefreshExpireTime").TotalMinutes),
                User = new UserDto
                {
                    Id = userExist.Id,
                    UserName = userExist.UserName!,
                    Email = userExist.Email!,
                    IsCompany = userExist.IsCompany
                }
            };
            return CustomResult<LoginResultDto>.Success(resultDto);
        }

        public async Task<CustomResult<RegisterResultDto>> Register(RegisterDto dto)
        {
            var userExist = await _identity.FindByEmailAsync(dto.Email);
            if (userExist != null)
                return CustomResult<RegisterResultDto>.Failure(CustomError.InvalidInput("User already exists"));
            var userNameExist = await _identity.FindByNameAsync(dto.UserName);
            if (userNameExist != null)
                return CustomResult<RegisterResultDto>.Failure(CustomError.InvalidInput("Username already exists"));
            var user = new ApplicationUser
            {
                Email = dto.Email,
                UserName = dto.UserName,
                FullName = $"{dto.FirstName} {dto.LastName}",
                IsCompany = dto.IsCompany
            };
            var result = await _identity.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return CustomResult<RegisterResultDto>.Failure(CustomError.InvalidInput("Failed to create user"));
            if (dto.IsCompany)
                await _identity.AddToRoleAsync(user, "Company");
            else
                await _identity.AddToRoleAsync(user, "User");
            var resultDto = new RegisterResultDto
            {
                Token = await createToken(user),
                TokenExpairdate = DateTime.Now.AddMinutes(_configuration.GetValue<TimeSpan>("JWTConfig:ExpireTime").TotalMinutes),
                RefreshToken = await GenerateRefreshToken(user, 64),
                RefreshTokenExpairdate = DateTime.Now.AddMinutes(_configuration.GetValue<TimeSpan>("JWTConfig:RefreshExpireTime").TotalMinutes),
                User = new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName!,
                    Email = user.Email!,
                    IsCompany = user.IsCompany
                }
            };
            await SendConfirmMail(user);
            return CustomResult<RegisterResultDto>.Success(resultDto);

        }
        public async Task<CustomResult<LoginResultDto>> RefreshToken(string userId, string refreshToken)
        {
            var userExist = await _identity.FindByIdAsync(userId);
            if (userExist == null)
                return CustomResult<LoginResultDto>.Failure(CustomError.NotFound("User Not Fouund"));
            var validToken = await _context.RefreshTokens.Where(rf => rf.UserId == userId && !rf.IsUsed 
                                                             && !rf.IsRevoked && rf.ExpiryDate >= DateTime.UtcNow)
                                                   .OrderByDescending(rf => rf.ExpiryDate)
                                                   .FirstOrDefaultAsync();  
            if(validToken == null || !string.Equals(validToken.Token, refreshToken))
                return CustomResult<LoginResultDto>.Failure(CustomError.InvalidInput("In Valid Refresh Token"));
            validToken.IsUsed = true;
            _context.RefreshTokens.Update(validToken);
            await _context.SaveChangesAsync();
            var resultDto = new LoginResultDto
            {
                Token = await createToken(userExist),
                TokenExpiration = DateTime.Now.AddMinutes(_configuration.GetValue<TimeSpan>("JWTConfig:ExpireTime").TotalMinutes),
                RefreshToken = await GenerateRefreshToken(userExist, 64),
                RefreshTokenExpiration = DateTime.Now.AddMinutes(_configuration.GetValue<TimeSpan>("JWTConfig:RefreshExpireTime").TotalMinutes),
                User = new UserDto
                {
                    Id = userExist.Id,
                    UserName = userExist.UserName!,
                    Email = userExist.Email!,
                    IsCompany = userExist.IsCompany
                }
            };
            return CustomResult<LoginResultDto>.Success(resultDto);

        }
        public async Task<CustomResult> Logout(string userId)
        {
            var userExist = await _identity.FindByIdAsync(userId);
            if (userExist == null)
                return CustomResult.Failure(CustomError.NotFound("User Not Found"));
            var refreshTokens = await _context.RefreshTokens.Where(rf => rf.UserId == userId && !rf.IsRevoked ).ToListAsync();
            foreach(var refreshToken in refreshTokens)
            {
                refreshToken.IsRevoked = true;
            }
            _context.RefreshTokens.UpdateRange(refreshTokens);
            await _context.SaveChangesAsync();
            return CustomResult.Success();

        }
        public async Task<CustomResult> ChangePassword(string userId, ChangePasswordRequest requestDto)
        {
            var User = await _identity.FindByIdAsync(userId);
            if (User == null)
                CustomResult.Failure(CustomError.NotFound("User Not Exist"));
            var complete =  await _identity.ChangePasswordAsync(User!, requestDto.OldPassword, requestDto.NewPassword);
            if (!complete.Succeeded)
                return CustomResult.Failure(CustomError.ServerError("Can't Change password"));
            return CustomResult.Success();
        }
        public async Task<CustomResult> ForgetPassword(string email)
        {
            var UserExist = await _identity.FindByEmailAsync(email);
            if (UserExist == null)
                CustomResult.Failure(CustomError.NotFound("User Not Exist"));
            var value = GenerateOTP(6);
            var OTP = new OTP
            {
                Value = value,
                UserId = UserExist!.Id,
                ExpaireDate = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false
            };
            await _context.OTPs.AddAsync(OTP);
            await _context.SaveChangesAsync();
            var body = EmailBodyOTPRequestBuilder.OTPBodyRequest(UserExist.UserName! , value);
            var sendingEmail = await _sendingMail.SendingEmail(UserExist.Email!, "Reset Password Request", body);
            if (!sendingEmail)
                return CustomResult.Failure(CustomError.ServerError("Faild in Sending OTP"));
            return CustomResult.Success();


        }
        public async Task<CustomResult<FrogetPasswordResponseDto>> CheckOTPValid(string email, string OTP)
        {
            var userExist = await _identity.FindByEmailAsync(email);
            if (userExist == null)
                CustomResult<FrogetPasswordResponseDto>.Failure(CustomError.NotFound("User Not Exist"));
            var userOTP = await _context.OTPs.Where(o => o.UserId == userExist!.Id && !o.IsUsed 
                                                      && o.ExpaireDate >= DateTime.UtcNow)
                                             .OrderByDescending(o => o.ExpaireDate)                             
                                             .FirstOrDefaultAsync();
            if (userOTP == null)
                CustomResult<FrogetPasswordResponseDto>.Failure(CustomError.InvalidInput("Invalid OTP"));
            var valid = string.Equals(userOTP!.Value, OTP);
            if (valid)
                CustomResult<FrogetPasswordResponseDto>.Failure(CustomError.InvalidInput("Invalid OTP"));
            var token = await _identity.GeneratePasswordResetTokenAsync(userExist!);
            var response = new FrogetPasswordResponseDto
            {
                Email = email,
                Token = token
            };
            return CustomResult<FrogetPasswordResponseDto>.Success(response);

        }
        public async Task<CustomResult> ResetPassword(ResetPasswordAfterForgetRequest dto)
        {
            var userExist = await _identity.FindByEmailAsync(dto.Email);
            if (userExist == null)
                CustomResult.Failure(CustomError.NotFound("User NOT Exist"));
            if (string.Equals(dto.Password, dto.ConfirmPassword))
                CustomResult.Failure(CustomError.InvalidInput("Invalid Input"));
            var result = await _identity.ResetPasswordAsync(userExist!, dto.Token, dto.Password);
            if (result.Succeeded)
               return  CustomResult.Success();
            return CustomResult.Failure(CustomError.InvalidInput("Invalid Input"));
        }
        public async Task<CustomResult> ConfirmMail(string email, string token)
        {
            var userExist = await _identity.FindByEmailAsync(email);
            if (userExist == null)
                return CustomResult.Failure(CustomError.InvalidInput("Invalid Input"));
            var decodeToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var confirm = await _identity.ConfirmEmailAsync(userExist, decodeToken);
            if(confirm.Succeeded)
                return CustomResult.Success();
            return CustomResult.Failure(CustomError.InvalidInput("Invalid Input"));
        }
        public async Task<CustomResult<UserDto>> UserInfo(string userId)
        {
            var userExist = await _identity.FindByIdAsync(userId);
            if (userExist == null)
                CustomResult<UserDto>.Failure(CustomError.NotFound("User Not Found"));
            var User = new UserDto
            {
                Id = userExist!.Id,
                UserName = userExist.UserName!,
                Email = userExist.Email!,
                IsCompany = userExist.IsCompany,
                CompanyId = userExist.CompanyId,
            };
            return CustomResult<UserDto>.Success(User);

        }
        private async Task<string> createToken(ApplicationUser user)
        {
            string key = _configuration.GetValue<string>("JWTConfig:Secert")!;
            var claims = await GetClaims(user);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(_configuration.GetValue<TimeSpan>("JWTConfig:ExpireTime").TotalMinutes),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private async Task<List<Claim>> GetClaims(ApplicationUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!)
            };
            var userClaims = await _identity.GetClaimsAsync(user);
            claims.AddRange(userClaims);
            var userRoles = await _identity.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
                var roleEntity = await _roleManager.FindByNameAsync(role);
                if(roleEntity == null) continue;
                var roleClaims = await _roleManager.GetClaimsAsync(roleEntity);
                claims.AddRange(roleClaims);
            }
            return claims;
        }
        private async Task<string> GenerateRefreshToken(ApplicationUser user, int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz_";
            var refreshToken = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            var entity = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiryDate = DateTime.Now.AddDays(30)
            };
            await _context.RefreshTokens.AddAsync(entity);
            await _context.SaveChangesAsync();
            return refreshToken;
        }
        private string GenerateOTP(int length)
        {
            var random = new Random();
            const string chars = "123456789";
            var OTP = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            return OTP;
        }
        private async Task SendConfirmMail(ApplicationUser user)
        {
            var token = await _identity.GenerateEmailConfirmationTokenAsync(user);
            var encodeToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var ConfirmMailUrl = $"https://localhost:7056/api/Authentication/confirm-email"
                      + $"?email={user.Email}&token={encodeToken}";
            var body = EmailBodyConfirmMailBuilder.ConfirmMailBody(ConfirmMailUrl);
            var send = await _sendingMail.SendingEmail(user.Email!, "Confirm Email", body);
        }


    }
}
