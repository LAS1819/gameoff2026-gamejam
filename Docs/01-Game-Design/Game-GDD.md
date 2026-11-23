# Game Off 2026 - GAME DESIGN DOCUMENT

- Aventura gráfica de terror psicológico
- Versión: 0.1.1-proto
- Estado: Vertical slice jugable completo
- Creado: 03/11/2026
- Actualizado: 23/11/2026

> **Estado `0.1.1-proto`:** este documento combina el diseño vigente con ideas
> descartadas o aplazadas. El loop completo implementado se define en
> [MVP-Overview.md](./MVP-Overview.md) y
> [MVP-SPECIFICATION.md](../02-Technical-Design/MVP-SPECIFICATION.md).

---

________________________________________

## 1. Visión general

Aventura gráfica de terror psicológico basada en la exploración, la observación y la resolución de puzles mediante el cambio entre dos realidades superpuestas.

El jugador deberá alternar entre ambas realidades utilizando la habilidad SHIFT para descubrir pistas, acceder a nuevas rutas, modificar elementos del entorno y desvelar la verdad oculta tras los acontecimientos.

La experiencia está diseñada específicamente para la Game Off 2026 y gira en torno a la interpretación del tema "SHIFT" como un cambio entre diferentes planos de existencia.

### Características principales

- Género: Aventura gráfica point & click de terror psicológico.
- Estética: Pixel art.
- Perspectiva: Cámara fija por escenario.
- Protagonista: No visible en pantalla.
- Plataformas objetivo: Navegador y Windows.
- Mecánica Principal: SHIFT entre dos realidades.
- Duración estimada: 30-60 minutos.
- Jugabilidad: Exploración, observación, resolución de puzles y cambio entre realidades.
- Rejugabilidad: Eventos opcionales y múltiples finales.
- Finalización: desenlace narrativo y reinicio opcional al Lobby.
- Alcance: Proyecto diseñado para completarse en una game jam de un mes.

________________________________________

## 2. Pilares jugables

El diseño se sostiene sobre cuatro pilares fundamentales:

### 2.1 Exploración

Investigar escenarios y descubrir elementos relevantes. Se busca un equilibrio entre exploración, resolución de problemas y situaciones de tensión.

### 2.2 SHIFT

Alternar entre dos versiones de la realidad para descubrir información y resolver puzles.

### 2.3 Estados del Mundo

Las acciones realizadas en una realidad pueden afectar a la otra.

### 2.4 Terror Psicológico

Generar tensión mediante la ambientación, los sonidos y los eventos inesperados.

________________________________________

## 3. Sistema SHIFT

El jugador dispone de la capacidad de alternar entre dos realidades:

### Realidad Normal

Representa el mundo físico aparente.

- **Características:**
  - Entornos relativamente seguros.
  - Información parcial.
  - Objetos funcionales.

### Realidad Distorsionada (El Otro Lado)

Representa una versión alterada del mundo.

- **Características:**
  - Aparición de elementos ocultos.
  - Cambios estructurales.
  - Manifestaciones hostiles.
  - Información adicional sobre la historia.

- **Reglas**
  - El cambio es instantáneo.
  - Está disponible en Hallway, Bedroom, Library y Basement.
  - No está disponible en Lobby ni en Forest.
  - El jugador conserva su posición.
  - Cada escenario (o casi todos) posee una versión en ambas realidades.

________________________________________

## 4. Sistema Temporal

**Fuera del alcance de `0.1.1-proto`.**

La propuesta inicial de reloj global, espera y eventos por hora se conserva como
idea histórica, pero no existe en la versión jugable. La progresión actual
avanza únicamente mediante flags, objetos e interacciones de rooms.

________________________________________

## 5. Sistema de inventario

### Características

- Sin límite de espacio.
- Objetos importantes únicamente.
- No existen acumulaciones.
- No existen recursos consumibles.

### Objetivos

- Reducir microgestión.
- Facilitar el desarrollo.
- Centrar la experiencia en los puzles.

________________________________________

## 6. Sistema de interacción

La interacción utiliza clic directo sin menú contextual.

### Flujo de interacción

1. Clic izquierdo: interactuar, recoger, usar, cambiar de sala o avanzar.
2. Clic derecho: examinar sin modificar el estado.
3. Los items seleccionados se validan contra el interactable.
4. Se actualizan flags, inventario y presentación.

### Acciones contempladas

