using Api.ViewModels.Administration.Users;
using Data.Models;
using Domain.Service.Abstraction;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class AdminService : IAdminService
{
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;

    public AdminService(
        UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IEnumerable<string>> FilterRolesThatAreNotAlreadySetAsync(IEnumerable<string> roles, User user)
    {
        var existingRoles = await _userManager.GetRolesAsync(user);

        var checkUnique = roles.Except(existingRoles);

        return checkUnique;
    }

    public async Task<IEnumerable<string>> FilterRolesThatExistsAsync(IEnumerable<string> roles)
    {
        var dbRoles = await _roleManager.Roles
            .Select(x => x.Name)
            .ToListAsync();

        return dbRoles.Where(roles.Contains);
    }

    public async Task<IEnumerable<UserViewModel>> GetAllUsersAsync()
    {

        var users = await _userManager.Users
            .Include(x => x.Roles)
            .ToListAsync();

        var userViewModels = new List<UserViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles.Select(role => new UserRoleViewModel
                {
                    Name = role,
                    RoleId = user.Roles.Select(x => x.RoleId).FirstOrDefault(),
                    UserId = user.Roles.Select(r => r.UserId).FirstOrDefault()
                }).ToList()
            };

            userViewModels.Add(userViewModel);
        }

        return userViewModels;
    }

    public async Task<UserViewModel> GetUserAsync(int id)
    {
        var user = await _userManager.Users
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Id == id);

        var roles = await _userManager.GetRolesAsync(user);

        var userViewModel = new UserViewModel
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Roles = roles.Select(role => new UserRoleViewModel
            {
                Name = role,
                RoleId = user.Roles.Select(x => x.RoleId).FirstOrDefault(),
                UserId = user.Roles.Select(r => r.UserId).FirstOrDefault()
            }).ToList()
        };

        return userViewModel;
    }
}
