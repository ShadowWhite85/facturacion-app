using Facturacion.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Facturacion.Api.Data;

public class FacturacionDbContext(DbContextOptions<FacturacionDbContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Factura> Facturas => Set<Factura>();
    public DbSet<DetalleFactura> DetallesFactura => Set<DetalleFactura>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producto>()
            .HasIndex(p => p.Codigo)
            .IsUnique();

        modelBuilder.Entity<Cliente>()
            .HasIndex(c => c.Identificacion)
            .IsUnique();

        modelBuilder.Entity<Factura>()
            .HasIndex(f => f.Numero)
            .IsUnique();

        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Datos de demostración realistas (Ecuador)
        // Datos de demostración realistas (Ecuador)
        // Los hashes son fijos (no BCrypt.HashPassword en runtime) porque el modelo
        // HasData debe ser determinístico o EF falla con PendingModelChangesWarning.
        modelBuilder.Entity<Usuario>().HasData(
            new Usuario { Id = 1, Email = "admin@facturacion.app", Nombre = "Administrador Demo", Rol = RolUsuario.Admin, PasswordHash = "$2a$11$kSg0JBib7cgq.yS9QbDwf.iy5dEAA0WjyWYLjmEh.S9qm//nqIg4S" },
            new Usuario { Id = 2, Email = "vendedor@facturacion.app", Nombre = "Vendedor Demo", Rol = RolUsuario.Vendedor, PasswordHash = "$2a$11$qcx6yyVjcElKjcrbM/KIO.vThaVt9l.BNDTUJqDRf8DvcZRgyTj6C" }
        );

        modelBuilder.Entity<Producto>().HasData(
            new Producto { Id = 1, Codigo = "HW-001", Nombre = "Hot Wheels Nissan Skyline GT-R (BNR34)", Descripcion = "Car Culture - Japan Historics", Precio = 12.50m, Stock = 8 },
            new Producto { Id = 2, Codigo = "HW-002", Nombre = "Hot Wheels '70 Dodge Charger", Descripcion = "Boulevard - STH", Precio = 25.00m, Stock = 3 },
            new Producto { Id = 3, Codigo = "HW-003", Nombre = "Hot Wheels Porsche 911 GT3 RS", Descripcion = "Premium - Euro Speed", Precio = 9.99m, Stock = 15 },
            new Producto { Id = 4, Codigo = "SV-001", Nombre = "Servicio de desarrollo de software (hora)", Precio = 25.00m, Stock = 999 },
            new Producto { Id = 5, Codigo = "LB-001", Nombre = "Novela digital - Capítulos compilados", PorcentajeIva = 0m, Precio = 4.99m, Stock = 500 }
        );

        modelBuilder.Entity<Cliente>().HasData(
            new Cliente { Id = 1, TipoIdentificacion = TipoIdentificacion.ConsumidorFinal, Identificacion = "9999999999999", Nombre = "Consumidor Final" },
            new Cliente { Id = 2, TipoIdentificacion = TipoIdentificacion.Ruc, Identificacion = "0601234567001", Nombre = "Comercial Riobamba S.A.", Email = "compras@comercialriobamba.ec", Telefono = "032950123", Direccion = "Av. Daniel León Borja y Chile, Riobamba" },
            new Cliente { Id = 3, TipoIdentificacion = TipoIdentificacion.Cedula, Identificacion = "0603456789", Nombre = "María Fernanda Quizhpi", Email = "mafe.quizhpi@mail.com", Telefono = "0987654321", Direccion = "Calbay y Primera Constituyente, Riobamba" }
        );
    }
}
