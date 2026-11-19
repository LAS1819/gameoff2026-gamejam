# Room Basement — Technical Spec

- Versión 0.3
- Creado: 17/11/2026
- Actualizado: 19/11/2026

---

## 1. Objetivo de la Room

El `Basement` es la primera `Room` donde la linterna deja de ser meramente un icono y se convierte en una condición de juego real.

Su función técnica es:

- aplicar una intensidad global muy baja para reforzar la sensación de oscuridad;
- habilitar la linterna solo en rooms permitidas (MVP: `basement`);
- bloquear `Use` crítico si el jugador no tiene la linterna encendida;
- ofrecer una evidencia narrativa clara, sin convertir la room en un bloque de contenido pesado;
- mantener una amenaza bloqueadora, pero sin IA ni combate complejo.

### Nota de implementación (fase base MVP)

La implementación inicial de código del Basement se ejecuta sin:

- gating por linterna,
- control de luz global,
- estados avanzados por realidad en interactuables.

Esta fase construye primero una `Room` funcional estándar con flujo de progreso, transición y flags base.

---

## 2. Definición de RoomData (MVP)

```text
RoomData
- id: "basement"
- displayName: "Basement"
- connectedRoomIds: ["library"]
- interactables:
  - basement_entry_door
  - storage_shelves
  - personal_effects_box
  - valve_handle
  - pressure_release_valve
  - ambulance_lights
  - basement_exit
  - denial_manifestation
- lightingProfile:
  - globalLightIntensity: 0.10
  - lanternAllowed: true
- narrativeGoal: "Break denial through evidence and pressure release"
```

### Estado inicial

- Reality: Normal
- Luz global muy baja, suficiente para orientación, no para lectura detallada.
- `LanternAllowed = true` en esta room.
- `LanternActive = false` al entrar.
- El jugador puede explorar, pero la verdad solo se entiende con la linterna encendida.
- La `Manifestación de Negación` sólo aparece cuando la presión no se ha resuelto y el jugador intenta avanzar sin condiciones correctas.

---

## 3. Contrato técnico de luz por Room

### 3.1 Regla general

No usar lógica automática basada solo en un umbral de intensidad. El sistema debe seguir un patrón explícito por room, igual que SHIFT:

- el `RoomManager` informa si la room actual permite linterna;
- el `LanternController` valida la room antes de activar un `Light2D`;
- si la room no permite linterna, se fuerza a `false` y no hay efecto visual.

### 3.2 Estructura sugerida

```text
RoomLightingProfile
- roomId
- globalLightIntensity (0..1)
- lanternAllowed (bool)
```

### 3.3 Valor recomendado

Para el `Basement`:

- `globalLightIntensity = 0.10`
- rango de ajuste para playtest: 0.08 a 0.18

Esto refuerza la oscuridad visual, pero sigue siendo un complemento del gating por room y no la única regla.

---

## 4. Linterna con sprite light 2D acoplada al cursor

```text
LanternController (runtime)
- input: toggle (HUD + teclado)
- checks: LanternUnlocked && LanternAllowed(currentRoom)
- state: LanternActive
- light source: Light2D (Point or Freeform)
- position source: cursor world position
```

### Reglas del MVP

- sólo puede encenderse si el jugador ya obtuvo la `Linterna` en `Bedroom`;
- si cambia a una room sin permiso, la linterna se desactiva automáticamente;
- no incluye batería ni desgaste durante esta iteración;
- la intensidad del haz puede hacerse sutil en `Global Light` y más fuerte con `Sprite Light 2D` local.

---

## 5. Interactables principales

### 5.1 StorageShelfLeft

```text
id: "storage_shelf_left"
displayName: "Storage Shelves"
interactionType: Examine
visibleInNormalReality: true
visibleInOtherReality: true
resultFlag: BasementShelfLeftExplored
```

**Propósito:**

- introducir la sensación de almacenamiento caótico y emocional;
- ocultar la evidencia y la pieza de progreso (`valve_handle` en `personal_effects_box`);
- reforzar que el espacio está lleno de recuerdos rechazados.

