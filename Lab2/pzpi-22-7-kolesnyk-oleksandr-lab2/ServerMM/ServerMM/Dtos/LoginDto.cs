﻿using System.ComponentModel.DataAnnotations;

namespace ServerMM.Dtos
{
    public record LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; }

        [Required]
        public string Password { get; init; }
    }

    public record LoginResultDto
    {
        public int UserId { get; init; }
        public string FirstName { get; init; }
        public string Email { get; init; }
    }
}
