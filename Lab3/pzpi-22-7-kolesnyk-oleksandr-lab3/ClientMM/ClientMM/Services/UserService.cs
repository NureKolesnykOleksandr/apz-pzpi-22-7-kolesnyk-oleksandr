using ClientMM.Dtos;
using ClientMM.Interfaces;
using ClientMM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.JSInterop;
using System.Text;
using System.Text.Json;

namespace ClientMM.Services
{
    public class UserService : IUserService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly HttpClient _httpClient;
        private readonly ICookieService _cookieService;

        public UserService(IJSRuntime jsRuntime, IHttpClientFactory httpClientFactory, ICookieService cookieService)
        {
            _jsRuntime = jsRuntime;
            _httpClient = httpClientFactory.CreateClient();
            _cookieService = cookieService;
        }

        public async Task<IdentityResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(forgotPasswordDto);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("https://localhost:5202/api/Auth/forgot-password", content);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Помилка: {response.StatusCode}");
                    return IdentityResult.Failed(new IdentityError { Code = response.StatusCode.ToString() });
                }
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }

        public async Task<List<User>> GetUsers()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:5202/api/User");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Помилка: {response.StatusCode}");
                    return new List<User>();
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<User>>(responseContent) ?? new List<User>();
            }
            catch
            {
                return new List<User>();
            }
        }


        public async Task<LoginResultDto?> Login(LoginDto loginDto, string ip)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(loginDto);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("https://localhost:5202/api/Auth/login", content);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Помилка: {response.StatusCode}");
                    return null;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var loginResult = JsonSerializer.Deserialize<LoginResultDto>(responseContent);

                if (loginResult == null)
                    return null;

                _cookieService.SetCookie("Id", loginResult.userId.ToString());
                _cookieService.SetCookie("Name", loginResult.firstName);
                _cookieService.SetCookie("Email", loginResult.email);

                return loginResult;
            }
            catch
            {
                return null;
            }
        }

        public async Task<LoginResultDto?> Register(RegisterDto registerDto)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(registerDto);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("https://localhost:5202/api/Auth/register", content);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Помилка: {response.StatusCode}");
                    return null;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var loginResult = JsonSerializer.Deserialize<LoginResultDto>(responseContent);

                if (loginResult == null)
                    return null;

                _cookieService.SetCookie("Id", loginResult.userId.ToString());
                _cookieService.SetCookie("Name", loginResult.firstName);
                _cookieService.SetCookie("Email", loginResult.email);

                return loginResult;
            }
            catch
            {
                return null;
            }
        }
        public async Task<IdentityResult> BanUser(int userId, string password)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(password);
                var response = await _httpClient.DeleteAsync($"https://localhost:5202/api/Admin/ban/{userId}?password={password}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Помилка: {response.StatusCode}");
                    return IdentityResult.Failed(new IdentityError { Code = response.StatusCode.ToString() });
                }
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }


        public async Task<IdentityResult> UnBanUser(int userId, string password)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(password);

                var response = await _httpClient.PutAsync($"https://localhost:5202/api/Admin/unban/{userId}?password={password}", null);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Помилка: {response.StatusCode}");
                    return IdentityResult.Failed(new IdentityError { Code = response.StatusCode.ToString()});
                }
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }

        public async Task<IdentityResult> UpdateProfile(int userId, UpdateProfileDto updateProfileDto)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(updateProfileDto);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"https://localhost:5202/api/User/{userId}", content);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Помилка: {response.StatusCode}");
                    return IdentityResult.Failed(new IdentityError { Code = response.StatusCode.ToString() });
                }
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }

        public async Task<IdentityResult> UpdateUserOptions(int userId, UpdateUserOptionsDto updateUserOptionsDto)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(updateUserOptionsDto);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"https://localhost:5202/api/User/update-options/{userId}", content);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Помилка: {response.StatusCode}");
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
