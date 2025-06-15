using System.ComponentModel.DataAnnotations;

namespace ClientMM.Dtos
{
    public record CreateDeviceDto
    {
        [Required]
        public string DeviceName { get; set; }

        [Required]
        public string DeviceType { get; set; }

        [Required]
        public string SerialNumber { get; set; }

        [Required]
        public int UserId { get; set; }

    }
}