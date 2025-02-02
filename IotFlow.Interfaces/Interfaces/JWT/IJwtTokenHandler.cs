using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IotFlow.Abstractions.Interfaces.JWT
{
    public interface IJwtTokenHandler
    {
        JwtSecurityToken CreateAccessToken(List<Claim> claims);
        JwtSecurityToken CreateRefreshToken(List<Claim> claims);
        ClaimsPrincipal ValidateToken(string token);
    }
}
