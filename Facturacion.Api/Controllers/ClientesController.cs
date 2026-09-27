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
public class ClientesController(FacturacionDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ClienteDto>>> GetAll([FromQuery] string? busqueda, CancellationToken ct)
    {
        var query = db.Clientes.Where(c => c.Activo);

        if (!string.IsNullOrWhiteSpace(busqueda))
            query = query.Where(c => c.Nombre.Contains(busqueda) || c.Identificacion.Contains(busqueda));

        return await query
            .OrderBy(c => c.Nombre)
            .Select(c => new ClienteDto(c.Id, c.TipoIdentificacion, c.Identificacion, c.Nombre, c.Email, c.Telefono, c.Direccion))
            .ToListAsync(ct);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClienteDto>> GetById(int id, CancellationToken ct)
    {
        var cliente = await db.Clientes.FindAsync([id], ct);
        if (cliente is null) return NotFound();

        return new ClienteDto(cliente.Id, cliente.TipoIdentificacion, cliente.Identificacion,
            cliente.Nombre, cliente.Email, cliente.Telefono, cliente.Direccion);
    }

    [HttpPost]
    public async Task<ActionResult<ClienteDto>> Create(CrearClienteDto dto, CancellationToken ct)
    {
        var identificacionExiste = await db.Clientes
            .AnyAsync(c => c.Identificacion == dto.Identificacion, ct);
        if (identificacionExiste)
            return Conflict(new { mensaje = "Ya existe un cliente con esa identificación." });

        var cliente = new Cliente
        {
            TipoIdentificacion = dto.TipoIdentificacion,
            Identificacion = dto.Identificacion,
            Nombre = dto.Nombre,
            Email = dto.Email,
            Telefono = dto.Telefono,
            Direccion = dto.Direccion
        };

        db.Clientes.Add(cliente);
        await db.SaveChangesAsync(ct);

        return CreatedAtAction(nameof(GetById), new { id = cliente.Id },
            new ClienteDto(cliente.Id, cliente.TipoIdentificacion, cliente.Identificacion,
                cliente.Nombre, cliente.Email, cliente.Telefono, cliente.Direccion));
    }
}
