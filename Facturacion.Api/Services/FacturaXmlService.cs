using System.Globalization;
using System.Xml.Linq;
using Facturacion.Api.Models;

namespace Facturacion.Api.Services;

/// <summary>
/// Genera el XML de comprobante electrónico "factura" versión 2.1.0 según el
/// esquema del SRI (ficha técnica de factura). Ambiente 1 = pruebas.
/// </summary>
public class FacturaXmlService(IConfiguration config)
{
    private static readonly CultureInfo Invariante = CultureInfo.InvariantCulture;

    public string GenerarXml(Factura factura)
    {
        var emisor = config.GetSection("Emisor");
        var partes = factura.Numero.Split('-');
        var estab = partes[0];
        var ptoEmi = partes[1];
        var secuencial = partes[2];

        var claveAcceso = ClaveAccesoGenerator.Generar(
            factura.FechaEmision,
            codigoDocumento: "01",
            ruc: emisor["Ruc"]!,
            ambiente: emisor["Ambiente"] ?? "1",
            establecimiento: estab,
            puntoEmision: ptoEmi,
            secuencial: secuencial);

        var detallesAgrupadosPorIva = factura.Detalles
            .GroupBy(d => d.PorcentajeIva)
            .ToList();

        var doc = new XDocument(
            new XDeclaration("1.0", "UTF-8", null),
            new XElement("factura",
                new XAttribute("id", "comprobante"),
                new XAttribute("version", "2.1.0"),

                // ===== infoTributaria =====
                new XElement("infoTributaria",
                    new XElement("ambiente", emisor["Ambiente"] ?? "1"),
                    new XElement("tipoEmision", "1"),
                    new XElement("razonSocial", emisor["RazonSocial"]),
                    new XElement("nombreComercial", emisor["NombreComercial"]),
                    new XElement("ruc", emisor["Ruc"]),
                    new XElement("claveAcceso", claveAcceso),
                    new XElement("codDoc", "01"),
                    new XElement("estab", estab),
                    new XElement("ptoEmi", ptoEmi),
                    new XElement("secuencial", secuencial),
                    new XElement("dirMatriz", emisor["DirMatriz"])
                ),

                // ===== infoFactura =====
                new XElement("infoFactura",
                    new XElement("fechaEmision", factura.FechaEmision.ToString("dd/MM/yyyy", Invariante)),
                    new XElement("dirEstablecimiento", emisor["DirEstablecimiento"]),
                    new XElement("obligadoContabilidad", emisor["ObligadoContabilidad"] ?? "NO"),
                    new XElement("tipoIdentificacionComprador", MapearTipoIdentificacion(factura.Cliente.TipoIdentificacion)),
                    new XElement("razonSocialComprador", factura.Cliente.Nombre),
                    new XElement("identificacionComprador", factura.Cliente.Identificacion),
                    new XElement("totalSinImpuestos", Formatear(factura.Subtotal)),
                    new XElement("totalDescuento", "0.00"),
                    new XElement("totalConImpuestos",
                        detallesAgrupadosPorIva.Select(g => new XElement("totalImpuesto",
                            new XElement("codigo", "2"), // 2 = IVA
                            new XElement("codigoPorcentaje", CodigoPorcentajeIva(g.Key)),
                            new XElement("baseImponible", Formatear(g.Sum(d => d.Subtotal))),
                            new XElement("tarifa", Formatear(g.Key)),
                            new XElement("valor", Formatear(g.Sum(d => d.Subtotal * d.PorcentajeIva / 100m)))
                        ))),
                    new XElement("propina", "0.00"),
                    new XElement("importeTotal", Formatear(factura.Total)),
                    new XElement("moneda", "DOLAR")
                ),

                // ===== detalles =====
                new XElement("detalles",
                    factura.Detalles.Select(d => new XElement("detalle",
                        new XElement("codigoPrincipal", d.Producto.Codigo),
                        new XElement("descripcion", d.Producto.Nombre),
                        new XElement("cantidad", d.Cantidad.ToString(Invariante)),
                        new XElement("precioUnitario", Formatear(d.PrecioUnitario)),
                        new XElement("descuento", "0.00"),
                        new XElement("precioTotalSinImpuesto", Formatear(d.Subtotal)),
                        new XElement("impuestos", new XElement("impuesto",
                            new XElement("codigo", "2"),
                            new XElement("codigoPorcentaje", CodigoPorcentajeIva(d.PorcentajeIva)),
                            new XElement("tarifa", Formatear(d.PorcentajeIva)),
                            new XElement("baseImponible", Formatear(d.Subtotal)),
                            new XElement("valor", Formatear(d.Subtotal * d.PorcentajeIva / 100m))
                        ))
                    ))
                ),

                // ===== infoAdicional =====
                new XElement("infoAdicional",
                    factura.Cliente.Email is not null
                        ? new XElement("campoAdicional", new XAttribute("nombre", "Email")) { Value = factura.Cliente.Email }
                        : null,
                    factura.Cliente.Direccion is not null
                        ? new XElement("campoAdicional", new XAttribute("nombre", "Dirección")) { Value = factura.Cliente.Direccion }
                        : null
                )
            )
        );

        var declaracion = doc.Declaration + Environment.NewLine;
        return declaracion + doc.Root!.ToString(SaveOptions.DisableFormatting);
    }

    /// <summary>04 = RUC, 05 = cédula, 06 = pasaporte, 07 = consumidor final.</summary>
    private static string MapearTipoIdentificacion(TipoIdentificacion tipo) => tipo switch
    {
        TipoIdentificacion.Ruc => "04",
        TipoIdentificacion.Cedula => "05",
        TipoIdentificacion.Pasaporte => "06",
        _ => "07"
    };

    /// <summary>Código SRI del porcentaje IVA: 0 = 0%, 4 = 15%.</summary>
    private static string CodigoPorcentajeIva(decimal porcentaje) => porcentaje switch
    {
        0m => "0",
        15m => "4",
        _ => "4"
    };

    private static string Formatear(decimal valor) => valor.ToString("F2", Invariante);
}
