# MVP Sprint 2 - Bedroom y flujo de rooms funcional

- Fecha inicio: 09/11/2026
- Fecha objetivo: 15/11/2026
- Estado: 🟡 PLANIFICADO - Base de Lobby/Hallway validada, Bedroom como prioridad MVP
- Creado: 10/11/2026
- Actualizado: 10/11/2026

## Objetivo

Cerrar el siguiente bloque de progreso del MVP con una Room que añada tensión, lógica narrativa y reaprovechamiento del sistema base ya validado.

La prioridad del sprint es:

- implementar la Bedroom como primer room con riesgo funcional;
- consolidar la lógica de luz + armario + monstruo como bloqueo claro;
- mantener la estructura del proyecto simple y data-driven;
- dejar el flujo Lobby → Hallway → Bedroom para ser ejecutable y testeable sin sobrecargar el sistema.

No se busca contenido completo, ni arte final, ni sistemas secundarios. La idea es que esta room funcione de forma legible y consistente con la mecánica general del juego.

---

## Alcance

### Incluido

- Bedroom room data y interactables
- luz de la habitación en la Realidad Normal
- Flashlight como herramienta permanente en UI
- Wardrobe como eje de bloqueo y progresión
- key hidden en el Otro Lado
- monstruo que bloquea la interacción al abrir el armario sin luz
- transiciones entre rooms sin romper la base del MVP
- validación de flujo de room + flags + realidad

### No incluido

- barra de salud / cordura
- sistema completo de daño o muerte
- UI de batería / carga para la linterna
- puzzle de biblioteca real
- audio final o VFX completos
- más de una room de amenaza funcional

---

## Prioridad de trabajo

### Prioridad 1 — Bedroom MVP core

El objetivo principal del sprint es conseguir una room que se sienta “terminada” a nivel de lógica con un alcance mínimo:

1. Room Bedroom cargada por ID
2. LampTrigger visible y usable solo en Normal
3. Flashlight pickup + UI tool
4. Wardrobe interactable visible en ambas realidades
5. WardrobeKey visible solo en Other Side cuando la luz está encendida
6. Monster bloquea la interacción si se abre el armario en Other Side sin luz
7. Si la luz está encendida, el monstruo desaparece y la llave se vuelve accesible

### Prioridad 2 — Consistencia del flujo

Una vez funcionando la Bedroom, hay que cerrar que la progresión tiene sentido y no se rompe por una room aislada:

- Lobby → Hallway → Bedroom
- Room transitions correctas
- RoomManager conserva estado entre cambios de room
- flags de luz y armario persistentes mientras el jugador se mueve por el building state

### Prioridad 3 — Preparación para la siguiente iteración

- definir una estructura reutilizable para futuras rooms;
- dejar el patrón de `Use` + `Toggle` + `Pickup` validado;
- preparar la base para que Library y Basement lleguen sin rediseñar el sistema.

---

## Requisitos funcionales mínimos

### 1. Estado del juego

- se mantienen `GameManager` + `RoomManager` + `RealityManager` como base del proyecto;
- los flags de la Bedroom deben existir y ser manejados de forma clara:
  - `BedroomLightOn`
  - `FlashlightUnlocked`
  - `WardrobeOpened`
  - `MonsterVisible`
  - `LibraryKeyFound`

### 2. Habitaciones

- Bedroom debe poder cargarse por `roomId` y ser descubierta desde Hallway;
- room transitions no deben romper la visibilidad de interactables ni el estado de realidad;
- cada room debe seguir el patrón de `InteractableData` + `RoomData` ya validado.

### 3. Interacción

- recoger linterna;
- activar lámpara solo en Normal;
- abrir armario;
- obtener llave desde Otro Lado cuando se cumplen las condiciones;
- bloquear la interacción con monstruo si se intenta abrir el armario sin luz.

### 4. Lógica de luz

- la luz se enciende desde la Realidad Normal;
- no se permite encenderla en el Otro Lado;
- si la luz está encendida, la amenaza del monstruo desaparece por completo;
- si la luz está apagada, el armario en Other Side presenta bloqueo.

### 5. SHIFT / realidad

- la room debe seguir apoyándose en el sistema de realidad actual;
- el jugador debe poder hacer SHIFT y volver a Normal sin perder el estado del armario ni la luz;
- la amenaza debe responder a la realidad y a la luz, no a una navegación aislada.

---

## Plan de trabajo por bloques

### Bloque A — Bedroom data + room structure (45-60 min)

- [ ] crear `RoomData` de Bedroom con los interactables base
- [ ] definir `InteractableData` para:
  - `bedroom_lamp`
  - `flashlight_pickup`
  - `wardrobe`
  - `wardrobe_key`
  - `wardrobe_monster`
  - `bedroom_exit`
