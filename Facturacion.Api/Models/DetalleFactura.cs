using System.ComponentModel.DataAnnotations.Schema;

namespace Facturacion.Api.Models;

public class DetalleFactura
{
    public int Id { get; set; }

    public int FacturaId { get; set; }
    public Factura Factura { get; set; } = null!;

    public int ProductoId { get; set; }
    public Producto Producto { get; set; } = null!;

    public int Cantidad { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PrecioUnitario { get; set; }

    /// <summary>IVA congelado al momento de la venta (protege ante cambios futuros de tarifa).</summary>
    [Column(TypeName = "decimal(5,2)")]
    public decimal PorcentajeIva { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Subtotal { get; set; }
}
