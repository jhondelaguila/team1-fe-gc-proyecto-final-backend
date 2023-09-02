using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using team1_fe_gc_proyecto_final_backend.Data;
using team1_fe_gc_proyecto_final_backend.Models;

namespace team1_fe_gc_proyecto_final_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly DataBaseContext _context;

        public TokenController(IConfiguration config, DataBaseContext context) 
        {
            _configuration = config;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Usuario _userData) 
        {
            if(_userData != null && _userData.Email != null && _userData.Pass != null)
            {
                var user = await GetUser(_userData.Email, _userData.Pass);
                if (user != null)
                {
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Nombre", user.Nombre),
                        new Claim("Apellidos", user.Apellidos),
                        new Claim("Email", user.Email),
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest();
                }
            }else
            {
                return BadRequest();
            }
        }

        private async Task<Usuario> GetUser(string email, string password)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email && u.Pass == password );
        }
    }
}
