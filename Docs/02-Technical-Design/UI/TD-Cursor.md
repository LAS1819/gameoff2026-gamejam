# TD-Cursor - Cursor personalizado runtime

- **Versión:** 1.0
- **Creado:** 2026-11-24
- **Actualizado:** 2026-11-24
- **Autor:** @LAS1819
- **Estado:** Implementado para el MVP; sin estados contextuales

---

## Objetivo

Implementar un único cursor retro visible durante toda la experiencia, mantener
la precisión de los clics y restaurar el cursor del sistema si falta el asset.

La primera versión no incluye estados por hover, selección, bloqueo o tipo de
interacción. El mismo sprite se usa sobre rooms, UI y interactables.

---

## Tecnología usada

Unity 6.6 acepta cursores personalizados en runtime con la API moderna
`UnityEngine.Cursor.SetCursor(...)`.

No se usa:

- Cursor legacy;
- cursores dibujados con GameObjects;
- cursores dependientes de la posición de la cámara;
- eventos de UI Toolkit para mover un cursor manual;
- detección de estados con `FindObjectOfType`.

El hook vive en `HUDController`, que ya coordina el HUD y se activa una sola vez
para todo el proyecto:

```csharp
UnityEngine.Cursor.SetCursor(
    customCursorTexture,
    customCursorHotspot,
    CursorMode.Auto);
```

Al desactivar `HUDController`, se restaura el cursor del sistema:

```csharp
UnityEngine.Cursor.SetCursor(
    null,
    Vector2.zero,
    CursorMode.Auto);
```

Si `customCursorTexture` no está asignado, la plataforma conserva su cursor
nativo sin necesidad de otra rama de código.

---

## Asset gráfico

### Canvas y forma

- resolución: `32 x 32` px;
- formato: PNG RGBA con transparencia;
- dirección: pixel art con bordes duros;
- estilo: flecha retro visible sobre fondos claros y oscuros;
- contorno oscuro externo para mantener contraste;
- relleno claro o crema para distinguir el interior;
- sin antialias, sombreado suave ni textura generada en altas resoluciones.

### Ruta

```text
Assets/
└── Visuals/
    └── UI/
        └── Cursor/
            └── sprite-ui-cursor-default-v1.png
```

El nombre debe mantenerse estable mientras siga existiendo una sola versión
del cursor. No crear versiones por room o por UI.

---

## Import Settings

Configurar en el Inspector del PNG:

```text
Texture Type = Cursor
Texture Shape = 2D
Alpha Source = From Input
Alpha Is Transparency = true
Filter Mode = Point
Wrap Mode = Clamp
Generate Mip Maps = false
Max Size = 32
Compression = None
Read/Write = false
```

Notas:

- `Texture Type = Cursor` evita que Unity altere el asset como si fuera una
  imagen de UI.
- `Point` mantiene los bordes pixelados.
- no activar mipmaps;
- no escalar el PNG a potencias mayores;
- el tamaño máximo de `32` evita redimensionado innecesario.

La API puede aceptar también un `Texture2D` importado como `Default`, pero
`Cursor` documenta mejor la intención del recurso.

---

## Hotspot

El hotspot identifica el pixel que representa la posición exacta del clic.

En Unity, `Vector2` usa coordenadas dentro del sprite y el punto suele medirse
desde la esquina superior izquierda del PNG:

```text
(0,0) ── ancho ──>
  │
  │
  ▼ alto
```

Para el cursor implementado en el MVP:

- punta activa: esquina superior izquierda de la flecha;
- hotspot recomendado inicial: `(2, 1)`;
- si el diseñador mueve la punta real, `customCursorHotspot` debe coincidir
  con el nuevo pixel visible.

No asumir que el centro del canvas es el hotspot: en una flecha, el punto
activo debe corresponder a la punta superior izquierda.

Ejemplo de configuración serializada:

```text
customCursorTexture = sprite-ui-cursor-default-v1.png
customCursorHotspot = (2,1)
```

---

## Implementación

El código ya se encuentra en `HUDController` y no requiere un nuevo Manager.
Los cursores CSS de UI Toolkit no se usan (`cursor: link`); ese mecanismo
busca cursores internos de runtime y puede emitir warnings en este proyecto.
El único puntero activo se define desde `HUDController`.

Campos serializados:

```csharp
[SerializeField] private Texture2D customCursorTexture;
[SerializeField] private Vector2 customCursorHotspot = Vector2.zero;
```

La activación se realiza en `OnEnable` y el fallback en `OnDisable`. El
`HUDController` debe existir una sola vez en el proyecto.

Comportamiento esperado:

1. Unity entra en Play Mode.
2. `HUDController.OnEnable()` aplica el cursor personalizado.
3. Los clics del mouse siguen usando las posiciones normales de Input System.
4. El hotspot coincide con la punta visible del sprite.
5. Cambiar de room, realidad o abrir overlays no cambia el cursor.
6. Si el HUD se desactiva, `null` restaura el cursor nativo.

No mover la lógica a `InteractableBehaviour`, `RoomManager` ni `UIManager`.
El cursor es global y mantenerlo junto al HUD evita cambios duplicados.

---

## Relación con clics

El asset cambia solo el dibujo del puntero. No cambia:

- `Mouse.current`;
- raycasts;
- Physics2D;
- colliders;
- selección de inventario;
- `InteractableBehaviour`;
- el hotspot de clicks en el `ScreenPointToRay`.

El evento de clic sigue ocurriendo donde está la punta activa configurada.
Antes de probar, asignar el hotspot exacto y comprobar que la punta aparece
apuntada al mismo objeto que recibe el raycast.

---

## Validación mínima

Probar en Play Mode a `640 x 360`:

- el cursor personalizado se ve sobre rooms claras y oscuras;
- la punta queda alineada con el hotspot;
- sigue visible sobre el HUD y los overlays;
- el cursor puede clicar slots e interactables;
- no se observa desfasaje al cambiar de room;
- si se vacía `customCursorTexture`, reaparece el cursor del sistema;
- no quedan warnings ni errores por referencia nula.

También probar WebGL Desktop y Windows:

- el cursor no se muestra borroso;
- no se reemplaza por el sistema al entrar o salir de una room;
- el hotspot no cambia al redimensionar la ventana.

---

## Próximo nivel opcional

No implementarlo en esta iteración. Cuando exista necesidad real, se podrán
añadir:

- estados hover;
- estado click;
- estado blocked;
- estado de inventory selection;
- animación mínima de 2 o 3 frames.

Cualquier expansión debe reutilizar el mismo canvas de `32 x 32`, el mismo
hotspot y el mismo hook de `HUDController`.
