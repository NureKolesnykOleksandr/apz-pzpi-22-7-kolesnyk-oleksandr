using ClientMM.Dtos;
using ClientMM.Interfaces;
using ClientMM.Models;
using Microsoft.AspNetCore.Identity;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ClientMM.Services
{
    public class RecommendationService : IRecomendationService
    {
        private readonly HttpClient _httpClient;

        public RecommendationService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<Recommendation> GenerateRecommendationForUser(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:5202/api/recommendation/{userId}/generate");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return null;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var recommendation = JsonSerializer.Deserialize<Recommendation>(responseContent);

                return recommendation;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<Recommendation>> GetAllRecommendationsForUser(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:5202/api/recommendation/user/{userId}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return Enumerable.Empty<Recommendation>();
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var recommendations = JsonSerializer.Deserialize<IEnumerable<Recommendation>>(responseContent);

                return recommendations ?? Enumerable.Empty<Recommendation>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return Enumerable.Empty<Recommendation>();
            }
        }
    }
}