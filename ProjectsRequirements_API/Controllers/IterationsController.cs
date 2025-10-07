using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectsRequirements_API.Data;
using ProjectsRequirements_API.Dtos;
using ProjectsRequirements_API.DTOs;
using ProjectsRequirements_API.Models;

namespace ProjectsRequirements_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IterationsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public IterationsController(ApplicationDbContext db) => _db = db;

        // POST: api/iterations
        [HttpPost]
        public async Task<ActionResult<IterationResponse>> Create([FromBody] CreateIterationRequest req, CancellationToken ct)
        {
            // Ensure valid project & user
            var projectExists = await _db.Projects.AnyAsync(p => p.Id == req.ProjectId, ct);
            if (!projectExists)
                return NotFound($"Project {req.ProjectId} not found.");

            var userExists = await _db.Users.AnyAsync(u => u.Id == req.CreatedById, ct);
            if (!userExists)
                return NotFound($"User {req.CreatedById} not found.");

            // Get latest version for this project
            var lastVersion = await _db.Iterations
                .Where(i => i.ProjectId == req.ProjectId)
                .MaxAsync(i => (int?)i.VersionNumber, ct) ?? 0;

            var nextVersion = lastVersion + 1;

            var iteration = new Iteration
            {
                ProjectId = req.ProjectId,
                CreatedBy = req.CreatedById,
                VersionNumber = nextVersion,
                Accepted = false,
                Description = req.Description
            };

            _db.Iterations.Add(iteration);
            await _db.SaveChangesAsync(ct);

            var resp = new IterationResponse
            {
                Id = iteration.Id,
                ProjectId = iteration.ProjectId,
                VersionNumber = iteration.VersionNumber,
                CompiledAt = iteration.CompiledAt,
                CreatedBy = iteration.CreatedBy,
                Accepted = iteration.Accepted,
                Description = iteration.Description
            };

            return CreatedAtAction(nameof(GetByProjectId), new { projectId = iteration.ProjectId }, resp);
        }

        [HttpGet("iteration/{projectId:int}")]
        public async Task<ActionResult<IEnumerable<IterationResponse>>> GetByProjectId(int projectId, CancellationToken ct)
        {
            var exists = await _db.Projects.AnyAsync(p => p.Id == projectId, ct);
            if (!exists)
                return NotFound($"Project {projectId} not found.");

            var iterations = await _db.Iterations
                .Where(i => i.ProjectId == projectId)
                .OrderByDescending(i => i.VersionNumber)
                .Select(i => new IterationResponse
                {
                    Id = i.Id,
                    ProjectId = i.ProjectId,
                    VersionNumber = i.VersionNumber,
                    CompiledAt = i.CompiledAt,
                    CreatedBy = i.CreatedBy,
                    Accepted = i.Accepted,
                    Description = i.Description
                })
                .ToListAsync(ct);

            return Ok(iterations);
        }
    }
}
