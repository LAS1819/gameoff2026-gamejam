# TD-HUD - UI mínima del MVP con Panel Renderer

- **Versión:** 1.1
- **Creado:** 2026-11-06
- **Actualizado:** 2026-11-07

## Objetivo

Diseñar un HUD mínimo, estable y reutilizable para la demo del sprint 1 sin añadir una segunda arquitectura de UI.

La idea es:

- mostrar inventario básico,
- dar feedback textual al jugador,
- integrar `SHIFT` en el mismo sistema,
- mantener el patrón ya validado por `PanelRenderer` y `UIShiftButton`.

## Regla base

No usamos `UIDocument` runtime para este caso. La UI del MVP debe seguir este patrón:

- `PanelRenderer` en un GameObject raíz
- `VisualTreeAsset` para estructura
- `USS` para estilos
- binding por `RegisterUIReloadCallback()`
- actualización por eventos, no por `Update()`

Esto es más estable para una jam y encaja con la arquitectura actual del proyecto.

## Archivos recomendados

- `Assets/UI/HUD.uxml`
- `Assets/UI/HUD.uss`
- `Assets/Scripts/UI/HUDController.cs`
- `Assets/Scripts/UI/UIShiftButton.cs` (se conserva, pero trabaja dentro del HUD)

## Jerarquía visual sugerida

Importante: con `PanelRenderer`, los bloques no son `GameObjects` separados dentro de la escena. Lo normal es tener un único `GameObject` con `PanelRenderer` y dentro de ese árbol visual crear los `VisualElement`s: `InventoryPanel`, `slot-*`, `feedback-text`, `shift-button`, etc.

```text
HUDRoot (GameObject con PanelRenderer + HUDController)
├── InventoryPanel (VisualElement)
│   ├── slot-0 (VisualElement)
│   ├── slot-1 (VisualElement)
│   └── slot-2 (VisualElement)
├── FeedbackText (Label)
├── ShiftButton (VisualElement)
├── TransitionOverlay (VisualElement)
└── RealityPulse (VisualElement)
```

### Componentes recomendados por bloque

- `HUDRoot`
  - `PanelRenderer`
  - `HUDController` (script)
  - `PanelSettings` asignado en el `PanelRenderer`
  - `VisualTreeAsset` asignado al `PanelRenderer`

- `InventoryPanel`
  - `VisualElement` (sin collider ni SpriteRenderer)
  - contenedor del inventario
  - generalmente sin script propio

- `slot-0`, `slot-1`, `slot-2`
  - `VisualElement` (sin collider ni SpriteRenderer)
  - pueden tener estilo de fondo, borde y tamaño fijo
  - si luego se añaden iconos, se usan `background-image` o un `Image` dentro del slot

- `FeedbackText`
  - `Label` (`UnityEngine.UIElements.Label`)
  - texto temporal para mensajes del juego
  - no necesita `TextMeshPro` ni `Text` nativo para este caso

- `ShiftButton`
  - `VisualElement` con `RegisterCallback<ClickEvent>()`
  - opcionalmente script `UIShiftButton` para la lógica de validación
  - no necesita `Button` nativo ni `BoxCollider`

- `TransitionOverlay`
  - `VisualElement`
  - fondo negro o fade, con clase `.active`
  - se activa solo durante la transición entre rooms

- `RealityPulse`
  - `VisualElement`
  - flash corto al cambiar de realidad
  - no necesita `Image` ni `SpriteRenderer` por sí mismo

## Qué hace cada bloque

- `InventoryPanel`: muestra objetos recogidos, con slots fijos para la demo.
- `FeedbackText`: texto temporal para confirmar acciones.
- `ShiftButton`: botón de cambio de realidad, integrado en el mismo HUD.
- `TransitionOverlay`: fade entre rooms.
- `RealityPulse`: flash pequeño al cambiar de realidad.

## Patrón de runtime

El `HUDController` será el punto central de la UI.

Debe:

- suscribirse a eventos de `InventoryManager`, `RoomManager` y `RealityManager`
- enlazar los elementos del árbol visual tras cada recarga del panel
- actualizar inventario y estado visual sin depender de `Update()`

### Eventos clave

- `InventoryManager.ItemAdded`
- `InventoryManager.ItemRemoved`
- `RoomManager.RoomChanged`
- `RealityManager.RealityChanged`

### Flujo de binding

```csharp
private void HandlePanelReloaded(PanelRenderer renderer, VisualElement root, int version)
{
    inventorySlots = new List<VisualElement>
    {
        root.Q<VisualElement>("slot-0"),
        root.Q<VisualElement>("slot-1"),
        root.Q<VisualElement>("slot-2")
    };

    feedbackText = root.Q<Label>("feedback-text");
    transitionOverlay = root.Q<VisualElement>("transition-overlay");
    realityPulse = root.Q<VisualElement>("reality-pulse");
    shiftButton = root.Q<VisualElement>("shift-button");

    RefreshInventory();
}
```

## Integración con el SHIFT actual

La clave aquí es que el `shift-button` no es una UI aislada. Debe formar parte del mismo HUD. El patrón correcto es:

