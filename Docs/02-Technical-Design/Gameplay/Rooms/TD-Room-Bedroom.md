# Room Bedroom — Technical Spec

- Versión 0.2
- Creado: 10/11/2026
- Actualizado: 10/11/2026

---

## 1. Objetivo de la Room

La Bedroom es la primera Room del juego diseñada como espacio íntimo, emocional y de descubrimiento. Aquí se prueba algo más que progresión: se introduce la relación entre memoria, amenaza y luz.

Su función técnica es:

- presentar una Room personal y específica del protagonista;
- introducir la luz como solución de seguridad y lectura psicológica;
- activar la recolección de un item clave para una futura puerta de biblioteca;
- definir un patrón de amenaza reactiva basado en el armario y en el Otro Lado;
- sentar la base para la mecánica de luz sin introducir salud en el MVP.

Es la primera Room que mezcla progresión narrativa y bloqueo de juego.

---

## 2. Definición de RoomData

```text
RoomData
- id: "bedroom"
- displayName: "Bedroom"
- connectedRoomIds: ["hallway"]
- interactables:
  - bedroom_lamp
  - flashlight_pickup
  - wardrobe
  - wardrobe_key
  - bedroom_exit
  - window
  - wardrobe_monster (visual blocker)
```

### Estado inicial

- Reality: Normal
- La habitación se ve como un cuarto personal y familiar.
- La lámpara está apagada por defecto.
- La linterna no está desbloqueada todavía.
- El armario no ofrece nada en la Realidad Normal.
- El monstruo sólo aparece como bloqueo contextual al abrir el armario en el Otro Lado si la luz no está encendida.
- No se incluye sistema de salud en este MVP.

---

## 3. Interactables principales

### 3.1 RoofLamp / RoofLampSwitch

```text
id: "roof-lamp"
displayName: "Roof Lamp"
interactionType: Toggle
visibleInNormalReality: true
visibleInOtherReality: true
allowedReality: Normal
resultFlag: BedroomLightOn
```

#### Propósito

- permitir iluminar la habitación;
- generar una regla clara: la luz reduce la amenaza;
- servir de ancla para la lógica del Otro Lado.

#### Reglas técnicas

- El interruptor sólo puede usarse en la Realidad Normal.
- `BedroomLightOn` es un flag booleano.
- Si `BedroomLightOn == true`, entonces el monstruo no aparece al abrir el armario en el Otro Lado.
- si el monstruo ya apareció y luego se enciende la luz en la Realidad Normal, el monstruo desaparece.

Esto debe quedar muy claro en diseño: la luz no es opcional; es la condición para resolver el bloqueo del armario.

---

### 3.2 FlashlightPickup

```text
id: "flashlight_pickup"
displayName: "Flashlight"
interactionType: Pickup
itemId: "flashlight"
visibleInNormalReality: true
visibleInOtherReality: false
resultFlag: FlashlightUnlocked
consumeAfterUse: true
```

#### Propósito

- introducir la linterna como herramienta permanente;
- evitar que se comporte como un objeto de inventario clásico;
- dejarla disponible en la UI como recurso activo.

#### Reglas técnicas

- La linterna no debe entrar en el inventario convencional.
- Debe existir como herramienta permanente (“tool slot” / HUD button). 
- Idealmente:
  - `FlashlightUnlocked = true`
  - `FlashlightActive` = toggle del jugador
  - `FlashlightBattery` = opcional, pero no necesario en MVP

#### Recomendación MVP

- Mantener solo “recogida + activación”, sin batería compleja.
- La linterna es un recurso constante, no un ítem de recolección más.

---

### 3.3 Wardrobe

```text
id: "wardrobe"
displayName: "Wardrobe"
interactionType: Use
visibleInNormalReality: true
visibleInOtherReality: true
resultFlag: WardrobeOpened
```

#### Propósito

- servir como punto central de tensión de la Room;
- activar la lógica de amenaza cuando el jugador explora el Otro Lado;
- ser la entrada de la llave de la librería.

#### Reglas técnicas

- El armario es visible en ambas realidades.
- En la Realidad Normal, abrirlo no tiene consecuencias especiales. Solo funciona como observación o como preparación del espacio.
- En el Otro Lado la lógica cambia por completo:
  - si `BedroomLightOn == false`: aparece el monstruo y bloquea la interacción;
  - si `BedroomLightOn == true`: no aparece el monstruo y la llave puede ser obtenida;
  - si el monstruo ya apareció y luego se enciende la luz en la Realidad Normal, el monstruo desaparece al volver al Otro Lado.

