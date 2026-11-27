# TD-SFX - Efectos sonoros y cues

- **Versión:** 1.0
- **Creado:** 2026-11-25
- **Estado:** Implementado en runtime; clips y mixer integrados

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
| `UI_CLICK` | clic principal de menú/hud | `sfx-interface-ui-click-v1.ogg` |
| `UI_DENIED` | bloqueo, faltan requisitos o Settings | `sfx-interface-ui-denied-v1.ogg` |
| `SHIFT` | cambio entre Normal y Other Side | `sfx-navigation-shift-v1.ogg` |
| `ROOM_TRANSITION` | carga de otra Room | `sfx-navigation-room-transition-v1.ogg` |
| `ITEM_PICKUP` | recoger llave, libro u otro item | `sfx-inventory-item-pickup-v1.ogg` |
| `ITEM_USE` | usar o instalar un item | `sfx-inventory-item-use-v1.ogg` |
| `DOOR_ACTION` | abrir, desbloquear o cruzar una puerta | `sfx-navigation-door-action-v1.ogg` |
| `LIGHT_SWITCH` | encender/apagar la lámpara de Bedroom | `sfx-inventory-light-switch-v1.ogg` |
| `MEMORY_DISCOVERY` | abrir una Memoria | `sfx-world-events-memory-discover-v1.ogg` |
| `STORY_OPEN` | abrir Intro, Carta o Resumen Final | `sfx-interface-ui-story-open-v1.ogg` |
| `SHELF_SECRET_ACTION` | activar o retirar el secret shelf | `sfx-world-events-shelf-secret-action-v1.ogg` |
| `PRESSURE_VALVE_ACTION` | reparar la válvula | `sfx-world-events-pressure-valve-action-v1.ogg` |
| `STEAM_LOOP` | sonido continuo de vapor | `sfx-world-events-steam-loop-v1.ogg` |

Los archivos adicionales deben justificarse por una diferencia jugable real.

Los nombres reales implementados incluyen prefijos por familia:

```text
sfx-interface-ui-click-v1.ogg
sfx-interface-ui-denied-v1.ogg
sfx-interface-ui-story-open-v1.ogg
sfx-navigation-shift-v1.ogg
sfx-navigation-room-transition-v1.ogg
sfx-navigation-door-action-v1.ogg
sfx-inventory-item-pickup-v1.ogg
sfx-inventory-item-use-v1.ogg
sfx-inventory-light-switch-v1.ogg
sfx-world-events-memory-discover-v1.ogg
sfx-world-events-shelf-secret-action-v1.ogg
sfx-world-events-pressure-valve-action-v1.ogg
sfx-world-events-steam-loop-v1.ogg
```

No son necesarias pistas separadas para:

- reinicio: reutiliza `ROOM_TRANSITION`;
- vapores: pueden compartir `PRESSURE_VALVE_ACTION` con pitch/volume;
- válvula: `STEAM_LOOP` cubre el sonido continuo; `PRESSURE_VALVE_ACTION` cubre la interacción inicial;
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

- [x] **WAV RAW de Sonic Pi**
- [x] **Edición Audacity y compresión a OGG**

### `sfx_navigation.rb`

- `SHIFT`
- `ROOM_TRANSITION`
- `DOOR_ACTION`

- [x] **WAV RAW de Sonic Pi**
- [x] **Edición Audacity y compresión a OGG**

### `sfx_inventory.rb`

- `ITEM_PICKUP`
- `ITEM_USE`
- `LIGHT_SWITCH`

- [x] **WAV RAW de Sonic Pi**
- [x] **Edición Audacity y compresión a OGG**

### `sfx_world_events.rb`

- `MEMORY_DISCOVERY`
- `SHELF_SECRET_ACTION`
- `PRESSURE_VALVE_ACTION`
- `STEAM_LOOP`

- [x] **WAV RAW de Sonic Pi**
- [x] **Edición Audacity y compresión a OGG**

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

Configuración vigente:

- `AudioSource` one-shot dedicado a cues cortos;
- `AudioSource` separado para `STEAM_LOOP`;
- volumen raíz configurable desde `SFXManager.SetRootVolume(...)`;
- volumen individual serializable por cada clip;
- fuente 2D y `playOnAwake = false`;
- loop activo solo en la fuente de vapor.

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
12. STEAM_LOOP.

El resto no debe bloquear el MVP.

---

## 8. Validación de SFX

### Interface

- [ ] `UI_CLICK`: debe reproducirse al hacer clic sobre un elemento interactuable.
  - Aquellos elementos que se puedan clicar deben disparar este sonido.
  - Ya sea Usar o Examinar.
  - Pruebas:
    - [x] Botón Start: funciona correctamente el Clip.
    - [ ] Botón de Ventanas Modales: no se escucha el Clip. Debería sonar cada vez que se pulse un Botón, ya sea Next o Continuar.
