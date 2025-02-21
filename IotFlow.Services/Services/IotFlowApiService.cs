using IotFlow.Abstractions.Interfaces.Services;
using IotFlow.Models.DTO.Commands;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace IotFlow.Services.Services
{
    public class IotFlowApiService : IIotFlowApiService<CommandDto>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public IotFlowApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiBaseUrl"] ?? throw new Exception("ApiBaseUrl not specified in the configuration");
        }
        public async Task SendCommandAsync(CommandDto commandRequest, CancellationToken cancellationToken = default)
        {
            var json = JsonSerializer.Serialize(commandRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}api/command/send", content, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
    }
}
