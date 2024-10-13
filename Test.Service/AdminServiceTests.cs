using Data.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using Services;
using Xunit;

namespace Test.Service;

public class AdminServiceTests
{
    private readonly AdminService _adminService;
    private readonly Mock<RoleManager<Role>> _roleManagerMock;
    private readonly Mock<UserManager<User>> _userManagerMock;

    public AdminServiceTests()
    {
        _userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
        _roleManagerMock = new Mock<RoleManager<Role>>(
            Mock.Of<IRoleStore<Role>>(), null, null, null, null);
        _adminService = new AdminService(_userManagerMock.Object, _roleManagerMock.Object);
    }

    [Fact]
    public async Task FilterRolesThatAreNotAlreadySetAsync_ReturnsCorrectRoles()
    {
        // Arrange
        var rolesToCheck = new List<string> { "Admin", "User" };
        var user = new User { Id = 1, UserName = "testuser", Email = "test@example.com" };
        _userManagerMock.Setup(um => um.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "User" });

        // Act
        var result = await _adminService.FilterRolesThatAreNotAlreadySetAsync(rolesToCheck, user);

        // Assert
        var expected = new List<string> { "Admin" };
        Assert.Equal(expected, result);
    }
}