- [ ] `UI_DENIED`: debe reproducirse al intentar una acción no permitida.
  - Cuando se intenta usar un Item en un lugar o situación no permitida.
  - Cuando se intenta usar un Botón no permitido.
  - Pruebas:
    - [ ] Botón "Settings": se escucha el Clip, pero no sé si se le ha añadido algún efecto porque no suena igual (parece).
    - [ ] Uso de Item Incorrecto: debería sonar el Clip cada vez que se intenta usar un Item en un lugar o situación no permitida.
- [ ] `UI_STORY_OPEN`: debe reproducirse al abrir la historia o sección narrativa.
  - [ ] Cuando se abre la historia desde el menú o interfaz correspondiente.
  - Pruebas:
    - [x] Ventana de Historia Inicial: debe sonar el Clip al abrir la historia desde el menú.
    - [ ] Ventana de Memoria: debe sonar el Clip al abrir la ventana de Memoria (cuando es examinado el Interactable correspondiente)
    - [x] Ventana Carta Revelación: debe sonar el Clip al abrir la ventana de Carta Revelación (cuando es examinado el Interactable correspondiente)

### Navigation

- [x] `SHIFT`: debe sonar siempre que se haga un cambio de Realidad.
- [ ] `ROOM_TRANSITION` debe reproducirse al cambiar de Room o al avanzar entre niveles (Room Forest)
  - Pruebas:
    - [x] Transición entre Rooms: debe sonar el Clip al realizar una transición entre Rooms.
    - [ ] Avance entre Niveles: debe sonar el Clip al avanzar entre niveles (Room Forest).
- [ ] `DOOR_ACTION`: deber reproducirse al interactuar con una puerta, ya sea abrirla o cerrarla.
  - Todas las Rooms se conectan mediant puertas, así que debería sonar que se transiciona entre ellas también.
  - Pruebas:
    - [ ] Interacción con Puerta: debe sonar el Clip al interactuar con una puerta, ya sea abrirla o cerrarla. Aunque a veces se use un interactable que no es una puerta, como por ejemplo para ir del Hallway de vuelta al Lobby, debería sonar también. Aunque sea un "botón", el paso debe ser a través de la puerta. En un futuro deberíamos marcar cada room si se conecta con una puerta o no.

### Inventory

- [ ] `ITEM_PICKUP`: debe reproducirse al recoger un objeto del inventario. Es decir, cuando un Item es añadido al inventario.
  - Pruebas:
    - [ ] Recoger Item: debe sonar el Clip al recoger un objeto del inventario.
- [ ] `ITEM_USE`: debe reproducirse al usar un objeto del inventario sobre un elemento del mundo o en una situación específica. Siempre que la acción sea válida.
  - Pruebas:
    - [ ] Uso de Item Correcto: debe sonar el Clip al usar un objeto del inventario en una situación válida.
- [ ] `LIGHT_SWITCH`: creo que se hizo para la Linterna. De momento no tiene uso, pero se prevé añadir un sistema simple de Luz Global para obligar a usarla cuando se está en el Basement.
  - Pruebas:
    - [ ] Encender/Apagar Linterna: debe sonar el Clip al encender o apagar la linterna.

### World Events

- [ ] `MEMORY_DISCOVERY`: debe reproducirse al descubrir una Memoria. Siempre que se muestre la ventana de Memorias
  - Pruebas:
    - [ ] Descubrimiento de Memoria: debe sonar el Clip al descubrir una Memoria.
- [ ] `SHELF_SECRET_ACTION`: debe reproducirse al interactuar con un estante secreto. Es decir, al activar un mecanismo oculto en un estante.
  - Pruebas:
    - [ ] Interacción con Estante Secreto: debe sonar el Clip al interactuar con un estante secreto.
- [ ] `PRESSURE_VALVE_ACTION`: debe reproducirse al interactuar con una válvula de presión. Es decir, al activar o ajustar una válvula de presión. En el Basement, en la Realidad Normal, cuando se inserta la Válvula en el mecanismo correspondiente.
  - Pruebas:
    - [ ] Interacción con Válvula de Presión: debe sonar el Clip al interactuar con una válvula de presión.
- [x] `STEAM_LOOP`: debe reproducirse de manera continua mientras el sistema de vapor esté activo. Es decir, cuando la válvula de vapor esté abierta y el mecanismo correspondiente esté en funcionamiento. En el Otro Mundo.
  - Pruebas:
    - [x] Sistema de Vapor Activo: debe sonar el Clip mientras el sistema de vapor esté activo.
