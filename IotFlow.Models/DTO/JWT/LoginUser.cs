using System.ComponentModel.DataAnnotations;

namespace IotFlow.Models.DTO.JWT
{
    public class LoginUser
    {
        [Required]
        public string NameOrEmail { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
