using System.ComponentModel.DataAnnotations;

namespace ClientMM.Dtos
{
    public record CreateAlertDto
    {
        [Required]
        public string AlertType { get; set; }

        [Required]
        public string AlertMessage { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
