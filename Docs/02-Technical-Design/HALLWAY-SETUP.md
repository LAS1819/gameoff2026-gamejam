# Hallway Room Configuration - MVP Setup

- Versión 0.1
- Fecha: 05/11/2026
- Actualización: 05/11/2026

---

## Objetivo

Configurar la Room Hallway en Unity para que:

1. El jugador pueda cambiar de realidad con LEFT SHIFT
2. El HallwayObstacle desaparezca en El Otro Lado
3. Los 3 interactables funcionen correctamente

---

## Prerequisitos

✅ Hallway room existe en la escena Main  
✅ Background de Hallway cargado  
✅ Block-obstacle visual creado  
✅ GameManager, RoomManager, InteractionManager en la escena  
✅ InputHandler.cs creado (nuevo script para LEFT SHIFT)

---

## Paso 1: Añadir InputHandler a la Escena

1. En la escena Main, selecciona el GameObject "System" (o donde estén los managers)
2. Añade un nuevo script **InputHandler.cs** como componente
   - Debería estar en Assets/Scripts/InputHandler.cs
3. Verifica que aparece en el Inspector

**Resultado esperado**: Un componente InputHandler sin parámetros editables

---

## Paso 2: Configurar HallwayObstacle

Este es el bloque visual que bloquea la ruta en Realidad Normal pero desaparece en El Otro Lado.

### En el Inspector:

1. Selecciona el GameObject **HallwayObstacle** (el block visual que creaste)
2. **Verifica que tenga:**
   - SpriteRenderer (con el sprite del bloque)
   - BoxCollider2D (IsTrigger = true)
   - InteractableBehaviour (script)

3. **En InteractableBehaviour, crea la InteractableData:**

```csharp
Data = new InteractableData
{
    id = "hallway_obstacle",
    displayName = "Blocking Obstacles",
    description = "A lot of obstacles here.",
    interactionType = InteractionType.Examine,
    visibleInNormalReality = true,
    visibleInOtherReality = false,
    resultFlag = "RealityUnlocked"
}
```

O si usas el Inspector directamente:

- **id**: `hallway_obstacle`
- **displayName**: `Blocking Obstacles`
- **description**: `A lot of obstacles here.`
- **interactionType**: `Examine`
- **visibleInNormalReality**: `true`
- **visibleInOtherReality**: `false`
- **resultFlag**: `RealityUnlocked`

**Resultado esperado**: Al clickear en el Hallway (después de haber entrado), el obstáculo registra un click y la flag RealityUnlocked se activa.

---

## Paso 3: Configurar ShiftTrigger

Este es el interactable que permite cambiar de realidad mediante click (además de LEFT SHIFT).

### En el Inspector:

1. Selecciona o crea un GameObject llamado **ShiftTrigger** en Hallway/Interactables
2. **Verifica que tenga:**
   - SpriteRenderer (sprite opcional, puede ser invisible o un símbolo sutil)
   - BoxCollider2D (IsTrigger = true)
   - InteractableBehaviour (script)

3. **En InteractableBehaviour, crea la InteractableData:**

```csharp
Data = new InteractableData
{
    id = "shift_trigger",
    displayName = "The Other Side",
    description = "Reality feels thin here...",
    interactionType = InteractionType.Toggle,
    visibleInNormalReality = true,
    visibleInOtherReality = true,
    resultFlag = "ShiftPerformed"
}
```

O en el Inspector:

- **id**: `shift_trigger`
- **displayName**: `The Other Side`
- **description**: `Reality feels thin here...`
- **interactionType**: `Toggle`
- **visibleInNormalReality**: `true`
- **visibleInOtherReality**: `true`
- **resultFlag**: `ShiftPerformed`

**Resultado esperado**: Al clickear en ShiftTrigger o presionar LEFT SHIFT, la realidad cambia (el fondo debería cambiar, el obstáculo debería desaparecer).

---

## Paso 4: Configurar LobbyExit

Este permite volver al Lobby.

### En el Inspector:

1. Selecciona o crea un GameObject llamado **LobbyExit** en Hallway/Interactables
2. **Verifica que tenga:**
   - SpriteRenderer (sprite marcador o invisible)
   - BoxCollider2D (IsTrigger = true)
   - InteractableBehaviour (script)

