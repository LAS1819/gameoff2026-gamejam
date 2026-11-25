# TD-HUD - HUD mínimo runtime (Sprint 2)

- **Versión:** 3.6
- **Creado:** 2026-11-06
- **Actualizado:** 2026-11-24
- **Autor:** @LAS1819
- **Estado:** Integrado en el HUD del prototipo; Sprint 4 amplía fuentes e inventario

## Objetivo

Mantener el HUD mínimo, estable y usable de `0.1.4-proto` sin crear una
segunda arquitectura de UI.

Incluye:

- slots de inventario,
- feedback textual temporal,
- botón SHIFT integrado en el mismo panel,
- pulse visual al cambiar de realidad,
- fade corto al cambiar de room.
- Story Overlay reutilizable para carta y final.
- componentes visuales separados por responsabilidad:
  - `HUDInventoryView`;
  - `HUDStoryOverlayView`;
  - `HUDMemoryDiscoveryView`.

## Fundaciones visuales Sprint 4

El Bloque 1 se diseña sin cambiar el sistema base del HUD:

- [TD-Fonts.md](TD-Fonts.md): fuente retro compartida y fallback.
- [TD-Inventory.md](TD-Inventory.md): reemplazo visual de los tres slots.
- [TD-Item-Sprites.md](TD-Item-Sprites.md): iconos prioritarios por item.
- [TD-Cursor.md](TD-Cursor.md): cursor personalizado y fallback del sistema.
- [TD-Overlay-Backgrounds.md](TD-Overlay-Backgrounds.md): fondos de Memorias, Carta y Final.

La implementación debe preservar el layout, los eventos y los estados existentes.

## Decisión técnica

Para runtime usamos **PanelRenderer + UXML + USS** (no `UIDocument`).

Patrón:

- `PanelRenderer` en un GO raíz (`HUDRoot`)
- binding por `RegisterUIReloadCallback`
- actualización por eventos de managers;
- lectura puntual de teclado y pointer para los botones modales.

## Estado de implementación real

### Archivos de UI

- [HUD.uxml](../../../Assets/UI/HUD.uxml)
- [HUD-style-sheet.uss](../../../Assets/UI/HUD-style-sheet.uss)

### Scripts

- [HUDController.cs](../../../Assets/Scripts/UI/HUDController.cs)
- [UIManager.cs](../../../Assets/Scripts/UI/UIManager.cs)
- [InteractionManager.cs](../../../Assets/Scripts/Interaction/InteractionManager.cs)

### Compatibilidad

- [UIShiftButton.cs](../../../Assets/Scripts/UI/UIShiftButton.cs) se mantiene como referencia histórica.
- El HUD centralizado (HUDController) gestiona completamente el `shift-button` ahora.
- El viejo `ShiftPanel` con su PanelRenderer debe ser eliminado de la escena para evitar duplicación.

## Jerarquía visual esperada

```text
HUDRoot (GameObject)
├── PanelRenderer
└── HUD visual tree (UXML)
    ├── inventory-panel
    │   ├── slot-0
    │   ├── slot-1
    │   └── slot-2
    ├── feedback-text
    ├── lantern-icon
    │   └── sprite-state (0 = no-obtained, 1 = obtained-off, 2 = obtained-on)
    ├── shift-button
    ├── transition-overlay
    ├── reality-pulse
    └── Story Overlay runtime subtree
        ├── backdrop
        ├── letter / ending content
        ├── Continue
        └── Restart game
```

La `lantern-icon` se coloca encima del `shift-button` para mantener la misma lógica visual que el resto del `HUD`: compacta, permanente y legible.

El Story Overlay se construye en runtime dentro del `hud-root`. Sus botones
quedan limitados al viewport y tienen fallback por teclado y pointer.

## Comportamiento implementado

### Inventario

- Se refresca al recibir `InventoryManager.ItemAdded` / `ItemRemoved`.
- Slots alternan clases `empty` / `full`.
- Los seis items del MVP muestran su Sprite por `item.id`.
- Un click sobre el item seleccionado lo deselecciona.
- Las interacciones resueltas limpian la selección para evitar usos obsoletos.
- `Floppy Disk`, `Basement Code` y `Valve Handle` usan el mismo flujo gráfico
  que Staff Key, Library Key y Worn Book.
- Los IDs futuros sin Sprite conservan el fallback textual del slot.

### Feedback

