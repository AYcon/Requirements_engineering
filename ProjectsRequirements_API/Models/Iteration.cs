using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectsRequirements_API.Models
{
    public class Iteration
    {
        public int Id { get; set; }

        // FK -> Projects
        [ForeignKey(nameof(Project))]
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        [Required]
        public int VersionNumber { get; set; }

        public DateTime? CompiledAt { get; set; }

        // Prefer FK to Users instead of free text
        [ForeignKey(nameof(CreatedBy))]
        public int CreatedById { get; set; }
        public User CreatedBy { get; set; } = null!;

        public bool Accepted { get; set; } = false;
    }
}