#### Resultado clave

Esta es la mecánica principal de la Room: la amenaza no se activa “por estar en Otro Lado”, sino por abrir el armario en esa realidad con la luz apagada.

---

### 3.4 WardrobeKey

```text
id: "wardrobe_key"
displayName: "Wardrobe Key"
interactionType: Pickup
itemId: "library_key"
visibleInNormalReality: false
visibleInOtherReality: true
requiresCondition: WardrobeOpened && BedroomLightOn
resultFlag: LibraryKeyFound
consumeAfterUse: true
```

#### Propósito

- esconder la llave de la librería en el Otro Lado;
- convertir el armario en el punto de riesgo y recompensa de la Room;
- dar una razón clara para volver a la habitación y resolver la amenaza.

#### Reglas técnicas

- En la Realidad Normal no debería verse.
- En el Otro Lado sólo debe aparecer si el armario ha sido abierto y la luz de la habitación está encendida.
- Si el monstruo está activo, la llave no debe volverse visible ni interactuable.
- Al recogerla, se activa `LibraryKeyFound = true`.

---

### 3.5 BedroomExit

```text
id: "bedroom_exit"
displayName: "Bedroom Exit"
interactionType: Transition
linkedRoomId: "hallway"
visibleInNormalReality: true
visibleInOtherReality: true (pero inaccesible)
```

#### Propósito

- salir de la habitación y volver al corredor principal;
- mantener la continuidad del recorrido;
- no convertir la Bedroom en una zona aislada.

---

### 3.6 Window

```text
id: "window"
displayName: "Window"
interactionType: Examine
visibleInNormalReality: true
visibleInOtherReality: false
```

#### Propósito

- servir como elemento de lectura narrativa;
- reforzar la sensación de que el Otro Lado ya no es un “mundo disponible” sino una réplica distorsionada del espacio.

#### Reglas técnicas

- Puede mantenerse como interactable de observación.
- En el Otro Lado, la ventana debe estar tapiada o bloqueada.
- La lógica no necesita complejidad: solo debe verse claramente que la realidad cambió.

---

### 3.7 WardrobeMonster / ThreatBlocker

```text
id: "wardrobe_monster"
displayName: "Monster"
interactionType: StateTrigger
visibleInNormalReality: false
visibleInOtherReality: true
resultFlag: MonsterVisible
```

#### Propósito

- representar la amenaza específica que bloquea la interacción cuando se abre el armario en el Otro Lado sin luz.
- actuar como bloqueo visual y de progresión, no como enemigo con IA compleja.

#### Reglas técnicas

- El monstruo no aparece por defecto en el Otro Lado.
- Sólo se activa cuando el jugador abre el armario en esa realidad y la luz está apagada.
- Si la luz se enciende en la Realidad Normal, el monstruo desaparece al volver al Otro Lado.
- El monstruo bloquea la interacción del armario y la obtención de la llave mientras esté activo.

---

## 4. Items del Room

### 4.1 Flashlight

```text
ItemData
- id: "flashlight"
- displayName: "Flashlight"
- type: Tool
- inventoryMode: PermanentUI
- canToggle: true
```

#### Reglas

- Debe aparecer en la UI como acceso rápido.
- No debe entrar al inventario clásico.
- Debe activarse siempre que el jugador quiera iluminar la habitación.

### 4.2 LibraryKey

```text
ItemData
- id: "library_key"
- displayName: "Library Key"
- type: Key
- inventoryMode: Standard
- useContext: "library_door"
```

#### Reglas

- Recolección secundaria, pero importante para continuidad.
- Debe relacionarse con la futura biblioteca y con la lógica del armario.

---

## 5. Flags y estado de progresión

| Flag | Descripción | Origen |
| ------ | ------------- | -------- |
| BedroomLightOn | La luz de la habitación está encendida | `roof-lamp` |
| FlashlightUnlocked | Se ha recogido la linterna | `flashlight_pickup` |
| WardrobeOpened | El jugador ha abierto el armario | `wardrobe` |
| MonsterVisible | El monstruo está bloqueando la interacción | `wardrobe_monster` |
| LibraryKeyFound | Se ha recogido la llave de la librería | `wardrobe_key` |

