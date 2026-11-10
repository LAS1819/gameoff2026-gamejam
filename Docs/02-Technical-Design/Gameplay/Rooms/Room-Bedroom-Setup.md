# Bedroom Room Setup - Unity Hierarchy

- Versión 0.1
- Creado: 10/11/2026
- Actualizado: 10/11/2026

---

## Objetivo

Este documento define la estructura exacta recomendada para la room `Bedroom` en Unity, manteniendo el patrón actual del proyecto y evitando mezclar la definición del contenido con la lógica del runtime.

La idea es simple:

- la Room vive como un `GameObject` hijo de `Rooms`;
- cada interactable vive dentro de esa room;
- la data de la room sigue en `RoomData` / `InteractableData`;
- la visual solo se encarga de mostrar y escuchar clics.

---

## Jerarquía recomendada

```text
Rooms
└── bedroom
    ├── Background
    │   ├── room_bg
    │   ├── wall_detail_01
    │   ├── wall_detail_02
    │   ├── window_normal
    │   └── window_other_side
    ├── Interactables
    │   ├── bed
    │   ├── nightstand
    │   ├── roof-lamp
    │   ├── roof-lamp-switch
    │   ├── wardrobe
    │   ├── window
    │   ├── hallway_exit
    │   ├── flashlight_tool
    │   └── library_key
    ├── Effects
    │   ├── light_volume
    │   ├── monster_blocker
    │   └── reality_noise
    └── Debug
        └── room_label
```

### Regla de nombres

Mantener nombres estables y muy claros:

- `bedroom` = GO principal de la room
- `bed` = cama
- `nightstand` = mesita de noche
- `roof-lamp` = lámpara de habitación
- `roof-lamp-switch` = interruptor del techo
- `wardrobe` = armario
- `window` = ventana
- `hallway_exit` = salida hacia el pasillo
- `flashlight_tool` = herramienta de linterna (si se usa como objeto visible en UI o placeholder visual)
- `library_key` = llave que se obtiene en el Otro Lado

---

## Qué debe ir dentro de cada interactable

Cada GO interactuable debe llevar como mínimo:

- `SpriteRenderer`
- `Collider2D`
- `InteractableBehaviour`

Y debe tener un `InteractableData` asociado con:

- `id`
- `displayName`
- `description`
- `interactionType`
- `visibleInNormalReality`
- `visibleInOtherReality`
- `requiredItemId` (si aplica)
- `resultFlag`
- `linkedRoomId` (si aplica)
- `itemId` (si recoge un item)
- `consumeAfterUse`

---

## Patrón de configuración para Bedroom

### 1. `bedroom`

- `id = "bedroom"`
- `displayName = "Bedroom"`
- `connectedRoomIds = [ "hallway" ]`

### 2. `wardrobe`

- `id = "wardrobe"`
- `interactionType = InteractionType.Examine` o `Use`
- `visibleInNormalReality = true`
- `visibleInOtherReality = true`
- `resultFlag = "BedroomWardrobeOpened"` o equivalente

### 3. `roof-lamp`

- `id = "roof-lamp"`
- `interactionType = InteractionType.Toggle`
- `visibleInNormalReality = true`
- `visibleInOtherReality = false`
- `resultFlag = "BedroomLightOn"`

### 4. `roof-lamp-switch`

- `id = "roof-lamp-switch"`
- `interactionType = InteractionType.Toggle`
- `visibleInNormalReality = true`
- `visibleInOtherReality = false`
- `resultFlag = "BedroomLightOn"`

### 5. `nightstand`

- `id = "nightstand"`
- `interactionType = InteractionType.Examine`
- `visibleInNormalReality = true`
- `visibleInOtherReality = true`

### 6. `window`

- `id = "window"`
- `interactionType = InteractionType.Examine`
- `visibleInNormalReality = true`
- `visibleInOtherReality = true`

### 7. `hallway_exit`

- `id = "hallway_exit"`
- `interactionType = InteractionType.Transition`
- `linkedRoomId = "hallway"`
- `visibleInNormalReality = true`
- `visibleInOtherReality = false`

### 8. `library_key`

- `id = "library_key"`
- `interactionType = InteractionType.Pickup`
- `itemId = "library_key"`
- `visibleInNormalReality = false`
- `visibleInOtherReality = true`
- `resultFlag = "HasLibraryKey"`
- `consumeAfterUse = true`

### 9. `flashlight_tool`

No debe ser un “pickup” clásico si estamos siguiendo el modelo de linterna permanente.

Debería tratarse como:

- un recurso persistente en UI;
- un estado o flag tipo `HasFlashlight`;
- un único interactuable que se activa cuando el jugador lo quiere usar.

---

## Estado visual recomendado

La room necesita dos capas visuales claras:

### Normal Reality

- luz cálida;
- mobiliario doméstico;
- objetos cotidianos visibles;
- ventana intacta;
- armario sin amenaza activa;
- la lámpara o interruptor se perciben como ayudas.

### Other Side

- la ventana está bloqueada o tapiada;
- la habitación conserva la misma geometría, pero con más tensión;
- el armario puede tener presencia de monstruo si la luz está apagada;
- la llave aparece de forma más extraña o más peligrosa;
- la luz se vuelve esencial para evitar la amenaza.

---

## Reglas de MVP

Para el MVP de Bedroom es recomendable mantener:

- 1 room principal (`bedroom`);
- 6 a 8 interactables máximo;
- la lógica de luz y monstruo muy legible;
- la llave en el armario del Otro Lado;
- la linterna como herramienta persistente, no como inventario tradicional;
- la amenaza del monstruo como bloqueo de interacción, no como enemigo complejo.

---

## Plantilla de RoomData para Bedroom

```csharp
var bedroom = new RoomData
{
    id = "bedroom",
    displayName = "Bedroom",
    connectedRoomIds = new List<string> { "hallway" }
};

bedroom.interactables.Add(new InteractableData
{
    id = "wardrobe",
    displayName = "Wardrobe",
    description = "An old wardrobe that hides something important.",
    interactionType = InteractionType.Examine,
    visibleInNormalReality = true,
    visibleInOtherReality = true,
    resultFlag = "BedroomWardrobeOpened"
});
```

Esto mantiene la lógica simple y clara, sin mezclar 2 fuentes de verdad.

---

## Conclusión

La Bedroom debe seguir el mismo patrón que las rooms actuales:

- un GO `bedroom` dentro de `Rooms`;
- los interactables como hijos con `InteractableBehaviour`;
- la data en `RoomData` y `InteractableData`;
- visibilidad y lógica condicionada por realidad;
- la room no debe ser un “sistema gigante”, sino un espacio claro y pequeño con lógica muy reconocible.

Con esto, cualquier nueva room se puede crear siguiendo el mismo esquema sin romper la estructura actual del juego.
