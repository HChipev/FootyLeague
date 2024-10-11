using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Data.Models;

namespace Domain.Service.Abstraction;

public interface IAuthService
{
    JwtSecurityToken GenerateJwtToken(List<Claim> claims);

    Task<string> CreateRefreshToken(User user);

    Task<string> GetStoredRefreshToken(User user);

    Task RemoveRefreshToken(User user);
}
