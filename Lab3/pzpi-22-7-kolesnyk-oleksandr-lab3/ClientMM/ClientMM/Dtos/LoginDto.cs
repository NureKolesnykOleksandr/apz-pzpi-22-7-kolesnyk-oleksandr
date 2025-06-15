using System.ComponentModel.DataAnnotations;

namespace ClientMM.Dtos
{
    public record LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public record LoginResultDto
    {
        public int userId { get; set; }
        public string firstName { get; set; }
        public string email { get; set; }
    }
}
