using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientMM.Models
{
    public class Device
    {
        [Key]
        public int deviceId { get; set; }

        [MaxLength(100)]
        public string deviceName { get; set; }

        public int userId { get; set; }

        [MaxLength(50)]
        public string deviceType { get; set; }

        [MaxLength(100)]
        public string serialNumber { get; set; }

        public DateTime registeredAt { get; set; } = DateTime.Now;
    }
}
