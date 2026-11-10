# Bedroom Room Design

- Versión 0.1 - Diseño de Sala
- Creado: 10/11/2026
- Actualizado: 10/11/2026

---

## 1. Qué representa esta Room

La Bedroom es la primera Room íntima del juego y probablemente la más importante emocionalmente. No es solo una estancia de la casa del protagonista: es el lugar donde empezaron a formarse la negación, la rutina, la fantasía y la esperanza que más tarde se convertirían en el Otro Lado.

La habitación representa la infancia y la adolescencia del protagonista. Es el espacio donde el recuerdo se vuelve frágil, donde el pasado todavía está vivo y donde la mentira empieza a instalarse como una forma de sobrevivir. Aquí no solo duerme o vive el personaje: aquí conviven la memoria, el duelo no resuelto y la necesidad de seguir creyendo que su padre regresará.

Su valor narrativa es claro:

- es la habitación del pasado;
- es el punto de origen de la construcción del Otro Lado;
- es el primer lugar donde la realidad del protagonista deja de ser estable.

Es la Room más personal del juego, y en ella debe sentirse que el edificio no está solo “distorsionando” el espacio, sino que lo está empujando hacia el interior de la mente del personaje.

---

## 2. Qué ocurre en esta Room

El jugador entra en una habitación reconocible, íntima y casi demasiado normal. La cama, la mesita de noche, la lámpara, el armario, la ventana y los objetos cotidianos generan sensación de hogar. Pero ahí empieza a notarse algo extraño: la distorsión no es únicamente ambiental, sino emocional.

La habitación parece ser la misma que la de la infancia o la adolescencia del protagonista, pero hay algo que no encaja. La ventana, la disposición de los objetos o la sensación de presencia parecen estar “vivos” en una forma que no corresponde al espacio real.

Conforme el jugador explora, se da cuenta de que la habitación no solo es un lugar físico: es un punto de memoria. La clave de la Room es que cuanto más observa, más se une a la idea de que la verdad del personaje estaba construida aquí, desde el principio.

---

## 3. Papel de la Room en la progresión

La Bedroom tiene tres funciones muy claras dentro de la progresión del juego:

1. Introducir la idea de la memoria como espacio activo.
2. Presentar la mecánica de luz como herramienta narrativa y de supervivencia.
3. Proporcionar un objeto esencial para la siguiente parte del juego: la Llave de la Librería.

Es también la primera Room en la que el jugador entiende que el concepto de “lugar” no es inocente. La habitación no es solo un sitio del edificio; es una zona donde el personaje ha construido una realidad alternativa para sostener su dolor.

Es la brecha entre lo que el protagonista cree y lo que la historia intenta hacerle aceptar.

---

## 4. Iconografía y tono visual

La Bedroom debe sentirse:

- familiar;
- íntima;
- algo inquietante sin llegar a ser terror puro;
- íntimamente ligada a la infancia y a la adolescencia.

### Realidad Normal

- colores cálidos y apagados;
- muebles antiguos pero reconocibles;
- objetos personales y cotidianos;
- sensación de privacidad y rutina;
- la luz debe ser uno de los elementos más importantes de la atmósfera.

### El Otro Lado

- la habitación mantiene la misma composición general, pero ya no es un lugar seguro;
- la ventana está tapiada o bloqueada por ladrillos;
- los elementos del entorno se vuelven más inhóspitos;
- la sensación de encierro aumenta;
- la presencia del monstruo empieza a volverse activa cuando la luz deja de sostener el espacio.

La habitación debe sentirse como un lugar que ha sido “re-escrito” por la mente del protagonista, no como un lugar completamente ajeno.

---

## 5. Interactables principales

### La cama (Bed)

- Es el eje emocional de la Room.
- Debe sugerir descanso, memoria, infancia, aislamiento y una presencia constante del padre o de la ausencia del padre.
- Puede contener o acompañar pistas narrativas sobre la vida del protagonista.

