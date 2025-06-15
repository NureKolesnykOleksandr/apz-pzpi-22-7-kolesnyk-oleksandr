using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientMM.Models
{
    public class Alert
    {
        [Key]
        public int alertId { get; set; }

        public int userID { get; set; }

        public User user { get; set; }

        public DateTime timestamp { get; set; } = DateTime.Now;

        [MaxLength(50)]
        public string alertType { get; set; }

        public string alertMessage { get; set; }

        public bool isAcknowledged { get; set; } = false;
    }
}
