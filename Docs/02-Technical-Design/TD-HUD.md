# TD-HUD - HUD mínimo runtime (Sprint 1)

- **Versión:** 2.0
- **Creado:** 2026-11-06
- **Actualizado:** 2026-11-07
- **Estado:** Implementación de código completada, pendiente wiring manual en escena

## Objetivo

Tener un HUD mínimo, estable y usable para la demo del sprint 1 sin crear una segunda arquitectura de UI.

Incluye:

- slots de inventario,
- feedback textual temporal,
- botón SHIFT integrado en el mismo panel,
- pulse visual al cambiar de realidad,
- fade corto al cambiar de room.

## Decisión técnica

Para runtime usamos **PanelRenderer + UXML + USS** (no `UIDocument`).

Patrón:

- `PanelRenderer` en un GO raíz (`HUDRoot`)
- binding por `RegisterUIReloadCallback`
- actualización por eventos de managers (sin `Update` para la UI)

## Estado de implementación real

### Archivos de UI

- [HUD.uxml](E:/repos/unity/GameOff2026-GameJam/Assets/UI/HUD.uxml)
- [HUD-style-sheet.uss](E:/repos/unity/GameOff2026-GameJam/Assets/UI/HUD-style-sheet.uss)

### Scripts

- [HUDController.cs](E:/repos/unity/GameOff2026-GameJam/Assets/Scripts/UI/HUDController.cs) ✅ nuevo
- [UIManager.cs](E:/repos/unity/GameOff2026-GameJam/Assets/Scripts/UIManager.cs) ✅ actualizado (bridge al HUD)
- [InteractionManager.cs](E:/repos/unity/GameOff2026-GameJam/Assets/Scripts/InteractionManager.cs) ✅ actualizado (feedback runtime)

### Compatibilidad

- [UIShiftButton.cs](E:/repos/unity/GameOff2026-GameJam/Assets/Scripts/UI/UIShiftButton.cs) se mantiene como referencia histórica.
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
    ├── shift-button
    ├── transition-overlay
    └── reality-pulse
```

## Comportamiento implementado

### Inventario

- Se refresca al recibir `InventoryManager.ItemAdded` / `ItemRemoved`.
- Slots alternan clases `empty` / `full`.

### Feedback

- `UIManager.ShowFeedback(...)` delega en `HUDController`.
- Mensajes temporales con auto-hide.
- `InteractionManager` envía feedback en pickup/use/examine y errores de gating.

### SHIFT

- Click sobre `shift-button` del HUD ejecuta `RealityManager.ToggleReality()`.
- Respeta gating por room (por defecto `hallway`).
- Actualiza clases visuales `normal` / `other-side`.
- Marca flag `ShiftPerformed`.

### Transiciones visuales

- `transition-overlay`: flash corto en `RoomChanged`.
- `reality-pulse`: flash corto en `RealityChanged`.

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

## Parte manual pendiente (Unity Editor)

### 1) Eliminar duplicación SHIFT

En [Main.unity](E:/repos/unity/GameOff2026-GameJam/Assets/Scenes/Main.unity), en `HUDRoot`:

- **Delete el GameObject `ShiftPanel`** (hijo de HUDRoot)
  - Es el que tiene `UIShiftButton.cs` y su propio `PanelRenderer` con [ShiftUI.uxml](E:/repos/unity/GameOff2026-GameJam/Assets/UI/ShiftUI.uxml)
  - Ya no es necesario; todo está en el HUD centralizado

### 2) HUDRoot configuración final

En `HUDRoot`:

- confirmar `PanelRenderer` con:
  - `PanelSettings` asignado
  - `Source Asset` = [HUD.uxml](E:/repos/unity/GameOff2026-GameJam/Assets/UI/HUD.uxml)
- confirmar `HUDController` adjunto
- confirmar que el botón SHIFT es visible en pantalla (más pequeño que antes)

### 3) UIManager (opcional)

En el GO de `UIManager`:

- opcional: asignar referencia a `HUDController` en Inspector  
- si no, se auto-resuelve por `FindAnyObjectByType`

### 4) Verificación rápida en Play Mode

1. recoger llave → slot pasa a `full` + feedback
2. usar puerta → feedback + fondo cambia
3. transición Lobby/Hallway → overlay visible corto
4. SHIFT en Hallway → cambia realidad + pulse + botón pulsable
5. SHIFT fuera de Hallway → botón deshabilitado (gris) + no reacciona

**El shift-button ahora es pequeño, pulsable y parte del HUD central.**

## Criterio de done para Demo 1

El HUD está listo cuando:

- el flujo MVP completo se entiende sin leer consola,
- el estado de inventario se ve en pantalla,
- SHIFT está integrado en el HUD principal,
- no hay errores críticos de UI en consola.
