using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using portfolio.DTOs;
using portfolio.Entities;
using portfolio.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace portfolio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IGenericRepository<Admin> _adminRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IGenericRepository<Admin> adminRepository, IConfiguration configuration)
        {
            _adminRepository = adminRepository;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var admins = await _adminRepository.FindAsync(a => a.Username == loginDto.Username);
                var admin = admins.FirstOrDefault();

                if (admin == null)
                {
                    return Unauthorized(new { message = "Invalid credentials - user not found" });
                }

                // CRITICAL FIX: .Trim() removes any trailing spaces SQL Server might have added
                var storedHash = admin.PasswordHash?.Trim() ?? "";
                var inputHash = HashPassword(loginDto.Password);

                var isPasswordValid = storedHash == inputHash;

                if (!isPasswordValid)
                {
                    return Unauthorized(new
                    {
                        message = "Invalid credentials - password mismatch",
                        debug_storedHashLength = storedHash.Length,
                        debug_inputHashLength = inputHash.Length
                    });
                }

                var token = GenerateJwtToken(admin);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error: {ex.Message}" });
            }
        }

        [HttpPost("setup")]
        public async Task<IActionResult> SetupAdmin()
        {
            try
            {
                var existingAdmins = await _adminRepository.FindAsync(a => a.Username == "admin");
                var existingAdmin = existingAdmins.FirstOrDefault();

                string password = "Admin123";
                string hashedPassword = HashPassword(password);

                if (existingAdmin != null)
                {
                    existingAdmin.PasswordHash = hashedPassword;
                    existingAdmin.Email = "admin@portfolio.com";
                    existingAdmin.UpdatedAt = DateTime.UtcNow;
                    await _adminRepository.UpdateAsync(existingAdmin);
                    return Ok(new { message = "Admin updated successfully!", username = "admin", password });
                }
                else
                {
                    var newAdmin = new Admin
                    {
                        Username = "admin",
                        PasswordHash = hashedPassword,
                        Email = "admin@portfolio.com",
                        CreatedAt = DateTime.UtcNow
                    };
                    await _adminRepository.AddAsync(newAdmin);
                    return Ok(new { message = "Admin created successfully!", username = "admin", password });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error: {ex.Message}" });
            }
        }

        [HttpGet("check")]
        public async Task<IActionResult> CheckAdmin()
        {
            var admins = await _adminRepository.FindAsync(a => a.Username == "admin");
            var admin = admins.FirstOrDefault();

            if (admin == null)
                return NotFound(new { message = "No admin found. Please call /api/Auth/setup first" });

            var testHash = HashPassword("Admin123");
            var storedHash = admin.PasswordHash?.Trim() ?? "";

            return Ok(new
            {
                username = admin.Username,
                email = admin.Email,
                storedHashLength = admin.PasswordHash?.Length,
                storedHashTrimmedLength = storedHash.Length,
                expectedHashLength = testHash.Length,
                hashesMatch = storedHash == testHash,
                hashStartsWith = admin.PasswordHash?.Substring(0, 10), // Fixed line
                expectedStartsWith = testHash.Substring(0, 10)
            });
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private AuthResponseDto GenerateJwtToken(Admin admin)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(
                _configuration["Jwt:Key"] ?? "YourSuperSecretKeyHereThatIsAtLeast32CharactersLong!");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                    new Claim(ClaimTypes.Name, admin.Username),
                    new Claim(ClaimTypes.Email, admin.Email),
                    new Claim(ClaimTypes.Role, "Admin")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                Username = admin.Username,
                Email = admin.Email,
                ExpiresAt = tokenDescriptor.Expires ?? DateTime.UtcNow.AddDays(7)
            };
        }
    }
}