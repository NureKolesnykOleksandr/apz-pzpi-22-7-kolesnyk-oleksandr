using System.ComponentModel.DataAnnotations;

namespace ClientMM.Dtos
{
    public record RegisterDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [EmailAddress]
        public string EmergencyEmail { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; } // Enum values: Male, Female, Other
    }
}
