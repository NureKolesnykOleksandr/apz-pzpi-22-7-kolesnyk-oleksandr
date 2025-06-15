using System.ComponentModel.DataAnnotations;

namespace ClientMM.Dtos
{
    public record ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
    }
}