- `HUDController` publica/actualiza el visual del botón
- `UIShiftButton` solo valida el click y ejecuta `RealityManager.Instance.ToggleReality()`
- la lógica visual queda en el controller, no en un script adicional de UI

Importante: si estamos usando `PanelRenderer`, no debemos esperar `OnMouseDown()` sobre un `MonoBehaviour` para el botón. La interacción real debe hacerse por `ClickEvent` sobre el `VisualElement` o por un callback del mismo `UIShiftButton` preparado para runtime UI.

## Estructura visual mínima

### HUD.uxml

```xml
<ui:UXML xmlns:ui="UnityEngine.UIElements">
    <ui:VisualElement name="hud-root">
        <ui:VisualElement name="inventory-panel">
            <ui:VisualElement name="slot-0" class="inventory-slot empty" />
            <ui:VisualElement name="slot-1" class="inventory-slot empty" />
            <ui:VisualElement name="slot-2" class="inventory-slot empty" />
        </ui:VisualElement>

        <ui:Label name="feedback-text" class="feedback-text" text="" />
        <ui:VisualElement name="shift-button" class="shift-button normal" />
        <ui:VisualElement name="transition-overlay" class="transition-overlay" />
        <ui:VisualElement name="reality-pulse" class="reality-pulse" />
    </ui:VisualElement>
</ui:UXML>
```

### HUD.uss

```css
#hud-root {
    position: absolute;
    inset: 0;
}

#inventory-panel {
    position: absolute;
    left: 50%;
    bottom: 24px;
    translate: -50% 0;
    flex-direction: row;
    gap: 12px;
}

.inventory-slot {
    width: 64px;
    height: 64px;
    border-radius: 12px;
    background-color: rgba(255,255,255,0.08);
    border-width: 2px;
    border-color: rgba(255,255,255,0.2);
}

.inventory-slot.full {
    background-color: rgba(255,255,255,0.18);
    border-color: rgba(255,255,255,0.6);
}

#feedback-text {
    position: absolute;
    top: 24px;
    left: 50%;
    translate: -50% 0;
    opacity: 0;
    transition: opacity 0.2s ease;
}

#feedback-text.visible {
    opacity: 1;
}

#shift-button {
    position: absolute;
    right: 24px;
    bottom: 24px;
    width: 56px;
    height: 56px;
    border-radius: 50%;
}

#transition-overlay {
    position: absolute;
    inset: 0;
    opacity: 0;
    background-color: rgba(0, 0, 0, 0.7);
    transition: opacity 0.3s ease;
}

#transition-overlay.active {
    opacity: 1;
}

#reality-pulse {
    position: absolute;
    inset: 0;
    opacity: 0;
    background-color: rgba(120, 130, 255, 0.18);
    transition: opacity 0.25s ease;
}

#reality-pulse.active {
    opacity: 1;
}
```

## Lógica mínima sugerida

```csharp
public class HUDController : MonoBehaviour
{
    [SerializeField] private PanelRenderer panelRenderer;

    private List<VisualElement> inventorySlots;
    private Label feedbackText;
    private VisualElement transitionOverlay;
    private VisualElement realityPulse;

    private void OnEnable()
    {
        panelRenderer.RegisterUIReloadCallback(HandlePanelReloaded);
        SubscribeEvents();
    }

    private void OnDisable()
    {
        panelRenderer.UnregisterUIReloadCallback(HandlePanelReloaded);
        UnsubscribeEvents();
    }

    private void HandlePanelReloaded(PanelRenderer renderer, VisualElement root, int version)
    {
        inventorySlots = new List<VisualElement>
        {
            root.Q<VisualElement>("slot-0"),
            root.Q<VisualElement>("slot-1"),
            root.Q<VisualElement>("slot-2")
        };

        feedbackText = root.Q<Label>("feedback-text");
        transitionOverlay = root.Q<VisualElement>("transition-overlay");
        realityPulse = root.Q<VisualElement>("reality-pulse");

        RefreshInventory();
    }

    private void RefreshInventory()
    {
        var items = InventoryManager.Instance.GetItems();
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            var slot = inventorySlots[i];
            slot.EnableInClassList("full", i < items.Count);
            slot.EnableInClassList("empty", i >= items.Count);
        }
    }

    public void ShowFeedback(string text)
    {
        if (feedbackText == null) return;
        feedbackText.text = text;
        feedbackText.AddToClassList("visible");
    }
}
```

## Qué priorizar

### Sí

- un único HUD principal
- `PanelRenderer` para runtime
- eventos para actualizar la UI
- 2-4 slots de inventario
- feedback textual corto
- fade y pulse cortos

### No

- `UIDocument` runtime
- varias UIs separadas por sistema
- más de un HUD principal
- transiciones largas o complejas
- scripts de UI duplicados

## Criterio de éxito

La solución es buena si:

- el jugador entiende qué ha recogido,
- la transición entre rooms y realidades se siente clara,
- el SHIFT está integrado en el mismo HUD,
- el sistema sigue siendo simple y mantenible para una jam de dos personas.

Eso nos da una base suficiente para la demo sin inflar el scope ni crear una segunda arquitectura.
