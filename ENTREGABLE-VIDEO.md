# 🎬 Entregable — Video de 90 segundos

> **✅ GRABADO 2026-09-28:** `C:\Users\User\Videos\GrabaciónFacturación.mp4` (56 s, 30.35 MB, MP4 válido)
> Contenido verificado por el autor: repo → login → productos → factura → PDF → repo ✓
> Duración real 56 s (< 90 s previstos) — aceptado: corto y completo.
> **Pendiente:** subir (YouTube no listado o LinkedIn) y enlazar en README.
> **Decisión 2026-09-28:** NO publicar — el video queda solo en el equipo para envío directo (WhatsApp/postulaciones).

**Objetivo:** demo profesional que muestre el sistema funcionando end-to-end.
**Grabador:** Xbox Game Bar nativo (`Win+Alt+R` iniciar/detener) o OBS (`winget install OBSProject.OBS.Studio`) si prefieres más control.
**Salida:** `C:\Users\User\Videos\Captures\*.mp4`

## Preparación (ANTES de grabar)

- [x] API despierta y probada E2E 2026-09-29: login + factura 001 + PDF 36 KB + XML v2.1.0 ✅
- [ ] Pestañas abiertas y ordenadas: ① Frontend GitHub Pages, ② Repo GitHub (pestañas simples y ordenadas)
- [ ] Cerrar/notificar todo lo personal (notificaciones en "No molestar": `Win+A` → enfoque)
- [ ] Navegador en pantalla completa (`F11`) y ventana a 1080p si es posible
- [ ] Limpiar pestañas personales — solo las del demo
- [ ] Probar el flujo 1 vez SIN grabar (facturar + PDF) para que sea fluido

## Guion (90 seg, sin diálogo — música libre opcional)

| Tiempo | Pantalla | Acción |
|---|---|---|
| **0:00–0:10** | Repo GitHub | Scroll rápido: badge CI verde + sección "Demo en vivo" del README |
| **0:10–0:18** | Frontend | Abrir `https://shadowwhite85.github.io/facturacion-app/` → login `admin@facturacion.app` / `admin123` |
| **0:18–0:25** | Productos | Ver inventario (5 productos Hot Wheels, stock) — búsqueda rápida |
| **0:25–0:55** | Nueva factura | "+ Nueva factura" → cliente (RUC/cedula o Consumidor Final) → agregar 2 productos → cantidad/descuento/IVA se calculan → **Guardar** → aparece el nuevo correlativo `001-001-000000002` (el 000000001 ya existe de la prueba) en la lista |
| **0:55–1:15** | PDF | Botón **PDF** de la factura recién creada → se abre el RIDE con clave de acceso de 49 dígitos → zoom a RUC, IVA y total |
| **1:15–1:30** | Repo | Volver al repo → mostrar estructura (API/Tests/Dockerfile/CI) → fin sobre el badge |

## Grabación

1. `Win+Alt+R` → grabar · `Win+Alt+R` → detener (mismo atajo)
2. Alternativa: `Win+G` → widget de captura (permite activar micrófono si narras)
3. El archivo queda en `C:\Users\User\Videos\Captures\`

## Después de grabar

- [x] Verificado (existencia, ftyp, duración 56 s) 2026-09-28
- [x] Contenido validado por el autor: guion completo ✓
- [x] Decisión: no publicar — uso directo (WhatsApp/postulaciones)
- [ ] Enviar con postulaciones / pedir feedback de un caso

## Fallback

Si Game Bar no captura tu navegador (raro): `winget install OBSProject.OBS.Studio` → escena única + fuente "Captura de pantalla" + Ctrl+F12 (grabar en OBS se configura en Ajustes → Salida).
