using Facturacion.Api.Data;
using Facturacion.Api.DTOs;
using Facturacion.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Facturacion.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductosController(FacturacionDbContext db) : ControllerBase
{
    /// <summary>Lista productos, con búsqueda opcional por nombre o código.</summary>
    [HttpGet]
    public async Task<ActionResult<List<ProductoDto>>> GetAll([FromQuery] string? busqueda, CancellationToken ct)
    {
        var query = db.Productos.Where(p => p.Activo);

        if (!string.IsNullOrWhiteSpace(busqueda))
            query = query.Where(p => p.Nombre.Contains(busqueda) || p.Codigo.Contains(busqueda));

        var productos = await query
            .OrderBy(p => p.Nombre)
            .Select(p => new ProductoDto(p.Id, p.Codigo, p.Nombre, p.Descripcion, p.Precio, p.PorcentajeIva, p.Stock, p.Activo))
            .ToListAsync(ct);

        return productos;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductoDto>> GetById(int id, CancellationToken ct)
    {
        var producto = await db.Productos.FindAsync([id], ct);
        if (producto is null) return NotFound();

        return new ProductoDto(producto.Id, producto.Codigo, producto.Nombre, producto.Descripcion,
            producto.Precio, producto.PorcentajeIva, producto.Stock, producto.Activo);
    }

    [HttpPost]
    public async Task<ActionResult<ProductoDto>> Create(CrearProductoDto dto, CancellationToken ct)
    {
        var producto = new Producto
        {
            Codigo = dto.Codigo,
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Precio = dto.Precio,
            PorcentajeIva = dto.PorcentajeIva,
            Stock = dto.Stock
        };

        db.Productos.Add(producto);
        await db.SaveChangesAsync(ct);

        return CreatedAtAction(nameof(GetById), new { id = producto.Id },
            new ProductoDto(producto.Id, producto.Codigo, producto.Nombre, producto.Descripcion,
                producto.Precio, producto.PorcentajeIva, producto.Stock, producto.Activo));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CrearProductoDto dto, CancellationToken ct)
    {
        var producto = await db.Productos.FindAsync([id], ct);
        if (producto is null) return NotFound();

        producto.Codigo = dto.Codigo;
        producto.Nombre = dto.Nombre;
        producto.Descripcion = dto.Descripcion;
        producto.Precio = dto.Precio;
        producto.PorcentajeIva = dto.PorcentajeIva;
        producto.Stock = dto.Stock;

        await db.SaveChangesAsync(ct);
        return NoContent();
    }

    /// <summary>Eliminación lógica: el producto queda inactivo para no romper facturas históricas. Solo admin.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = nameof(RolUsuario.Admin))]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var producto = await db.Productos.FindAsync([id], ct);
        if (producto is null) return NotFound();

        producto.Activo = false;
        await db.SaveChangesAsync(ct);
        return NoContent();
    }
}
