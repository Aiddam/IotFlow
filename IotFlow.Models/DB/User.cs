using IotFlow.Abstractions.Abstrations;
using IotFlow.Models.Enum;

namespace IotFlow.Models.DB
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string PasswordHash { get; set; } = String.Empty;
        public string RefreshToken { get; set; } = String.Empty;
        public DateTime RefreshTokenExpires { get; set; }
        public Role Role { get; set; }
    }
}