- `UIManager.ShowFeedback(...)` delega en `HUDController`.
- Mensajes temporales con auto-hide.
- El texto de feedback usa un fondo semitransparente oscuro para mantener
  contraste sobre fondos claros.
- `InteractionManager` envía feedback en pickup/use/examine y errores de gating.

### Memorias

- `Use` o clic principal sobre el interactable solo muestra feedback narrativo
  ("mira más de cerca").
- `Examine` mediante clic derecho es la única acción que desbloquea, recupera
  o reabre la Memoria.
- El descubrimiento muestra el overlay completo y permanece abierto.
- Al volver a Examinar el mismo Interactable se abre de nuevo; la flag de
  descubrimiento solo registra progreso y no impide la reconsulta.
- El timeout automático está eliminado.
- Continue, Escape, Enter y Space cierran el overlay.

### Layout del Story Overlay

- Carta y Resumen usan paneles relativos al viewport.
- El panel limita su alto a `220-320 px` y permite que título, cuerpo y acciones
  adapten su espacio sin solaparse.
- El texto usa `flex-shrink` y ajuste normal para evitar cortes intermitentes
  entre loops o resoluciones.
- En el gate `640 x 360`, el panel mantiene el contenido dentro de la vista
  y conserva acciones accesibles.

### Hover

- El nombre del interactable visible aparece solo mientras el cursor está sobre él.
- El tooltip usa el sistema HUD y desaparece fuera del hotspot o ante overlays.

### SHIFT

- Click sobre `shift-button` del HUD ejecuta `RealityManager.ToggleReality()`.
- Respeta gating por room: Hallway, Bedroom, Library y Basement.
- Actualiza clases visuales `normal` / `other-side`.
- Marca flag `ShiftPerformed`.

### Lantern UI

- El icono de la `Linterna` es permanente y visible desde el inicio en la `UI`.
- En estado inicial se usa el Sprite 0 del `spritesheet-ui-icon-lantern-v1`.
- Este sprite representa la silueta de la `Linterna` sin obtener.
- Cuando el jugador recoge la `Linterna` en `Bedroom`, el `HUD` cambia al Sprite 1 del mismo atlas.
- El Sprite 1 representa la `Linterna` obtenida, pero apagada.
- No existe estado activo de linterna en `0.1.1-proto`; el icono representa
  únicamente si la herramienta fue obtenida.
- Light 2D, batería y gating por uso se mantienen fuera del MVP.
- El icono se sitúa por encima del `shift-button`, sin romper la compactación visual del `HUD`.

### Transiciones visuales

- `transition-overlay`: flash corto en `RoomChanged`.
- `reality-pulse`: flash corto en `RealityChanged`.

### Story Overlay

- muestra la Revelation Letter;
- bloquea controles de gameplay;
- ofrece un botón Continue;
- cambia al resumen de Ending;
- muestra contador de Memorias y créditos provisionales;
- ofrece Restart game y devuelve al Lobby;
- está preparado para arte final y localización.

### Separación de responsabilidades

`HUDController` actúa como fachada y coordinador de:

- configuración de `PanelRenderer`;
- suscripciones a eventos globales;
- estado SHIFT;
- feedback temporal;
- transiciones visuales.

Las vistas específicas mantienen su propio estado:

- `HUDInventoryView`: slots, selección e inspección de objetos.
- `HUDStoryOverlayView`: carta, final, reinicio y navegación modal.
- `HUDMemoryDiscoveryView`: construcción, fade y presentación temporal de una Memoria.

### Arte de overlays

Los requisitos visuales completos están separados en
[TD-Overlay-Backgrounds.md](TD-Overlay-Backgrounds.md):

- Memorias: panel azul ceniza con detalles de archivo;
- Carta: textura de papel sin texto pintado;
- Resumen Final: marco oscuro con ornamento dorado.

Los PNG sirven como fondo. Los textos permanecen en UI Toolkit para permitir
wrap, redimensionado y localización.

---

## Feedback sobre fondos claros

Mantener el background semitransparente en los textos temporales:

- `feedback-text`: fondo `rgba(18,16,24,0.82)`, borde dorado tenue;
- `hover-name-text`: fondo `rgba(18,16,24,0.82)`, borde dorado tenue;
- padding mínimo de `6 px 12 px`;
- texto blanco con opacidad `0.95`;
- no añadir sombra de texto ni sustitutos con outline.

