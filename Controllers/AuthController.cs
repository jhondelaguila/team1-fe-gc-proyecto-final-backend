using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using team1_fe_gc_proyecto_final_backend.Data;
using team1_fe_gc_proyecto_final_backend.DTOs;
using team1_fe_gc_proyecto_final_backend.Models;

namespace team1_fe_gc_proyecto_final_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly DatabaseContext _context;

        public AuthController(IConfiguration config, DatabaseContext context)
        {
            _configuration = config;
            _context = context;
        }

        [HttpPost, Route("signin")]
        public async Task<IActionResult> Post(Usuario _userData)
        {
            if (_userData != null && _userData.Email != null && _userData.Pass != null)
            {
                var user = await GetUser(_userData.Email);
                bool validPass = BCrypt.Net.BCrypt.Verify(_userData.Pass, user.Pass);
                if (validPass)
                {
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Id", user.Id.ToString()),
                        new Claim("Email", user.Email),
                        new Claim("Admin", user.Admin.ToString())
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
                    string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                    var response = new SignInResponseDto
                    {
                        Token = tokenString,
                        Usuario = user,
                    };

                    return new OkObjectResult(response);
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("signup")]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'DatabaseContext.Usuarios'  is null.");
            }
            usuario.Pass = BCrypt.Net.BCrypt.HashPassword(usuario.Pass);
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return await _context.Usuarios.FindAsync(usuario.Id);
        }

        private async Task<Usuario> GetUser(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
