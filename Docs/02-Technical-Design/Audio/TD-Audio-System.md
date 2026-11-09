# Audio System Technical Design

- **Autor:** @LAS1819
- **Versión:** 1.0
- **Fecha:** 2026-11-09
- **Actualización:** 2026-11-09

## Resumen

El sistema de audio se encarga de gestionar la reproducción de música, ambientes y efectos sonoros dentro del juego.

Su objetivo principal es reforzar la atmósfera, comunicar cambios entre realidades y acompañar la progresión emocional de la historia, especialmente los temas de memoria, pérdida, negación y aceptación.

El audio constituye uno de los pilares fundamentales de la experiencia y complementa las limitaciones visuales inherentes a una producción retro de pequeño alcance.

---

## Componentes del Sistema de Audio

### Audio Manager (AudioManager)

Responsable de la gestión global del sistema.

Funciones:

- Reproducción de música.
- Reproducción de ambientes.
- Reproducción de efectos de sonido.
- Control de volumen.
- Gestión de transiciones.
- Comunicación con otros sistemas.

### Music Manager (MusicManager)

Responsable de la reproducción musical.

Funciones:

- Reproducir temas musicales.
- Gestionar transiciones entre versiones.
- Gestionar cambios entre Realidad Normal y El Otro Lado.
- Controlar volumen y mezcla.

### Ambient Manager (AmbientManager)

Responsable de los sonidos ambientales.

Funciones:

- Reproducción de ambientes por sala.
- Transiciones ambientales.
- Variaciones según la realidad activa.
- Capas ambientales dinámicas.

### SFX Manager (SFXManager)

Responsable de los efectos sonoros.

Funciones:

- Interacciones.
- Inventario.
- Eventos.
- Sistema SHIFT.
- Feedback del jugador.

---

## Plan de implementación compatible con el sistema de sistemas actual

El proyecto actual no está construido sobre ECS ni sobre un framework de audio pesado; su base es un conjunto de `MonoBehaviour` de runtime gestionados desde `GameManager` y levantados con `EnsureRuntimeSystems()`. Por eso, la forma más sencilla y mantenible de integrar el audio es mantener esa misma lógica: el audio debe ser otro sistema gestionado, no un sistema autónomo con acceso directo a todo el resto del juego.

### 1. Arquitectura base propuesta

- `GameManager` sigue siendo el root del sistema de sistemas.
- `GameManager.EnsureRuntimeSystems()` debe crear también `AudioManager`.
- `AudioManager` será el punto de entrada público del sistema.
- `MusicManager`, `AmbientManager` y `SFXManager` se implementarán como subcomponentes dentro del mismo objeto de audio o como hijos del mismo GameObject.
- Cada uno mantendrá una API simple y específica:
  - `PlayMusic(string cueId)`
  - `SetAmbient(string roomId, RealityType reality)`
  - `PlaySfx(string cueId, Vector3? position = null)`
  - `SetMasterVolume(float value)`
  - `SetRealityMix(RealityType reality)`

Esto permite que el audio quede aislado como un subsistema y que el resto del juego solo conozca una capa de alto nivel, sin depender de `AudioSource`, clips o mezclas internas.

### 2. Cómo se comunica con el resto del juego

La integración debe hacerse por eventos y por lectura de estado público, no por referencias directas a objetos de escena.

Puntos de integración recomendados:

- `RoomManager.RoomChanged`: cambia el ambiente asociado a la sala activa.
- `RealityManager.RealityChanged`: aplica mezcla, filtros o capas adicionales de música/ambiente.
- `GameManager.FlagChanged`: dispara cues narrativas y SFX condicionados por eventos del juego.
- `InteractionManager`: emite SFX de interacción, pickup, uso y transición.
- `EventManager`: centraliza eventos globales de narrativa, estado de historia o transiciones.

El patrón ideal es:

