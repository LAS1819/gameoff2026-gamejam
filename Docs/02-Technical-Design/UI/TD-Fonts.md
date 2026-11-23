# TD-Fonts - Tipografia compartida Sprint 4

- **Version:** 1.0
- **Creado:** 2026-11-23
- **Última actualización:** 2026-11-23
- **Autor:** @LAS1819
- **Estado:** Implementación aplicada en el HUD
- **Alcance:** HUD, inventario, Memorias y Story Overlay

---

## Objetivo

Definir una fuente retro legible y reutilizable sin duplicar estilos ni cambiar
la arquitectura UI existente.

El proyecto usa UI Toolkit runtime mediante `PanelRenderer`, por lo que la
implementación debe usar **TextCore / FontAsset**, no TMP_FontAsset ni estilos
de uGUI.

## Artefactos

Archivos previstos:

- `Assets/Fonts/<font-family>.ttf` o `.otf`;
- `Assets/Fonts/<font-family>-SDF.asset`;
- `Assets/UI Toolkit/TextSettings.asset` solo si el proyecto necesita un fallback
  global compartido;
- licencia o atribución en el directorio de arte o en la sección de credits.

No hace falta crear un manager de fuentes.

## Decision técnica

### Fuente principal

La familia elegida debe cumplir:

- lectura clara a `640 x 360`;
- caracteres ingleses completos;
- soporte de mayúsculas, números y signos usados por el MVP;
- siluetas diferenciadas a 11-20 px;
- licencia compatible con distribución en itch.io y builds comerciales.

La fuente se asigna al HUD compartido mediante una `FontAsset` de TextCore.
La prueba inicial debe hacerse con un único peso legible; no conviene importar
varios pesos hasta que el layout base sea estable.

### Asignación compartida

La fuente principal se configura en el nivel compartido de UI Toolkit:

- PanelSettings o TextSettings del proyecto para el fallback global;
- USS para herencia y jerarquia visual;
- no insertar `font` ni `-unity-font` elemento por elemento.

Los estilos existentes en `HUD-style-sheet.uss` deben aprovechar una clase o
regla compartida para los textos nuevos. Las clases visuales pueden variar el
tamaño, peso y color, pero no deben cambiar de fuente.

### Jerarquía de tamaño

| Uso | Rango recomendado |
| --- | --- |
| Feedback / nombre de item | 14-16 px |
| Botones Continue / Restart | 14-15 px |
| Memoria: titulo | 18-20 px |
| Memoria: cuerpo | 14 px |
| Story Overlay: titulo | 20-22 px |
| Story Overlay: cuerpo | 14-16 px |

Los tamaños deben validarse visualmente y no extenderse a toda la app sin
prueba a `640 x 360`.

## Fallback

La fuente debe tener fallback si falla la carga o no contiene un carácter:

1. FontAsset principal.
2. Fallback TextCore definido en TextSettings.
3. Fuente por defecto del proyecto si el FontAsset principal no está asignado.

La implementación no debe dejar referencias rotas a archivos eliminados. Si
la fuente aún no está disponible:

- conservar el color y el tamaño actuales;
- usar la fuente por defecto para no bloquear el UI;
- documentar el asset pendiente;
- no crear variantes visuales del layout solo por el placeholder.

## Textos afectados

El cambio compartido debe cubrir:

- `feedback-text`;
- etiquetas temporales de slots;
- Continue y Restart del Story Overlay;
- titulo, cuerpo y contador de Memories;
- letra y tres finales;
- posibles nombres de interactables cuando se implemente hover.

No hace falta localizar texto en este bloque.

---

## Recomendación Tipográfica - Sprint 4

### Fuente Principal: Pixelify Sans

Utilizar **Pixelify Sans** como fuente principal de todo el proyecto.

Motivos:

- Mantiene una estética claramente retro y pixel-art.
- Resulta mucho más legible que Press Start 2P en tamaños pequeños.
- Funciona especialmente bien entre 14 y 20 px.
- Encaja con la filosofía visual del proyecto: "Retro Pixel Art + Presentación Moderna".
- Permite mostrar textos narrativos largos sin generar fatiga visual.
- Se adapta muy bien a HUD, Story Overlay, Memorias e inventario.  【1-738986\|4】