### La mesita de noche (Nightstand)

- Aquí es donde se ubica la linterna.
- Debe ser un objeto pequeño, reconocible y evocador.
- Visualmente debe invitar a la exploración sin romper la sensación de intimidad del espacio.

### La lámpara / interruptor de la habitación (Lamp / Switch)

- Es la primera pieza clave de la mecánica de luz.
- La habitación se puede iluminar para reducir la amenaza o para permitir reconocer ciertos detalles narrativos.
- El interruptor debe parecer algo sutil pero muy importante dentro del espacio.

### El armario (Closet)

- Es el elemento más peligroso y más cargado de tensión.
- Aquí se guarda la llave que abre la puerta de la Librería.
- En la realidad normal, puede ser un mueble común, casi banal.
- En el Otro Lado, el contenido del armario deja de ser inocuo: la clave aparece de forma más extraña, y la amenaza del monstruo puede estar ligada a abrirlo sin las condiciones adecuadas.

### La ventana (Window)

- Debe ser un punto visual muy importante.
- En la Realidad Normal puede existir como elemento de la habitación.
- En el Otro Lado aparece tapiada con ladrillos o bloqueada de forma imposible.
- Esta diferencia refuerza la idea de que el espacio se ha cerrado y que la salida real ya no existe.

### La puerta / salida a corredor (Door / Hallway Exit)

- Es una salida funcional para llegar al resto del edificio.
- También puede ser una marca de transición emocional: al salir de la habitación, el protagonista abandona la sensación de infancia para entrar en una fase más difícil de la verdad.
- Sólo se puede volver atrás en el Mundo Normal.

---

## 6. Ítems y objetos clave

### Linterna (Flashlight)

Es uno de los objetos más importantes del juego.

La idea base es que la linterna no forme parte del inventario como un objeto tradicional, sino que se convierta en un recurso de interacción permanente en la interfaz.

#### Recomendación de diseño

- La linterna debe aparecer en la UI como un interactuable permanente.
- El jugador la activa cuando quiera.
- Debe tener pilas o carga, no necesariamente un sistema complejo en MVP.
- Su uso es esencial para la exploración y para la seguridad en el Otro Lado.
- No debe sentirse como un objeto “recogido” y luego olvidado; debe ser una herramienta que acompaña al personaje de forma constante.
- Podemos obligar a usar la Linterna en momentos específicos, donde la sala lo requiera. Usando la técnica de Luz Global (Global Light) y los efectos de Luz con sprites (Light 2D)

Esto encaja muy bien con la identidad del juego: no es un objeto de colección, es una extensión del personaje y de su capacidad para soportar lo que ocurre.

### Llave de la Librería (Library Key)

- Es el primer objeto de progreso que no se resuelve en el Lobby.
- Su localización ideal es en el armario, en el Otro Lado.
- Su presencia obliga al jugador a explorar un espacio íntimo y emocionalmente peligroso para obtener algo que le permitirá seguir avanzando.

### Objetos narrativos

La Bedroom puede incluir objetos secundarios como:

- fotos antiguas;
- ropa infantil o adolescente;
- libros o cuadernos;
- juguetes;
- pequeños detalles del día a día del protagonista.

Estos objetos no tienen que ser funcionales, pero sí deben reforzar la sensación de memoria y de identidad que define la Room.

---

## 7. El Otro Lado y el monstruo

### Importante

La idea del monstruo es buena, pero debe mantenerse clara y fácil de leer. No tiene por qué ser un enemigo complejo desde el principio. Debe sentirse más como amenaza existencial y psicológica que como un combate tradicional.

### Mecanismo recomendado

- En la Realidad Normal, la habitación puede iluminarse con la lámpara o con el interruptor.
- Cuando el jugador entra en el Otro Lado, esta luz debe ser crucial.
- Si la luz en la habitación está apagada o no se ha restaurado, el monstruo puede aparecer y atacar o bloquear el progreso.
- Si la luz está encendida, el monstruo se disipa o desaparece temporalmente.