- Los sistemas de gameplay no llaman directamente a `AudioSource`.
- Los sistemas de gameplay solo invocan `AudioManager.PlaySfx(...)`, `AudioManager.PlayMusic(...)` o disparan un evento de nombre concreto.
- El `AudioManager` es el único responsable de decidir qué clip, qué mezcla y qué fade aplicar.

### 3. Aislamiento del sistema

Para evitar acoplamientos innecesarios, el audio debe seguir estas reglas:

- No se debe depender de `FindObjectOfType` ni referencias manuales a objetos del gameplay desde el audio.
- No se deben lanzar llamadas desde la UI directa a `AudioSource` o a `MusicManager` concreto.
- El audio no debe conocer la estructura interna de habitaciones, inventario y lógica de puzzles.
- Las decisiones de audio se basarán en eventos, flags y estado de realidad, no en reglas ad hoc por escena.

La forma más simple de mantener esto es una capa de servicio:

- `AudioManager` = fachada pública.
- `MusicManager`, `AmbientManager`, `SFXManager` = lógica interna.
- `AudioCue` / `AudioData` = datos serializables del contenido sonoro.

De este modo, si en el futuro se añade una escena nueva o un nuevo sistema, solo hace falta publicar un evento y dejar que `AudioManager` resuelva qué reproducción corresponde.

### 4. Capa de datos recomendada

La solución más simple y reutilizable para el proyecto es usar datos descriptivos en lugar de código específico por elemento:

- `AudioCue` (ScriptableObject) con:
  - `clip`
  - `volume`
  - `pitch`
  - `loop`
  - `fadeIn`
  - `fadeOut`
  - `priority`
  - `realityMask`
- `AudioRoomProfile` con:
  - `normalAmbient`
  - `otherSideAmbient`
  - `musicLayer`
- `AudioMixerGroup` para:
  - `Master`
  - `Music`
  - `Ambient`
  - `SFX`

Esto permite reutilizar contenido entre realidades sin duplicar lógica y encaja muy bien con la filosofía del documento: "mismo material, mezclas y procesamiento distintos".

### 5. Patrón de flujo recomendado

Flujo mínimo para MVP:

1. `GameManager` crea `AudioManager`.
2. `AudioManager` suscribe la escucha de eventos de juego y realidad.
3. Cuando `RealityManager` cambia de estado, `AudioManager` dispara `SetRealityMix(RealityType)`.
4. Cuando `RoomManager` cambia de sala, `AudioManager` activa el ambiente correcto.
5. Cuando un interactable o evento dispara una acción, se llama al `SFXManager` con una cue de audio.
6. El `MusicManager` mantiene una pista base con transiciones suaves y una segunda variante para la otra realidad.

Esto mantiene la lógica de audio centralizada, hace que el sistema sea fácil de depurar y evita mezclar responsabilidades con los managers de gameplay.

### 6. Recomendación final para este proyecto

La implementación más sensata para una game jam / proyecto pequeño es:

- Mantener el patrón actual de managers.
- Añadir `AudioManager` como un sistema más del runtime.
- No crear una red de dependencia compleja ni un sistema de audio independiente del resto del juego.
- Usar eventos para la comunicación.
- Usar `AudioMixer` y `ScriptableObject` para contenido reutilizable.
- Mantener la música y el ambiente como capas modales, y los SFX como eventos transitorios.

En otras palabras: el audio debe comportarse como un sistema de servicio del juego, no como un actor independiente con lógica de mundo. Eso lo hace más fácil de probar, más simple de ampliar y más coherente con la arquitectura existente del proyecto.

### Refinamientos adicionales recomendados

Además de la estructura base, conviene dejar definidos unos puntos operativos para que el sistema no se desmadre en la implementación:

