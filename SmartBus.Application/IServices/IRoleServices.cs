using SmartBus.Application.Dtos.UserDtos;
using SmartBus.Application.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.IServices
{
    public interface IRoleServices
    {
        public Task<CustomResult> CreateRole(string roleName);
        public Task<CustomResult> DeleteRole(string roleName);
        public Task<CustomResult> AddUserToRole(string userId, string roleName);
        public Task<CustomResult> RemoveUserFromRole(string userId, string roleName);
        public Task<CustomResult<List<string>>> GetUserRoles(string userId);
        public Task<CustomResult<List<string>>> GetAllRoles();
        public Task<CustomResult<List<UserDto>>> GetUsersInRole(string roleName);
    }
}
