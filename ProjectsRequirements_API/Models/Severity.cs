using System.ComponentModel.DataAnnotations;

namespace ProjectsRequirements_API.Models
{
    public class Severity
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Description { get; set; } = null!;

        // Navigation
        public ICollection<Requirement> Requirements { get; set; } = new List<Requirement>();
    }
}
