using SmartBus.Application.Dtos.UserDtos;
using SmartBus.Application.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.IServices
{
    public interface IUserServices
    {
        public Task<CustomResult<UserDto>> User(string id);
        public Task<CustomResult> UpdateUser(string id, UserDto user);
    }
}
