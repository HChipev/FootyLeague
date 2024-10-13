using Api.ViewModels.Administration.Users;
using Data.Models;

namespace Domain.Service.Abstraction;

public interface IAdminService
{
    Task<IEnumerable<UserViewModel>> GetAllUsersAsync();

    Task<UserViewModel> GetUserAsync(int id);

    Task<IEnumerable<string>> FilterRolesThatExistsAsync(IEnumerable<string> roles);

    Task<IEnumerable<string>> FilterRolesThatAreNotAlreadySetAsync(IEnumerable<string> roles, User user);
}
