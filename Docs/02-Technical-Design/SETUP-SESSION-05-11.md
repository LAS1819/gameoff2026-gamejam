# Setup Session 05/11/2026 - MVP Sprint 1 Cierre

- **Creado:** 2026-11-05
- **Última Actualización:** 2026-11-06

---

## 🎯 Objetivo Final

Completar el MVP Sprint 1 con un flujo funcional end-to-end:

1. Lobby → Recoger llave → Abrir puerta → Entrar Hallway
2. Hallway → SHIFT (LEFT SHIFT) → Cambiar realidad visualmente
3. Hallway → Click en puertas Library/Bedroom → Logs de confirmación

---

## 📋 CHECKLIST ESCENA UNITY

### Paso 1: Verificar Estructura Base de la Escena ✅

**Main Scene:**

- [ ] Contiene GameObject "Systems" o similar
- [ ] InputHandler.cs está presente y adjunto (para LEFT SHIFT)
- [ ] RealityManager está presente
- [ ] Existe una carpeta "Rooms" en la escena
- [ ] Existe subcarpeta "Lobby" con estructura:
  - [ ] Visuals/Background
  - [ ] Interactables (StaffKey, FrontDoor, HallwayEntrance)
- [ ] Existe subcarpeta "Hallway" (parcialmente completa)

---

### Paso 2: Preparar Hallway - Backgrounds ✅ CRÍTICO

**El Hallway necesita DOS backgrounds:**

#### BackgroundNormal

- Path esperado: `Main/Rooms/Hallway/Visuals/BackgroundNormal`
- Sprite: `Assets/Art/Hallway/room-hallway-background-v1.png`
- SpriteRenderer:
  - Sorting Order: 0
  - Position: (0, 0, 0)
  - Scale: (1, 1, 1)
- Estado inicial: **ACTIVO**

#### BackgroundOtherSide

- Path esperado: `Main/Rooms/Hallway/Visuals/BackgroundOtherSide`
- Sprite: **CREAR** - `Assets/Art/Hallway/room-hallway-otherside-v1.png`
  - Puede ser igual que BackgroundNormal pero con filtro/color diferente
  - O una versión modificada visualmente
  - **Para MVP demo:** copiar v1 y modificar tint desde Unity (SpriteRenderer → Color)
- SpriteRenderer:
  - Sorting Order: 0
  - Position: (0, 0, 0)
  - Scale: (1, 1, 1)
- Estado inicial: **INACTIVO**

**Actions:**

1. [x] Crear sprite OtherSide (copiar v1 y aplicar filtro de color/oscuridad)
2. [x] En Unity, crear GameObject "BackgroundOtherSide" bajo Hallway/Visuals
3. [x] Adjuntar sprite OtherSide
4. [x] Verificar SpriteRenderer (Sorting Order 0)
5. [x] Desactivar GameObject (será activado por RealityManager)

---

### Paso 3: Configurar HallwayObstacle ✅ CRÍTICO

**GameObject:** `Main/Rooms/Hallway/Interactables/HallwayObstacle`

**Verificar/Crear:**

- [x] SpriteRenderer
  - Sprite: `Assets/Art/Hallway/room-hallway-block-obstacle-v1.png`
  - Sorting Order: 2
  - Position: (5, 0, 0)
- [x] BoxCollider2D
  - IsTrigger: **true**
  - Size: (2, 3) o similar
- [x] InteractableBehaviour script
  - Data debe estar configurada (ver abajo)

**InteractableData en Inspector:**

```plaintext
id: "hallway_obstacle"
displayName: "Obstacles blocking hallway"
description: "A strange obstacle blocks the hallway."
interactionType: Examine
visibleInNormalReality: true
visibleInOtherReality: false ← CLAVE: Desaparece en OtherSide
resultFlag: "RealityUnlocked"
```

**Verificación clave:**

- Al cargar Hallway en Normal → HallwayObstacle VISIBLE
- Al presionar LEFT SHIFT → HallwayObstacle INVISIBLE
- Al presionar LEFT SHIFT de nuevo → HallwayObstacle VISIBLE