Esto conecta de forma muy directa con la idea de la “luz” como mecanismo de anclaje emocional y psicológico.

### Recomendación para MVP

- Evitar que el monstruo mate de golpe al inicio.
- Mejor idea: el monstruo hace que el jugador pierda salud, empuje al personaje fuera de la zona o le fuerce a volver a la Realidad Normal.
- La pérdida de salud puede ser breve y clara, con recuperación automática al volver al mundo normal.

Esto hace que la Room sea peligrosa, pero aún manejable, y permite sentir la amenaza sin crear frustración excesiva.

---

## 8. [Descartado en esta iteración] Sistema de salud / cordura

El jugador debe tener una referencia visual muy clara de su estado físico y emocional.

### Recomendación

- Barra de salud o cordura en la parte superior izquierda.
- La barra puede representar un estado vulnerable, no necesariamente “vida” y “mente” separadas.
- En el Otro Lado, el daño aumenta más rápidamente.
- En la Realidad Normal, la salud se recupera con el paso del tiempo o al alejarse de la amenaza.

Esto conecta muy bien con la historia:

- la realidad alternativa daña al personaje;
- la realidad segura le permite recuperarse;
- el mundo no es neutral, está relacionado con su forma de soportar la verdad.

### Regla sugerida

- En el Otro Lado: pierde salud.
- En la Realidad Normal: recupera salud.
- Si llega a cero, el juego debe responder con una consecuencia clara, no necesariamente con un “game over” directo.

Para MVP, la opción más simple es: respawn en el punto de control de la habitación o retroceso a la zona segura.

---

## 9. Sugerencias de diseño para MVP

Estas son las decisiones que recomiendo mantener para un primer prototipo sin sobrecargar la implementación:

### Mantener

- La Bedroom como espacio personal y narrativo.
- La linterna como herramienta permanente en la UI.
- El armario como localización de la Llave.
- La luz como elemento central del peligro.
- El monstruo como amenaza reactiva, no como combate.
- [Descartado en esta iteración] La barra de salud / cordura visible.

### Simplificar

- No hace falta un sistema complejo de baterías en la primera versión.
- No hace falta que el monstruo tenga IA compleja; basta con que aparezca, persiga o bloquee la progresión.
- Si se quiere bajar aún más la complejidad, el monstruo puede ser un “estado de amenaza” más que un enemigo con ataque a distancia.
- La recuperación de salud puede ser automática en la Realidad Normal.

### Descartar por ahora

- Sistema de muerte total prematura.
- Cualquier puzzle adicional complicado dentro de la Bedroom.
- Una mecánica de luz demasiado compleja con varios niveles de intensidad.
- Más de un tipo de monstruo durante la primera iteración.
- Sistema de Salud no es necesario en esta iteración.

La Bedroom debe ser clara, memorable y emocionalmente fuerte, no demasiado compleja.

---

## 10. Información clave

- La Bedroom es la Room del recuerdo, la culpa y la negación.
- Es el lugar donde el protagonista construye su versión del Otro Lado.
- La luz no es solo una mecánica: es un ancla emocional del espacio.
- La linterna debe ser una herramienta accesible y permanente.
- La llave del armario es el primer gran objetivo dentro de la Room.
- El monstruo funciona mejor como amenaza de supervivencia que como enemigo de combate.
- [Descartado en esta iteración] La salud visualizada en la UI debe reflejar el coste emocional y físico de estar en la realidad alternativa.

---

## 11. Resumen corto

La Bedroom es mucho más que una habitación del juego: es el punto de origen de la historia del protagonista, el lugar donde comenzó a construir la mentira que le permite seguir adelante. Aquí se introducen la luz, la linterna, la amenaza del Otro Lado y la llegada de la primera llave esencial. El espacio debe sentirse íntimo, reconocible y profundamente personal, pero también amenazante, porque es el punto en el que la memoria del personaje empieza a romperse.