**Regla técnica:**

- sin linterna activa, la descripción del espacio es confusa;
- con linterna, la caja y la pieza comienzan a ser legibles.

**Visual:** `sprite-basement-storage-shelf-left.png` ✅ <- de momento, usamos el mismo sprite que para `StorageShelfRight`.

---

### 5.2 StorageShelfRight

```text
id: "storage_shelf_right"
displayName: "Storage Shelves"
interactionType: Examine
visibleInNormalReality: true
visibleInOtherReality: true
resultFlag: BasementShelfRightExplored
```

**Propósito:**

- introducir la sensación de almacenamiento caótico y emocional;
- reforzar que el espacio está lleno de recuerdos rechazados.

**Regla técnica:**

- sin linterna activa, la descripción del espacio es confusa;
- con linterna, los elementos cobran claridad y se vuelven legibles.

**Visual:** `sprite-basement-storage-shelf-right.png` ✅ <- de momento, usamos el mismo sprite que para `StorageShelfLeft`.

---

### 5.3 PersonalEffectsBox

```text
id: "personal_effects_box"
displayName: "Personal Effects Box"
interactionType: Use
visibleInNormalReality: false
visibleInOtherReality: true
resultFlag: BasementEvidenceFound
```

**Propósito:**

- ser la evidencia principal de la `Room`. Contiene la `Valve Handle` en su interior;
- materializar la incoherencia de la historia del protagonista;
- ofrecer la primera “prueba” clara de que la mentira ya no es una teoría.

**Regla técnica:**

- sólo puede usarse en `Other Side`;
- al usarla, añade `ValveHandle` al inventario y activa `ValveHandleFound`;
- la caja sigue visible como evidencia; no se consume.

**Visual:**

- **Mundo Normal:** `sprite-basement-personal-effects-box-normal`
- **Otra Realidad:** `sprite-basement-personal-effects-box-other`
- **Sprite Sheet:** `spritesheet-basement-personal-effects-box.png` ✅

---

### 5.4 Pipelines

```text
id: "pipelines"
displayName: "Pipelines"
interactionType: Examine
visibleInNormalReality: true
visibleInOtherReality: true
resultFlag: BasementPipelinesExplored
```

**Propósito:**

- mostrar la infraestructura que contiene la presión emocional y narrativa;
- reforzar la sensación de espacio mecánico y claustrofóbico;
- guiar al jugador hacia la necesidad de reparar el sistema.

**Regla técnica:**

- sin linterna activa, los contornos son borrosos y la información es parcial;
- con linterna, los detalles de las tuberías y su estado se vuelven legibles.

**Visual:**

- **Mundo Normal:** `sprite-basement-pipelines-normal.png`
- **Otra Realidad:** `sprite-basement-pipelines-other.png`
- **Sprite Sheet:** `spritesheet-basement-pipelines.png` ✅

---

### 5.5 ValveHandle

`ValveHandle` es un item de inventario obtenido al usar `PersonalEffectsBox` en `Other Side`.

**Regla técnica:**

- no existe como pickup separado en la room;
- al usarlo sobre `PressureReleaseValve`, se consume del inventario;
- el GameObject visual `ValveHandle` se activa junto a la válvula sólo tras el uso correcto;
- el interactable activo sigue siendo `PressureReleaseValve`.

**Visual:**

- **Mundo Normal:** `sprite-basement-valve-handle-normal.png` ✅

---

### 5.6 PressureReleaseValve

```text
id: "pressure_release_valve"
displayName: "Pressure Release Valve"
interactionType: Use
visibleInNormalReality: true
visibleInOtherReality: false
requiredItemId: "valve_handle"
consumeRequiredItemOnUse: true
resultFlags: 
  - PressureValveRepaired
  - SteamEnabled
requiresLantern: true
```

**Propósito:**

- es el centro de la resolución de la room;
- simboliza la liberación de una culpa o presión que ya no puede contenerse;
- activa la desaparición o retroceso de la `Manifestación de Negación`.

**Reglas técnicas:**

