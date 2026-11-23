# TD-Fonts - Tipografia compartida Sprint 4

- **Version:** 1.0
- **Creado:** 2026-11-23
- **Última actualización:** 2026-11-23
- **Autor:** @LAS1819
- **Estado:** Planificado para el Bloque 1
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
- licencia o atribución en el directorio de arte o en la seccion de credits.

No hace falta crear un manager de fuentes.

## Decision técnica

### Fuente principal

La familia elegida debe cumplir:

- lectura clara a `640 x 360`;
- caracteres ingleses completos;
- soporte de mayúsculas, números y signos usados por el MVP;
- siluetas diferenciadas a 11-20 px;
- licencia compatible con distribucion en itch.io y builds comerciales.

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
tamano, peso y color, pero no deben cambiar de fuente.

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

## Implementación

1. Importar el TTF o OTF con licencia válida.
2. Generar el FontAsset SDF en Unity.
3. Crear o revisar el TextSettings compartido.
4. Aplicar el FontAsset a la hoja base del HUD.
5. Ajustar solo si es necesario los tamaños definidos por las vistas existentes.
6. Probar Mayúsculas, minúsculas, números, signo `?`, apóstrofe y textos largos.
7. Revisar paneles en `640 x 360` y en el embed WebGL `640 x 360`.

## Validación

Debe comprobarse:

- ningún texto corta controles;
- ningún texto invade el panel de inventario;
- los labels de Memorias y Story Overlay envuelven texto;
- la fuente no aparece borrosa a 16:9;
- el fallback renderiza si falta la fuente principal;
- la licencia o atribucion esta incluida en credits o repo.

## Criterio de corte

Usar la fuente actualizada solo si mejora claridad sin romper layout. Si la
prueba de legibilidad falla, conservar el fallback y retirar el FontAsset
experimental, no crear sistemas alternativos.
