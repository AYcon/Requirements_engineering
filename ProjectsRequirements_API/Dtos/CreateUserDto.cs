using System.ComponentModel.DataAnnotations;

namespace ProjectsRequirements_API.Dtos
{
    public class CreateUserDto
    {
        [Required, MaxLength(200)]
        public string Name { get; set; } = null!;

        [Required, EmailAddress, MaxLength(320)]
        public string Email { get; set; } = null!;

        [Required, MinLength(8), MaxLength(128)]
        public string Password { get; set; } = null!;
    }
}
