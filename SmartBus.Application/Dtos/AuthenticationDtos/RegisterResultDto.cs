using SmartBus.Application.Dtos.UserDtos;

namespace SmartBus.Application.Dtos.AuthenticationDtos
{
    public class RegisterResultDto
    {
        public string Token { get; set; }
        public DateTime TokenExpairdate { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpairdate { get; set; }
        public UserDto User { get; set; } = new UserDto();
    }
}
