using IotFlow.Abstractions.Interfaces.JWT;
using IotFlow.Models.DI;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IotFlow.Utilities.JWT
{
    public class JwtTokenHandler : IJwtTokenHandler
    {
        private readonly JwtConfiguration _jwtConfiguration;

        public JwtTokenHandler(IOptions<JwtConfiguration> jwtConfigurations)
        {
            _jwtConfiguration = jwtConfigurations.Value ?? throw new ArgumentNullException("There are no jwt configurations on server");
        }

        public JwtSecurityToken CreateAccessToken(List<Claim> claims)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfiguration.AccessSecretCode));

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                issuer: _jwtConfiguration.Issuer,
                audience: _jwtConfiguration.Audience,
                expires: DateTime.Now.AddHours(_jwtConfiguration.AccessLifetime)
            );

            return token;
        }

        public JwtSecurityToken CreateRefreshToken(List<Claim> claims)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfiguration.RefreshSecretCode));

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                issuer: _jwtConfiguration.Issuer,
                audience: _jwtConfiguration.Audience,
                expires: DateTime.Now.AddHours(_jwtConfiguration.RefreshLifetime)
            );

            return token;
        }

        public ClaimsPrincipal ValidateToken(string strToken)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfiguration.AccessSecretCode)),
                ValidateAudience = false,
                ValidateIssuer = false,
            };

            SecurityToken token;

            ClaimsPrincipal cp = handler.ValidateToken(strToken, validationParameters, out token);

            return cp;
        }
    }
}
