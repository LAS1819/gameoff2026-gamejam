# GD-UI - UI del juego

- **Versión** 0.1 - Diseño de UI
- **Autor:** @LAS1819
- **Creado:** 12/11/2026
- **Actualizado:** 12/11/2026

---

## 1. Propósito de la UI

La interfaz del juego debe ser mínima, clara y narrativa. No debe competir con el escenario ni convertirse en un sistema de menú complejo; debe comunicar tres cosas de forma inmediata:

- dónde está el jugador respecto al mundo,
- qué puede hacer ahora,
- qué estado de progreso o descubrimiento ha activado.

La UI debe reforzar la sensación de tensión y exploración del juego sin romper la inmersión. Por eso, los elementos permanentes deben ser poco numerosos y muy legibles.

---

## 2. Principios de diseño

### Simplicidad visual

La UI trabaja como un anexo del mundo, no como un panel técnico. La información útil debe estar siempre a la vista, pero nunca cargando la pantalla.

### Jerarquía clara

Los elementos más importantes deben liderar la lectura:

1. Estado de realidad
2. Acciones principales
3. Inventario / feedback
4. Información secundaria

### Información narrativa

La UI puede contar parte de la historia sin narrarlo explícitamente. Un icono visible, un cambio de sprite o un brillo sutil son suficientes para comunicar que algo importante ha ocurrido.

---

## 3. Elementos UI permanentes

### 3.1 Shift button

Es el elemento de control principal del cambio de realidad.

- Siempre presente en la interfaz cuando el jugador está en un espacio jugable.
- Se sitúa en la esquina superior derecha del HUD.
- Tiene un estado activo/inactivo según la sala y el contexto del juego.
- debe mostrar un feedback claro cuando cambia la realidad.
- Realiza la misma acción que la tecla SHIFT izquierda del teclado.

Su función no es solo mecánica: también marca la presencia del personaje en un conflicto entre memoria, verdad y percepción.

### 3.2 Lantern icon

La `Linterna` es un icono persistente, complementario al SHIFT.

- Surge como elemento de `UI` desde el inicio, pero en estado no obtenido.
- Se coloca sobre el `shift-button` en la misma capa visual del `HUD`.
- Tiene tres estados visuales principales:
  - Sprite 0: silueta de la linterna sin obtener / sin activar.
  - Sprite 1: linterna obtenida, en estado apagada.
  - Sprite 2: linterna encendida (cuando se implemente su uso en el Sótano).
- Una vez se recoge la `Linterna` en el `Bedroom`, el icono pasa al Sprite 1.
- Este cambio no significa que la `Linterna` esté activa; solo refleja que el objeto ya pertenece al `Inventario` del jugador y que queda como herramienta disponible para el futuro.

La `Linterna` no debe ser un icono “bonito” de decoración: debe ser un marcador claro de progresión y preparación.

### 3.3 Inventario y feedback

- Los `Ítems` recogidos deben mostrarse como información útil y legible.
- El feedback textual (por ejemplo, "Has encontrado una llave", "La puerta está bloqueada") debe aparecer de forma breve y clara.
- La `UI` no debe explicar demasiado; lo importante es que el jugador comprenda qué ha ocurrido y qué tiene que hacer a continuación.

---

## 4. Estado visual de la Linterna

### Estado 0: no obtenida

- Se ve la silueta de la `Linterna` en gris / contorno.
- El icono está presente, pero no comunica pertenencia del jugador.
- Representa la idea de “algo que aún no se ha reclamado”.

### Estado 1: obtenida, apagada

- La silueta se reemplaza por el sprite de la linterna conseguida.
- El icono sigue siendo visible y estable.
- La `Linterna` no se usa todavía; solo ha sido descubierta y añadida a la progresión del jugador.

### Estado 2: encendida

- La `Linterna` se muestra con el sprite correspondiente al estado encendido.
- Indica que el jugador puede usarla activamente (sólo en el `Sótano`).
- El icono sigue siendo visible y estable mientras la `Linterna` esté encendida.

### Estado futuro: usable en el Sótano

Más adelante, cuando el juego introduzca el `Sótano` y la mecánica de explotación de la `Linterna`, el icono podrá pasar a un tercer estado:

- encendida,
- apagada,
- sin disponibilidad en `Salas` no permitidas.

Por ahora, el diseño se mantiene simple: no usarla fuera del `Sótano` y mostrarla solo como estado de posesión.

---

## 5. Ubicación en pantalla

La distribución visual de la `HUD` debe respetar una lógica simple:

- esquina superior derecha del viewport,
- `Shift-Button` como base de control,
- `Lantern-Icon` sobrepuesto a ese botón,
- no apilar más de dos capas de interacción en ese punto del `HUD`.

Esto mantiene la `UX` compacta y deja espacio para la lectura del resto de la interfaz.

---

## 6. Relación con la narrativa

La `Linterna` tiene un valor más fuerte que el de un simple objeto del `Inventario`.

- Representa la capacidad del protagonista para enfrentarse a lo oculto.
- Es un símbolo de preparación contra la corrupción y la pérdida de control.
- Funciona como un indicador de que el juego sigue avanzando hacia una verdad más profunda.

La `UI`, en este sentido, no solo informa: también acompaña la evolución del personaje.

---

## 7. Criterio de diseño para la implementación

La `UI` debe cumplir estas normas:

- Ser visible desde el inicio.
- Usar un sistema de sprites sencillo y consistente.
- Mantener un solo icono principal para la `Linterna`.
- No introducir estados complejos ni efectos extra hasta que la mecánica de uso tenga contexto real.
- Respetar la lógica de progresión del diseñador: obtener la `Linterna` en `Bedroom`, no usarla aún fuera del `Sótano`.

---

## 8. Resumen rápido

La `UI` del juego debe ser una capa discreta pero legible, con dos grandes ejes:

- la realidad (`SHIFT`),
- la progresión (`Linterna` / objetos / feedback).

La `Linterna` se presenta como un icono persistente sobre el botón `SHIFT`. Desde el inicio aparece en estado de silueta no obtenida; cuando se recoge en el `Bedroom` cambia al estado de `Linterna` obtenida, apagada y lista para futuras zonas del juego.