3. **En InteractableBehaviour, crea la InteractableData:**

```csharp
Data = new InteractableData
{
    id = "lobby_exit",
    displayName = "Back to Lobby",
    description = "The path back.",
    interactionType = InteractionType.Transition,
    visibleInNormalReality = true,
    visibleInOtherReality = true,
    linkedRoomId = "lobby"
}
```

O en el Inspector:

- **id**: `lobby_exit`
- **displayName**: `Back to Lobby`
- **description**: `The path back.`
- **interactionType**: `Transition`
- **visibleInNormalReality**: `true`
- **visibleInOtherReality**: `true`
- **linkedRoomId**: `lobby`

**Resultado esperado**: Al clickear en LobbyExit, se carga la escena Lobby nuevamente.

---

## Paso 5: Configurar las imágenes de fondo (Normal vs OtherSide)

Para que el cambio de realidad sea visible, necesitas dos versiones del fondo.

### Opción A: Dos GameObjects separados (recomendado para MVP)

1. Crea un GameObject llamado **BackgroundNormal** con el sprite de Hallway Normal
2. Crea un GameObject llamado **BackgroundOtherSide** con el sprite de Hallway OtherSide
3. Ambos deben estar en la posición (0, 0, 0)
4. **En Start() o mediante script**, desactiva BackgroundOtherSide al iniciar
5. **En RealityManager**, añade la lógica para cambiar entre fondos (ver Paso 6)

### Opción B: Un solo fondo (cambio de color/shader)

Menos recomendado para MVP, requiere más trabajo.

---

## Paso 6: Conectar el RealityManager con los fondos

Si usas dos GameObjects (Opción A), necesitas actualizar RealityManager:

**En RealityManager.cs, añade:**

```csharp
private GameObject backgroundNormal;
private GameObject backgroundOtherSide;

private void Start()
{
    // Busca los backgrounds en la escena
    backgroundNormal = GameObject.Find("Main/Rooms/Hallway/BackgroundNormal");
    backgroundOtherSide = GameObject.Find("Main/Rooms/Hallway/BackgroundOtherSide");
}

public void SetReality(RealityType reality)
{
    if (CurrentReality == reality)
    {
        return;
    }

    CurrentReality = reality;

    // Actualiza la visibilidad del fondo
    if (backgroundNormal != null && backgroundOtherSide != null)
    {
        backgroundNormal.SetActive(reality == RealityType.Normal);
        backgroundOtherSide.SetActive(reality == RealityType.OtherSide);
    }

    RoomManager.Instance?.RefreshInteractables();
    RealityChanged?.Invoke(CurrentReality);
}
```

---

## Paso 7: Test de Flujo

1. Abre la escena Main y presiona Play
2. Haz clic en Lobby para interactuar con StaffKey y FrontDoor (debería funcionar como antes)
3. Entra al Hallway
4. **Presiona LEFT SHIFT**: El fondo debería cambiar y el obstáculo debería desaparecer
5. **Presiona LEFT SHIFT de nuevo**: Vuelves a Realidad Normal, el obstáculo reaparece
6. **Haz clic en el obstáculo en Realidad Normal**: Debería marcar RealityUnlocked
7. **Haz clic en ShiftTrigger**: Debería cambiar la realidad (mismo que LEFT SHIFT)
8. **Haz clic en LobbyExit**: Debería volver al Lobby

---

## Resultado Esperado

✅ LEFT SHIFT funciona para cambiar de realidad  
✅ HallwayObstacle aparece/desaparece según la realidad  
✅ Fondos cambian visualmente  
✅ Los 3 interactables responden correctamente  
✅ El flujo Lobby → Hallway → Lobby funciona sin errores  

---

## Troubleshooting

| Problema | Solución |
|----------|----------|
| LEFT SHIFT no funciona | Verifica que InputHandler.cs esté adjunto a un GameObject activo |
| Obstáculo no desaparece | Verifica que `visibleInOtherReality = false` en HallwayObstacle |
| Fondo no cambia | Verifica que los GameObjects de fondo existan con los nombres correctos |
| "RealityManager not found" | Asegúrate de que RealityManager está en la escena Main |

---

**Una vez completado, avisa y pasamos a la siguiente fase del MVP.** ✅
