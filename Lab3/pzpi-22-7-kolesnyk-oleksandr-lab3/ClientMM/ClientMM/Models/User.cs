using System.ComponentModel.DataAnnotations;

namespace ClientMM.Models
{
    public class User
    {
        [Key]
        public int userId { get; set; }

        [MaxLength(50)]
        public string firstName { get; set; }

        [MaxLength(50)]
        public string lastName { get; set; }

        [EmailAddress]
        public string email { get; set; }

        [EmailAddress]
        public string emergencyEmail { get; set; }

        public string passwordHash { get; set; }

        public bool isBanned { get; set; } = false;

        public DateTime? dateOfBirth { get; set; }

        [MaxLength(10)]
        public string gender { get; set; }

        public UserOptions userOptions { get; set; }

        public DateTime createdAt { get; set; } = DateTime.Now;
    }
}
