using Facturacion.Api.Data;
using Facturacion.Api.DTOs;
using Facturacion.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Facturacion.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FacturasController(FacturacionDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<FacturaDto>>> GetAll(CancellationToken ct)
    {
        return await db.Facturas
            .Include(f => f.Cliente)
            .Include(f => f.Detalles).ThenInclude(d => d.Producto)
            .OrderByDescending(f => f.FechaEmision)
            .Select(f => MapearDto(f))
            .ToListAsync(ct);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FacturaDto>> GetById(int id, CancellationToken ct)
    {
        var factura = await db.Facturas
            .Include(f => f.Cliente)
            .Include(f => f.Detalles).ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(f => f.Id == id, ct);

        return factura is null ? NotFound() : MapearDto(factura);
    }

    /// <summary>
    /// Emite una factura: calcula IVA por línea, descuenta stock y
    /// genera el número secuencial en formato ecuatoriano. Todo en una transacción.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<FacturaDto>> Create(CrearFacturaDto dto, CancellationToken ct)
    {
        var cliente = await db.Clientes.FindAsync([dto.ClienteId], ct);
        if (cliente is null)
            return BadRequest(new { mensaje = "El cliente no existe." });

        var productoIds = dto.Detalles.Select(d => d.ProductoId).ToList();
        var productos = await db.Productos
            .Where(p => productoIds.Contains(p.Id) && p.Activo)
            .ToDictionaryAsync(p => p.Id, ct);

        foreach (var detalle in dto.Detalles)
        {
            if (!productos.TryGetValue(detalle.ProductoId, out var producto))
                return BadRequest(new { mensaje = $"El producto {detalle.ProductoId} no existe o está inactivo." });

            if (producto.Stock < detalle.Cantidad)
                return BadRequest(new { mensaje = $"Stock insuficiente para '{producto.Nombre}'. Disponible: {producto.Stock}." });
        }

        await using var transaccion = await db.Database.BeginTransactionAsync(ct);

        var ultimoNumero = await db.Facturas
            .OrderByDescending(f => f.Id)
            .Select(f => (int?)f.Id)
            .FirstOrDefaultAsync(ct) ?? 0;

        var factura = new Factura
        {
            Numero = $"001-001-{(ultimoNumero + 1):D9}",
            ClienteId = dto.ClienteId,
            Detalles = dto.Detalles.Select(d =>
            {
                var producto = productos[d.ProductoId];
                var subtotal = producto.Precio * d.Cantidad;

                producto.Stock -= d.Cantidad;

                return new DetalleFactura
                {
                    ProductoId = producto.Id,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = producto.Precio,
                    PorcentajeIva = producto.PorcentajeIva,
                    Subtotal = subtotal
                };
            }).ToList()
        };

        factura.Subtotal = factura.Detalles.Sum(d => d.Subtotal);
        factura.TotalIva = factura.Detalles.Sum(d => d.Subtotal * d.PorcentajeIva / 100m);
        factura.Total = factura.Subtotal + factura.TotalIva;

        db.Facturas.Add(factura);
        await db.SaveChangesAsync(ct);
        await transaccion.CommitAsync(ct);

        return CreatedAtAction(nameof(GetById), new { id = factura.Id }, MapearDto(factura));
    }

    /// <summary>Anula la factura y devuelve el stock de los productos.</summary>
    [HttpPost("{id:int}/anular")]
    public async Task<IActionResult> Anular(int id, CancellationToken ct)
    {
        var factura = await db.Facturas
            .Include(f => f.Detalles).ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(f => f.Id == id, ct);

        if (factura is null) return NotFound();
        if (factura.Estado == EstadoFactura.Anulada)
            return BadRequest(new { mensaje = "La factura ya está anulada." });

        await using var transaccion = await db.Database.BeginTransactionAsync(ct);

        factura.Estado = EstadoFactura.Anulada;
        foreach (var detalle in factura.Detalles)
            detalle.Producto.Stock += detalle.Cantidad;

        await db.SaveChangesAsync(ct);
        await transaccion.CommitAsync(ct);

        return NoContent();
    }

    private static FacturaDto MapearDto(Factura f) => new(
        f.Id,
        f.Numero,
        f.FechaEmision,
        f.Cliente.Nombre,
        f.Cliente.Identificacion,
        f.Detalles.Select(d => new DetalleFacturaDto(
            d.ProductoId, d.Producto.Nombre, d.Cantidad, d.PrecioUnitario, d.PorcentajeIva, d.Subtotal)).ToList(),
        f.Subtotal,
        f.TotalIva,
        f.Total,
        f.Estado);
}
