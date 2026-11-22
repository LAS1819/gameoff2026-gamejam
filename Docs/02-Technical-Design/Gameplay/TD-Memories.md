# TD - Memories

- **Versión:** 0.2
- **Creado:** 2026-11-22
- **Última actualización:** 2026-11-22
- **Estado:** implementación lógica iniciada; pendiente asignar los cinco sprites de sombras

## 1. Objetivo

Las `Memorias` son eventos narrativos opcionales activados por el `Examine` de interactuables específicos.

El sistema debe cumplir estas reglas:

- Una única `Memoria` por `Room` principal
- Disponibilidad exclusiva en `El Otro Lado` (`Other Side`)
- Activación persistente durante la sesión mediante una Flag
- Presentación visual breve en el momento del descubrimiento
- Ausencia de contador visible durante el juego
- Inclusión del total descubierto únicamente en la pantalla final
- Sin inventario ni objetos recogibles
- Sin modificaciones de la progresión principal

## 2. Diseño seleccionado

Para el MVP se mantiene la arquitectura actual basada en:

- `InteractableData` para visibilidad e interacción
- `InteractionManager` para resolver acciones
- `GameManager` para flags persistentes
- `HUDController` para feedback visual
- Un catálogo estático de Memorias para textos y presentación

No se requiere crear un Singleton nuevo ni un sistema de colección separado.

### Decisión sobre la UI

El descubrimiento no reutiliza el `Story Overlay`.

El `Story Overlay` es modal y está reservado a:

- Carta de revelación
- Pantalla final
- Controles explícitos de Continue/Restart

Una Memoria usa un overlay independiente, breve y no modal:

```text
MemoryDiscoveryOverlay
├── backdrop semitransparente
├── concepto/imagen de la Room
├── título de la Memoria
└── texto breve
```

La presentación dura aproximadamente 3.5 segundos:

- 0.3 segundos de entrada;
- 2.6 segundos de contenido;
- 0.6 segundos de salida.

El jugador puede seguir viendo la `Room` actual detrás del overlay. No recibe el contador de `Memorias` y no necesita pulsar un botón.

## 3. Datos

### Interactuable

Cada `Memoria` se representa como un `InteractableData` de tipo `Examine`:

```csharp
new InteractableData
{
    id = "memory_01_family_picture",
    displayName = "Family Picture",
    interactionType = InteractionType.Examine,
    visibleInNormalReality = false,
    visibleInOtherReality = true,
    resultFlag = GameFlagNames.Memory_01,
    memoryId = "Memory_01"
}
```

No sería necesario añadir `memoryId` si el ID de la Flag ya identifica la Memoria, pero el campo opcional separa explícitamente narración y estado:

- `resultFlag`: persistencia y resolución de final;
- `memoryId`: selección del texto y el arte del descubrimiento.

Esta separación es útil si una `Memoria` necesita cambiar su `interactuable` o texto sin alterar el ID usado por la lógica de finales.

### Definición de la Memoria

```csharp
public class MemoryDefinition
{
    public string id;
    public string title;
    public string body;
}
```
  
Los textos deben mantenerse en un catálogo, no dentro de los métodos especiales de `interacción`.

La definición puede extenderse con una referencia visual serializada en el inspector:

```csharp
public Sprite conceptArt;
public float overlayDuration = 3.5f;
```

Durante el MVP se puede omitir `MemoryDefinition.conceptArt` y utilizar una imagen por `Room` cargada por `MemoryCatalog`. La versión final puede asignar bocetos exclusivos por `Memoria`.

## 4. Catálogo

El catálogo recomendado contiene cinco entradas:

| `memoryId` | Room | Interactable |
| --- | --- | --- |
| `Memory_01` | `Lobby` | Cuadro familiar |
| `Memory_02` | `Hallway` | Mancha de humedad con forma de amonita |
| `Memory_03` | `Bedroom` | Ventana tapiada |
| `Memory_04` | `Library` | Reloj de pared roto |
| `Memory_05` | `Basement` | Caja de herramientas |

El catálogo debe exponer:

- búsqueda por `memoryId`;
- texto de la `Memoria`;
- referencia visual;
- duración opcional.

Si no se añade una clase propia, los datos pueden resolverse mediante un método estático similar a `MemoryCatalog.GetDefinition(memoryId)`.

---

## 5. Flujo de interacción

### Primera ejecución

1. El jugador pulsa el botón derecho sobre el `interactuable`.
2. `InteractionManager.TryExamine()` valida la `Room` y la `Realidad`.
3. Si el `interactuable` pertenece a una `Memoria`, se obtiene su `memoryId`.
4. `GameManager` comprueba si su `Flag` ya está activa.
5. Si todavía está activa no hay repetición:
   - se muestra la descripción normal del `interactuable`;
   - no se vuelve a abrir el overlay.
6. Si no estaba activa:
   - se activa la `Flag` correspondiente;
   - se muestra el descubrimiento;
   - se refrescan el estado y los `interactuables`.

### Comprobación de realidad

Los `interactuables` deben tener:

- `visibleInNormalReality = false`;
- `visibleInOtherReality = true`.

Además, el resolver puede validar explícitamente que la realidad actual es `OtherSide` antes de activar la Flag.

Esto evita obtener Memorias mediante datos obsoletos o estados del HUD fuera de sincronía.

### Repetición

