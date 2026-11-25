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
| `UI_DENIED` | bloqueo, faltan requisitos o Settings | `sfx_ui_denied.ogg` |
| `SHIFT` | cambio entre Normal y Other Side | `sfx_shift.ogg` |
| `ROOM_TRANSITION` | carga de otra Room | `sfx_room_transition.ogg` |
| `ITEM_PICKUP` | recoger llave, libro u otro item | `sfx_item_pickup.ogg` |
| `ITEM_USE` | usar o instalar un item | `sfx_item_use.ogg` |
| `DOOR_ACTION` | abrir, desbloquear o cruzar una puerta | `sfx_door_action.ogg` |
| `LIGHT_SWITCH` | encender/apagar la lámpara de Bedroom | `sfx_light_switch.ogg` |
| `MEMORY_DISCOVERY` | abrir una Memoria | `sfx_memory_discovery.ogg` |
| `STORY_OPEN` | abrir Intro, Carta o Resumen Final | `sfx_story_open.ogg` |
| `SHELF_SECRET_ACTION` | activar o retirar el secret shelf | `sfx_shelf_secret_action.ogg` |
| `PRESSURE_VALVE_ACTION` | reparar la válvula | `sfx_pressure_valve_action.ogg` |

Los archivos adicionales deben justificarse por una diferencia jugable real.

No son necesarias pistas separadas para:

- reinicio: reutiliza `ROOM_TRANSITION`;
- vapores: pueden compartir `PRESSURE_VALVE_ACTION` con pitch/volume;
- ordenador/armario: pueden usar `ITEM_USE`, `DOOR_ACTION` o `UI_CLICK`;
- abrir el Inventario: no existe transición propia en el diseño actual;
- cartas individuales: reutilizan `STORY_OPEN`;
- tipos de puerta: reutilizan `DOOR_ACTION`.

---

## 3. Variaciones

Para ahorrar producción:

- pickup puede variar con pitch `0.95-1.05`;
- SHIFT conserva una única identidad sonora;
- DOOR_ACTION sirve para todas las puertas;
- ITEM_USE sirve para instalar la manivela, insertar el disk y usar llaves;
- STORY_OPEN sirve para Intro, Letter y Ending;
- ROOM_TRANSITION también cubre Restart;
- UI_DENIED cubre Settings, item incorrecto y acciones bloqueadas.

No duplicar pistas por room si solo cambia el contexto.

---

## 4. Scripts Sonic Pi

Agrupar por familia para mantener un proyecto limpio:

```text
Works/sonic-pi/sfx/
├── sfx_interface.rb
├── sfx_navigation.rb
├── sfx_inventory.rb
└── sfx_world_events.rb
```

Los WAV de grabación y los OGG finales se guardan fuera de los scripts,
preferiblemente bajo `Works/sonic-pi/exports/WAV/` y `Assets/Audio/SFX/`
respectivamente.

### `sfx_interface.rb`

- `UI_CLICK`
- `UI_DENIED`
- `UI_STORY_OPEN`

### `sfx_navigation.rb`

- `SHIFT`
- `ROOM_TRANSITION`
- `DOOR_ACTION`

### `sfx_inventory.rb`

- `ITEM_PICKUP`
- `ITEM_USE`
- `LIGHT_SWITCH`

### `sfx_world_events.rb`

- `MEMORY_DISCOVERY`
- `SHELF_SECRET_ACTION`
- `PRESSURE_VALVE_ACTION`

Actualmente los cuatro scripts son autocontenidos. Un helper compartido solo
se añadirá si aparecen patrones repetidos suficientes para justificarlo.

---

## 5. Producción en Sonic Pi

- todos los scripts usan `use_bpm 80`;
- amplitud base de interfaz, navegación, inventario y eventos: `1.25`;
- cada script incluye live loops individuales con `stop` y uno `SFX TEST ALL`;
- ejecutar un archivo a la vez para evitar solapes de pruebas.

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
UI_CLICK           = pulse corto + tick
SHIFT              = barrido breve ascendente/descendente
ITEM_PICKUP        = dos tonos cortos ascendentes
ITEM_USE           = tono estable + release
MEMORY_DISCOVERY   = campana suave o armónico de cristal
ROOM_TRANSITION    = ruido filtrado + fade
SHELF_SECRET       = madera/mecha mecánica breve
PRESSURE_VALVE     = metal breve + hiss
```

---

## 6. Importación Unity

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

## 7. Disparadores

Prioridad de implementación:

1. UI_CLICK.
2. SHIFT.
3. ROOM_TRANSITION.
4. ITEM_PICKUP / ITEM_USE.
5. MEMORY_DISCOVERY.
6. STORY_OPEN.
7. DOOR_ACTION.
8. UI_DENIED.
9. LIGHT_SWITCH.
10. SHELF_SECRET_ACTION.
11. PRESSURE_VALVE_ACTION.

El resto no debe bloquear el MVP.
