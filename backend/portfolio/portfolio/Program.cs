using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using portfolio.Data;
using portfolio.Entities;
using portfolio.Interfaces;
using portfolio.Middleware;
using portfolio.Repositories;
using portfolio.Services;
using portfolio.Validators;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Remove this line completely:
// builder.WebHost.UseWebRoot("wwwroot");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Rest of your configuration remains the same...
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Portfolio API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Database Configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtKey = builder.Configuration["Jwt:Key"]
            ?? "YourSuperSecretKeyHereThatIsAtLeast32CharactersLong!";

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateProjectValidator>();

// Dependency Injection - Repositories
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

// Dependency Injection - Services
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IExperienceService, ExperienceService>();
builder.Services.AddScoped<ITestimonialService, TestimonialService>();
builder.Services.AddScoped<IContactMessageService, ContactMessageService>();
builder.Services.AddScoped<IFileService, FileService>();

// Additional services
builder.Services.AddHttpContextAccessor();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var app = builder.Build();

// Ensure wwwroot and upload directories exist
var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
if (!Directory.Exists(wwwrootPath))
{
    Directory.CreateDirectory(wwwrootPath);

    // Create upload subdirectories
    var uploadsPath = Path.Combine(wwwrootPath, "uploads");
    Directory.CreateDirectory(uploadsPath);
    Directory.CreateDirectory(Path.Combine(uploadsPath, "projects"));
    Directory.CreateDirectory(Path.Combine(uploadsPath, "testimonials"));

    Console.WriteLine("[SETUP] Created wwwroot and upload directories");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(); // Serve static files from wwwroot
app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ========== AUTO-SEED ADMIN ON STARTUP ==========
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Apply migrations
    try
    {
        dbContext.Database.Migrate();
        Console.WriteLine("[DB] Database migrations applied successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[DB] Error applying migrations: {ex.Message}");
    }

    // Seed admin user
    var adminExists = dbContext.Admins.Any(a => a.Username == "admin");
    if (!adminExists)
    {
        string password = "Admin123";
        string hashedPassword;
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            hashedPassword = Convert.ToBase64String(hashedBytes);
        }

        var admin = new Admin
        {
            Username = "admin",
            PasswordHash = hashedPassword,
            Email = "admin@portfolio.com",
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Admins.Add(admin);
        dbContext.SaveChanges();
        Console.WriteLine($"[SEED] Admin user created. Username: admin, Password: {password}");
    }
    else
    {
        Console.WriteLine("[SEED] Admin user already exists.");
    }
}
// ========== END AUTO-SEED ==========

app.Run();