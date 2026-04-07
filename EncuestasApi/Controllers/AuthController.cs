using Microsoft.AspNetCore.Mvc;
using EncuestasApi.Models;
using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    public AuthController(AppDbContext db) => _db = db;

    [HttpPost("login")]
    public IActionResult Login([FromBody] Usuario usuario)
    {
        var user = _db.Usuarios.FirstOrDefault(u => u.NombreUsuario == usuario.NombreUsuario);
        if (user == null || !BCrypt.Net.BCrypt.Verify(usuario.Password, user.Password))
            return Unauthorized();

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes("supersecretkey12345");
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("IdUsuario", user.IdUsuario.ToString()) }),
            Expires = DateTime.UtcNow.AddHours(12),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        });

        return Ok(new { token = tokenHandler.WriteToken(token) });
    }
}