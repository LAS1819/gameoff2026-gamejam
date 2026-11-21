# GD Room - Library Design

- Versión 0.1
- Creado: 12/11/2026
- Actualizado: 12/11/2026

## 1. Qué representa esta Room

La `Library` es la `Room` de la memoria organizada.

Mientras que la `Bedroom` representa recuerdos íntimos y personales, la `Library` representa aquellos recuerdos que el protagonista ha archivado cuidadosamente para evitar enfrentarse a ellos. No es un lugar dedicado al conocimiento: es un lugar dedicado a conservar versiones del pasado.

Cada libro simboliza una memoria, una emoción o un acontecimiento relacionado con su padre. Las estanterías funcionan como un archivo donde el protagonista guarda aquello que todavía no está preparado para aceptar.

Desde la perspectiva narrativa, esta Room representa:

- la memoria;
- la reinterpretación de los recuerdos;
- la construcción de la mentira;
- la distancia entre lo ocurrido y lo recordado.

La `Library` es el primer lugar donde el jugador entiende que el edificio podría estar construido a partir de los recuerdos del protagonista.

### 2. Qué ocurre en esta Room

Al entrar en la `Library`, el jugador encuentra un espacio silencioso y aparentemente tranquilo. Estanterías, libros antiguos, mesas de lectura y documentos olvidados crean la sensación de un lugar dedicado al conocimiento.

Sin embargo, pronto descubre algo extraño.

Los libros parecen describir recuerdos personales del protagonista y de su padre.

Al cambiar al Otro Lado, los mismos libros siguen presentes, pero sus títulos y significados cambian.

Lo que antes parecía nostalgia comienza a transformarse en culpa, negación y dudas.

A medida que explora la sala, el jugador descubre un libro especial que puede recoger. Este libro contiene un `Disquete` oculto y se convierte en la pieza central del principal puzle de la habitación.

La `Library` es el primer escenario donde la información cambia de significado según la realidad activa.

### 3. Papel de la Room en la progresión

La `Library` tiene cuatro funciones principales:

- Introducir la idea de los recuerdos como elementos físicos del mundo.
- Profundizar en la narrativa relacionada con el padre del protagonista.
- Entregar el `Disquete` necesario para utilizar el ordenador de la `Bedroom`.
- Revelar el acceso al Sótano mediante un puzle basado en SHIFT.

Es también la primera `Room` donde la solución requiere observar cómo un mismo objeto se comporta de forma distinta en ambas realidades.

Narrativamente representa un paso importante:

El protagonista deja de buscar únicamente a su padre y empieza a enfrentarse a las contradicciones de su propia memoria.

### 4. Iconografía y tono visual de la `Library`

La `Library` debe sentirse:

- silenciosa;
- melancólica;
- llena de recuerdos;
- ligeramente acogedora en la `Realidad Normal`;
- inquietante y opresiva en el `Other Side`.

#### Realidad Normal

- estanterías completas;
- libros conservados;
- iluminación cálida y envejecida;
- sensación de nostalgia;
- ambiente de recuerdo y contemplación.

#### El Otro Lado

- sombras más profundas;
- títulos alterados;
- símbolos y mensajes ocultos entre los libros;
- estanterías deformadas;
- sensación de que los recuerdos están siendo reescritos.

La composición general debe mantenerse prácticamente igual para facilitar la producción y reforzar la conexión entre ambas realidades.

### 5. Interactables principales

#### Estanterías de recuerdos (Memory Shelves)

- Constituyen el elemento principal de la Room.
- Cada libro representa una memoria relacionada con el padre.
- Sus títulos cambian entre realidades.
- Proporcionan información narrativa y pistas opcionales.

#### Libro Desgastado (Worn Book) - Pick Up

- Es el objeto principal de la Room.
- Puede recogerse y almacenarse en el inventario.
- Contiene el Disquete oculto.
- Su descripción cambia según la realidad activa.

#### Estantería especial (Special Shelf)

