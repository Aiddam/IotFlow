using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IotFlow.Abstractions.Interfaces.Services
{
    public interface IUserService<TUserDto, TRegisterUser, TLoginUser, TRefreshUser>
    where TUserDto : IUserDto
    {
        Task<TUserDto> RegisterNewUserAsync(TRegisterUser userModel, CancellationToken cancellationToken = default);
        Task<TUserDto> LoginUserAsync(TLoginUser loginUser, CancellationToken cancellationToken = default);
        Task<TUserDto> RefreshUserAsync(TRefreshUser refreshUser, CancellationToken cancellationToken = default);
        Task<bool> DoesNameOrEmailExist(string nameOrEmail, CancellationToken cancellationToken = default);
    }
}
public interface IUserDto
{
}