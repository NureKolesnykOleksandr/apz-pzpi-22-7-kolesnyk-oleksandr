using System.ComponentModel.DataAnnotations;

namespace ClientMM.Dtos
{
    public record UpdateDeviceDto
    {
        [Required]
        public string? DeviceName { get; set; }
        [Required]
        public string? DeviceType { get; set; }
    }
}
