using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectsRequirements_API.Models
{
    public class Requirement
    {
        public int Id { get; set; }

        public int IterationId { get; set; }

        public int CategoryId { get; set; }

        [Required]
        public string Text { get; set; } = null!; // Long text. If you want to bound it, add [MaxLength(n)]

        public int? PriorityId { get; set; }

        public int? SeverityId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
