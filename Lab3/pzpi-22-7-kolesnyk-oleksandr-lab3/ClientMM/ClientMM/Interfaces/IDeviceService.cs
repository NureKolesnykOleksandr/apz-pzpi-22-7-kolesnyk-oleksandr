using ClientMM.Dtos;
using ClientMM.Models;
using Microsoft.AspNetCore.Identity;

namespace ClientMM.Interfaces
{
    public interface IDeviceService
    {
        Task<IdentityResult> AddDevice(CreateDeviceDto addDeviceDto);

        Task<IdentityResult> AddSensorData(string SerialNumber, AddSensorDataDto addSensorDataDto);

        Task<List<Device>> GetDevices(int userId);

        Task<IdentityResult> UpdateDevice(int deviceId, UpdateDeviceDto updateDeviceDto);

        Task<IdentityResult> DeleteDevice(int deviceId);

        Task<List<SensorData>> GetDeviceHistory(int deviceId);
        Task<IdentityResult> DeleteDeviceHistory(int deviceId);
    }
}
