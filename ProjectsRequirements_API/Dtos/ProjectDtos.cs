using System.ComponentModel.DataAnnotations;

namespace ProjectsRequirements_API.Dtos
{
    public class CreateProjectRequest
    {
        [Required, MaxLength(400)]
        public string Title { get; set; } = null!;

        /// <summary>
        /// User Id (FK to Users.Id)
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "CreatedBy must be a positive user id.")]
        public int CreatedBy { get; set; }
    }

    public class ProjectResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int CreatedBy { get; set; }
    }
}
