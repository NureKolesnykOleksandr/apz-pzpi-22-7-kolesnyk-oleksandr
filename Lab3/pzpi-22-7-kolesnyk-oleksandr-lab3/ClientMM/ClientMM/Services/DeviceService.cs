using ClientMM.Dtos;
using ClientMM.Interfaces;
using ClientMM.Models;
using Microsoft.AspNetCore.Identity;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ClientMM.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly HttpClient _httpClient;

        public DeviceService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IdentityResult> AddDevice(CreateDeviceDto addDeviceDto)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(addDeviceDto);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("https://localhost:5202/api/Device", content);

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

        public async Task<IdentityResult> AddSensorData(string serialNumber, AddSensorDataDto addSensorDataDto)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(addSensorDataDto);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"https://localhost:5202/api/Device/SensorData/{serialNumber}", content);

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

        public async Task<IdentityResult> DeleteDevice(int deviceId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"https://localhost:5202/api/Device/{deviceId}");

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

        public async Task<IdentityResult> DeleteDeviceHistory(int deviceId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"https://localhost:5202/api/Device/{deviceId}/history");

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

        public async Task<List<SensorData>> GetDeviceHistory(int deviceId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:5202/api/Device/{deviceId}/history");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return new List<SensorData>();
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var sensorData = JsonSerializer.Deserialize<List<SensorData>>(responseContent);

                return sensorData ?? new List<SensorData>();
            }
            catch
            {
                return new List<SensorData>();
            }
        }

        public async Task<List<Device>> GetDevices(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:5202/api/Device/{userId}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return new List<Device>();
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var devices = JsonSerializer.Deserialize<List<Device>>(responseContent);

                return devices ?? new List<Device>();
            }
            catch
            {
                return new List<Device>();
            }
        }

        public async Task<IdentityResult> UpdateDevice(int deviceId, UpdateDeviceDto updateDeviceDto)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(updateDeviceDto);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"https://localhost:5202/api/Device/{deviceId}", content);

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