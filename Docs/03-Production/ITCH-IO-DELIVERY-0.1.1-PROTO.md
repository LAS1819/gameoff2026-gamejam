# Entrega en itch.io - `0.1.1-proto`

Guía breve para preparar y verificar la primera página pública de la build candidata.

## Datos de la entrega

- **Título:** Silent Feel
- **Short title / subtítulo:** A psychological point-and-click horror prototype
- **Versión:** `0.1.1-proto`
- **Autor / estudio:** Zig-Art Studio
- **Jam:** Game Off 2026
- **Tema:** SHIFT
- **Clasificación:** Horror, Adventure, Point & Click, Pixel Art
- **Plataformas:** WebGL (recomendada) y Windows
- **Estado:** Prototype / Released in development
- **Duración aproximada:** 30-60 minutos

## Archivos

1. Subir el ZIP de WebGL como **Play in browser**.
2. Comprimir el contenido de la build, no la carpeta `0.1.1-proto` exterior.
3. Comprobar que `index.html` quede en la raíz del ZIP.
4. Subir Windows x64 como ZIP descargable independiente.
5. Identificar las descargas:
   - `SilentFeel-0.1.1-proto-WebGL.zip`
   - `SilentFeel-0.1.1-proto-Windows-x64.zip`

## Configuración recomendada

- **Embed:** `960 x 540`, ratio 16:9.
- **Fullscreen button:** activado.
- **Mobile friendly:** no.
- **Pricing:** Free o `0` si la jam no requiere otro formato.
- **Visibility:** Draft hasta superar las pruebas; Restricted mientras se valida con testers.
- No marcar todavía la página como disponible de forma irreversible antes de probar el embed.

## Texto para la página

### Descripción breve

> **Silent Feel** es una aventura point & click de terror psicológico creada para Game Off 2026. Explora un edificio atrapado entre dos realidades, usa SHIFT para descubrir lo que permanece oculto y reúne fragmentos de memoria antes de enfrentarte a la verdad.

### Descripción completa

> Un edificio silencioso. Dos realidades superpuestas. Cinco recuerdos que alguien intentó borrar.
>
> Explora salas conectadas, encuentra objetos, observa los detalles del entorno y cambia entre la realidad normal y el **Otro Lado** mediante SHIFT. Cada interacción puede revelar una nueva ruta o acercarte un poco más a la verdad sobre la desaparición de tu padre.
>
> Esta versión `0.1.1-proto` contiene un recorrido completo desde el Lobby hasta el desenlace final. Los recuerdos son opcionales y determinan cuál de los tres finales descubrirás.
>
> ### Controles
>
> - **Clic izquierdo:** interactuar, usar, recoger o cambiar de sala.
> - **Clic derecho:** examinar.
> - **SHIFT o botón del HUD:** cambiar de realidad en las salas compatibles.
> - **Escape / Enter / Espacio:** avanzar o cerrar textos narrativos.
> - **Restart:** reiniciar la partida y volver al Lobby.
>
> ### Versión de prototipo
>
> Esta entrega es un prototipo completo de game jam, no la versión final del juego. Puede contener textos provisionales, ajustes visuales pendientes y pequeñas incidencias. No hay sistema de guardado persistente: el progreso se conserva únicamente durante la partida actual.
>
> ### Notas técnicas
>
> - Se recomienda jugar la versión WebGL a pantalla completa en un navegador de escritorio.
> - También se incluye una build descargable para Windows x64.
> - Si el embed no carga o el navegador bloquea la reproducción, descarga la versión de Windows.

### Créditos mínimos

```text
Desarrollado por Zig-Art Studio
Creado para Game Off 2026
Motor: Unity 6000.6.9f1
```

Sustituir este bloque si se confirma una lista final de colaboradores, música, fuentes o assets externos.

## Advertencias recomendadas

La página debería indicar que el juego contiene:

- atmósfera de terror psicológico;
- referencias a desaparición y muerte;
- iluminación tenue y efectos visuales de tensión.

No describir escenas específicas si forman parte del puzle o del desenlace.

## Checklist antes de publicar

- [ ] Título y portada cargados
- [ ] Descripción revisada sin spoilers
- [ ] Build WebGL ejecutable desde el embed
- [ ] Botón de pantalla completa probado
- [ ] Build Windows x64 descargada, descomprimida y ejecutada
- [ ] Versión `0.1.1-proto` visible
- [ ] Controles legibles en la página
- [ ] Advertencia de contenido visible
- [ ] Etiquetas y categorías añadidas
- [ ] Enlace al repositorio público añadido
- [ ] Página probada en modo incógnito o con caché limpia
- [ ] Flujo Lobby → Ending → Restart validado en itch.io

Ver también la guía de generación de builds:
[BUILD-0.1.1-PROTO-WEBGL-WINDOWS.md](./BUILD-0.1.1-PROTO-WEBGL-WINDOWS.md).
