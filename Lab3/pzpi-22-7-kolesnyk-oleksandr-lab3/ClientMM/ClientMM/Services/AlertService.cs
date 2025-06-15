using ClientMM.Dtos;
using ClientMM.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.Text.Json;

namespace ClientMM.Services
{
    public class AlertService : IAlertService
    {
        private readonly HttpClient _httpClient;

        public AlertService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IdentityResult> CreateAlert(CreateAlertDto alertDto)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(alertDto);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("https://localhost:5202/api/alert", content);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return IdentityResult.Failed(new IdentityError { Code = response.StatusCode.ToString() });
                }

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }
    }
}
