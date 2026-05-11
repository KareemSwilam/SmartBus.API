using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Dtos.AuthenticationDtos
{
    public class FrogetPasswordResponseDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
