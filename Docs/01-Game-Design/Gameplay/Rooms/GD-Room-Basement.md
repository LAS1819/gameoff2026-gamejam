# GD Room - Basement Design

- Versión 0.4
- Creado: 17/11/2026
- Actualizado: 19/11/2026

---

## 1. Qué representa esta Room

El `Basement` es la primera `Room` en la que la mentira del protagonista deja de ser una estructura emocional y se vuelve prueba material.

Si la `Bedroom` es intimidad, la `Library` es memoria archivada y la `Hallway` es tránsito, el `Basement` es presión: la verdad filtrándose a través de la maquinaria que el protagonista intentó ignorar.

A nivel narrativo representa:

- el punto en el que la historia ya no puede seguir ocultándose;
- la culpa y la ira convertidas en una infraestructura física;
- el duelo contenido en tuberías, vapor, sirenas y humedad;
- la evidencia de que la versión de los hechos del protagonista no es fiable.

La clave es que el jugador no “resuelve un puzle abstracto”, sino que descubre una verdad enterrada bajo el edificio y bajo la mente del personaje.

---

## 2. Objetivo de la Room

El objetivo del `Basement` no es solo abrir un acceso ni obtener un item; es romper la sensación de que todo sigue siendo decorativo.

Debe dejar claro que:

- el edificio guarda cosas que el protagonista no quiere mirar;
- la historia del padre no fue un simple misterio, sino una negación materializada;
- el jugador ya no está leyendo un recuerdo: está comprobando una evidencia.

---

## 3. Qué ocurre en la Room

El jugador entra desde la `Library` mediante el acceso que se revela con el `Basement Code`.

La sala está casi a oscuras: hay una luz global mínima, suficiente para distinguir siluetas, pero no para interpretar detalles con precisión. El espacio se vuelve incómodo, claustrofóbico y mecánico.

Aquí aparece la regla central de la `Room`:

- sin `Linterna` activa, `Examinar` ofrece una lectura incompleta o ambigua;
- sin `Linterna`, los `Usar` críticos están bloqueados;
- con `Linterna`, el jugador puede identificar la evidencia real y completar la acción necesaria.

La room debe ser breve y densa: 2-3 interacciones principales, alto peso emocional y una sensación de descubrimiento inevitable.

---

## 4. Reglas de luz y linterna

### Principio base

La `Linterna` solo debe habilitarse en las `Rooms` donde su uso sea narrativamente necesario. En el MVP, la `Basement` es la sala de prueba principal.

### Regla de diseño

- El HUD muestra la linterna como herramienta permanente.
- La room establece si el uso está permitido o no.
- Si el jugador intenta usarla fuera de una room válida, no funciona.
- Si la `Basement` entra en una escena con luz global muy baja, la linterna se vuelve una necesidad lógica.

### Importante

La intensidad global no debe ser la única regla. La solución más limpia es:

- `gating por room` como regla principal;
- `luz global baja` como refuerzo visual y emocional.

Esto evita que el sistema se vuelva frágil o dependiente de umbrales muy precisos.

---

## 5. Interactables principales

### 5.1 Estanterías de almacenamiento

Las estanterías son el primer elemento que obliga al jugador a moverse con la `Linterna` encendida.

**Visual:**

- cajas oxidadas;
- trozos de herramientas y pertenencias olvidadas;
- metal corroído y humedad en los estantes;
- sensación de almacenamiento improvisado, no de lugar funcional.

**Función:**

- ocultar la evidencia clave;
- reforzar la sensación de búsqueda y desorden emocional;
- crear una base visual que haga que el espacio se sienta “ocupado por una historia no resuelta”.

**Sin Linterna:**

- los contornos son borrosos;
- los objetos parecen sombras o cajas sin identidad;
- un `Examine` resulta confuso o incompleto.

---

### 5.2 Caja de efectos personales / Personal Effects Box

Es la `Evidencia Principal` del `Basement`.

Se encuentra entre cajas y objetos almacenados. En ella se localiza la `Manivela de la válvula`.

En el MVP actual se usa desde el `Otro Lado`; la lógica visual y los requisitos de linterna se integrarán en una iteración posterior.

**Contenido:**

