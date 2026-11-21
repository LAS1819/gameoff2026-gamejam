# Game Off 2026 - HABITACIÓN / BEDROOM

- Versión 0.1 - Diseño Visual
- Creado: 10/11/2026
- Actualizado: 10/11/2026

---

## 1. Objetivo del escenario

La Bedroom es la primera habitación íntima del juego y la más importante emocionalmente.

Su función visual es:

- transmitir la sensación de infancia/adolescencia;
- hacer que el espacio se sienta familiar y personal;
- introducir la sensación de memoria y de presencia ausente;
- convertir la luz en un elemento narrativo y mecánico central;
- preparar la tensión del Otro Lado sin romper la claridad del diseño.

El jugador debe entender enseguida:

- que se encuentra en un espacio personal y cargado emocionalmente;
- que hay una estructura doméstica muy reconocible;
- que la habitación no es neutral: es el punto de origen de la deformación;
- que la luz y la oscuridad no son solo estética, sino una herramienta de supervivencia.

---

## 2. Composición general

### Cámara

- Cámara fija.
- Fondo de 640x360.
- Sin desplazamiento.
- Sin personaje visible.
- Un único punto de vista frontal y ligeramente elevado.

### Encuadre

La composición debe sentirse como una habitación real, ordenada, doméstica y algo íntima. El jugador debe verla de inmediato como el refugio del protagonista.

Distribución aproximada:

```plaintext
┌───────────────────────────────────────┐
│                                       │
│         Pared / Mesa / Decoración     │
│                                       │
│           Cama / Colchón              │
│                                       │
│         Mesita de Noche      Armario  │
│                                       │
│    Lámpara / Interruptor        Ventana│
│                                       │
│       Puerta salida / Sala siguiente   │
│                                       │
└───────────────────────────────────────┘
```

La composición no tiene que ser compleja, pero debe ser legible de un vistazo.

---

## 3. Historia ambiental

La habitación debe sugerir vida cotidiana, no muerte total. Debe sentirse habitada o al menos vivida, como si alguien siguiera dejando marcas del paso del tiempo.

Elementos recomendados:

- cama simple y baja;
- mesita de noche con objetos de uso cotidiano;
- lámpara de habitación con interruptor cercano;
- armario de madera antiguo;
- ventana con marco sencillo;
- pared con pósteres, fotos o dibujos;
- ropa, libros o juguetes discretos;
- detalles personales que refuercen la idea de infancia/adolescencia.

La tensión no llega por exceso de elementos, sino por la sensación de que la room está “viva” de forma indebida.

---

## 4. Paleta visual

### Realidad Normal

Colores dominantes:

- beige apagado;
- marrón claro y oscuro;
- verde sucio o gris cálido;
- amarillo tenue de objetos viejos;
- rosa o azul muy desaturado en detalles personales.

Sensación:

- memoria;
- permanencia;
- intimidad;
- nostalgia no amable.

### El Otro Lado

Colores dominantes:

- morado muy oscuro;
- azul profundo;
- negro mate;
- tonos grises helados;
- luces enfermizas o casi blancas.

Sensación:

- distorsión;
- claustrofobia;
- presencia dañina;
- realidad reescrita.

### Iluminación

La luz debe ser un elemento narrativo clave.

- Realidad Normal: la habitación puede estar algo apagada, pero digestible;
- la lámpara o la luz general deben ayudar a reconocerla y a dar sensación de refugio;
- El Otro Lado: la luz se vuelve más rara, más fría y más necesaria.

La diferencia entre ambas realidades debe ser clara sin que la escena deje de ser legible.

---

## 5. Interactivos principales

### Cama - Bed

Función:

- pieza central de la habitación;
- refuerza la sensación de memoria y de intimidad;
- puede incluir un pequeño detalle narrativo o una pista de la vida del protagonista.

Visualmente:

- debe ser reconocible en una lectura rápida;
- no hace falta más detalle que el imprescindible;
- puede ser el punto visual más claro de la room.

---

### Mesita de noche - Nightstand

Función:

- alberga la linterna;
- marca la zona más personal del espacio;
- ayuda a introducir la idea de “objeto persistente” mediante una UI de linterna.

Visualmente:

- pequeña y reconocible;
- debe tener una posición clara dentro de la composición;
- su forma puede ser más ornamental que funcional.

---

### Lámpara / interruptor - Lamp / Switch

Función:

- activa la luz de la habitación;
- se convierte en la herramienta principal para mitigar la amenaza del Otro Lado;
- comunica que la luz del espacio es un mecanismo narrativo y de supervivencia.

Visualmente:

- debe ser un objeto legible desde lejos;
- su encendido debe ser claramente visible;
- puede estar junto a la mesita o en la pared cercana.

---

