namespace IotFlow.Models.DTO.JWT
{
    public class JwtUserInfoDto : IUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
