using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectsRequirements_API.Data;
using ProjectsRequirements_API.Dtos;
using ProjectsRequirements_API.Models;

namespace ProjectsRequirements_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ProjectsController(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Create a new Project.
        /// </summary>
        /// <remarks>
        /// - Validates that CreatedBy user exists.
        /// - Uses UTC timestamps (CreatedAt defaults to SYSUTCDATETIME() at DB level; also set server-side for immediate response).
        /// </remarks>
        [HttpPost]
        public async Task<ActionResult<ProjectResponse>> Create([FromBody] CreateProjectRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            // Ensure the FK (CreatedBy) exists to return a clean 400 instead of a SQL FK exception
            var userExists = await _db.Users
                .AsNoTracking()
                .AnyAsync(u => u.Id == request.CreatedBy, ct);

            if (!userExists)
            {
                return BadRequest(new
                {
                    error = "InvalidCreatedBy",
                    message = $"No user found with id {request.CreatedBy}."
                });
            }

            // Map DTO -> Entity
            var entity = new Project
            {
                Title = request.Title.Trim(),
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow,   // mirrors DB default for immediate response consistency
                CompletedAt = null
            };

            _db.Projects.Add(entity);
            await _db.SaveChangesAsync(ct);

            var firstIteration = new Iteration
            {
                ProjectId = entity.Id,
                CreatedBy = request.CreatedBy,
                VersionNumber = 1,
                Accepted = false,
                Description = "Initial version"
            };
            _db.Iterations.Add(firstIteration);
            await _db.SaveChangesAsync(ct);


            // Map Entity -> DTO
            var response = new ProjectResponse
            {
                Id = entity.Id,
                Title = entity.Title,
                CreatedAt = entity.CreatedAt,
                CompletedAt = entity.CompletedAt,
                CreatedBy = entity.CreatedBy
            };

            // Return 201 with a Location header without needing a GET endpoint
            return Created($"/api/projects/{response.Id}", response);
        }
    }
}
