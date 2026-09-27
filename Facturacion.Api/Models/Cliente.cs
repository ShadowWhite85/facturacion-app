using System.ComponentModel.DataAnnotations;

namespace Facturacion.Api.Models;

public enum TipoIdentificacion
{
    Cedula,
    Ruc,
    Pasaporte,
    ConsumidorFinal
}

public class Cliente
{
    public int Id { get; set; }

    public TipoIdentificacion TipoIdentificacion { get; set; } = TipoIdentificacion.Cedula;

    /// <summary>Cédula (10 dígitos), RUC (13) o pasaporte. Consumidor final: 9999999999999.</summary>
    [Required, MaxLength(13)]
    public string Identificacion { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Nombre { get; set; } = string.Empty;

    [EmailAddress, MaxLength(200)]
    public string? Email { get; set; }

    [MaxLength(20)]
    public string? Telefono { get; set; }

    [MaxLength(300)]
    public string? Direccion { get; set; }

    public bool Activo { get; set; } = true;

    public List<Factura> Facturas { get; set; } = [];
}