### Regla técnica sugerida

```text
If currentReality == OtherSide
    If WardrobeOpened == true and BedroomLightOn == false
        MonsterVisible = true
        WardrobeKey cannot be interacted with

    If WardrobeOpened == true and BedroomLightOn == true
        MonsterVisible = false
        WardrobeKey becomes interactable

If currentReality == Normal and BedroomLightOn == true
    MonsterVisible = false
```

La lógica debe ser simple y legible. El monstruo no es un enemigo general; es una restricción de armario.

---

## 6. Sistema de bloqueo y resolución (sin salud)

### Regla de MVP

- No se incluye salud ni cordura por ahora.
- El monstruo no daña; simplemente bloquea la interacción y obliga al jugador a resolver la luz.

#### Flujo exacto

```text
Player opens Wardrobe in Normal
→ nothing happens

Player switches to OtherSide
→ opens Wardrobe with light OFF
→ MonsterVisible = true
→ key hidden / blocked

Player returns to Normal
→ turns on roof-lamp
→ returns to OtherSide
→ MonsterVisible = false
→ key becomes visible

Player picks up key
→ LibraryKeyFound = true
```

Esto mantiene la Room clara, peligrosa y manejable sin sobrecargar el sistema.

---

## 7. Reglas de visibilidad por realidad

### Normal Reality

- `roof-lamp` visible
- `flashlight_pickup` visible
- `wardrobe` visible
- `window` visible
- `wardrobe_key` oculto
- `wardrobe_monster` oculto
- `bedroom_exit` visible

### Other Side

- `roof-lamp` visible, pero no usable para encenderla
- `flashlight_pickup` oculto
- `wardrobe` visible
- `window` bloqueada / tapiada
- `wardrobe_key` oculto hasta que la luz esté encendida
- `wardrobe_monster` activo solo cuando el armario se abre con la luz apagada
- `bedroom_exit` visible pero inaccesible

La clave aquí es que la habitación mantenga la misma composición general, pero cambie el estado emocional y la lógica del riesgo.

---

## 8. Flujo técnico

```text
Load Room bedroom
→ player enters room in Normal
→ finds flashlight and picks it up
→ flashlight becomes permanent UI tool
→ player turns on roof-lamp in Normal
→ player switches to OtherSide
→ opens wardrobe
→ if lamp is ON: no monster, key can appear
→ if roof-lamp is OFF: monster appears and blocks the interaction
→ player returns to Normal and turns roof-lamp on
→ returns to OtherSide
→ monster disappears
→ key becomes collectible
→ player picks up Library Key
```

---

## 9. MVP Scope recomendado

### Mantener

- bedroom as a real memory room
- flashlight as permanent UI tool
- lamp as main safety trigger
- wardrobe as central interactable
- monster blocking only via wardrobe in OtherSide
- no health system in this iteration

### Simplificar

- sin batería compleja
- sin varias fases del monstruo
- sin amenaza persistente fuera del armario
- sin muerte instantánea o sistemas de daño

### Dejar fuera del MVP

- más de un tipo de monstruo
- puzzles adicionales dentro de la habitación
- más de una ruta de salida
- animaciones complejas de amenaza

---

## 10. Estructura sugerida en escena

```text
Bedroom
├── Visuals
│   ├── BackgroundNormal
│   └── BackgroundOther
├── Interactables
│   ├── BedroomLamp
│   ├── FlashlightPickup
│   ├── Wardrobe
│   ├── WardrobeKey
│   ├── Window
│   ├── BedroomExit
│   └── WardrobeMonster
├── UI
│   └── FlashlightButton
``` 

Esta estructura deja la room muy clara: visual + interactable principal + bloqueo + herramienta permanente.

---

## 11. Resumen técnico

La Bedroom es la Room de memoria, presión psicológica y resolución de peligro. Desde el punto de vista técnico, es la primera zona que combina:

- una herramienta permanente en UI;
- una luz con activación restringida al Mundo Normal;
- un armario como punto de tensión y lógica de progresión;
- un monstruo que bloquea solo al abrir el armario en el Otro Lado sin luz;
- una llave clave escondida en la solución del problema.

Es una Room muy útil para el MVP porque tiene lógica clara, riesgo legible y un objetivo específico sin necesidad de sobrecargar el sistema.