El fondo pertenece al texto, no a la room. No debe aplicarse transparencia
global al Inventario ni a los slots. Para HUD sobre fondo claro, se recomienda
un fondo opaco al `82-90%` para que el copy siga legible.

---

## Transición entre rooms

Usar el mismo elemento `transition-overlay` para todas las rooms:

- duración objetivo: `0.35 s`;
- fondo oscuro: `rgba(0,0,0,0.70)`;
- apertura y cierre con opacity: `0.3 s` de transición;
- no se deben crear overlays por room;
- no bloquear controles después de terminar la transición;
- respetar `RoomChanged` como único evento disparador.

Si se incorpora un sprite o patrón de fade, debe ocupar toda la pantalla y
mantener el centro visual simple. Evitar 9-slice, bordes pixelados o animaciones
que requieran nuevos assets por room.

---

## Pulse de cambio de realidad

Usar `reality-pulse` como flash global y breve:

- duración objetivo: `0.25 s`;
- tint suave: `rgba(120,130,255,0.18)`;
- cubrir toda la pantalla;
- dispararse solo en `RealityChanged`;
- no bloquear mouse ni teclado;
- no sustituir por animación de todos los interactables.

El pulse comunica el cambio de realidad; no debe usarse como efecto de carga
de room ni como sustituto del fade de transición.

---

## USS compatible con esta versión de UI Toolkit

Evitar propiedades no soportadas en este proyecto:

- no usar `inset`
- no usar `gap`

Usar:

- `left/top/right/bottom`
- spacing con `margin-right`

## Flujo de eventos usado

- `InventoryManager.ItemAdded`
- `InventoryManager.ItemRemoved`
- `RoomManager.RoomChanged`
- `RealityManager.RealityChanged`

## Verificación de integración

### 1) Eliminar duplicación SHIFT

En [Main.unity](../../../Assets/Scenes/Main.unity), en `HUDRoot`:

- **Delete el GameObject `ShiftPanel`** (hijo de HUDRoot)
  - `UIShiftButton.cs` y [ShiftUI.uxml](../../../Assets/UI/ShiftUI.uxml) son
    referencias históricas; no deben coexistir como un segundo SHIFT runtime
  - Ya no es necesario; todo está en el HUD centralizado

### 2) HUDRoot configuración final

En `HUDRoot`:

- confirmar `PanelRenderer` con:
  - `PanelSettings` asignado
  - `Source Asset` = [HUD.uxml](../../../Assets/UI/HUD.uxml)
- confirmar `HUDController` adjunto
- confirmar que el botón SHIFT es visible en pantalla (más pequeño que antes)

### 3) UIManager (opcional)

En el GO de `UIManager`:

- opcional: asignar referencia a `HUDController` en Inspector  
- si no, se auto-resuelve por `FindAnyObjectByType`

### 4) Verificación rápida en Play Mode

1. recoger `llave` → slot pasa a `full` + feedback
2. usar `puerta` → feedback + fondo cambia
3. transición `Lobby`/`Hallway` → overlay visible corto
4. SHIFT en `Hallway` → cambia realidad + pulse + botón pulsable
5. SHIFT fuera de `Hallway` → botón deshabilitado (gris) + no reacciona
6. `Linterna` en estado inicial → Sprite 0 visible en `HUD`
7. obtener la `Linterna` en `Bedroom` → Sprite 1 visible en `HUD`
8. entrar en `Basement` → toggle habilitado de `Linterna` y paso a Sprite 2 al encender
9. salir de room permitida → `Linterna` vuelve a estado apagado (Sprite 1) o se bloquea el toggle según regla de room

**El `shift-button` ahora es pequeño, pulsable y parte del `HUD` central.**

### 5) Configuración del Lantern icon en HUD

En el `HUD` centralizado, confirmar:

- `lantern-icon` existe como elemento visual de la capa superior del `HUD`
- se usa el `spritesheet-ui-icon-lantern-v1`
- el Sprite 0 se asigna al inicio del juego
- al recoger la `Linterna` en `Bedroom` se cambia a Sprite 1
- al activar la `Linterna` en room permitida se usa Sprite 2
- el icono se deja visible aunque no esté disponible para usar en la `sala` actual
- el botón SHIFT permanece debajo para preservar la jerarquía visual

## Criterio de done para Demo 1

El HUD está listo cuando:

- el flujo MVP completo se entiende sin leer consola,
- el estado de inventario se ve en pantalla,
- SHIFT está integrado en el HUD principal,
- no hay errores críticos de UI en consola.