| Acción | Descripción |
| -------- | ------------- |
| Examinar | Obtener información, pistas o detalles del entorno. |
| Coger | Recoger un objeto e incorporar al inventario. |
| Usar | Interactuar con un objeto, mecanismo o elemento del escenario. |

Las acciones específicas como Abrir, Cerrar, Empujar o Activar serán resueltas internamente por el sistema y no se mostrarán como opciones independientes.

________________________________________

## 7. Tipos de elementos interactivos

### 7.1. Objetos interactivos

Elementos del escenario con los que se pueden realizar acciones del Jugador.
Ejemplos: puertas, ventanas, teléfonos, muebles y mecanismos.

### 7.2. Objetos recogibles

Elementos que pueden incorporarse al inventario y utilizarse posteriormente para resolver puzles.

### 7.3. Elementos de observación

Elementos que proporcionan información, pistas o descripciones, pero que no necesariamente permiten acciones físicas.
Un mismo elemento podrá pertenecer a varias categorías.

## 8. Estados de los objetos

Los elementos interactivos podrán tener diferentes estados internos dependiendo de la realidad activa.

### Estados Principales

- Visible / Oculto
- Abierto / Cerrado
- Intacto / Dañado
- Activo / Inactivo
- Presente / Ausente

Los cambios de estado podrán afectar:

- Las acciones disponibles.
- El acceso a zonas.
- La aparición de pistas.
- La resolución de puzles.

Los estados constituyen la base de la mayoría de puzles del juego.

________________________________________

## 9. Uso de Objetos y Experimentación

El jugador podrá utilizar los objetos del inventario sobre elementos del escenario.

### Principios de diseño

- Las combinaciones válidas deberán tener coherencia.
- Las pistas deben permitir deducir la solución.
- Las respuestas incorrectas deben ser coherentes.
- Se evitarán combinaciones absurdas.

### Restricciones

- No se permitirá combinar objetos directamente dentro del inventario.
- Las interacciones siempre se realizarán entre:
- Objeto del Inventario  Elemento del Escenario

________________________________________

## 10. Guardado y Progreso

### Reinicio de prototipo

El prototipo mantiene el progreso durante la sesión y ofrece reinicio completo desde el final.

### Principios

- No hay partidas persistentes en el scope actual.
- No hay puntos de guardado manual.
- Reiniciar restablece flags, inventario, profundidad del Forest y room activa.

Un sistema de guardado persistente queda fuera del MVP. Puede evaluarse en una iteración posterior si el alcance final lo justifica.

________________________________________

## 11. Eventos dinámicos y rejugabilidad

El juego contará con eventos opcionales capaces de modificar determinados acontecimientos de cada partida.

### Tipos de eventos implementados

- Solo visibles o accesibles en una de las dos realidades.

#### Eventos de decisión

- Activados por determinadas acciones realizadas por el jugador.
- Las Memorias opcionales dependen de un primer examen y permanecen como flags
  durante la sesión.

### Objetivo

- Aumentar la sensación de descubrimiento y favorecer múltiples finales sin incrementar significativamente el contenido necesario.

________________________________________

## 12. Perspectiva y presentación

El juego utilizará escenarios de pixel art con cámara fija.

- Protagonista no visible.
- Una vista fija por escenario.
- Navegación mediante puntos de salida interactivos.
- Transiciones simples entre escenarios.

El uso de cámara fija reduce el alcance de producción y aumenta el detalle visual de cada localización.

________________________________________

## 13. Plataformas y accesibilidad

### Plataforma objetivo

- Navegador (WebGL)
- PC (Windows)

### Principios de accesibilidad

- Controles simples
- Interacciones claras
- Interfaz legible
- Compatible con ratón y teclado
- Sesiones de juego cortas

________________________________________

## 14. Filosofía de diseño

El proyecto está diseñado específicamente para ser desarrollado por una sola persona (de momento) durante una game jam.

### Prioridades

- La mecánica SHIFT es el núcleo de toda interacción.
- Cada escenario debe tener una razón para existir en dos realidades.
- La complejidad debe surgir de las relaciones entre sistemas simples.
- Se prioriza la finalización del proyecto sobre la cantidad de contenido.
- Cada elemento del juego debe contribuir al misterio principal.
- Menos escenarios, más significado.
- Reutilización inteligente de sistemas.
- Alcance controlado y finalización del proyecto por encima de la cantidad de contenido.

### Alcance controlado

- Alcance controlado y finalización del proyecto por encima de la cantidad de contenido.

________________________________________
