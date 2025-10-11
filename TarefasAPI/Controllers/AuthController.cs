using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TarefasAPI.Models;
using Microsoft.EntityFrameworkCore;
using TarefasAPI.Data;

namespace TarefasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserModel user)
        {
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
                return BadRequest(new { message = "Usuário já existe." });

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuário cadastrado com sucesso!" });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserModel user)
        {
            var usuario = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == user.Username && u.Password == user.Password);

            if (usuario == null)
                return Unauthorized(new { message = "Usuário ou senha inválidos." });

            var token = GerarToken(usuario);
            return Ok(new { token });
        }

        private string GerarToken(UserModel user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim("id", user.Id.ToString())
            };

            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
