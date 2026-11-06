# TD-HUD - UI mínima del MVP con Panel Renderer

- **Versión:** 1.0
- **Creado:** 2026-11-06
- **Actualizado:** 2026-11-06

---

## Objetivo

Diseñar un HUD mínimo, estable y reutilizable para la demo del sprint 1 sin crear una arquitectura de UI compleja.

La intención es:

- mostrar inventario básico,
- dar feedback textual al jugador,
- integrar el SHIFT button dentro del mismo sistema,
- reutilizar el patrón que ya funciona con `PanelRenderer` y `UIShiftButton`.

---

## Principio general

Usaremos el mismo enfoque que ya tenemos para el SHIFT:

- `PanelRenderer` como raíz visual runtime
- `VisualTreeAsset` para la estructura del HUD
- `USS` para estilos
- binding por `RegisterUIReloadCallback()`
- actualización por eventos, no por `Update()`

Esto evita el patrón legacy de `UIDocument`, mantiene la lógica sencilla y encaja con un proyecto de jam de 2 personas.

---

## Estructura recomendada

### 1) Archivos

- `Assets/UI/HUD.uxml`
- `Assets/UI/HUD.uss`
- `Assets/Scripts/UI/HUDController.cs`
- `Assets/Scripts/UI/UIShiftButton.cs` (se conserva, pero pasa a ser un botón dentro del HUD)

### 2) Jerarquía visual

```text
HUDRoot (GameObject con PanelRenderer)
├── InventoryPanel
│   ├── slot-0
│   ├── slot-1
│   └── slot-2
├── FeedbackText
├── ShiftButton
├── TransitionOverlay
└── RealityPulse
```

### 3) Roles

- `InventoryPanel`: muestra objetos recogidos.
- `FeedbackText`: texto temporal de acción y progreso.
- `ShiftButton`: botón de SHIFT, integrado en el mismo HUD.
- `TransitionOverlay`: fade para cambiar de room.
- `RealityPulse`: ligero flash al cambiar de realidad.

---

## Patrón de runtime

### HUDController

`HUDController` será el punto de control visual del sistema.

Sus responsabilidades:

- suscribirse a eventos de `InventoryManager`, `RoomManager` y `RealityManager`
- enlazar elementos del `VisualTreeAsset` tras cada recarga del panel
- refrescar inventario y feedback cuando cambie el estado
- activar/desactivar overlay y pulse visual

### Eventos a escuchar

- `InventoryManager.ItemAdded`
- `InventoryManager.ItemRemoved`
- `RoomManager.RoomChanged`
- `RealityManager.RealityChanged`

### Flujo de actualización

```csharp
private void HandlePanelReloaded(PanelRenderer panelRenderer, VisualElement root, int version)
{
    inventorySlots = new List<VisualElement>
    {
        root.Q<VisualElement>("slot-0"),
        root.Q<VisualElement>("slot-1"),
        root.Q<VisualElement>("slot-2")
    };

    feedbackLabel = root.Q<Label>("feedback-text");
    transitionOverlay = root.Q<VisualElement>("transition-overlay");
    realityPulse = root.Q<VisualElement>("reality-pulse");
    shiftButton = root.Q<VisualElement>("shift-button");

    RefreshInventoryUI();
    RefreshFeedbackState();
}
```

---

## Integración con el SHIFT actual

El `UIShiftButton` no debe ser una UI aislada con su propio binding completo. Debe seguir este patrón:

- el `ShiftButton` vive en el mismo `HUDRoot`
- `UIShiftButton` solo maneja click y lógica de validación
- el `HUDController` gestiona el estado visual del botón a partir de `RealityManager` y `RoomManager`

### Regla clave

`UIShiftButton` no debe hacer binding manual de cada elemento del HUD. Solo debe:

- verificar si SHIFT está permitido en la room actual
- llamar a `RealityManager.Instance.ToggleReality()`
- activar el flag `ShiftPerformed`
- delegar la visualización del estado al `HUDController`

Esto mantiene el mismo patrón que ya hemos validado con el botón SHIFT y evita fragmentar la UI.

