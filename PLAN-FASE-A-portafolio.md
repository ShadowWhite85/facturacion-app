# FASE A — Portafolio .NET vendible
**Objetivo:** tener 1–2 proyectos desplegados en vivo que prueben que automatizo negocios.
**Proyecto base:** Sistema de Facturación (API .NET 10 + Angular 22) — ya funciona localmente.
**Ubicación código:** en esta misma carpeta — `FacturacionApp.slnx`, `Facturacion.Api\`, `facturacion-web\`

---

## ✅ Estado al iniciar
- [x] API REST completa: Productos, Clientes, Facturas (emitir/anular), EF Core 10 + SQLite
- [x] Frontend Angular 22 conectado, probado E2E (factura con IVA 15%)
- [x] `NuGet.config` local creado (el feed `pruebas.unach.edu.ec` está muerto — si falla restore, revisar NuGet.config)

## Arrancar en local
```powershell
dotnet run --project Facturacion.Api          # API → http://localhost:5059/swagger
npx ng serve                                   # dentro de facturacion-web → :4200
```

---

## Paso A.1 — Robustecer el proyecto

### A.1.1 Git + GitHub
```powershell
git init
# crear .gitignore con: bin/ obj/ node_modules/ dist/ *.db .playwright-mcp/
git add -A; git commit -m "Sistema de facturación: API .NET 10 + Angular 22"
git remote add origin https://github.com/<mi-usuario>/facturacion-app.git
git push -u origin main
```
- [x] Repo público en GitHub creado y subido ✅ 2026-09-27 → https://github.com/ShadowWhite85/facturacion-app

### A.1.2 README.md profesional
Debe contener: título + descripción 2 líneas, capturas (`productos.png`, `facturas.png` están en la raíz del proyecto), stack, cómo ejecutar, endpoints, roadmap.
- [ ] README publicado

### A.1.3 Autenticación JWT + roles
Criterio de aceptación: login devuelve token; endpoint de anular factura exige rol admin.
- Paquetes: `Microsoft.AspNetCore.Authentication.JwtBearer`
- Entidad `Usuario` (email, passwordHash BCrypt, rol)
- Endpoints: `POST /api/auth/login`, config JWT en Program.cs
- Angular: interceptor HTTP + guard de rutas + pantalla login
- [ ] JWT funcionando E2E

### A.1.4 PDF de factura
- Paquete: `QuestPDF` (licencia Community gratis)
- Endpoint: `GET /api/facturas/{id}/pdf` → PDF con formato factura Ecuador
- Angular: botón "Descargar PDF" en lista de facturas
- [ ] PDF descargable

### A.1.5 Tests (xUnit)
- Proyecto `Facturacion.Tests` (xUnit + EF Core InMemory)
- Tests mínimos: cálculo IVA, descuento stock, error stock insuficiente, anulación devuelve stock, numeración secuencial
- Ejecutar: `dotnet test`
- [ ] 5+ tests pasando

### A.1.6 EF Core Migrations
```powershell
dotnet tool install --global dotnet-ef   # si no está
dotnet ef migrations add Inicial --project Facturacion.Api
```
- Reemplazar `EnsureCreatedAsync()` por `MigrateAsync()` en Program.cs
- [ ] Migraciones en el repo

### A.1.7 Dockerfile
Multi-stage: SDK build → runtime aspnet:10.0. Puerto 8080.
- [ ] `docker build -t facturacion-api .` funciona (o validar sintaxis si no hay Docker)

### A.1.8 GitHub Actions
`.github/workflows/ci.yml`: push → setup-dotnet 10 → restore, build, test; + job Node 24 que buildea Angular.
- [ ] Badge verde en README

---

## Paso A.2 — Deploy público gratis
- [ ] **A.2.1** API en Render (free): conectar repo, `Dockerfile` o build command `dotnet publish`. URL anotada aquí: `________`
- [ ] **A.2.2** Angular en GitHub Pages (`angular.json` base-href) o Cloudflare Pages. URL: `________`
- [ ] **A.2.3** `environment.prod.ts` apuntando a la API desplegada + rebuild + redeploy
- [ ] **A.2.4** README actualizado con links en vivo + credenciales demo

---

## Paso A.3 — Proyecto 2 (opcional): API de agendamiento
Para citas de clínicas/peluquerías. Reutiliza aprendizajes; API minimalista + Swagger público.
- [ ] Scaffold + CRUD citas + JWT
- [ ] Deploy en Render

## 📦 Entregable de cierre de fase
Portafolio con demo en vivo + video de 90 seg grabado (OBS) mostrando: login → emitir factura → descargar PDF → repo GitHub.

## 🔄 Registro
| Fecha | Avance |
|---|---|
| 2026-09-27 | Base E2E funcionando en local |
| 2026-09-27 | A.1.1 ✅ Git + GitHub: repo público ShadowWhite85/facturacion-app |
