# Build guide - `0.1.1-proto` (WebGL + Windows)

Objetivo: sacar builds de prueba estables y repetibles para validar entrega temprana, con prioridad en WebGL y respaldo en Windows.

## 1) Preparación común (una vez)

1. Abrir **File > Build Profiles**.
2. Confirmar que la escena principal jugable está en **Scenes In Build** y en el orden correcto.
3. En **Player Settings**:
   - [x] Company: Zig-Art Studio
   - [x] Product Name: SilentFeel-GameOff2026-GameJam
   - [x] Version: `0.1.1-proto`.
   - [x] Scripting Backend WebGL: **IL2CPP** (WebGL no usa otro backend en Unity).
   - [x] Scripting Backend Windows: **IL2CPP** para las candidatas de entrega; **Mono** solo puede usarse temporalmente para acelerar una prueba local de depuración.
   - [x] API Compatibility Level: mantener el valor configurado actualmente en el proyecto. No cambiarlo en `0.1.1-proto` salvo que aparezca un error de compilación concreto.
   - [x] Color Space: **Linear**, según la configuración actual del proyecto.
4. Hacer un build de prueba local antes de publicar.

## 2) Build WebGL (prioridad)

**Perfil recomendado:**

- [x] Platform: **WebGL**
- [x] WebGL Template: **SilentFeel16x9** (centers and scales the canvas at 16:9).
- [x] Custom template: the project includes `Assets/WebGLTemplates/SilentFeel16x9`, with a responsive 960x540 16:9 canvas and local fullscreen control.
- [x] Development Build: **OFF** (para build candidata)
- [x] Autoconnect Profiler: **OFF**
- [x] Script Debugging: **OFF**
- [x] Compression Format: **Gzip** para la primera build. Es la opción más compatible y está bien soportada por itch.io. Brotli reduce más el tamaño, pero conviene usarlo solo al validar también el alojamiento final y sus cabeceras.
- [x] Name Files As Hashes: **ON**. Los nombres con hash permiten que el navegador detecte una versión nueva sin reutilizar archivos antiguos por caché.
- [x] Data Caching: **ON**. Permite al navegador reutilizar los datos de la build descargada y acelerar cargas posteriores.
- [x] Color Space: mantener **Linear**, según la configuración actual del proyecto.

### Resolución WebGL

- Resolución interna del proyecto: **640 x 360**.
- Resolución objetivo 16:9: **1920 x 1080**.
- Resolución recomendada para el embed de itch.io: **960 x 540**.
- Cambiar en **Player Settings > Resolution and Presentation**:
  - Default Screen Width (Web): `960`.
  - Default Screen Height (Web): `540`
- El proyecto usa render 640 x 360 y escalado exacto x3; 960 x 540 es equivalente a un escalado x1.5 en el embed.
- En la página de itch.io, configurar el embed con el mismo ratio **16:9**.

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
- [x] Architecture: **x86_64**
- [x] Development Build: **OFF** (para candidata)
- [x] Script Debugging: **OFF**
- [x] Fullscreen Mode: **Fullscreen Window** para abrir en la resolución del monitor, o **Windowed** si se necesita depuración fácil.
- [x] Resolution Windows: mantener **1920 x 1080** como resolución predeterminada para coincidir con el objetivo visual.
- [x] Resizable Window: **OFF** en `0.1.1-proto` para evitar redimensionados no validados.

### Resolución Windows

- Resolución interna del proyecto: **640 x 360**.
- Resolución objetivo recomendada: **1920 x 1080**.
- Formato obligatorio: **16:9**.
- Valor actual del proyecto:
  - Default Screen Width: `1920`.
  - Default Screen Height: `1080`.
  - Fullscreen Mode: `Fullscreen Window`.
- Para una primera prueba en pantalla pequeña, puede usarse una ventana de **1280 x 720** sin cambiar el diseño base.
- Si se activa modo ventana, mantener el ratio 16:9 al redimensionar manualmente.
- No modificar aún el ratio de cámara o los fondos para “encajar” resoluciones distintas.

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

La configuración de la página, los metadatos y el texto listo para publicar están en
[ITCH-IO-DELIVERY-0.1.1-PROTO.md](./ITCH-IO-DELIVERY-0.1.1-PROTO.md).

## 5) Checklist final de salida

- [ ] Versión `0.1.1-proto` visible en Player Settings
- [ ] Build WebGL generada y probada
- [ ] Build Windows generada y probada
- [ ] Test básico de flujo completo superado
- [ ] Draft de itch.io publicado en privado para validación interna
- [ ] Resolución WebGL configurada a **960 x 540** y probada dentro de un embed 16:9
- [ ] Resolución Windows configurada a **1920 x 1080** como predeterminada y probada también en una ventana 1280 x 720

Si este checklist pasa, la versión queda lista como baseline para seguir iterando con menor riesgo.
