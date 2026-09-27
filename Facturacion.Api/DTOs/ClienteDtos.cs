using System.ComponentModel.DataAnnotations;
using Facturacion.Api.Models;

namespace Facturacion.Api.DTOs;

public record ClienteDto(
    int Id,
    TipoIdentificacion TipoIdentificacion,
    string Identificacion,
    string Nombre,
    string? Email,
    string? Telefono,
    string? Direccion);

public class CrearClienteDto
{
    public TipoIdentificacion TipoIdentificacion { get; set; } = TipoIdentificacion.Cedula;

    [Required, StringLength(13, MinimumLength = 10)]
    public string Identificacion { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Nombre { get; set; } = string.Empty;

    [EmailAddress, MaxLength(200)]
    public string? Email { get; set; }

    [MaxLength(20)]
    public string? Telefono { get; set; }

    [MaxLength(300)]
    public string? Direccion { get; set; }
}