---

### Por qué NO usar Press Start 2P como fuente principal

Aunque Press Start 2P transmite una estética retro muy potente, presenta varios problemas:

- Espaciado excesivo.
- Baja densidad de información.
- Lectura incómoda en párrafos.
- Ocupa demasiado ancho horizontal.
- Dificulta interfaces con mucho texto.

El proyecto contiene:

- Story Overlays.
- Cartas.
- Memorias.
- Finales narrativos.
- Feedback textual frecuente.  

Por ello, utilizar Press Start 2P en todo el juego probablemente perjudique la experiencia de lectura.

---

### Uso recomendado de fuentes

#### Pixelify Sans

Utilizar para:

- Feedback Text.
- Nombres de objetos.
- Inventario.
- Texto de Memorias.
- Story Overlay.
- Carta Final.
- Descripciones de Examine.
- Pantalla de Finales.
- Créditos.

Es la fuente base del proyecto.

---

#### Press Start 2P

Reservar únicamente para:

- Título del juego.
- Game Over.
- Continue.
- Restart.
- Recovered Memories.
- Pantallas especiales.
- Logotipos.

Debe actuar como fuente de acento, no de lectura.

---

## Compatibilidad con la escala del proyecto

La resolución objetivo del juego es:

- Interna: 640x360.
- Escalado: x3.
- Salida: 1920x1080. 【1-738986\|10】【1-738986\|110】

Además, el documento de fuentes propone tamaños entre:

- 14-16 px para cuerpos.
- 18-22 px para títulos.

Pixelify Sans suele comportarse especialmente bien precisamente dentro de esos rangos.

---

## Coherencia narrativa

Un detalle importante: Silent Feel no es un juego arcade.

El núcleo emocional gira alrededor de:

- duelo;
- negación;
- memoria;
- aceptación;
- reinterpretación de recuerdos.

Por ese motivo considero más adecuado utilizar una fuente que sugiera:

- nostalgia;
- melancolía;
- lectura cómoda;

y no una fuente excesivamente asociada a la cultura arcade o a los videojuegos de acción de los años ochenta.

Pixelify Sans aporta exactamente ese equilibrio.

---

## Propuesta Final

**Fuente principal:** Pixelify Sans
**Fuente de títulos:** Press Start 2P

### Resultado esperado

- Lectura cómoda.
- Estética retro clara.
- Presentación moderna.
- Buena legibilidad a 640x360.
- Coherencia con la dirección artística.
- Compatibilidad con UI Toolkit y FontAsset.
- Menor riesgo de rehacer layouts más adelante.

---

## Implementación

1. Importar el TTF o OTF con licencia válida.
2. Generar el FontAsset SDF en Unity.
3. Crear o revisar el TextSettings compartido.
4. Aplicar el FontAsset a la hoja base del HUD.
5. Ajustar solo si es necesario los tamaños definidos por las vistas existentes.
6. Probar Mayúsculas, minúsculas, números, signo `?`, apóstrofe y textos largos.
7. Revisar paneles en `640 x 360` y en el embed WebGL `640 x 360`.

### Estado aplicado

- Ambas fuentes se importaron como TextCore `FontAsset` con fuente SDF y atlas
  `1024 x 1024`;
- Pixelify Sans se asignó como fuente heredada de `#hud-root`;
- Press Start 2P se asignó a títulos de Memoria, títulos del Story Overlay,
  contador de Memorias y controles Continue / Restart;
- los cuerpos, créditos, feedback e inventario heredan Pixelify Sans;
- no se requiere código C# adicional para aplicar la tipografía;
- `PanelSettings` mantiene la resolución de referencia `640 x 360`.

## Validación

Debe comprobarse:

- ningún texto corta controles;
- ningún texto invade el panel de inventario;
- los labels de Memorias y Story Overlay envuelven texto;
- la fuente no aparece borrosa a 16:9;
- el fallback renderiza si falta la fuente principal;
- la licencia o atribución está incluida en credits o repo.

## Criterio de corte

Usar la fuente actualizada solo si mejora claridad sin romper layout. Si la
prueba de legibilidad falla, conservar el fallback y retirar el FontAsset
experimental, no crear sistemas alternativos.
