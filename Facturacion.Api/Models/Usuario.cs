using System.ComponentModel.DataAnnotations;

namespace Facturacion.Api.Models;

public enum RolUsuario
{
    Vendedor,
    Admin
}

public class Usuario
{
    public int Id { get; set; }

    [Required, EmailAddress, MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public RolUsuario Rol { get; set; } = RolUsuario.Vendedor;

    public bool Activo { get; set; } = true;
}
