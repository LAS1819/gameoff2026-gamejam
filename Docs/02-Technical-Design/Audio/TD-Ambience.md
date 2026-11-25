# TD-Ambience - Ambientes y mezclas por Room

- **Versión:** 1.0
- **Creado:** 2026-11-25
- **Estado:** Planificado; sin clips integrados

---

## 1. Objetivo

Dar identidad sonora a las Rooms sin saturar la música.

El ambiente debe:

- reforzar el edificio;
- cambiar suavemente por realidad;
- evitar loops evidentes;
- mantenerse bajo los SFX y la voz narrativa.

---

## 2. Estrategia MVP

Usar solo cuatro bases:

| Clip | Uso |
| --- | --- |
| `amb_main_building_normal.ogg` | base de Lobby/Hallway/Bedroom/Library |
| `amb_main_building_otherside.ogg` | base de realidad alternativa |
| `amb_basement_pipes.ogg` | Basement |
| `amb_forest_wind.ogg` | Forest |

Horizon/Main Menu puede reutilizar `amb_main_building_normal` con menor volumen.

No crear un clip distinto por habitación hasta que existan diferencias visuales
y narrativas que lo justifiquen.

---

## 3. Tiempos

- fade in room: `1-2 s`;
- fade out room: `1-3 s`;
- fade reality: `0.5-1.5 s`;
- loop continuo;
- no repetir con cambios menores de estado.

---

## 4. Mezcla

- ambiente: `-12 dB` inicial;
- realidad alternativa: hasta `-9 dB`, pero sin superar música;
- Basement: puede subir brevemente cuando el vapor se activa;
- Forest: ambiente más amplio y silencioso.

El sistema debe evitar reducir el tema principal a valores extremos.

---

## 5. Producción en Sonic Pi

Características:

- drone lento;
- ruido rosa o blanco filtrado;
- HVAC/ventilación;
- pequeñas gotas;
- presión de tuberías;
- viento amplio para Forest;
- textura continua sin ritmo marcado.

Formato:

- OGG o WAV;
- 44.1 kHz;
- loop de `30-90 s`;
- fade de entrada/salida embebido;
- sin cambios bruscos de fase.

---

## 6. Importación

- Load Type: `Streaming`;
- Preload: `false`;
- 2D;
- loop: `true`;
- routing: `Ambient`.

---

## 7. Integración

`AmbientManager` debe reaccionar solo a:

- `RoomChanged`;
- `RealityChanged`;
- reset/restart;
- Ending o cierre opcional.

La Room determina el clip base y la realidad determina volumen/pitch o mezcla.

No crear AudioSources en cada escena. Usar una fuente reutilizable.