- Ciclo de vida del sistema: `Awake` para singleton y persistencia, `OnEnable` para suscribirse a eventos, `OnDisable` para desuscribirse y evitar fugas de suscriptores, `Start` para inicializar mixers, estados por defecto y ambiente base.
- Canales de audio por responsabilidad: una fuente principal para música, una para ambiente y varias de SFX o un pool de SFX para evitar saturar la escena con `AudioSource` repetidos.
- Mapeo por ID: cada cue debe identificarse por nombre estable (`music_main_theme`, `ambient_hallway_normal`, `sfx_key_pickup`) para no depender de referencias arbitrarias en código.
- Transiciones suaves: no solo fade in/out, sino también ducking del volumen de música cuando aparece un SFX importante o un diálogo/feedback visual.
- Estado como fuente de verdad: la música y el ambiente deben derivarse del estado actual de `RoomManager`, `RealityManager` y `GameManager`, no de comprobaciones ad hoc a lo largo del código.
- Fallback silencioso: si no existe una cue, el sistema debe hacer log de warning y no romper la partida; no debe fallar con excepción ni producir sonido aleatorio.
- Organización de assets: conviene separar por carpetas `Audio/Music`, `Audio/Ambient`, `Audio/SFX`, `Audio/Mixers` y `Audio/Data` para mantener el contenido controlado durante la jam.

Con estos puntos definidos, la parte técnica queda mucho más fácil de implementar y de escalar sin perder la simplicidad necesaria en un proyecto de game jam.

---

## Implementación técnica recomendada (v2)

La siguiente capa de detalle ya nos lleva a una propuesta más concreta, sin perder la simplicidad del proyecto ni el patrón actual de managers.

### 1. Nombres exactos de clases y responsabilidades

- `AudioManager` — fachada principal del sistema. Es el punto de acceso del resto del juego.
- `MusicManager` — controla la música principal, transiciones y mezcla por realidad.
- `AmbientManager` — controla ambiente por sala y capas de sonido ambiental.
- `SFXManager` — controla cues transitorias: interacción, inventario, shift, feedback, etc.
- `AudioCueSO` — ScriptableObject que representa una cue de audio individual.
- `AudioRoomProfileSO` — ScriptableObject que define el perfil de audio de una sala.
- `AudioEventNames` — clase estática con IDs de eventos de audio.
- `AudioMixerController` — opcionalmente, un wrapper mínimo para controlar grupos de mezcla.

Esto mantiene una nomenclatura limpia y un modelo fácil de enseñar a un equipo pequeño o a una sola persona.

### 2. Estructura de archivos recomendada

```text
Assets/
  Scripts/
    Audio/
      AudioManager.cs
      MusicManager.cs
      AmbientManager.cs
      SFXManager.cs
      AudioEventNames.cs
      AudioCueSO.cs
      AudioRoomProfileSO.cs
      AudioMixerController.cs
    Gameplay/
      ...
```

Y para assets sonoros:

```text
Assets/
  Audio/
    Music/
      MainTheme_Normal.mp3
      MainTheme_OtherSide.mp3
    Ambient/
      Lobby_Normal.mp3
      Lobby_OtherSide.mp3
      Hallway_Normal.mp3
      Hallway_OtherSide.mp3
    SFX/
      key_pickup.ogg
      door_open.ogg
      shift_transition.ogg
      feedback_ui.ogg
    Mixers/
      Master.mixer
```

Esto es suficiente para comenzar y no exige una arquitectura pesada. La clave es mantener los archivos ordenados desde el principio.

### 3. Flujo de eventos recomendado

Este es el patrón más limpio para una game jam:

1. `RoomManager` dispara `RoomChanged` cuando cambia la sala activa.
2. `RealityManager` dispara `RealityChanged` cuando cambia la realidad.
3. `GameManager` dispara `FlagChanged` cuando un estado del mundo cambia (`DoorOpened`, `ShiftPerformed`, etc.).
4. `AudioManager` observa esos eventos y decide si:
   - cambia el ambiente actual,
   - ajusta la realidad sonora,
   - reproduce un SFX o un cue narrativo,
   - o aplica ducking a la música principal.
5. `MusicManager` decide qué versión del tema debe sonar y si hace fade.
6. `AmbientManager` decide si la sala tiene ambiente normal u otro lado.
7. `SFXManager` reproduce efectos cortos y laterales sin bloquear la música.

