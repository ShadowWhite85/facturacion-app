using System.ComponentModel.DataAnnotations;

namespace Facturacion.Api.DTOs;

public class LoginDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;
}

public record RespuestaLoginDto(
    string Token,
    string Nombre,
    string Email,
    string Rol,
    DateTime Expira);
