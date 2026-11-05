# Setup Session 05/11/2026 - MVP Sprint 1 Cierre

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

### Paso 2: Preparar Hallway - Backgrounds 🔴 CRÍTICO

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

### Paso 3: Configurar HallwayObstacle 🔴 CRÍTICO

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

```
id: "hallway_obstacle"
displayName: "Massive Figure"
description: "A strange figure blocks the passage in this reality."
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

### Paso 4: Crear UI Shift Button (Bottom-Right) 🔴 IMPORTANTE

**Ubicación:** Canvas UI en pantalla (no world-space interactable)

**Estructura esperada:**

```plaintext
UICanvas (Canvas, resolución 640x360)
└── ShiftIconContainer (RectTransform, Button)
    └── Icon (Image, sprite encapuchado 64x64)
```

**Setup detallado:**

1. **Canvas:**
   - [ ] Crear Canvas en raíz de Main
   - [ ] Canvas Scaler → Scale With Screen Size (Reference Resolution: 640x360)
   - [ ] Render Mode: Camera

2. **ShiftIconContainer:**
   - [ ] RectTransform: Anchors Bottom-Right
   - [ ] Anchor Position: (-20, 20) ← margen desde esquina
   - [ ] Size: (64, 64) ← tamaño en píxeles
   - [ ] Añadir Button component
   - [ ] Navigation: None

3. **Icon (child):**
   - [ ] Image component
   - [ ] Source Image: shift-icon-hooded-figure-v1.png (sprite a crear/importar)
   - [ ] Preserve Aspect: true
   - [ ] Set Native Size: true

4. **Button OnClick:**
   - [ ] Configurar listener
   - [ ] Target: RealityManager
   - [ ] Method: ToggleReality()

**Sprite Requirements:**

- Size: 64x64 píxeles (coherente con Pixel Perfect Camera)
- Paleta: Encapuchado/figura de horror
- Filter Mode: Point (no Linear)
- Pixels Per Unit: 16

**Nota:** El sprite puede ser placeholder por ahora. Lo importante es que el botón funcione. Luego mejoramos arte.

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

```
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
   - [ ] Icono visible en bottom-right de pantalla
   - [ ] Hover en icono → cambio visual (color, etc)
   - [ ] Click en icono → realidad cambia (igual que LEFT SHIFT)
   - [ ] Console log: "[UIShiftButton] Shift clicked" (si usas script)
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

1. CacheBackgrounds() en Start() → busca objetos por nombre
2. UpdateBackgrounds() en SetReality() → cambia visibilidad de GameObjects
3. RoomManager.RefreshInteractables() → actualiza visibility de InteractableBehaviour según reglas

### Debug - Si algo falla

```
Problema: LEFT SHIFT no funciona
→ Verificar que InputHandler existe y está en un GameObject activo

Problema: Backgrounds no cambian
→ Verificar nombres exactos: "BackgroundNormal" y "BackgroundOtherSide"
→ Verificar que existen bajo Hallway/Visuals

Problema: HallwayObstacle no desaparece
→ Verificar que InteractableData tiene visibleInOtherReality = false
→ Verificar que RealityManager.UpdateBackgrounds() se ejecuta

Problema: Transiciones no funcionan
→ Verificar linkedRoomId en InteractableData
→ Verificar que nombre de room en GameObject coincide con ID
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
