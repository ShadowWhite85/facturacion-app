using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Facturacion.Api.Models;

public class Producto
{
    public int Id { get; set; }

    [Required, MaxLength(25)]
    public string Codigo { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Descripcion { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Precio { get; set; }

    /// <summary>IVA que aplica al producto. En Ecuador el general es 15%.</summary>
    [Column(TypeName = "decimal(5,2)")]
    public decimal PorcentajeIva { get; set; } = 15m;

    public int Stock { get; set; }

    public bool Activo { get; set; } = true;

    public List<DetalleFactura> Detalles { get; set; } = [];
}
