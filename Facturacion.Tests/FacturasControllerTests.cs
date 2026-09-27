using Facturacion.Api.Controllers;
using Facturacion.Api.Data;
using Facturacion.Api.DTOs;
using Facturacion.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Facturacion.Tests;

/// <summary>
/// Tests de la lógica de facturación con SQLite en memoria
/// (a diferencia de InMemory, soporta transacciones reales).
/// </summary>
public class FacturasControllerTests : IDisposable
{
    private readonly SqliteConnection _conexion;
    private readonly FacturacionDbContext _db;
    private readonly FacturasController _controller;

    public FacturasControllerTests()
    {
        _conexion = new SqliteConnection("Data Source=:memory:");
        _conexion.Open();

        var opciones = new DbContextOptionsBuilder<FacturacionDbContext>()
            .UseSqlite(_conexion)
            .Options;

        _db = new FacturacionDbContext(opciones);
        _db.Database.EnsureCreated();

        // HasData ya sembró productos (ids 1-5, precio 12.50/25/9.99...) y clientes (id 1 = Consumidor Final)
        var productoTest = new Producto { Codigo = "TEST-01", Nombre = "Producto test", Precio = 10m, PorcentajeIva = 15m, Stock = 5 };
        _db.Productos.Add(productoTest);
        _db.SaveChanges();
        _productoTestId = productoTest.Id;

        _controller = new FacturasController(_db, null!, null!);
    }

    private const int ClienteId = 1; // Consumidor Final del seed
    private readonly int _productoTestId;

    [Fact]
    public async Task EmitirFactura_CalculaIva15PorCiento()
    {
        var resultado = await _controller.Create(new CrearFacturaDto
        {
            ClienteId = ClienteId, // consumidor final del seed (o el agregado)
            Detalles = [new CrearDetalleFacturaDto { ProductoId = _productoTestId, Cantidad = 2 }]
        }, CancellationToken.None);

        var factura = ExtraerFactura(resultado);
        Assert.Equal(20m, factura.Subtotal);   // 2 × 10
        Assert.Equal(3m, factura.TotalIva);    // 15%
        Assert.Equal(23m, factura.Total);      // 20 + 3
    }

    [Fact]
    public async Task EmitirFactura_DescuentaStock()
    {
        await _controller.Create(new CrearFacturaDto
        {
            ClienteId = ClienteId,
            Detalles = [new CrearDetalleFacturaDto { ProductoId = _productoTestId, Cantidad = 2 }]
        }, CancellationToken.None);

        var producto = await _db.Productos.FirstAsync(p => p.Codigo == "TEST-01");
        Assert.Equal(3, producto.Stock); // 5 - 2
    }

    [Fact]
    public async Task EmitirFactura_StockInsuficiente_RetornaBadRequest()
    {
        var resultado = await _controller.Create(new CrearFacturaDto
        {
            ClienteId = ClienteId,
            Detalles = [new CrearDetalleFacturaDto { ProductoId = _productoTestId, Cantidad = 99 }]
        }, CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(resultado.Result);
    }

    [Fact]
    public async Task AnularFactura_DevuelveStockYMarcaAnulada()
    {
        var creada = ExtraerFactura(await _controller.Create(new CrearFacturaDto
        {
            ClienteId = ClienteId,
            Detalles = [new CrearDetalleFacturaDto { ProductoId = _productoTestId, Cantidad = 2 }]
        }, CancellationToken.None));

        await _controller.Anular(creada.Id, CancellationToken.None);

        var producto = await _db.Productos.FirstAsync(p => p.Codigo == "TEST-01");
        var factura = await _db.Facturas.FirstAsync(f => f.Id == creada.Id);
        Assert.Equal(5, producto.Stock); // se devuelve el stock
        Assert.Equal(EstadoFactura.Anulada, factura.Estado);
    }

    [Fact]
    public async Task EmitirFactura_NumeracionSecuencialFormatoEcuatoriano()
    {
        var f1 = ExtraerFactura(await _controller.Create(new CrearFacturaDto
        {
            ClienteId = ClienteId,
            Detalles = [new CrearDetalleFacturaDto { ProductoId = _productoTestId, Cantidad = 1 }]
        }, CancellationToken.None));

        var f2 = ExtraerFactura(await _controller.Create(new CrearFacturaDto
        {
            ClienteId = ClienteId,
            Detalles = [new CrearDetalleFacturaDto { ProductoId = _productoTestId, Cantidad = 1 }]
        }, CancellationToken.None));

        Assert.Matches(@"^001-001-\d{9}$", f1.Numero);
        Assert.Matches(@"^001-001-\d{9}$", f2.Numero);
        Assert.NotEqual(f1.Numero, f2.Numero);
    }

    private static FacturaDto ExtraerFactura(ActionResult<FacturaDto> resultado)
    {
        var created = Assert.IsType<CreatedAtActionResult>(resultado.Result);
        return Assert.IsType<FacturaDto>(created.Value);
    }

    public void Dispose() => _conexion.Dispose();
}
