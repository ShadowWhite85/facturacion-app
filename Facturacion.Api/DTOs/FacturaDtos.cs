using System.ComponentModel.DataAnnotations;
using Facturacion.Api.Models;

namespace Facturacion.Api.DTOs;

public record DetalleFacturaDto(
    int ProductoId,
    string ProductoNombre,
    int Cantidad,
    decimal PrecioUnitario,
    decimal PorcentajeIva,
    decimal Subtotal);

public record FacturaDto(
    int Id,
    string Numero,
    DateTime FechaEmision,
    string ClienteNombre,
    string ClienteIdentificacion,
    List<DetalleFacturaDto> Detalles,
    decimal Subtotal,
    decimal TotalIva,
    decimal Total,
    EstadoFactura Estado);

public class CrearFacturaDto
{
    [Required]
    public int ClienteId { get; set; }

    [Required, MinLength(1, ErrorMessage = "La factura debe tener al menos un detalle.")]
    public List<CrearDetalleFacturaDto> Detalles { get; set; } = [];
}

public class CrearDetalleFacturaDto
{
    [Required]
    public int ProductoId { get; set; }

    [Range(1, 10000)]
    public int Cantidad { get; set; }
}
