using SmartBus.Application.Dtos.UserDtos;

namespace SmartBus.Application.Dtos.AuthenticationDtos
{
    public class LoginResultDto
    {
        public string Token { get; set; }
        public DateTime TokenExpiration { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
        public UserDto User { get; set; } = new UserDto();
    }
}
