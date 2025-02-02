namespace IotFlow.Models.DTO.JWT
{
    public class JwtUserDto : JwtOnlyTokenDto, IUserDto
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
