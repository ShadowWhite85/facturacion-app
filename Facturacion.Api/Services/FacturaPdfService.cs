using Facturacion.Api.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Facturacion.Api.Services;

/// <summary>Genera el RIDE (representación impresa) de la factura en PDF.</summary>
public class FacturaPdfService(IConfiguration config)
{
    public byte[] GenerarPdf(Factura factura)
    {
        var emisor = config.GetSection("Emisor");

        var documento = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A5);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text(emisor["RazonSocial"]).FontSize(14).Bold();
                        col.Item().Text(emisor["NombreComercial"]);
                        col.Item().Text($"RUC: {emisor["Ruc"]}");
                        col.Item().Text(emisor["DirMatriz"]).FontSize(9);
                    });
                    row.RelativeItem().Border(1).Padding(10).Column(col =>
                    {
                        col.Item().AlignCenter().Text("FACTURA").FontSize(16).Bold();
                        col.Item().AlignCenter().Text($"No. {factura.Numero}").Bold();
                        col.Item().AlignCenter().Text($"Fecha: {factura.FechaEmision:dd/MM/yyyy}");
                        if (factura.Estado == EstadoFactura.Anulada)
                            col.Item().AlignCenter().Text("ANULADA").FontColor(Colors.Red.Medium).Bold();
                    });
                });

                page.Content().PaddingVertical(15).Column(col =>
                {
                    col.Item().Text($"Cliente: {factura.Cliente.Nombre}").Bold();
                    col.Item().Text($"Identificación: {factura.Cliente.Identificacion}");
                    if (factura.Cliente.Direccion is not null)
                        col.Item().Text($"Dirección: {factura.Cliente.Direccion}");

                    col.Item().PaddingTop(10).Table(tabla =>
                    {
                        tabla.ColumnsDefinition(cols =>
                        {
                            cols.ConstantColumn(45);
                            cols.RelativeColumn();
                            cols.ConstantColumn(60);
                            cols.ConstantColumn(60);
                        });

                        tabla.Header(header =>
                        {
                            header.Cell().Text("Cant.").Bold();
                            header.Cell().Text("Descripción").Bold();
                            header.Cell().AlignRight().Text("P. Unit.").Bold();
                            header.Cell().AlignRight().Text("Total").Bold();
                        });

                        foreach (var d in factura.Detalles)
                        {
                            tabla.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Text(d.Cantidad.ToString());
                            tabla.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Text(d.Producto.Nombre);
                            tabla.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).AlignRight().Text($"${d.PrecioUnitario:F2}");
                            tabla.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).AlignRight().Text($"${d.Subtotal:F2}");
                        }
                    });

                    col.Item().AlignRight().PaddingTop(10).Column(tot =>
                    {
                        tot.Item().Text($"Subtotal: ${factura.Subtotal:F2}");
                        tot.Item().Text($"IVA: ${factura.TotalIva:F2}");
                        tot.Item().Text($"TOTAL: ${factura.Total:F2}").FontSize(13).Bold();
                    });
                });

                page.Footer().AlignCenter().Text(t =>
                {
                    t.Span("Documento generado por FacturaciónApp · Ambiente de PRUEBAS · ");
                    t.Span("Sin validez tributaria").FontSize(8);
                });
            });
        });

        return documento.GeneratePdf();
    }
}
