namespace ProjectsRequirements_API.DTOs
{
    public class CreateIterationRequest
    {
        public int ProjectId { get; set; }
        public int CreatedById { get; set; }
        public string? Description { get; set; }
    }

    public class IterationResponse
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int VersionNumber { get; set; }
        public DateTime? CompiledAt { get; set; }
        public int CreatedBy { get; set; }
        public bool Accepted { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateAcceptanceRequest
    {
        public bool Accepted { get; set; } = true; // default to accepting
    }
}
