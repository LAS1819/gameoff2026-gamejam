# TD-Audio-System - Audio runtime y mezcla

- **Versión:** 2.0
- **Creado:** 2026-11-23
- **Actualizado:** 2026-11-25
- **Estado:** Base integrada; SFX implementados; ambientes pendientes

---

## 1. Estado actual

El proyecto ya dispone de:

- `AudioManager` como entrada pública.
- `MusicManager` para la música principal.
- `AudioMixer` `MainAudioMixer`.
- grupos `Master`, `Music`, `Ambient` y `SFX`.
- un clip musical: `father-theme-progressive-v1.mp3`.

Los 13 SFX están integrados en el runtime y el loop de vapor está preparado.
No hay todavía clips ambientales en la escena. El Mixer ya contiene todos los
buses previstos por el Bloque 4.

---

## 2. Responsabilidades

### AudioManager

- punto de entrada para música, SFX y ambiente;
- subscribe a eventos globales;
- evita que gameplay, Rooms o UI accedan a `AudioSource`.

### MusicManager

- mantiene el tema activo;
- usa el grupo `Music`;
- loop por defecto;
- volumen y pitch diferentes según Normal / Other Side.

### SFXManager

- componente futuro mínimo para cues cortos;
- busca o reutiliza uno o varios AudioSources;
- reproduce clips por ID;
- debe enrutar a `SFX`.

### AmbientManager

- componente futuro mínimo;
- mantiene una fuente ambiental base;
- cambia por Room y Reality con fade corto;
- debe enrutar a `Ambient`.

---

## 3. Eventos

La implementación debe observar sistemas existentes:

| Evento | Uso |
| --- | --- |
| `RoomManager.RoomChanged` | ambiente y transición |
| `RealityManager.RealityChanged` | mezcla music/ambience |
| `InteractionManager.InteractionResolved` | pickup, use, door y puzzle |
| `InventoryManager.ItemAdded/Removed` | pickup |
| `HUDMemoryDiscoveryView` | cue de descubrimiento |
| `HUDStoryOverlayView` | carta, Intro y Ending |

No es necesario añadir eventos nuevos para los cues básicos.

---

## 4. Arquitectura mínima

```text
GameManager
└── AudioManager
    ├── MusicManager
    ├── SFXManager
    └── AmbientManager
```

No crear un Singleton adicional. `AudioManager.Instance` ya existe y permanece
como única fachada pública.

Cada efecto se dispara con un ID:

```text
UI_CLICK
SHIFT
ROOM_TRANSITION
ITEM_PICKUP
ITEM_USE
UI_DENIED
MEMORY_DISCOVERY
STORY_OPEN
PRESSURE_VALVE
```

No usar nombres de archivo como API.

---

## 5. Audio Mixer

Jerarquía verificada:

```text
Master
├── Music
├── Ambient
└── SFX
```

La inspección actual confirma que los cuatro grupos existen y están bajo
`Master`. No hace falta crear grupos nuevos en el mixer del MVP.

La implementación futura puede exponer:

- `MasterVolume`;
- `MusicVolume`;
- `AmbientVolume`;
- `SFXVolume`.

Los parámetros del mixer no requieren cambios hasta que exista Settings real.

---

## 6. Importación

### Música

- MP3/OGG;
- Load Type: `Streaming`;
- Preload: `false`;
- 2D;
- loop durante reproducción normal.

### Ambiente

- WAV/OGG;
- loop sin corte audible;
- Load Type: `Streaming` o `Decompress On Load`;
- 2D.

### SFX

- OGG preferido;
- Load Type: `Decompress On Load`;
- Preload: `true`;
- mono;
- Spatial Blend: `0`.

---

## 7. Mezcla base

Valores iniciales recomendados:

| Canal | Nivel inicial |
| --- | --- |
| Master | 0 dB |
| Music | -6 dB |
| Ambient | -12 dB |
| SFX | -3 dB |

Usar los clips y probar a WebGL `640 x 360`. Ajustar por percepción, no por
preferencias arbitrarias.

---

## 8. Prioridad del Bloque 4

Primero:

1. tema musical ya integrado;
2. clic y confirmación de UI;
3. SHIFT y transición;
4. pickup/use;
5. memoria y carta;
6. ambiente base;
7. vale de presión y pulido especial.

Todo lo demás se pospone si aumenta mucho el alcance.

Documentación relacionada:

- [GD-Audio.md](../../01-Game-Design/Audio/GD-Audio.md)
- [TD-SFX.md](TD-SFX.md)
- [TD-Music.md](TD-Music.md)
- [TD-Ambience.md](TD-Ambience.md)