### Armario - Wardrobe

Función:

- objeto central de tensión;
- aquí está la Llave de la Librería;
- en el Otro Lado puede revelar la presencia del monstruo si la luz está apagada;
- debe ser la amenaza directa más clara de la habitación.

Visualmente:

- gran, oscuro, reconocible;
- se puede abrir con un movimiento simple y claro;
- su forma debe ser muy nítida para que el jugador entienda que es un punto de riesgo.

Importante:

- no es un “enemigo” en sí mismo;
- es un punto de tensión estructural que se activa por la lógica de la realidad.

---

### Ventana - Window

Función:

- ancla la sensación de espacio doméstico;
- transmite la idea de encierro y de realidad rota;
- en El Otro Lado debe estar tapiada o bloqueada con ladrillos.

Visualmente:

- debe dibujar una fuerte silueta de ventana;
- con su cambio en el Otro Lado, la diferencia debe ser ligera pero explícita;
- no necesita ser grande, pero sí debe leerse rápidamente.

---

### Puerta / salida - Door / Hallway Exit

Función:

- permite salir de la habitación;
- conecta con la progresión del edificio;
- debe sentirse una salida funcional, no un elemento decorativo.

Visualmente:

- un marco claro con una composición fácil de interpretar;
- no debe competir con el armario ni con el punto de luz.

---

## 6. Ítems y accesorios clave

### Linterna - Flashlight

Debe ser un objeto muy claro, reconocible y visualmente destacable.

Recomendación:

- no debe depender de un sprite extremadamente complejo;
- el silueta debe ser simple y con un contorno definido;
- debe funcionar bien en la UI de inventario o en un slot especial de herramienta.

La idea principal es que no se “guarde” como objeto normal; debe sentirse como una herramienta constante al servicio del jugador.

---

### Llave de la Librería - Library Key

- debe aparecer en el armario en el Otro Lado;
- debe sentirse un objeto directo, compacto y legible;
- si se acompaña de un icono o de un prominente glow mínimo, mejor para su legibilidad;
- no es un item decorativo: es la pieza de progresión que saca al jugador de la habitación.

---

### Objetos secundarios

Se pueden incluir detalles como:

- fotos;
- libros;
- criadores o muñecos pequeños;
- cajones;
- ropa;
- ilustraciones en la pared;
- objetos de infancia/adolescencia.

Estos elementos no tienen que ser funcionales, pero sí deben reforzar la narrativa y la comodidad del espacio.

---

## 7. El Otro Lado

La habitación debe seguir manteniendo casi la misma composición general en ambos mundos, para reforzar la sensación de continuidad temporal y emocional.

### Cambios recomendados

- la ventana está tapiada o rota;
- la luz cambia de temperatura y de intensidad;
- el armario se vuelve más hostil y más cargado de tensión;
- la habitación gana sombras más largas y un aire más opresivo;
- el color general se vuelve más frío;
- la sensación de cierre aumenta.

La clave es que la room siga siendo reconocible. El terror debe venir de la diferencia interna, no de la desaparición del espacio.

---

## 8. Recomendación de producción artística

### Nivel de detalle

No hace falta un fondo muy complejo. Lo importante es:

- claridad visual;
- legibilidad de cada interactivo;
- coherencia entre room normal y room distorsionada;
- una sensación de intimidad enorme.

### Regla principal

La Bedroom debe ser una habitación “sencilla” en estructura, pero muy fuerte en presencia. Si un elemento no aporta: identidad, memoria, tensión o progresión, no hace falta incluirlo.

---

## 9. Checklist de producción

### Obligatorio

- [x] Cama
- [x] Mesita de noche
- [x] Lámpara / interruptor
- [x] Armario
- [x] Ventana
- [x] Puerta de salida
- [x] Linterna
- [x] Llave de la Librería
- [x] Diferenciación visual entre Realidad Normal y Otro Lado
- [x] Legibilidad del objeto clave

### Recomendado

- [ ] Fotos en pared
- [ ] Libros o cuadernos
- [ ] Ropa o juguetes
- [ ] Detalles personales del protagonista
- [ ] Grietas, sombras o marcas sutiles en la pared
- [ ] Efecto de brillo o tenue distorsión en la lámpara

---

## 10. Resultado esperado

La Bedroom debe sentirse como:

- un lugar de memoria;
- un sitio íntimo y poco seguro;
- el primer punto en el que la mente del protagonista empieza a imponer su propia lógica;
- el primer espacio donde la luz se convierte en un recurso de supervivencia;
- el lugar donde el jugador aprende que la casa no es solo un edificio, sino un campo emocional distorsionado.

En resumen: la room debe ser muy clara, muy reconocible y muy cargada emocionalmente, con la luz como eje principal de la experiencia.
