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
- [x] README publicado ✅ 2026-09-27

### A.1.3 Autenticación JWT + roles
Criterio de aceptación: login devuelve token; endpoint de anular factura exige rol admin.
- Paquetes: `Microsoft.AspNetCore.Authentication.JwtBearer`
- Entidad `Usuario` (email, passwordHash BCrypt, rol)
- Endpoints: `POST /api/auth/login`, config JWT en Program.cs
- Angular: interceptor HTTP + guard de rutas + pantalla login
- [x] JWT funcionando E2E ✅ 2026-09-27 (401/403/200 verificados; interceptor + guard + login en Angular)

### A.1.4 PDF de factura
- Paquete: `QuestPDF` (licencia Community gratis)
- Endpoint: `GET /api/facturas/{id}/pdf` → PDF con formato factura Ecuador
- Angular: botón "Descargar PDF" en lista de facturas
- [x] PDF descargable ✅ 2026-09-27 — **y adicional: XML esquema SRI v2.1.0 + clave de acceso módulo 11** (pendientes para facturación real: firma XAdES-BES con firma electrónica + autorización en servidores SRI)

### A.1.5 Tests (xUnit)
- Proyecto `Facturacion.Tests` (xUnit + EF Core InMemory)
- Tests mínimos: cálculo IVA, descuento stock, error stock insuficiente, anulación devuelve stock, numeración secuencial
- Ejecutar: `dotnet test`
- [x] 8 tests pasando ✅ 2026-09-27 (IVA, stock, bad request, anulación, numeración, clave acceso módulo 11)

### A.1.6 EF Core Migrations
```powershell
dotnet tool install --global dotnet-ef   # si no está
dotnet ef migrations add Inicial --project Facturacion.Api
```
- Reemplazar `EnsureCreatedAsync()` por `MigrateAsync()` en Program.cs
- [x] Migraciones en el repo ✅ 2026-09-27 (dotnet-ef local vía `.config/dotnet-tools.json`; MigrateAsync reemplaza EnsureCreated; hashes seed fijos por PendingModelChangesWarning)

### A.1.7 Dockerfile
Multi-stage: SDK build → runtime aspnet:10.0. Puerto 8080.
- [x] Dockerfile (multi-stage) + .dockerignore ✅ 2026-09-27 — *sin Docker local: comandos restore/publish validados simulando las capas* (build real pendiente en CI/Render)

### A.1.8 GitHub Actions
`.github/workflows/ci.yml`: push → setup-dotnet 10 → restore, build, test; + job Node 24 que buildea Angular.
- [x] Workflow creado ✅ 2026-09-27 (validado con actionlint 1.7.7, exit 0)
- [x] Badge verde en README ✅ 2026-09-27 (run 36369654312: ambos jobs verdes)

---

## Paso A.2 — Deploy público gratis
- [ ] **A.2.1** API en Render (free): conectar repo, `Dockerfile` o build command `dotnet publish`. URL anotada aquí: `________`
  - Preparado ✅ 2026-09-27: CORS configurable (`Cors:Origins` en appsettings, incluye `shadowwhite85.github.io`) + build OK — *falta cuenta de Render (requiere usuario)*
- [ ] **A.2.2** Angular en GitHub Pages (`angular.json` base-href) o Cloudflare Pages. URL: `________`
  - Workflow `deploy-pages.yml` listo ✅ (actionlint ok, build con base-href `/facturacion-app/` verificado) — *falta push + habilitar Pages en Settings → Pages → Source: GitHub Actions*
- [ ] **A.2.3** `environment.prod.ts` apuntando a la API desplegada + rebuild + redeploy
  - `environment.prod.ts` creado ✅ con URL provisional `https://facturacion-app.onrender.com/api` (confirmar nombre de servicio al crearlo en Render)
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
| 2026-09-27 | A.1.2 ✅ README profesional publicado |
| 2026-09-27 | A.1.3 ✅ JWT + roles (admin/vendedor) con pruebas E2E de seguridad |
| 2026-09-27 | A.1.4 ✅ PDF (QuestPDF RIDE) + XML SRI v2.1.0 con clave de acceso mod-11 |
| 2026-09-27 | A.1.5 ✅ Facturacion.Tests (xUnit) con 8 tests verdes, incluido en la solución |
| 2026-09-27 | A.1.6 ✅ EF Core Migrations (Inicial) + MigrateAsync + dotnet-ef local |
| 2026-09-27 | A.1.7 ✅ Dockerfile multi-stage + .dockerignore (comandos validados sin Docker local) |
| 2026-09-27 | A.1.8 ✅ GitHub Actions CI verificado en verde (run 36369654312) + badge |
| 2026-09-27 | A.2 preparación ✅ CORS configurable + environment.prod.ts + workflow deploy-pages (pendiente: push, habilitar Pages, cuenta Render) |
