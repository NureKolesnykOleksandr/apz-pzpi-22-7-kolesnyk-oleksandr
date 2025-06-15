using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientMM.Models
{
    public class UserLogin
    {
        [Key]
        public int loginId { get; set; }

        public int userId { get; set; }
        public DateTime loginTime { get; set; } = DateTime.Now;

        [MaxLength(45)]
        public string iPAddress { get; set; }
    }
}
