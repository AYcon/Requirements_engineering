using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectsRequirements_API.Models
{
    public class Category
    {
        [Key]
        [Column("Category_Id")]
        public int CategoryId { get; set; }

        [Required, MaxLength(200)]
        public string Description { get; set; } = null!;
    }
}