Diagrama mental de flujo:

```text
RoomManager / RealityManager / GameManager
                │
                ▼
          AudioManager
        ┌───────┴────────┐
        ▼                ▼
  MusicManager      AmbientManager
        │                │
        └───────┬────────┘
                ▼
            SFXManager
```

Esto evita que el gameplay conozca el audio; el sistema de audio observa el estado del juego y responde a él, sin acoplarse a detalle de escena.

### 4. Ejemplo mínimo de integración con `RealityManager` y `RoomManager`

El siguiente ejemplo es solo una base de trabajo, pensada para ser simple y clara:

```csharp
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private MusicManager musicManager;
    private AmbientManager ambientManager;
    private SFXManager sfxManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicManager = GetComponent<MusicManager>();
        ambientManager = GetComponent<AmbientManager>();
        sfxManager = GetComponent<SFXManager>();
    }

    private void OnEnable()
    {
        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.RoomChanged += HandleRoomChanged;
        }

        if (RealityManager.Instance != null)
        {
            RealityManager.Instance.RealityChanged += HandleRealityChanged;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.FlagChanged += HandleFlagChanged;
        }
    }

    private void OnDisable()
    {
        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.RoomChanged -= HandleRoomChanged;
        }

        if (RealityManager.Instance != null)
        {
            RealityManager.Instance.RealityChanged -= HandleRealityChanged;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.FlagChanged -= HandleFlagChanged;
        }
    }

    private void Start()
    {
        if (RoomManager.Instance != null)
        {
            HandleRoomChanged(RoomManager.Instance.CurrentRoom);
        }

        if (RealityManager.Instance != null)
        {
            HandleRealityChanged(RealityManager.Instance.CurrentReality);
        }
    }

    private void HandleRoomChanged(RoomData room)
    {
        if (room == null)
        {
            return;
        }

        ambientManager?.SetRoomAmbient(room.id);
    }

    private void HandleRealityChanged(RealityType reality)
    {
        musicManager?.SetReality(reality);
        ambientManager?.SetReality(reality);
    }

    private void HandleFlagChanged(string flagName)
    {
        switch (flagName)
        {
            case GameFlagNames.DoorOpened:
                sfxManager?.PlayCue("door_open");
                break;

            case GameFlagNames.ShiftPerformed:
                sfxManager?.PlayCue("shift_transition");
                break;

            default:
                break;
        }
    }

    public void PlaySfx(string cueId)
    {
        sfxManager?.PlayCue(cueId);
    }

    public void PlayMusic(string cueId)
    {
        musicManager?.PlayCue(cueId);
    }
}
```

La idea es simple: el `AudioManager` es la centralización de la lógica de audio, mientras que los otros managers no tienen que saber nada de `AudioSource` ni de mezclas. Lo único que hacen es avisar del cambio de estado.

### 5. Primer esquema de `ScriptableObject` para Unity

Este es un primer esquema útil para empezar sin dejar el sistema rígido:

```csharp
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Audio/Audio Cue")]
public class AudioCueSO : ScriptableObject
{
    public string cueId;
    public AudioClip clip;

    [Range(0f, 2f)]
    public float volume = 1f;

    [Range(-3f, 3f)]
    public float pitch = 1f;

    public bool loop;
    public float fadeInSeconds = 0.25f;
    public float fadeOutSeconds = 0.25f;

    public AudioMixerGroup mixerGroup;
    public RealityType[] allowedRealities = { RealityType.Normal, RealityType.OtherSide };
}
```

```csharp
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Audio/Room Profile")]
public class AudioRoomProfileSO : ScriptableObject
{
    public string roomId;
    public AudioCueSO normalAmbient;
    public AudioCueSO otherSideAmbient;
    public AudioCueSO musicLoop;
    public float crossfadeSeconds = 2f;
}
```

Con esto, la lógica queda muy clara:

