using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartBus.Application.Dtos.UserDtos;
using SmartBus.Application.IServices;
using SmartBus.Application.Result;
using SmartBus.Infrasturcture.Identity;

namespace SmartBus.Infrasturcture.Repository
{
    public class RoleServices : IRoleServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleServices(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<CustomResult> AddUserToRole(string userId, string roleName)
        {
            var userExist = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (userExist == null)
                return CustomResult.Failure(CustomError.InvalidInput("User does not exist"));
            var roleExist = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == roleName);  
            if (roleExist == null)
                return CustomResult.Failure(CustomError.InvalidInput("Role does not exist"));
            var complete = await _userManager.AddToRoleAsync(userExist, roleName);
            if (!complete.Succeeded)
                return CustomResult.Failure(CustomError.InvalidInput("Failed to add user to role"));
            return CustomResult.Success();
        }

        public async Task<CustomResult> CreateRole(string roleName)
        {
            var roleExist = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (roleExist != null)
                return CustomResult.Failure(CustomError.InvalidInput("Role already exists"));
            var complete = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (!complete.Succeeded)
                return CustomResult.Failure(CustomError.InvalidInput("Failed to create role"));
            return CustomResult.Success();
        }

        public async Task<CustomResult> DeleteRole(string roleName)
        {
            var roleExist = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == roleName); 
            if(roleExist == null)
                return CustomResult.Failure(CustomError.InvalidInput("Role does not exist"));
            var complete = await _roleManager.DeleteAsync(roleExist);
            if (!complete.Succeeded)
                return CustomResult.Failure(CustomError.InvalidInput("Failed to delete role"));
            return CustomResult.Success();
        }

        public async Task<CustomResult<List<string>>> GetAllRoles()
        {
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync(); 
            return CustomResult<List<string>>.Success(roles!);
        }

        public async Task<CustomResult<List<string>>> GetUserRoles(string userId)
        {
            var userExist = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId); 
            if (userExist == null)
                return CustomResult<List<string>>.Failure(CustomError.InvalidInput("User does not exist"));
            var roles = await _userManager.GetRolesAsync(userExist);
            return CustomResult<List<string>>.Success(roles.ToList());
        }

        public async Task<CustomResult<List<UserDto>>> GetUsersInRole(string roleName)
        {
            var roleExist = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (roleExist == null)
                return CustomResult<List<UserDto>>.Failure(CustomError.InvalidInput("Role does not exist"));
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            var userDtos = users.Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                IsCompany = u.IsCompany
            }).ToList();
            return CustomResult<List<UserDto>>.Success(userDtos);
        }

        public async Task<CustomResult> RemoveUserFromRole(string userId, string roleName)
        {
            var userExist = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (userExist == null)
                return CustomResult.Failure(CustomError.InvalidInput("User does not exist"));
            var roleExist = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (roleExist == null)
                return CustomResult.Failure(CustomError.InvalidInput("Role does not exist"));
            var complete = await _userManager.RemoveFromRoleAsync(userExist, roleName);
            if (!complete.Succeeded)
                return CustomResult.Failure(CustomError.InvalidInput("Failed to remove user from role"));
            return CustomResult.Success();  
        }
    }
}