No se consume el `interactuable` para permitir que conserve su descripción ambiental.

La comprobación principal es la propia Flag:

```text
si Memory_XX == true:
  resolver como Examine normal
si Memory_XX == false:
  activar Flag
  mostrar overlay
```

No se debe depender de `consumedInteractables`, porque ese estado pertenece a otros objetos y se reinicia junto al runtime.

---

## 6. Overlay visual

### MVP

La entrega inicial utiliza cinco imágenes horizontales rápidas creadas en Aseprite. Las imágenes representan a padre e hijo como sombras y presentan un elemento mínimo asociado a cada recuerdo.

Recomendación para el `overlay`:

- mostrar la imagen con opacidad alta;
- conservar el fondo ligeramente translúcido;
- usar una única paleta cromática común;
- mostrar el título de la `Memoria`;
- limitar el cuerpo a una o dos frases.

El `overlay` debe evitar:

- Nombres internos como `Memory_01`
- Contador `Recovered Memories`
- Símbolos de colección
- Botones obligatorios
- Animaciones complejas
- Múltiples paneles superpuestos

### Evolución visual

Después de la entrega pueden sustituirse los bocetos por composiciones más detalladas.

No conviene producir una escena completa por `Memoria`. La opción más eficiente es mantener:

- una composición horizontal común por `Memoria`;
- reutilizar el tratamiento visual común;
- mantener la silueta de padre e hijo;
- cambiar únicamente el elemento que identifica cada recuerdo.

### Especificaciones de los sprites

Los recursos gráficos se producirán con estas características:

- **Canvas recomendado:** `320 x 112` píxeles.
- **Formato:** PNG sin transparencia innecesaria.
- **Proporción:** horizontal, adaptada al área visual actual del panel.
- **Paleta:** 8-16 colores por imagen.
- **Contornos:** 1-2 píxeles de grosor, sin antialiasing.
- **Sombras:** bloques compactos de negro, azul oscuro o morado oscuro.
- **Objeto asociado:** máximo 16-32 píxeles de ancho.
- **Fondo:** simple; nunca debe competir con las siluetas.
- **Exportación:** Sprite único por Memoria, sin hojas de sprites salvo que se añada animación.

Nombres recomendados:

```text
memory-01-first-steps.png
memory-02-fossil.png
memory-03-closed-window.png
memory-04-eleven-thirty.png
memory-05-orchard.png
```

Si se crean variaciones de normalidad o distorsión, deben compartir:

- las mismas dimensiones;
- el mismo pivote;
- el mismo tamaño de siluetas;
- la misma posición de padre e hijo;
- el mismo tratamiento de borde.

---

## 7. Integración con el HUD

`UIManager` expone un método específico:

```csharp
ShowMemoryDiscovery(MemoryDefinition memory)
```

`HUDController` se encarga de:

- Crear o mostrar el overlay;
- Asignar imagen, título y texto;
- Ejecutar fade de entrada/salida;
- Ocultarlo automáticamente;
- No modificar el contador de Memorias;
- Ignorar la interacción con el Story Overlay mientras ya esté activo.

El método no debe usar `ShowStoryOverlay()`.

---

## 8. Eventos y audio

El feedback puede incluir:

- Un fade breve
- Un tono melancólico
- Una respiración o textura ambiental
- Silencio temporal del ambiente

El audio es opcional en el MVP. La visibilidad y duración del overlay deben funcionar sin audio. El evento debe sentirse como un descubrimiento interno, no como una recompensa de logros.

---

## 9. Finales

La resolución del final continúa contando las cinco `Flags` existentes:

- `0-2`: Final Malo
- `3-4`: Final Neutral
- `5`: Final Verdadero

El contador se calcula una sola vez al entrar al final o al abrir el desenlace. El sistema de Memorias no debe modificar directamente Forest. Forest sigue leyendo el mismo contador y variando eventos según qué Memoria fue obtenida.

---

## 10. Reinicio

Al reiniciar:

- Las cinco `Flags` se restablecen a `false`
- El estado consumido de `interactuables` se limpia
- El `overlay` se oculta
- El `catálogo` permanece disponible
- El `contador` vuelve a `0/5`

El reinicio debe probarse desde la pantalla final y desde el Lobby.

---

## 11. Pruebas del MVP

### Casos obligatorios

- [ ] Cada interactuable aparece únicamente en El Otro Lado
- [ ] El primer examen activa su Flag
- [ ] El overlay muestra título, imagen y texto correctos
- [ ] El overlay desaparece sin bloquear controles
- [ ] Repetir el examen no vuelve a mostrar el overlay
- [ ] Volver a cambiar de realidad no reactiva Memorias
- [ ] Reiniciar elimina todas las Flags
- [ ] Contar 0-2, 3-4 y 5 produce el final correspondiente
- [ ] No aparece ningún contador durante el recorrido
- [ ] La pantalla final muestra `Recovered Memories: X / 5`

---

## 12. Scope

Mantener para el MVP:

- Cinco interactuables;
- Cinco Flags;
- Cinco imágenes reutilizadas o bocetos mínimos;
- Un overlay no modal;
- Duración fija;
- Conteo existente al final.

Dejar fuera:

- Logros;
- Marcadores en minimapa;
- Lista de Memorias durante la partida;
- Animaciones complejas;
- Batalla o recompensas;
- Persistencia entre partidas.