---

### Paso 4: Crear UI SHIFT con UI Toolkit + Panel Renderer ✅ IMPORTANTE

**Ubicación:** Runtime UI con Panel Renderer (sin Canvas/Button legacy).

**Estructura esperada:**

```plaintext
UI
└── ShiftPanel
    ├── PanelRenderer
    │   ├── Panel Settings: Assets/UI Toolkit/PanelSettings.asset
    │   └── Source Asset: Assets/UI/ShiftUI.uxml
    └── UIShiftButton.cs
```

**Setup detallado:**

1. **ShiftPanel:**
   - [x] GameObject activo en raíz `UI/ShiftPanel`
   - [x] Componente `PanelRenderer` activo
   - [x] `Panel Settings` asignado
   - [x] `Source Asset` asignado (`ShiftUI.uxml`)

2. **UXML (`ShiftUI.uxml`):**
   - [x] Existe un `VisualElement` con `name="shift-button"`
   - [x] El elemento se visualiza en la esquina inferior derecha mediante estilos USS
   - [x] No se usa un `Button` nativo para evitar el fondo por defecto del tema

3. **Binding C#:**
   - [x] `UIShiftButton.cs` en el mismo GameObject que `PanelRenderer`
   - [x] Registra callback con `RegisterUIReloadCallback(VersionedUIReloadCallback)`
   - [x] Busca el elemento con `root.Q<VisualElement>("shift-button")`
   - [x] Conecta `RegisterCallback<ClickEvent>(OnShiftButtonClicked)`
   - [x] En click ejecuta `RealityManager.Instance.ToggleReality()`
   - [x] Lista `shiftEnabledRoomIds` configurada (MVP: `hallway`)
   - [x] En room no permitida: warning log y sin toggle
   - [x] Al click válido: set de flag `ShiftPerformed`
   - [x] El cambio entre mundo normal y Other Side se gestiona mediante clases CSS (`normal` / `other-side`)
   - [x] El styling visual (fondos, hover, pressed, opacity) vive en `shift-button-style-sheet.uss`

4. **Limpieza de legado Canvas (importante):**
   - [x] Mantener desactivado/eliminar `Canvas` y `ShiftIconContainer` anteriores para evitar configuraciones mezcladas
   - [x] Evitar tener dos sistemas activos para el mismo botón SHIFT

**Estilo visual actual (UI Toolkit):**

- El `VisualElement` `shift-button` es el único elemento visible.
- No se usa `Button` para evitar el tema por defecto y el fondo blanco.
- La textura se asigna desde la clase CSS `normal` / `other-side`.
- El hover/pressed se controlan con `:hover` y `:active` en USS.
- El estado bloqueado se representa con `:disabled` y con la lógica de room gating en C#.

**Sprite Requirements (si usas icono personalizado):**

- Size recomendado: 64x64 px (coherente con 640x360)
- Filter Mode: Point
- Paleta coherente con el arte del juego

**Nota:** El MVP prioriza comportamiento fiable de click + toggle. La apariencia visual del icono se resuelve en UI Toolkit, manteniendo la lógica separada del styling.

---

### Paso 5: Crear LobbyExit 🟡 IMPORTANTE

**GameObject:** `Main/Rooms/Hallway/Interactables/LobbyExit`

**Crear si no existe:**

- [ ] GameObject "LobbyExit" bajo Interactables
- [ ] SpriteRenderer (puede ser invisible)
  - Sprite: [opcional]
  - Sorting Order: 2
  - Position: (-7, 0, 0)
- [ ] BoxCollider2D
  - IsTrigger: **true**
  - Size: (1, 3)
- [ ] InteractableBehaviour script

**InteractableData en Inspector:**

```plaintext
id: "lobby_exit"
displayName: "Back to Lobby"
description: "The passage leads back."
interactionType: Transition
visibleInNormalReality: true
visibleInOtherReality: true
linkedRoomId: "lobby"
```

