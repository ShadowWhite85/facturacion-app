using System.ComponentModel.DataAnnotations;

namespace Facturacion.Api.DTOs;

public record ProductoDto(
    int Id,
    string Codigo,
    string Nombre,
    string? Descripcion,
    decimal Precio,
    decimal PorcentajeIva,
    int Stock,
    bool Activo);

public class CrearProductoDto
{
    [Required, MaxLength(25)]
    public string Codigo { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Descripcion { get; set; }

    [Range(0, 999999)]
    public decimal Precio { get; set; }

    [Range(0, 100)]
    public decimal PorcentajeIva { get; set; } = 15m;

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
}
