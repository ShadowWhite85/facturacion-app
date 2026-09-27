using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Facturacion.Api.Models;

public enum EstadoFactura
{
    Emitida,
    Anulada
}

public class Factura
{
    public int Id { get; set; }

    /// <summary>Número de factura formato ecuatoriano: 001-001-000000001</summary>
    [Required, MaxLength(17)]
    public string Numero { get; set; } = string.Empty;

    public DateTime FechaEmision { get; set; } = DateTime.UtcNow;

    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;

    public List<DetalleFactura> Detalles { get; set; } = [];

    [Column(TypeName = "decimal(18,2)")]
    public decimal Subtotal { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalIva { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; }

    public EstadoFactura Estado { get; set; } = EstadoFactura.Emitida;
}