- Posee un hueco vacío.
- Permite utilizar el Libro Desgastado.
- Está vinculada al mecanismo que revela el acceso al `Sótano`.

#### Puerta del `Sótano` (Basement Door)

- Permanece oculta inicialmente.
- Sólo puede descubrirse mediante el puzle de la estantería.
- Posee un candado.
- Requiere disponer del ítem `Código` para abrirse.

#### Salida al `Hallway`

- Conecta con el resto del edificio.
- Permite regresar a zonas anteriores.

### 6. Ítems y objetos clave

#### Libro Desgastado (Worn Book)

- Objeto principal de la Room.
- Su descripción cambia según la realidad activa.
- Introduce la idea de los ítems influenciados por SHIFT.

#### Disquete (Floppy Disk)

- Se encuentra oculto dentro del Libro Desgastado.
- Puede recuperarse examinando el libro desde el inventario.
- Se utiliza posteriormente en el ordenador de la Bedroom.

#### Código (Code)

- Se obtiene utilizando el Disquete en el ordenador de la Bedroom.
- Funciona como una llave de progreso.
- Permite abrir el candado del acceso al Sótano.

#### Libros de memoria

- No son objetos recogibles.
- Sirven como recurso narrativo.
- Refuerzan la historia del protagonista y de su padre.

### 7. Puzle principal

- **Objetivo**: acceder al `Sótano`.
- **Paso 1**: encontrar el Libro Desgastado.
- **Paso 2**: examinar el Libro Desgastado para descubrir el Disquete oculto.
- **Paso 3**: usar el Disquete en el ordenador de la Bedroom y obtener el Código.
- **Paso 4**: regresar a la Biblioteca.
- **Paso 5**: colocar el Libro Desgastado en la Estantería Especial.
- **Paso 6**: cambiar al Otro Lado.
- **Resultado**: la estantería se desplaza y revela una puerta oculta hacia el Sótano.
- **Paso Final**: usar el Código sobre el candado para acceder a la nueva zona.

### 8. Eventos opcionales

La Biblioteca es una de las mejores Rooms para contener fenómenos opcionales relacionados con la memoria.

Posibles ejemplos:

- aparición de una figura observando entre estanterías;
- títulos de libros que cambian al volver a examinarlos;
- recuerdos opcionales relacionados con el padre;
- mensajes ocultos visibles únicamente en el `Otro Lado`.

Estos eventos no deben bloquear la progresión principal, pero sí ampliar la comprensión de la historia.

### 9. Sugerencias de diseño para MVP

#### Mantener

- El `Libro Desgastado`.
- El `Disquete` oculto.
- El acceso secreto al `Sótano`.
- Los cambios de títulos entre `realidades`.
- El uso narrativo de las `estanterías`.

#### Simplificar

- No crear docenas de libros interactivos.
- Reutilizar pocas estanterías importantes.
- Limitar los cambios visuales a textos y algunos elementos concretos.

#### Descartar por ahora

- Sistemas complejos de clasificación de `libros`.
- Puzles con múltiples combinaciones.
- Lectura extensa de `documentos`.

La Biblioteca debe sentirse profunda narrativamente pero simple mecánicamente.

### 10. Información clave

- La `Library` es la `Room` de la memoria archivada.
- Los libros representan recuerdos del protagonista.
- El `Otro Lado` altera el significado de esos recuerdos.
- Aquí se obtiene el `Disquete`.
- El `Código` necesario para el `Sótano` se obtiene posteriormente en la `Bedroom`.
- El acceso al `Sótano` se revela mediante un uso inteligente de SHIFT.
- La `Room` refuerza los temas de memoria, negación y aceptación.

### 11. Resumen corto

La `Library` es el archivo de recuerdos del protagonista. Sus estanterías contienen fragmentos de una vida compartida con su padre, pero en el `Otro Lado` esos mismos recuerdos se deforman y revelan la verdad que intenta ocultar. Aquí el jugador encuentra el `Libro Desgastado` y el `Disquete`, descubre cómo la memoria cambia entre `realidades` y termina revelando el acceso secreto al `Sótano`.
