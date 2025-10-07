using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectsRequirements_API.Data;
using ProjectsRequirements_API.Dtos;
using ProjectsRequirements_API.Models;
using ProjectsRequirements_API.Security;

namespace ProjectsRequirements_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public UsersController(ApplicationDbContext db) => _db = db;

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDto>> Get(int id)
        {
            var u = await _db.Users.FindAsync(id);
            if (u == null) return NotFound();

            return Ok(new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                CreatedAt = u.CreatedAt
            });
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<UserDto>> Create(CreateUserDto dto)
        {
            // unique email check
            var exists = await _db.Users.AnyAsync(x => x.Email == dto.Email);
            if (exists) return Conflict("Email already in use.");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = PasswordHasher.Hash(dto.Password),
                // CreatedAt handled by model default & DB default
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            var result = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };

            return CreatedAtAction(nameof(Get), new { id = user.Id }, result);
        }

        // POST: api/users/login  (simple demo endpoint)
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] CreateUserDto credentials)
        {
            var u = await _db.Users.FirstOrDefaultAsync(x => x.Email == credentials.Email);
            if (u == null) return Unauthorized();

            if (!PasswordHasher.Verify(credentials.Password, u.PasswordHash))
                return Unauthorized();

            // Normally you’d issue a JWT here; returning DTO for demo.
            return Ok(new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                CreatedAt = u.CreatedAt
            });
        }
    }
}