- `Manivela de la válvula`
- Fotografías
- Documentos personales
- Objetos con valor emocional y simbólico
- Piezas relacionadas con la muerte del padre o con el día de la desaparición

**Objetivo narrativo:**

- No explicar la verdad de golpe
- Mostrar que la historia del protagonista está rota
- Introducir dudas imposibles de ignorar

**Realidades:**

**Realidad Normal:**

- Caja de cartón abierta y relativamente ordenada.
- Etiqueta visible con la palabra `PERSONAL`.
- Fotografías y documentos sobresalen ligeramente.
- Se percibe como una caja corriente almacenada entre otros objetos olvidados.

**Otro Lado:**

- La misma caja aparece deteriorada.
- La etiqueta ha cambiado y ahora muestra la palabra `MIND`.
- Los documentos aparecen desplazados.
- Algunas fotografías muestran detalles imposibles o contradictorios.
- La caja transmite la sensación de que el recuerdo ha sido manipulado o reescrito.

La transición visual entre `PERSONAL` y `MIND` simboliza el paso desde un recuerdo aparentemente normal hacia una construcción mental basada en la negación.

La caja no debe revelar toda la verdad, sino materializar la incoherencia de la historia que el protagonista se ha contado durante meses.

---

### 5.3 Manivela de la válvula / Valve Handle

La `Manivela` es la pieza de progreso necesaria para poner en marcha la resolución de la room.

**Características:**

- se encuentra oculta en el almacén;
- exige buscar con la linterna;
- se puede recoger y llevar temporalmente;
- tiene sentido claro dentro del espacio: `crea la necesidad de reparar la presión del sistema`.

La idea debe ser muy legible: hay una maquinaria rota y el jugador necesita arreglarla para poder hacer algo más que explorar.

---

### 5.4 Válvula de descarga

Es el interactuable clave del `Basement`, el punto de resolución central.

**Regla principal:**

- en la `Realidad Normal` puede accionarse;
- en el `Otro Lado` aparece dañada y la manipulación no es viable.

**Función narrativa:**

La válvula representa la liberación de la presión emocional acumulada. No es una máquina “realista” para ser entendida; es el símbolo de la verdad que deja de ser contenida.

**Representación visual de la presión:**

Cuando la `Válvula de Descarga` es activada en la `Realidad Normal`, varias tuberías del `Otro Lado` comienzan a expulsar vapor a presión.

**Implementación MVP:**

- efecto visual simple;
- 2-4 sprites animados reutilizables;
- animación en bucle;
- transparencia parcial;
- movimiento vertical lento y turbulento;
- puede reutilizarse en múltiples tuberías de la room.

**Objetivo narrativo:**

El vapor no representa únicamente una reacción mecánica.

Representa la presión emocional acumulada durante meses que finalmente encuentra una vía de escape.

Su aparición debe comunicar de forma inmediata que algo ha cambiado en la relación entre ambas realidades.

**Resultado:**

- liberar la presión del sistema;
- permitir que el vapor, el ruido y la presencia de la negación se disipen;
- abrir el camino a la salida de la room.

La acción debe sentirse como un acto de resolución, no como un “puzle de la puerta”.

---

### 5.5 Luces de ambulancia

Estas luces aparecen solo en el `Otro Lado` como destellos rojos y azules, suaves y erráticos.

**Función:**

- no cambian la jugabilidad;
- refuerzan la memoria del día en que ocurrió todo;
- devuelven fragmentos del recuerdo del padre fallecido;
- crean la idea de que la historia del protagonista está siendo empujada por la verdad.

**Ejemplos de texto de `Examine`:**

- “Las luces parpadean como si alguien intentara recordármelo.”
- “No quiero pensar en aquel día.”
- “Vuelven a aparecer igual que la última vez.”

No deben explicar la muerte; simplemente la evocan.

---

### 5.6 Salida del Basement

La salida funciona como cierre emocional de la room.

**Condición de progresión:**

- la `Manifestación de Negación` debe haber sido disipada;
- la `Válvula de Descarga` debe haber sido usada con éxito;
- el jugador debe haber comprendido que la room ya no es un simple pasaje.

La salida no introduce más puzles. Su función es reforzar que algo ha cambiado de forma irreversible.

