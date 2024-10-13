using Api.ViewModels.Administration.Users;
using Data.Common;
using Data.Models;
using Domain.Service.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FootyLeague.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = Constants.AdminRoleName)]
public class UsersController : ControllerBase
{
    private readonly IAdminService _adminService;
    private readonly UserManager<User> _userManager;

    public UsersController(
        UserManager<User> userManager,
        IAdminService adminService)
    {
        _userManager = userManager;
        _adminService = adminService;
    }

    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Get()
    {
        var users = await _adminService.GetAllUsersAsync();

        if (!users.Any())
        {
            return NotFound();
        }

        return Ok(users.Select(x => new UserViewModel
        {
            Email = x.Email,
            Id = x.Id,
            Roles = x.Roles,
            UserName = x.UserName
        }));
    }

    [HttpGet("{id:int}")]
    [Produces("application/json")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Get(int id)
    {
        var user = await _adminService.GetUserAsync(id);

        return Ok(new UserViewModel
        {
            Email = user.Email,
            Id = user.Id,
            Roles = user.Roles,
            UserName = user.UserName
        });
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Patch(int id, [FromBody] List<string> roles)
    {
        var filteredRoleList = await _adminService.FilterRolesThatExistsAsync(roles);

        if (!filteredRoleList.Any())
        {
            return BadRequest();
        }

        var user = await _userManager.Users.Include(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
        {
            return NotFound();
        }

        var checkUnique = await _adminService.FilterRolesThatAreNotAlreadySetAsync(filteredRoleList, user);

        if (!checkUnique.Any())
        {
            return BadRequest();
        }

        var result = await _userManager.AddToRolesAsync(user, checkUnique);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        var userViewModel = await _adminService.GetUserAsync(id);

        return Ok(userViewModel);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id, [FromBody] List<string> roles)
    {
        var filteredRoleList = await _adminService.FilterRolesThatExistsAsync(roles);

        if (!filteredRoleList.Any())
        {
            return BadRequest();
        }

        var user = await _userManager.Users.Include(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
        {
            return NotFound();
        }

        var existingRoles = await _userManager.GetRolesAsync(user);

        var rolesToRemove = filteredRoleList.Intersect(existingRoles);

        if (!rolesToRemove.Any())
        {
            return NotFound();
        }

        var result = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        var userViewModel = await _adminService.GetUserAsync(id);

        return Ok(userViewModel);
    }
}
