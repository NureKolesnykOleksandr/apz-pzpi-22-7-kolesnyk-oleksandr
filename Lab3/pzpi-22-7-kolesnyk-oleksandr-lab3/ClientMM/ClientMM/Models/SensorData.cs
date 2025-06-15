using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientMM.Models
{
    public class SensorData
    {
        [Key]
        public int dataId { get; set; }

        public int deviceId { get; set; }

        public Device device { get; set; }

        public DateTime timestamp { get; set; }

        public int? heartRate { get; set; }
        public double? bloodOxygenLevel { get; set; }
        public double? bodyTemperature { get; set; }

        [MaxLength(10)]
        public string activityLevel { get; set; }

        [MaxLength(10)]
        public string sleepPhase { get; set; }
    }
}
