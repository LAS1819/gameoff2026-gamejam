# Room Main Menu — Technical Spec

- **Versión:** 1.0
- **Creado:** 2026-11-25
- **Estado:** Integrado en el flujo de RoomData

---

## 1. Decisión técnica

El Main Menu usa `RoomManager` y `InteractableBehaviour`. No se crea Canvas,
escena o Manager exclusivos para menú.

La jerarquía:

```text
Rooms
└── MainMenu
    ├── Visuals
    │   ├── Background
    │   ├── HorizonSign
    │   └── GameTitle
    └── Interactables
        ├── Start
        ├── Settings
        ├── Exit
        └── EntranceDoor
```

El nombre exacto `MainMenu` normaliza al id `main_menu`.

---

## 2. RoomData

```text
RoomData
- id: "main_menu"
- displayName: "Main Menu"
- connectedRoomIds: ["lobby"]
- interactables:
  - main_menu_start
  - main_menu_settings
  - main_menu_exit
  - main_menu_entrance
```

Los tres botones usan `InteractionType.Use`. La puerta usa `Transition` y
enlaza con `lobby`.

---

## 3. Flags

| Flag | Propósito |
| --- | --- |
| `MainMenuIntroStarted` | Oculta Start / Settings / Exit |
| `MainMenuEntryUnlocked` | Activa el cartel y permite clicar la puerta |

Ambas flags se reinician a `false` en `InitializeMVP()`.

---

## 4. Estados

### 4.1 Pre-intro

```text
MainMenuIntroStarted == false
MainMenuEntryUnlocked == false
```

Visibles:

- Start;
- Settings;
- Exit;
- título;
- puerta como parte del fondo, sin collider activo;
- cartel apagado.

### 4.2 Intro abierta

```text
MainMenuIntroStarted == true
MainMenuEntryUnlocked == false
```

- botones ocultos;
- título visible;
- Story Overlay en modo `Intro`;
- gameplay bloqueado por el overlay;
- puerta sin interacción.

### 4.3 Entrada disponible

```text
MainMenuIntroStarted == true
MainMenuEntryUnlocked == true
```

- overlay cerrado;
- cartel encendido;
- collider de puerta activo;
- clic izquierdo transiciona al Lobby.

---

## 5. Integración con Story Overlay

`StoryOverlayMode` incorpora `Intro`.

```text
ShowIntro(...)
```

La acción primaria:

- cierra el overlay;
- activa `MainMenuEntryUnlocked`;
- refresca interactables;
- devuelve controles de gameplay visibles.

El modo `Intro` usa dos páginas:

- página 1: `"After six months"` y dos párrafos de contexto;
- página 2: `"The invitation"` y el mensaje de HORIZON;
- botón 1: `Next`;
- botón 2: `Continue`;
- Enter, Space, Escape y clic en el botón avanzan o cierran según la página;
- `MainMenuEntryUnlocked` se activa solo al cerrar la segunda página.

No reutilizar el flujo de cierre de Letter, que conduce al Ending.

---

## 6. Resolución especial

`InteractionSpecialCases` resuelve:

- `main_menu_start`: flag `MainMenuIntroStarted`, abre Intro;
- `main_menu_settings`: feedback; no cambia estado;
- `main_menu_exit`: `Application.Quit()` solo en standalone;
- `main_menu_entrance`: transición a Lobby.

`MainMenuController` es solo un puente visual ligero:

- escucha `GameManager.FlagChanged`;
- escucha `RoomManager.RoomChanged`;
- muestra/oculta botones;
- cambia el sprite del cartel;
- activa/desactiva el collider de la puerta.

No contiene historia, routing de room ni flags de gameplay.

Los botones `main_menu_start`, `main_menu_settings` y `main_menu_exit`
implementan feedback local en `InteractableBehaviour`:

- hover: tint crema cálido;
- hover pulsado: escala `0.94`;
- reposo: escala original y tint blanco;
- el collider y el hotspot no cambian de tamaño;
- al salir del room o ocultarse el interactable, se restablece el estado visual.

El Restart se resuelve en `HUDStoryOverlayView.Restart()`:

- ejecuta `GameManager.InitializeMVP()` para limpiar flags e inventario;
- carga `MainMenuRoomId`;
- fuerza la realidad Normal;
- deja el menu con sus botones y el HORIZON apagado.

---

## 7. HUD

En `main_menu`, el HUD de gameplay se oculta:

- Inventory;
- feedback;
- SHIFT;
- Lantern.

El feedback no puede ocultarse si Settings o Exit necesitan informar al jugador.
Por ello `HUDController` debe distinguir:

- overlay modal;
- modo menu;
- modo gameplay.

Implementación mínima:

- los controles de gameplay se mantienen ocultos;
- el feedback se conserva visible en Main Menu para Settings y Exit.

---

## 8. Assets que debe crear la parte gráfica

Rutas:

```text
Assets/Visuals/UI/MainMenu/
├── sprite-main-menu-background.png
├── sprite-main-menu-title.png
├── sprite-main-menu-start.png
├── sprite-main-menu-settings.png
├── sprite-main-menu-exit.png
├── sprite-main-menu-horizon-off.png
└── sprite-main-menu-horizon-on.png
```

Estados `HORIZON`:

```text
0 = `sprite-main-menu-horizon-off.png`
1 = `sprite-main-menu-horizon-on.png`
```

### Resolución

- background completo: `640 x 360`;
- title: ajustado al ancho visual real, recomendado hasta `240 x 96`;
- botones: mismo ancho y altura entre ellos, recomendado `128 x 32`;
- sign: recomendado `144 x 32`, dos frames.

Import:

- Texture Type = `Sprite (2D and UI)`;
- Filter Mode = `Point`;
- PPU según el pipeline de room, preferiblemente `16`;
- mipmaps desactivados;
- Compression = `None` durante implementación.

No mover hotspots después de integrar el arte final. Ajustar solo SpriteRenderer
y collider existentes.

---

## 9. Escala y reemplazo

Los assets provisionales pueden usar rectángulos y texto temporal. Cada
GameObject ya queda preparado con:

- `SpriteRenderer`;
- `InteractableBehaviour`;
- `BoxCollider2D` cuando corresponde.

La entrega artística sustituye el `sprite` sin crear otra jerarquía.

---

## 10. Validación

- iniciar en build abre Main Menu;
- Editor puede seguir usando `debugStartRoomId`;
- Start abre Intro;
- Use sobre Intro cerrado no duplica overlays;
- Continue enciende HORIZON y habilita puerta;
- EntranceDoor carga Lobby;
- Lobby comienza limpio y Normal;
- Restart desde Ending vuelve a Main Menu;
- Settings no bloquea el menú;
- Exit no finge cerrar WebGL;
- no aparecen Inventory, SHIFT ni Lantern en Main Menu;
- feedback de Settings/Exit sigue legible en Main Menu.