- cada cue tiene su identidad y su configuración propia,
- cada sala define su ambiente por realidad,
- cada música tiene una versión normal y otra opuesta,
- y el sistema no necesita hardcodear clips por `if (roomId == ...)` en el código.

### 6. Recomendación práctica para el MVP

Para la fase actual del proyecto, la recomendación más segura es:

- Implementar solo `AudioManager`, `MusicManager` y `SFXManager` al principio.
- Dejar `AmbientManager` como una capa auxiliar si la sala requiere mezcla más compleja.
- Reutilizar una sola plantilla principal de `AudioCueSO` para todo el contenido.
- Empezar con 3 o 4 cues de música y 5-8 SFX, no más.
- No complicar la mezcla con muchos grupos si todavía no hay contenido real.

La regla es: primero funciona, luego se expande. La mezcla por realidades y por salas es la parte que más aporta a la experiencia, pero no necesita ser una infraestructura gigantesca para empezar.

---

## Filosofía Técnica

El sistema debe favorecer la reutilización de contenido.

Siempre que sea posible:

- Un tema musical tendrá varias versiones derivadas.
- Los ambientes compartirán capas y variaciones.
- Los efectos utilizarán modificaciones mediante volumen, pitch y filtros.
- Las diferencias entre realidades se apoyarán en la mezcla y el procesamiento antes que en la creación de contenido completamente nuevo.

---

## Formatos de Audio

### Música (Music)

Formato final:

- MP3
- 192 kbps

Objetivos:

- Reducir tamaño final.
- Mantener calidad suficiente.
- Conservar una ligera degradación coherente con la estética retro.

### Ambiente (Ambient)

Formato final:

- MP3
- 192 kbps

Objetivos:

- Mantener coherencia con la música.
- Reducir espacio ocupado.
- Simplificar el pipeline de producción.

### Efectos de Sonido (SFX)

Formato final:

- OGG

Objetivos:

- Mejor respuesta para sonidos cortos.
- Menor aparición de artefactos de compresión.
- Tamaño reducido respecto a WAV.

---

## Pipeline de Exportación

Todos los audios generados desde Sonic Pi serán exportados inicialmente en formato WAV.

Posteriormente se procesarán mediante Audacity para realizar:

1. Eliminación de silencios innecesarios.
2. Ajuste de inicio y final.
3. Aplicación de Fade In.
4. Aplicación de Fade Out.
5. Normalización.
6. Exportación al formato final correspondiente.

---

## Normalización de Volumen

Todos los archivos finales serán normalizados a:

- -3 dB

Objetivos:

- Mantener coherencia entre pistas.
- Evitar diferencias bruscas de volumen.
- Disponer de margen para mezcla posterior dentro del juego.

---

## Fade In

Recomendaciones:

- Música: 0,5 - 2 segundos
- Ambiente: 1 - 4 segundos
- Efectos de sonido: 0,1 - 0,5 segundos

---

## Fade Out

Recomendaciones:

- Música: 2 - 8 segundos
- Ambiente: 2 - 8 segundos
- Efectos de sonido: 0,1 - 0,5 segundos

---

## Integración con SHIFT

La transición entre Realidad Normal y El Otro Lado constituye uno de los aspectos más importantes del diseño sonoro.

Siempre que sea posible:

- Se reutilizarán los mismos temas base.
- Se reutilizarán las mismas capas ambientales.
- Las diferencias surgirán de cambios de mezcla, filtros, efectos y procesamiento.

El objetivo es que el jugador no perciba un cambio brusco de pista, sino la sensación de que el propio mundo está transformándose a su alrededor.

---

## Consideraciones Narrativas

El tema principal del juego representa la búsqueda del padre por parte del protagonista.

Durante la partida, dicho tema podrá aparecer fragmentado, incompleto o parcialmente oculto.

En el Final Verdadero, el motivo principal podrá reproducirse por primera vez de forma completa para reforzar la reconstrucción de la memoria y la aceptación final de la verdad.