using System.ComponentModel.DataAnnotations;

namespace ClientMM.Dtos
{
    public record UpdateProfileDto
    {
        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public string? Gender { get; set; }
    }
}