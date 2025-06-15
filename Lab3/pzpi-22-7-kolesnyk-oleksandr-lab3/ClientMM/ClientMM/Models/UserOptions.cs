using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClientMM.Models
{
    public record UserOptions
    {
        [Key]
        [ForeignKey("User")]
        public int userId { get; set; }

        public int minPulse { get; set; }
        public int maxPulse { get; set; }
        public int minOxygenLevel { get; set; }
        public double minBodyTemperature { get; set; }
        public double maxBodyTemperature { get; set; }

        [JsonIgnore]
        public User user { get; set; }
    }
}
