# TD-Inventory - Inventario gráfico Sprint 4

- **Version:** 1.0
- **Creado:** 2026-11-23
- **Actualizado:** 2026-11-23
- **Autor:** @LAS1819
- **Estado:** Planificado para el Bloque 1

---

## Objetivo

Reemplazar los placeholders visuales de los tres slots del inventario sin
cambiar el comportamiento actual de selección, inspección y eliminación.

El sistema debe seguir usando `PanelRenderer + UXML + USS + HUDInventoryView`;
no requiere una escena nueva, un manager nuevo ni migración a ScriptableObjects.

## Estado actual

La UI ya existe en:

- [HUD.uxml](../../../Assets/UI/HUD.uxml);
- [HUD-style-sheet.uss](../../../Assets/UI/HUD-style-sheet.uss);
- [HUDInventoryView.cs](../../../Assets/Scripts/UI/HUDInventoryView.cs);
- [InventoryManager.cs](../../../Assets/Scripts/Inventory/InventoryManager.cs).

El HUD contiene tres slots:

```text
inventory-panel
├── slot-0
├── slot-1
└── slot-2
```

Cada slot usa:

- clase `empty` o `full`;
- clase `selected` al seleccionar;
- una etiqueta `slot-label`;
- left click para seleccionar;
- right click o context click para inspeccionar.

---

## Decisión técnica

### Representación visual

Cada slot mantiene el mismo `VisualElement`. Dentro de él se reutiliza:

- `slot-icon` para el sprite del item;
- `slot-label` solo como fallback textual.

La jerarquia runtime debe ser:

```text
inventory-slot
├── slot-icon
└── slot-label
```

`slot-label` queda oculto cuando existe icono valido y se conserva como
fallback de nombre corto si falta el sprite.

### Datos

No hace falta crear un catálogo nuevo ni ampliar `InventoryItemData` con
referencias a Assets. La opción recomendada mantiene el dato del item como
información de gameplay y resuelve su icono por `item.id` en la capa visual.

No se recomienda almacenar rutas de Assets como strings en el modelo runtime.
Las referencias de Sprite se mantienen serializadas en `HUDController` y se
entregan a `HUDInventoryView`.

### Asignación desde HUDController

`HUDController` puede seguir siendo el punto de configuración visual:

- serializar los Sprites prioritarios;
- pasarles una referencia o resolver al construir `HUDInventoryView`;
- no crear un IconManager ni un ScriptableObject por este bloque.

La asignación exacta se detalla en [TD-Item-Sprites.md](TD-Item-Sprites.md).

## Dimensión y layout

Mantener el tamaño actual de `64 x 64 px` como base y un margen lateral de
`12 px`.

Import settings de los iconos:

- Texture Type = Sprite (2D and UI);
- Filter Mode = Point;
- mipmaps desactivados;
- pixels per unit coherente con `64 px`;
- preservar transparencia;
- evitar escalado no entero.

El slot usa `scale-to-fit` para que el sprite completo quede visible.

## Estados

### Empty

- fondo placeholder discreto;
- borde visible;
- sin icono;
- sin label;
- picking mode Ignore.

### Full

- icono o fallback;
- borde activo;
- left click disponible;
- right click de inspección disponible.

### Selected

- conserva el icono;
- aplica la clase `selected`;
- el feedback confirma el item seleccionado.

## Fallback

Si falta un sprite:

- el slot permanece `full`;
- se conserva la inicial o abreviatura actual;
- la selección e inspección siguen funcionando;
- no se elimina el item del inventario.

Si falta el label por algún motivo:

- el slot conserva fondo, borde y estado seleccionado;
- el item sigue disponible para selección e inspección.

## Implementación

1. Serializar los seis Sprites prioritarios en `HUDController`.
2. Pasar las referencias necesarias al construir `HUDInventoryView`.
3. Resolver el icono por `item.id` al refrescar el slot.
4. Reusar los slots existentes y no cambiar el numero ni el layout.
5. Crear o resolver `slot-icon` desde `HUDInventoryView`.
6. Mantener `slot-label` como fallback.
7. No cambiar eventos, flags ni consumo de items.
8. Actualizar [TD-HUD.md](TD-HUD.md) con el estado final del inventario.

## Validación

Probar:

- slots vacíos;
- un, dos y tres items;
- slot seleccionado;
- fallback sin sprite;
- left click;
- right click;
- pickup y removal;
- reset desde Restart;
- resolución `640 x 360`;
- embed WebGL `640 x 360`.

## Criterio de done

El inventario pasa si:

- los tres slots son legibles;
- los iconos no cortan ni deforman;
- el fallback no bloquea acciones del item;
- el reset limpia slots e iconos;
- no se crean cuellos de botella ni managers nuevos.
