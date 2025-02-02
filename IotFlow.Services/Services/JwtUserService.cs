using IotFlow.Abstractions.Interfaces.DA;
using IotFlow.Abstractions.Interfaces.JWT;
using IotFlow.Abstractions.Interfaces.Services;
using IotFlow.Models.DB;
using IotFlow.Models.DI;
using IotFlow.Models.DTO.JWT;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using IotFlow.Utilities;
using IotFlow.Models.ClaimNames;
using System.Threading;

namespace IotFlow.Services
{
    public class JwtUserService : IUserService<JwtUserDto, RegisterUser, LoginUser, RefreshUser>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenHandler _tokenHandler;
        private readonly JwtConfiguration _jwtConfiguration;

        public JwtUserService(IUnitOfWork unitOfWork, IJwtTokenHandler tokenHandler, IOptions<JwtConfiguration> jwtConfigurations)
        {
            _unitOfWork = unitOfWork;
            _tokenHandler = tokenHandler;
            _jwtConfiguration = jwtConfigurations.Value ?? throw new ArgumentException(nameof(jwtConfigurations));
        }

        public async Task<JwtUserDto> RegisterNewUserAsync(RegisterUser registerUser, CancellationToken cancellationToken = default)
        {
            var userRepository = await _unitOfWork.GetRepository<User>();

            IEnumerable<User> usersWithSameCredentials = await userRepository.ReadEntitiesByPredicate(user => user.Name == registerUser.Name || user.Email == registerUser.Email, cancellationToken: cancellationToken);

            if (usersWithSameCredentials.FirstOrDefault() != null)
            {
                throw new ArgumentException("User with this credentials has been already registered");
            }

            User newUser = new User
            {
                Name = registerUser.Name,
                PasswordHash = StringHasher.HashStringSHA256(registerUser.Password),
                Email = registerUser.Email,
            };

            return await ProcessUserAsync(newUser, cancellationToken: cancellationToken);
        }

        public async Task<JwtUserDto> LoginUserAsync(LoginUser loginUser, CancellationToken cancellationToken = default)
        {
            var userRepository = await _unitOfWork.GetRepository<User>();

            string hashedPassword = StringHasher.HashStringSHA256(loginUser.Password);

            IEnumerable<User> usersWithSameCredentials = await userRepository.ReadEntitiesByPredicate(user => (user.Name == loginUser.NameOrEmail || user.Email == loginUser.NameOrEmail) && user.PasswordHash == hashedPassword, cancellationToken: cancellationToken);

            User? user = usersWithSameCredentials.FirstOrDefault();

            if (user == null)
            {
                throw new ArgumentException("Wrong credentials.");
            }

            return await ProcessUserAsync(user, cancellationToken: cancellationToken);
        }

        public async Task<JwtUserDto> RefreshUserAsync(RefreshUser refreshUser, CancellationToken cancellationToken = default)
        {
            var userRepository = await _unitOfWork.GetRepository<User>();

            ClaimsPrincipal cp = _tokenHandler.ValidateToken(refreshUser.RefreshToken);

            bool successful = int.TryParse(cp.Claims.FirstOrDefault(claim => claim.Type == UserClaimName.Id)?.Value, out int id);

            if (!successful)
            {
                throw new ArgumentException("Unable to read user's id");
            }

            User? user = await userRepository.ReadEntityByIdAsync(id);

            if (user == null)
            {
                throw new ArgumentException("There is no user with this id");
            }

            if (user.RefreshToken != refreshUser.RefreshToken)
            {
                throw new ArgumentException("Wrong refresh code.");
            }

            return await ProcessUserAsync(user, cancellationToken: cancellationToken);
        }

        public async Task<JwtUserDto> ProcessUserAsync(User user, CancellationToken cancellationToken = default)
        {
            var userRepository = await _unitOfWork.GetRepository<User>();

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            user.RefreshToken = handler.WriteToken(GenerateJwtRefreshTokenForUser(user));
            user.RefreshTokenExpires = DateTime.Now.AddHours(_jwtConfiguration.RefreshLifetime).ToUniversalTime();

            if (user.Id == 0)
            {
                await userRepository.CreateAsync(user, cancellationToken: cancellationToken);
            }
            else
            {
                await userRepository.UpdateAsync(user, cancellationToken: cancellationToken);
            }
            await _unitOfWork.CommitAsync();

            JwtSecurityToken token = GenerateJwtAccessTokenForUser(user);

            return new JwtUserDto()
            {
                RefreshToken = user.RefreshToken,
                Token = handler.WriteToken(token)
            };
        }

        public async Task<bool> DoesNameOrEmailExist(string nameOrEmail, CancellationToken cancellationToken = default)
        {
            var userRepository = await _unitOfWork.GetRepository<User>();

            IEnumerable<User> users = await userRepository.ReadEntitiesByPredicate(user => user.Name == nameOrEmail || user.Email == nameOrEmail, cancellationToken: cancellationToken);

            return users.Any();
        }

        private JwtSecurityToken GenerateJwtAccessTokenForUser(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(UserClaimName.Name, user.Name),
                new Claim(UserClaimName.Id, user.Id.ToString())
            };

            return _tokenHandler.CreateAccessToken(claims);
        }

        private JwtSecurityToken GenerateJwtRefreshTokenForUser(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(UserClaimName.Id, user.Id.ToString())
            };

            return _tokenHandler.CreateRefreshToken(claims);
        }
    }
}