---

### Paso 6: Crear Puertas Placeholder Library/Bedroom 🟡 IMPORTANTE

Estas son puertas que SOLO loguean cuando se clickea. No implementan lógica de transición.

#### LibraryDoor

- [ ] GameObject "LibraryDoor" bajo Hallway/Interactables
- [ ] SpriteRenderer
  - Sprite: [marker o sprite visual sutil]
  - Sorting Order: 2
  - Position: (2, 0, 0)
- [ ] BoxCollider2D (IsTrigger: true, Size: (1.5, 3))
- [ ] InteractableBehaviour script

**InteractableData:**

```
id: "library_door"
displayName: "Mysterious Door (Library)"
description: "This door leads to the Library. [NOT YET IMPLEMENTED]"
interactionType: Examine
visibleInNormalReality: true
visibleInOtherReality: true
resultFlag: "LibraryExamined"
```

**Script especial:** Opcional - en InteractableBehaviour, Al hacer click, loguear:

```csharp
Debug.Log("[MVP] Clicked on Library Door - Navigation to Library not yet implemented");
```

#### BedroomDoor

- [ ] GameObject "BedroomDoor" bajo Hallway/Interactables
- [ ] SpriteRenderer
  - Sprite: [marker o sprite visual sutil]
  - Sorting Order: 2
  - Position: (8, 0, 0)
- [ ] BoxCollider2D (IsTrigger: true, Size: (1.5, 3))
- [ ] InteractableBehaviour script

**InteractableData:**

```
id: "bedroom_door"
displayName: "Bedroom Entrance"
description: "A corridor leading to private quarters. [NOT YET IMPLEMENTED]"
interactionType: Examine
visibleInNormalReality: true
visibleInOtherReality: true
resultFlag: "BedroomExamined"
```

---

### Paso 7: Verificar RealityManager en la Escena 🟢 CONTROL

**En Main scene:**

- [ ] Existe un GameObject "RealityManager" (probablemente bajo Systems o creado en runtime)
- [ ] Contiene el script RealityManager.cs
- [ ] El script está actualizado con la lógica de backgrounds

**En Bootstrap o Main:**

- [ ] Existe un GameObject con InputHandler.cs adjunto
- [ ] El script gestiona LEFT SHIFT → RealityManager.ToggleReality()
- [ ] Existe `UI/ShiftPanel` con `PanelRenderer + UIShiftButton`
- [ ] `PanelRenderer` tiene `PanelSettings` y `ShiftUI.uxml` asignados

---

### Paso 8: Validación de Flujo 🟢 VALIDAR

**En Play Mode, ejecutar esta secuencia:**

1. **Lobby Load:**
   - [ ] Entrar en Play Mode
   - [ ] Lobby carga correctamente
   - [ ] Puedo ver StaffKey

2. **Recoger Llave:**
   - [ ] Click en StaffKey → se recoge (desaparece visualmente)
   - [ ] Console log: "[InteractionManager] TryInteract..." ✓
   - [ ] Flag "HasStaffKey" se activa ✓

3. **Usar Llave en Puerta:**
   - [ ] Click en FrontDoor → puerta se abre (debería desaparecer o cambiar visual)
   - [ ] Flag "DoorOpened" se activa ✓
   - [ ] HallwayEntrance se vuelve interactiva ✓

4. **Entrar a Hallway:**
   - [ ] Click en HallwayEntrance → transición a Hallway
   - [ ] Console log: "[RoomManager] Loaded room: hallway" ✓
   - [ ] HallwayObstacle VISIBLE (está en Normal reality)
   - [ ] BackgroundNormal ACTIVO
   - [ ] BackgroundOtherSide INACTIVO

5. **SHIFT a OtherSide:**
   - [ ] Presionar LEFT SHIFT
   - [ ] Console log: "[RealityManager] Reality changed to: OtherSide" ✓
   - [ ] BackgroundNormal → INACTIVO
   - [ ] BackgroundOtherSide → ACTIVO
   - [ ] HallwayObstacle → INVISIBLE (visibleInOtherReality: false)
   - [ ] Fondos cambian visualmente