- sin `LanternActive == true`, el uso se bloquea con feedback textual;
- si falta la `valve_handle`, la interacción ofrece una pista pero no se resuelve;
- cuando se usa correctamente, se activa una reacción visual y sonora de alivio/descarga.

**Estado del Vapor:**

Cuando la válvula se activa correctamente:

- `SteamEnabled` = `true`
- Varias tuberías del `Otro Lado` comienzan a expulsar vapor a presión.
- El vapor se mantiene activo mientras las presión permanezca liberada.
- La `Manifestación de Negación` entra en estado de retirada o dispersión.

**Visual:**

- **Mundo Normal:** `sprite-basement-pressure-release-valve-normal.png` ✅ <- Sólo visible en `Normal Reality`
- **SteamPipes:** `spritesheet-basement-steam-pipes.png` ✅
  - **Animación Otro Lado:** 4 sprites en el `spritesheet-basement-steam-pipes.png`

---

### 5.7 AmbulanceLights

```text
id: "ambulance_lights"
displayName: "Ambulance Lights"
interactionType: Examine
visibleInNormalReality: false
visibleInOtherReality: true
resultFlag: AmbulanceMemoryTriggered
```

**Propósito:**

- activar recuerdos fragmentados del día del fallecimiento del padre;
- reforzar el vínculo emocional sin explicar completamente la tragedia;
- apoyar la sensación de que la historia del protagonista está haciendo crisis.

**Regla técnica:**

- solo aparecen en `Other Side`;
- no afectarán a la progresión principal;
- su texto debe ser evocador, no expositivo.

---

### 5.8 ForestExitDoor

```text
id: "forest_exit_door"
displayName: "Forest Exit Door"
interactionType: Examine
visibleInNormalReality: true
visibleInOtherReality: true
resultFlag: ForestExitUnlocked
```

**Propósito:**

- cerrar la room con un avance narrativo y funcional;
- no introducir más puzles ni rutas paralelas.

**Condición:**

- `PressureValveRepaired == true`;
- `PressureValveRepaired == true`;
- `SteamEnabled == true`;
- `DenialManifestationRejected == true`.

**Visual:** No tiene visual, sólo es un colisionador que se activa tras cumplir las condiciones.

---

### 5.9 DenialManifestation

```text
id: "denial_manifestation"
displayName: "Denial Manifestation"
interactionType: StateTrigger
visibleInNormalReality: false
visibleInOtherReality: true
resultFlag: DenialManifestationVisible
```

**Propósito:**

- actuar como bloqueo emocional de la room;
- adaptar el patrón del `wardrobe-monster` al `Basement` sin convertirlo en combate;
- reforzar la idea de que la negación del protagonista es una fuerza activa.

**Regla técnica:**

- sólo aparece en `Other Side`;
- bloquea la salida mientras `PressureValveRepaired == false` o mientras el jugador intenta avanzar sin cumplir la condición;
- desaparece cuando la válvula se activa correctamente y el sistema de presión se equilibra.

**Visual:** `spritesheet-basement-denial-manifestation.png` ✅ <- Dos sprites para la animación loop

---

### 5.10 LibraryExitDoor

```text
id: "library_exit_door"
displayName: "Library Exit Door"
interactionType: Transition
linkedRoomId: "Library"
visibleInNormalReality: true
visibleInOtherReality: true
```

**Propósito:**

- permitir la transición desde el sótano a la biblioteca

**Visual:** `sprite-basement-library-exit-door.png` ✅ <- Es un sprite de un mensaje con el texto "Back".

---

## 6. Regla de interacción con oscuridad

Para mantener el sistema sin crear scripts ad hoc, el `InteractionManager` debe aceptar dos condiciones generales:

```text
If interactionType == Examine
    If interactable.requiresLantern == true and LanternActive == false
        show obscured description
    Else
        show full description

If interactionType == Use
    If interactable.requiresLantern == true and LanternActive == false
        deny interaction
        show feedback: "No puedes hacer esto a ciegas"
        trigger blocker if relevant
    Else
        resolve use normally
```

Esto permite que la room tenga lógica clara sin duplicar un comportamiento especial para cada objeto.