---

## Estado visual recomendado

### Inventario

Clases CSS:

- `.inventory-slot`
- `.inventory-slot.full`
- `.inventory-slot.empty`

Regla:

- slot vacío -> fondo gris / opacidad baja
- slot con ítem -> icono visible + borde activo

### Feedback

Clases CSS:

- `.feedback-text`
- `.feedback-text.visible`

Regla:

- se activa con una pequeña animación de aparición
- se oculta tras 1.5s a 2s
- no hay cola de mensajes compleja

### Transición

Clases CSS:

- `.transition-overlay`
- `.transition-overlay.active`

Regla:

- alpha de 0 a 1 al entrar en room
- alpha de 1 a 0 al salir de la transición
- duración 0.25s-0.5s

### Pulse de realidad

Clases CSS:

- `.reality-pulse`
- `.reality-pulse.active`

Regla:

- flash corto al cambiar normal ↔ Other Side
- no más de 0.3s-0.5s

---

## Estructura de la capa visual

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
    left: 0;
    top: 0;
    right: 0;
    bottom: 0;
}

#inventory-panel {
    position: absolute;
    bottom: 24px;
    left: 50%;
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

---

## Código sugerido

### HUDController.cs

```csharp
public class HUDController : MonoBehaviour
{
    [SerializeField] private PanelRenderer panelRenderer;

    private List<VisualElement> inventorySlots;
    private Label feedbackLabel;
    private VisualElement transitionOverlay;
    private VisualElement realityPulse;
    private VisualElement shiftButton;

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

        feedbackLabel = root.Q<Label>("feedback-text");
        transitionOverlay = root.Q<VisualElement>("transition-overlay");
        realityPulse = root.Q<VisualElement>("reality-pulse");
        shiftButton = root.Q<VisualElement>("shift-button");

        RefreshInventoryUI();
    }

    private void SubscribeEvents()
    {
        InventoryManager.Instance.ItemAdded += HandleItemAdded;
        InventoryManager.Instance.ItemRemoved += HandleItemRemoved;
        RoomManager.Instance.RoomChanged += HandleRoomChanged;
        RealityManager.Instance.RealityChanged += HandleRealityChanged;
    }

    private void RefreshInventoryUI()
    {
        var items = InventoryManager.Instance.GetItems();
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            var slot = inventorySlots[i];
            slot.RemoveFromClassList("full");
            slot.AddToClassList(i < items.Count ? "full" : "empty");
        }
    }

    public void ShowFeedback(string text)
    {
        if (feedbackLabel == null) return;
        feedbackLabel.text = text;
        feedbackLabel.AddToClassList("visible");
    }
}
```

### UIShiftButton.cs

Mantenerlo corto y claro:

- validación de room
- toggle de realidad
- flag `ShiftPerformed`
- no binding de elementos del HUD

```csharp
public class UIShiftButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (!CanShiftInCurrentRoom())
        {
            return;
        }

        RealityManager.Instance.ToggleReality();
        GameManager.Instance.SetFlag(GameFlagNames.ShiftPerformed, true);
    }
}
```

Preferible: si se usa como `VisualElement` del HUD, entonces queda aún más limpio con callback directo al `ClickEvent` del propio `shift-button` en `HUDController`.

---

## Qué priorizar

### Sí

- un solo HUD principal
- `PanelRenderer` para runtime
- eventos para actualizar la UI
- mínimo 2-4 slots de inventario
- feedback de texto temporal
- fade y pulse cortos

### No

- `UIDocument` runtime
- varias UIs separadas para cada sistema
- un sistema completo de inventario con data tables y assets extra
- transiciones largas o complejas
- scripts de UI con lógica duplicada

---

## Criterio de éxito

La implementación está bien si:

- el flujo de demo queda claro y legible,
- el jugador ve que recogió la llave y que el mundo cambia,
- la UI no rompe la arquitectura actual,
- el sistema es fácil de mantener aunque solo seamos dos personas en la jam.

Esto da una base decente para el siguiente sprint sin convertir la demo en un sistema de UI gigante.
