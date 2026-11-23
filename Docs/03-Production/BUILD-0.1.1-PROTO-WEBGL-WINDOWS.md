# Build guide - `0.1.1-proto` (WebGL + Windows)

Objetivo: sacar builds de prueba estables y repetibles para validar entrega temprana, con prioridad en WebGL y respaldo en Windows.

## 1) Preparación común (una vez)

1. Abrir **File > Build Profiles**.
2. Confirmar que la escena principal jugable está en **Scenes In Build** y en el orden correcto.
3. En **Player Settings**:
   - [x] Company: Zig-Art Studio
   - [x] Product Name: SilentFeel-GameOff2026-GameJam
   - [x] Version: `0.1.1-proto`.
   - [ ] Scripting Backend: **IL2CPP**.
   - [ ] API Compatibility Level: mantener el valor actual del proyecto (no cambiar para esta fase).
4. Hacer un build de prueba local antes de publicar.

## 2) Build WebGL (prioridad)

**Perfil recomendado:**

- [x] Platform: **WebGL**
- [x] Development Build: **OFF** (para build candidata)
- [x] Autoconnect Profiler: **OFF**
- [ ] Script Debugging: **OFF**
- [x] Compression Format: **Gzip** (más simple para pruebas en itch.io) <- Por qué Gzip y no Brotli? Es necesario poner Brotli cuando se haga la build final para producción?
- [x] Name Files As Hashes: **ON** <- Por qué se recomienda activar esto para evitar problemas de cache en el navegador?
- [x] Data Caching: **ON** <- Permite que el navegador almacene datos de la build para acelerar cargas posteriores.
- [x] Color Space: mantener la del proyecto Linear (sin cambios para esta versión)

**Carpeta de salida sugerida:**

`Builds/WebGL/0.1.1-proto/`

**Validación rápida WebGL:**

1. Ejecutar la build localmente (Build and Run o servidor estático).
2. Comprobar:
   - carga inicial sin error crítico en consola,
   - entrada de ratón (clic izquierdo/derecho),
   - transición entre salas clave,
   - HUD visible y funcional,
   - ending y retorno al lobby.

## 3) Build Windows

**Perfil recomendado:**

- [ ] Platform: **Windows**
- [ ] Architecture: **x86_64**
- [ ] Development Build: **OFF** (para candidata)
- [ ] Script Debugging: **OFF**
- [ ] Fullscreen Mode: **Windowed Fullscreen** o el valor actual del proyecto
- [ ] Resolution: dejar la actual del proyecto para evitar scope extra

**Carpeta de salida sugerida:**

`Builds/Windows/0.1.1-proto/`

**Validación rápida Windows:**

1. Ejecutar `.exe` en una carpeta limpia.
2. Comprobar:
   - arranque sin popup de error,
   - input y navegación idénticos a Editor,
   - audio y UI correctos,
   - cierre y reapertura sin problemas.

## 4) Entrega temprana en itch.io (recomendada)

Para evitar sorpresas de última hora:

1. Subir primero la build WebGL como **Draft**.
2. Página mínima con:
   - título de versión: `0.1.1-proto`,
   - controles (clic izquierdo / clic derecho),
   - nota de que es build de prueba.
3. Test rápido en navegador desde la página privada de itch.io.
4. Subir build Windows como descarga opcional en el mismo draft.

## 5) Checklist final de salida

- [ ] Versión `0.1.1-proto` visible en Player Settings
- [ ] Build WebGL generada y probada
- [ ] Build Windows generada y probada
- [ ] Test básico de flujo completo superado
- [ ] Draft de itch.io publicado en privado para validación interna

Si este checklist pasa, la versión queda lista como baseline para seguir iterando con menor riesgo.
