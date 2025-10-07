using System.ComponentModel.DataAnnotations;

namespace ProjectsRequirements_API.Models
{
    public class Priority
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Description { get; set; } = null!;

    }
}
