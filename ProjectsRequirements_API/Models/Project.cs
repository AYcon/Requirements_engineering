using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectsRequirements_API.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required, MaxLength(400)]
        public string Title { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? CompletedAt { get; set; }

        // FK -> Users
        [ForeignKey(nameof(CreatedBy))]
        public int CreatedBy { get; set; }
    }
}
