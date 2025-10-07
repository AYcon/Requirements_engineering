using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectsRequirements_API.Models
{
    public class Iteration
    {
        [Required]
        public int Id { get; set; }

        // FK -> Projects
        public int ProjectId { get; set; }

        
        public int VersionNumber { get; set; }

        public DateTime? CompiledAt { get; set; }

        // Prefer FK to Users instead of free text
        [ForeignKey(nameof(CreatedBy))]
        public int CreatedBy { get; set; }

        public bool Accepted { get; set; } = false;

        public string? Description { get; set; }
    }
}
