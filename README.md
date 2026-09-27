# 📦 FacturaciónApp

Sistema de **facturación e inventario para pymes** adaptado al contexto ecuatoriano
(RUC, cédula, consumidor final, IVA configurable). API REST en **.NET 10** con
frontend en **Angular 22**.

> Proyecto de portafolio — desarrollado en Riobamba, Ecuador 🇪🇨

## ✨ Funcionalidades

- **Productos e inventario:** CRUD completo con búsqueda, eliminación lógica y alerta de stock bajo
- **Clientes:** cédula, RUC, pasaporte o consumidor final, con identificación única
- **Facturación:** emisión con numeración secuencial formato Ecuador (`001-001-000000001`),
  cálculo de IVA por línea (15% por defecto, configurable por producto), descuento de stock
  transaccional y anulación con devolución de stock
- **API REST documentada** con Swagger UI
- **Base de datos con datos de demostración** al primer arranque

## 📸 Capturas

| Inventario | Facturas emitidas |
|---|---|
| ![Productos](productos.png) | ![Facturas](facturas.png) |

## 🛠️ Stack

| Capa | Tecnología |
|---|---|
| Backend | .NET 10, ASP.NET Core, EF Core 10, SQLite |
| Frontend | Angular 22 (standalone, signals, control flow `@if`/`@for`) |
| Docs API | Swagger / OpenAPI |
| Control de versiones | Git + GitHub |

## 🚀 Ejecutar en local

```bash
# 1. API (http://localhost:5059, Swagger en /swagger)
dotnet run --project Facturacion.Api

# 2. Frontend (http://localhost:4200)
cd facturacion-web
npm install
npx ng serve
```

La base de datos SQLite se crea automáticamente con datos de demostración
(productos y clientes de ejemplo) — no se requiere ningún setup adicional.

## 📡 Endpoints principales

| Método | Ruta | Descripción |
|---|---|---|
| GET/POST/PUT/DELETE | `/api/productos` | Gestión de productos |
| GET/POST | `/api/clientes` | Gestión de clientes |
| GET/POST | `/api/facturas` | Emisión y consulta de facturas |
| POST | `/api/facturas/{id}/anular` | Anulación con devolución de stock |

## 🗺️ Roadmap

- [ ] Autenticación JWT con roles (admin / vendedor)
- [ ] Descarga de factura en PDF (QuestPDF)
- [ ] Tests unitarios de la lógica de facturación (xUnit)
- [ ] EF Core Migrations + Docker + GitHub Actions
- [ ] Deploy público (Render + GitHub Pages)

## 📄 Licencia

MIT
