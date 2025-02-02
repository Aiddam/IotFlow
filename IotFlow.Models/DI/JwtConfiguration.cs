
namespace IotFlow.Models.DI
{
    public class JwtConfiguration
    {
        public string AccessSecretCode { get; set; } = string.Empty;
        public string RefreshSecretCode { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty ;
        public int AccessLifetime { get; set; }
        public int RefreshLifetime { get; set; }
    }
}
