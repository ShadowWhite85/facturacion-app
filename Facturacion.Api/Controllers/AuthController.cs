using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Facturacion.Api.Data;
using Facturacion.Api.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Facturacion.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(FacturacionDbContext db, IConfiguration config) : ControllerBase
{
    /// <summary>Autentica un usuario y devuelve un JWT con su rol.</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<RespuestaLoginDto>> Login(LoginDto dto, CancellationToken ct)
    {
        var usuario = await db.Usuarios
            .FirstOrDefaultAsync(u => u.Email == dto.Email && u.Activo, ct);

        if (usuario is null || !BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
            return Unauthorized(new { mensaje = "Email o contraseña incorrectos." });

        var expira = DateTime.UtcNow.AddHours(8);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim(ClaimTypes.Role, usuario.Rol.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: expira,
            signingCredentials: credenciales);

        return new RespuestaLoginDto(
            new JwtSecurityTokenHandler().WriteToken(token),
            usuario.Nombre,
            usuario.Email,
            usuario.Rol.ToString(),
            expira);
    }
}
