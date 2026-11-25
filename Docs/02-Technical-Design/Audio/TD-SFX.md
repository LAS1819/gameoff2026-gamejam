# TD-SFX - Efectos sonoros y cues

- **Versión:** 1.0
- **Creado:** 2026-11-25
- **Estado:** Planificado; clips pendientes

---

## 1. Objetivo

Crear una familia corta de efectos claros para:

- confirmar acciones;
- comunicar cambios de estado;
- reforzar los eventos críticos;
- mantener el prototipo legible sin saturación.

No programar un AudioSource por acción. `SFXManager` reproducirá los clips
mediante IDs semánticos.

---

## 2. Catálogo mínimo

| ID | Acción | Archivo sugerido |
| --- | --- | --- |
| `UI_CLICK` | clic principal de menú/hud | `sfx_ui_click.ogg` |
| `UI_DENIED` | Settings o acción bloqueada | `sfx_ui_denied.ogg` |
| `SHIFT` | cambio de realidad | `sfx_shift.ogg` |
| `ROOM_TRANSITION` | entrada/salida de Room | `sfx_room_transition.ogg` |
| `ITEM_PICKUP` | recoger llave/libro/item | `sfx_item_pickup.ogg` |
| `ITEM_USE` | usar item con éxito | `sfx_item_use.ogg` |
| `ITEM_USE_DENIED` | falta item o selección | `sfx_item_denied.ogg` |
| `DOOR_UNLOCKED` | puerta desbloqueada | `sfx_door_unlocked.ogg` |
| `MEMORY_DISCOVERY` | abrir una Memoria | `sfx_memory_discovery.ogg` |
| `LETTER_OPEN` | abrir Carta o Resumen | `sfx_letter_open.ogg` |
| `PRESSURE_VALVE` | reparar la válvula | `sfx_pressure_valve.ogg` |
| `STEAM_RELEASE` | activar vapor | `sfx_steam_release.ogg` |
| `RESTART` | reiniciar al Main Menu | `sfx_restart.ogg` |

Los archivos adicionales deben justificarse por una diferencia jugable real.

---

## 3. Variaciones

Para ahorrar producción:

- pickup puede variar con pitch `0.95-1.05`;
- UI puede compartir dos samples cortos;
- SHIFT puede usar una única señal con más duración;
- DOOR puede reutilizarse en varias puertas;
- RESTART puede ser una versión suave de ROOM_TRANSITION.

No duplicar pistas por room si solo cambia el contexto.

---

## 4. Producción en Sonic Pi

Formato recomendado:

- salida exportada: WAV;
- duración:
  - UI: `0.05-0.15 s`;
  - feedback: `0.15-0.30 s`;
  - transición/evento: `0.40-1.20 s`;
- mono;
- 44.1 kHz;
- sin cola larga salvo en SHIFT/transition;
- cola corta con fade natural.

Familia sonora:

- retro digital;
- tonos simples;
- ruido reducido;
- pitidos u osciladores breves;
- sin texturas largas que se parezcan a ambiente.

Ejemplos de diseño:

```text
UI_CLICK      = pulse corto + tick
SHIFT         = frecuencia ascendente/descendente breve
ITEM_PICKUP   = dos tonos cortos ascendentes
ITEM_USE      = tono estable + release
MEMORY        = campana suave o glass armonico
ROOM_TRANS     = ruido filtrado + fade
PRESSURE      = metal breve + hiss
```

---

## 5. Importación Unity

- `.ogg` preferido;
- Load Type: `Decompress On Load`;
- Preload Audio Data: `true`;
- Spatial Blend: `0`;
- Force To Mono: `true`;
- routing: grupo `SFX`.

Nombre de asset:

```text
sfx_<category>_<action>.ogg
```

Ejemplos:

```text
sfx_ui_click.ogg
sfx_shift.ogg
sfx_item_pickup.ogg
```

---

## 6. Disparadores

Prioridad de implementación:

1. UI_CLICK.
2. SHIFT.
3. ROOM_TRANSITION.
4. ITEM_PICKUP / ITEM_USE / ITEM_USE_DENIED.
5. MEMORY_DISCOVERY.
6. LETTER_OPEN.
7. DOOR_UNLOCKED.
8. PRESSURE_VALVE / STEAM_RELEASE.
9. RESTART.

El resto no debe bloquear el MVP.