---

## 6. Manifestación de Negación

La `Manifestación de Negación` es el equivalente conceptual del `wardrobe-monster`, pero adaptado al `Basement`.

No es un enemigo de combate. Es un bloqueo psicológico y narrativo.

**Características:**

- aparece solo en el `Otro Lado`;
- bloquea la salida del `Basement`;
- se mueve de forma espasmódica e inquietante;
- genera tensión por presencia y sonido, no por combate;
- representa la negación del protagonista como fuerza activa.

**Resuelve de la siguiente forma:**

1. el jugador encuentra la `Manivela`;
2. la instala en la `Válvula de Descarga`;
3. la repara en la `Realidad Normal`;
4. cambia al `Otro Lado`;
5. observa cómo el vapor, las roturas y la presión se liberan;
6. la `Manifestación de Negación` se distorsiona y retrocede;
7. la salida queda accesible.

La desaparición debe verse como un desvanecimiento breve pero perceptible:

- mantiene el sprite visible al comenzar;
- encoge y distorsiona ligeramente la silueta;
- reduce su opacidad durante aproximadamente 1.35 segundos;
- termina ocultando el objeto.

Es importante que no sea “derrotada” como un monstruo. Pierde su capacidad de bloquear al jugador, pero su presencia sigue funcionando como símbolo de la negación del protagonista.

---

## 7. Iconografía y tono visual

### Realidad Normal

- sótano húmedo y deteriorado;
- tuberías oxidadas y conectores rotos;
- oscuridad fría y muy baja;
- sensación de maquinaria antigua abandonada.

### El Otro Lado

- sombras más duras;
- destellos rojos/azules de ambulancia;
- vapor saliendo de las tuberías;
- humedad y gotas que parecen “llorar” en la pared;
- un espacio que se siente vivo, pero quebrado.

### Simbolismo ambiental

- cañerías rotas = furia;
- vapor = ira contenida;
- gotas = lágrimas;
- luces de sirena = recuerdo del día de la muerte del padre.

### Vapor

El vapor es uno de los elementos visuales más importantes del Basement.

- aparece únicamente después de activar la `Válvula de Descarga`;
- surge por grietas y tuberías rotas del `Otro Lado`;
- utiliza pocos sprites reutilizables;
- debe ser claramente visible incluso en zonas oscuras;
- ayuda a dirigir la atención del jugador hacia la resolución de la room.

De forma narrativa, simboliza emociones reprimidas que ya no pueden permanecer contenidas.

Todo esto debe ser breve, legible y útil de forma narrativa; no un espectáculo visual sin función.

---

## 8. Reglas narrativas específicas

- El `Basement` debe acercar al jugador a la verdad sin revelar todo.
- El texto sin linterna debe ser confuso, fragmentado y emocionalmente defensivo.
- El texto con linterna debe ser claro, específico y doloroso.
- La room debe terminar con una sensación de “ya no se puede volver atrás”.

La idea no es que el jugador “aprenda la historia” como un dato, sino que la experimente como un cambio de verdad.

---

## 9. Scope del MVP

### Mantener

- 1 evidencia central;
- 1 pieza de progreso (`Manivela`);
- 1 resolución principal (`Válvula de Descarga`);
- 1 bloqueo emocional (`Manifestación de Negación`);
- uso de la linterna como requisito real para la lectura y la acción;
- room muy oscura, muy breve y muy clara.

### Simplificar

- sin batería;
- sin combate;
- sin lógica compleja de maquinaria;
- sin árbol de estados grande;
- sin más de 2-3 interacciones centrales.

### Dejar fuera por ahora

- consumo de batería;
- persecución prolongada;
- puzles secundarios;
- extensión del `Huerto`.

El `Huerto` puede quedar como semilla narrativa para una iteración posterior, pero no forma parte del MVP del `Basement`.

---

## 10. Resumen corto

El `Basement` es la room donde la historia empieza a romperse de verdad. El jugador entra con una oscuridad que exige linterna, descubre la evidencia material del conflicto del protagonista, repara la presión del sistema y supera la manifestación de negación que bloquea la salida. Es una room corta, potente, psicológica y decisiva: la primera gran ruptura narrativa del juego.