6. **SHIFT de vuelta a Normal:**
   - [ ] Presionar LEFT SHIFT de nuevo
   - [ ] BackgroundNormal → ACTIVO
   - [ ] BackgroundOtherSide → INACTIVO
   - [ ] HallwayObstacle → VISIBLE

7. **Test UI Shift Button:**
   - [ ] Botón `shift-button` visible en pantalla (Panel Renderer)
   - [ ] Hover en botón funciona
   - [ ] En Lobby: click en botón NO cambia realidad y muestra warning
   - [ ] En Hallway: click en botón SI cambia realidad (igual que LEFT SHIFT)
   - [ ] Console log: "[UIShiftButton] Bound click handler to 'shift-button'."
   - [ ] Console log: "[UIShiftButton] Shift button clicked - toggling reality"
   - [ ] Console log warning cuando se pulsa en room no permitida
   - [ ] El icono cambia entre Normal / OtherSide
   - [ ] Background y obstáculo cambian igual que con LEFT SHIFT

8. **Test Puertas Placeholder:**
   - [ ] Click en LibraryDoor → Console log sobre Library
   - [ ] Click en BedroomDoor → Console log sobre Bedroom
   - [ ] Ambas puertas responden

9. **Volver a Lobby:**
   - [ ] Click en LobbyExit → transición a Lobby
   - [ ] Console log: "[RoomManager] Loaded room: lobby" ✓

---

## 🎬 Resultado Final Esperado

Una demo funcional que demuestre:

✅ Flujo de progresión: Lobby → Hallway → OtherSide → Volver  
✅ Sistema SHIFT funcional con cambios visuales  
✅ Obstacles desaparecen según realidad  
✅ Puertas placeholder responden  
✅ Sin errores críticos en consola  

---

## 📝 Notas Técnicas

### Orden de Precedencia en RealityManager

1. `RoomManager.LoadRoom()` activa room actual.
2. `RealityManager.HandleRoomChanged()` aplica la realidad actual a los backgrounds de esa room.
3. `RealityManager.SetReality()` alterna estado y reaplica backgrounds.
4. `RoomManager.RefreshInteractables()` sincroniza visibilidad de interactables con la realidad activa.

### Debug - Si algo falla

```
Problema: LEFT SHIFT no funciona
→ Verificar que InputHandler existe y está en un GameObject activo

Problema: Backgrounds no cambian
→ Verificar nombres exactos: "BackgroundNormal" y "BackgroundOtherSide"
→ Verificar que existen bajo RoomActual/Visuals (ej. Hallway/Visuals)
→ Verificar que RoomManager.CurrentRoom.id coincide con el nombre de room en escena

Problema: HallwayObstacle no desaparece
→ Verificar que InteractableData tiene visibleInOtherReality = false
→ Verificar que RealityManager.UpdateBackgrounds() se ejecuta

Problema: Transiciones no funcionan
→ Verificar linkedRoomId en InteractableData
→ Verificar que nombre de room en GameObject coincide con ID

Problema: Botón SHIFT de UI Toolkit no responde
→ Verificar PanelRenderer con PanelSettings + ShiftUI.uxml asignados
→ Verificar que el UXML contiene name="shift-button"
→ Verificar log: "[UIShiftButton] Bound click handler to 'shift-button'."
→ Eliminar/desactivar Canvas legacy para evitar confusión de configuración
```

---

## ✅ DEFINICIÓN DE "DONE"

Cuando la siguiente grabación sea posible SIN PARAR:

1. Presionar Play
2. Recoger llave
3. Usar llave
4. Entrar Hallway
5. Presionar LEFT SHIFT 2 veces (toggle realidad)
6. Ver cambios visuales
7. Click en puertas (ver logs)
8. Volver a Lobby sin errores

Si esto funciona limpiamente → **MVP Sprint 1 COMPLETE**