---

## 7. Flags y progreso

| Flag | Descripción | Origen |
| --- | --- | --- |
| BasementEntered | El jugador entra en el Basement | `basement_entry_door` |
| BasementShelfLeftExplored | El almacén izquierdo ha sido revisado | `storage_shelf_left` |
| BasementShelfRightExplored | El almacén derecho ha sido revisado | `storage_shelf_right` |
| BasementEvidenceFound | La evidencia principal fue identificada | `personal_effects_box` |
| ValveHandleFound | La pieza de reparación fue localizada | `valve_handle` |
| PressureValveRepaired | La válvula se usa correctamente | `pressure_release_valve` |
| SteamEnabled | El sistema libera vapor en el `Otro Lado` | `pressure_release_valve` |
| DenialManifestationVisible | La manifestación bloquea la salida | `denial_manifestation` |
| DenialManifestationRejected | El vapor ha rechazado a la manifestación | `pressure_release_valve` |
| BasementCodeUsed | El código fue usado en el panel | `code_panel` |
| BasementExitUnlocked | La salida queda accesible | `basement_exit` |
| BasementResolved | Room completada | al cruzar los estados de evidencia + válvula |

### Regla de cierre

```text
If BasementEvidenceFound == true
   and PressureValveRepaired == true
   and SteamEnabled == true
then BasementResolved = true
```

---

## 8. Reglas narrativas de la room

- La room debe dar una sensación clara de “esto ya no es decoración”.
- El jugador debe entender que el sótano guarda lo que el protagonista intenta no aceptar.
- `Examine` sin linterna debe ser ambiguo, confuso y emocionalmente defensivo.
- `Examine` con linterna debe ser directo, concreto y doloroso.
- La salida no debe sentirse como una recompensa superficial: debe ser la consecuencia de que el personaje acepta una verdad que le duele.

---

## 9. VFX y SFX recomendados

Estos eventos son narrativos, no sistemas nuevos:

- `SteamBurst` en las tuberías;
- `DripEvent` puntual para dramatizar la humedad;
- `AmbulanceSirenFlash` en `Other Side`;
- ruido de presión y liberación en la válvula cuando se activa.

No conviene crear una “máquina de efectos” para todo. Una línea temporal minimalista y unos pocos triggers son suficientes.

### SteamBurst

Elemento visual principal del Basement.

**Implementación MVP:**

- 4 sprites reutilizables
- Animación en bucle
- Transparencia parcial
- Desplazamiento vertical lento
- Color blanco grisáceo
- Sin sistema de Partículas complejo (sin generar partículas individuales)

**Activación:**

- `PressureValveRepaired` == `true`
- `Reality` == `OtherSide`

**Objetivo:**

- Comunicar liberación de presión
- Reforzar la transformación del espacio
- Dirigir la atención hacia la desaparición de la `Manifestación de Negación`

---

## 10. Scope del MVP

### Mantener

- 1 evidencia central;
- 1 pieza de reparación;
- 1 activación principal (`PressureReleaseValve`);
- 1 bloqueo emocional (`DenialManifestation`);
- reglas de luz por room;
- 2-3 interacciones/importantes entre el inicio y la salida.

### Simplificar

- sin batería;
- sin persecución;
- sin IA compleja;
- sin más de una estructura de presión visible;
- sin añadir nuevas rutas o mini-bosses.

### Dejar fuera

- sistema de batería;
- combate o chase sequence;
- extensión del `Huerto`;
- lógica de varias cuerdas de progreso simultáneas.

---

## 11. Resumen técnico

El `Basement` debe implementarse como una `Room` de baja luz, de pocas interacciones y alto impacto emocional. La `Linterna` se vuelve una herramienta real en esta room, la `Caja de Efectos Personales` actúa como evidencia material, la `Manivela` ofrece la pieza de resolución y la `Válvula` cierra la arcada principal. La `Manifestación de Negación` funciona como bloqueo simbólico y psicológico, no como enemigo. Con esto, la room queda potente, compacta y alineada con la intención narrativa del proyecto.
