using Microsoft.AspNetCore.Identity;
using SmartBus.Application.Dtos.UserDtos;
using SmartBus.Application.IServices;
using SmartBus.Application.Result;
using SmartBus.Infrasturcture.Identity;
using SmartBus.Infrasturcture.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Infrasturcture.Repository
{
    public class UserServices: IUserServices
    {
        private readonly UserManager<ApplicationUser> _identity;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationContext _context;
        public UserServices(UserManager<ApplicationUser> identity,
                            RoleManager<IdentityRole> roleManager,
                            ApplicationContext context)
        {
            _identity = identity;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<CustomResult> UpdateUser(string id, UserDto user)
        {
            var User = await _identity.FindByIdAsync(id);
            User!.UserName = user.UserName;
            User.Email = user.Email;
            User.CompanyId = user.CompanyId;
            var complete = await _identity.UpdateAsync(User);
            if (complete.Succeeded)
                return CustomResult.Success();
            return CustomResult.Failure(CustomError.ServerError("Failed in Updating User"));

        }

        public async Task<CustomResult<UserDto>> User(string id)
        {
            var User = await _identity.FindByIdAsync(id);
            if (User == null)
                CustomResult.Failure(CustomError.NotFound("User Not Exist"));
            var userDto = new UserDto()
            {
                UserName = User!.UserName!,
                Email = User.Email!,
                PhoneNumber = User.PhoneNumber!,
                IsCompany = User.IsCompany,
                CompanyId = User.CompanyId,
            };
            return CustomResult<UserDto>.Success(userDto);

        }
    }
}