- [ ] definir `RoomConnection` y `linkedRoomId` para volver a Hallway
- [ ] asegurar visibilidad por realidad con los valores correctos

### Bloque B — Flashlight + UI tool (45-60 min)

- [ ] crear el item `flashlight`
- [ ] hacer que no se comporte como inventario clásico
- [ ] conectar a UI permanente o HUD button
- [ ] validar que se activa correctamente y se mantiene disponible

### Bloque C — Lamp + Wardrobe gating (60-90 min)

- [ ] `bedroom_lamp` cambia `BedroomLightOn`
- [ ] solo usable en Normal
- [ ] `wardrobe` abre y registra `WardrobeOpened`
- [ ] `wardrobe_monster` aparece solo si `WardrobeOpened && OtherSide && !BedroomLightOn`
- [ ] cierre del armario/monstruo al encender la luz

### Bloque D — Key reward + flow validation (45-60 min)

- [ ] `wardrobe_key` visible solo cuando se cumplen condiciones
- [ ] recoger key actualiza `LibraryKeyFound`
- [ ] key queda visible en el flujo real y no rompe la room
- [ ] probar retorno a Hallway y acceso posterior si se requiere

### Bloque E — End-to-end validation (30-45 min)

- [ ] entrar a Bedroom desde Hallway
- [ ] recoger linterna
- [ ] encender la luz en Normal
- [ ] cambiar a Other Side
- [ ] abrir armario con luz encendida para acceder a la llave
- [ ] volver a la habitación y comprobar que la lógica no se desincroniza
- [ ] volver a Hallway sin errores
- [ ] validar consola y flags

---

## Regla de diseño para el sprint

### Mantener

- la estructura simple de rooms basada en data;
- la luz como acción clave y no como extra opcional;
- la amenaza del monstruo como bloqueo circunstancial, no como enemigo completo;
- la lógica del armario como punto de decisión claro;
- una room con un punto fuerte de narración y un objetivo concreto.

### Simplificar

- sin sistema de salud o cordura;
- sin batería compleja para la linterna;
- sin varias condiciones de monstruo;
- sin añadir lógica de combat o IA extra;
- sin crear más triggers que los necesarios para la room.

### Dejar fuera del sprint

- segunda amenaza de terror distinta;
- puzzles adicionales dentro de la Bedroom;
- implementación de objetos de biblioteca reales;
- save system, checkpoints complejos o sistema de muerte.

---

## Criterios de aceptación del sprint

La Room Bedroom se considera correcta si se cumple la siguiente prueba de flujo:

1. El jugador entra a Bedroom desde Hallway.
2. Encuentra y recoge la linterna.
3. Se activa como tool permanente en UI.
4. El jugador enciende la lámpara en la Realidad Normal.
5. Cambia a la otra realidad.
6. Abre el armario y, si la luz está encendida, el monstruo no aparece y la llave queda accesible.
7. Si prueba abrir el armario con la luz apagada, el monstruo bloquea la interacción.
8. Vuelve a la Realidad Normal, enciende la lámpara y resuelve la condición.
9. La llave puede obtenerse en el Otro Lado y la room funciona sin desincronizar flags.
10. El flujo completo no rompe la estructura del proyecto ni requiere lógica extra paralela.

---

## Checklist de cierre del sprint

### Bloqueantes

- [ ] Bedroom room data creada
- [ ] Flashlight tool disponible en UI
- [ ] BedroomLamp solo funciona en Normal
- [ ] Wardrobe muestra lógica variable por realidad
- [ ] MonsterVisible solo se activa con la condición correcta
- [ ] WardrobeKey aparece solo cuando la luz está encendida
- [ ] no existen errores de room loading ni flags inconsistentes

### Funcional

- [ ] SHIFT sigue funcionando al cambiar entre realidades
- [ ] exit de Bedroom vuelve correctamente a Hallway
- [ ] el flujo Lobby → Hallway → Bedroom es reproducible
- [ ] la lógica del armario es clara y no requiere “trucos” del diseñador

### Scope guard

- [ ] no se añade salud por ahora
- [ ] no se inicia trabajo de Library real
- [ ] la room se mantiene dentro del enfoque MVP
- [ ] no se crea infraestructura de más para un sistema futuro inmediato

---

## Recomendación técnica final

El Sprint 2 debe centrarse en una sola idea sólida:

la Bedroom es la primera Room donde las reglas del juego se vuelven más humanas, más emocionales y más peligrosas, pero sin dejar de ser manejables.

Por eso, la prioridad no es añadir complejidad, sino dejar bien resuelto el bloque clave:

- luz en Normal;
- monstruo en Other Side si el armario se abre sin luz;
- llave en armario como recompensa y progresión;
- flujo limpio entre rooms y realidades.

Si esta parte funciona, la base del proyecto queda mucho más fuerte para el siguiente bloque de contenido y para la siguiente iteración de diseño.
