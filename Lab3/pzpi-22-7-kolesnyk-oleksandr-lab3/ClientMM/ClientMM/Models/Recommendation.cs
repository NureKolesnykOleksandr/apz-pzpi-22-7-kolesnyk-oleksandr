using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClientMM.Models
{
    public class Recommendation
    {
        [Key]
        public int recommendationId { get; set; }

        public int userId { get; set; }

        [JsonIgnore]
        public User user { get; set; }

        public DateTime generatedAt { get; set; } = DateTime.Now;

        public string recommendationText { get; set; }
    }
}
