using Microsoft.EntityFrameworkCore;
using ProjectsRequirements_API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var conn = builder.Configuration.GetConnectionString("ProjectRequirementsDb");
builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(conn));

// --- Add CORS BEFORE building the app ---
var allowedOrigins = "_allowedOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowedOrigins,
        policy =>
        {
            policy
                .WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// --- Build the app ---
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Apply CORS middleware
app.UseCors(allowedOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
