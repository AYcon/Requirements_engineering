using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectsRequirements_API.Models
{
    public class Requirement
    {
        public int Id { get; set; }

        // FK -> Iterations
        [ForeignKey(nameof(Iteration))]
        public int IterationId { get; set; }
        public Iteration Iteration { get; set; } = null!;

        // FK -> Categories
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        [Required]
        public string Text { get; set; } = null!; // Long text. If you want to bound it, add [MaxLength(n)]

        // FK -> Priorities (optional)
        [ForeignKey(nameof(Priority))]
        public int? PriorityId { get; set; }
        public Priority? Priority { get; set; }

        // FK -> Severities (optional)
        [ForeignKey(nameof(Severity))]
        public int? SeverityId { get; set; }
        public Severity? Severity { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
