# Game Off 2026 - MVP

- **Versión:** 0.1
- **Creación:** 03/11/2026
- **Última actualización:** 03/11/2026

## Objetivo

Validar la arquitectura principal del proyecto y demostrar que la mecánica SHIFT funciona dentro de un flujo de juego real.

El MVP no busca demostrar contenido completo ni calidad artística final.

Su único objetivo es demostrar que los sistemas fundamentales funcionan juntos correctamente.

---

## Alcance

### Habitaciones

- Vestíbulo
- Pasillo Principal

### Objetos

- Llave del Personal

### Mecánicas

- Interacción
- Inventario
- Cambio de sala
- Sistema de Flags
- Sistema SHIFT

### Fuera del alcance

- Sistema temporal
- Eventos dinámicos
- Finales
- Biblioteca
- Dormitorio
- Sótano
- Sala Ritual
- Guardado
- Arte definitivo
- Audio definitivo

---

## Flujo Jugable

Entrar en Vestíbulo

↓

Examinar entorno

↓

Recoger Llave del Personal

↓

Activar Flag: HasStaffKey

↓

Usar llave sobre puerta

↓

Activar Flag: DoorOpened

↓

Acceso al Pasillo Principal

↓

Cambiar de sala

↓

Activar SHIFT

↓

Cambio a El Otro Lado

↓

Actualización visual del escenario

↓

Elementos visibles cambian

↓

El obstáculo desaparece

↓

Paso libre

---

## Sistemas Obligatorios

### GameManager

Responsable del estado global del juego.

---

## RoomManager

Debe permitir:

- Cargar habitaciones
- Cambiar habitaciones
- Mostrar los elementos correspondientes

---

### InventoryManager

Debe permitir:

- Añadir objetos
- Eliminar objetos
- Consultar objetos
- Usar objetos

---

### InteractionManager

Debe permitir:

- Examinar
- Coger
- Usar

---

### RealityManager

Debe permitir:

- Cambiar entre Normal y Other Side
- Actualizar el estado de los interactivos
- Actualizar elementos visuales

---

### Flag System

Debe permitir:

- SetFlag()
- HasFlag()

Toda la progresión del MVP dependerá exclusivamente de Flags.

---

## Flags del MVP

```text
HasStaffKey
DoorOpened
RealityUnlocked
```

---

## Casos de uso a validar

### Caso 1

Recoger un objeto.

Resultado esperado:

- El objeto desaparece del escenario.
- El objeto aparece en el inventario.

---

### Caso 2

Utilizar objeto sobre interactivo.

Resultado esperado:

- Se consume la interacción.
- Se activa una Flag.

---

### Caso 3

Transición entre habitaciones.

Resultado esperado:

- Nueva habitación cargada.
- Interactivos actualizados.

---

### Caso 4

Cambio de realidad.

Resultado esperado:

- Cambio visual inmediato.
- Cambio de interactivos visibles.
- Persistencia de la posición del jugador.

---

## Arte Temporal

Durante el MVP se utilizarán:

- Sprites provisionales.
- Fondos provisionales.
- UI mínima.
- Efectos temporales.

No se desarrollará arte definitivo.

La prioridad absoluta es validar la arquitectura de juego.

---

## Definición de Terminado

El MVP se considerará finalizado cuando sea posible grabar un vídeo donde se vea:

Vestíbulo

↓

Recoger llave

↓

Inventario actualizado

↓

Abrir puerta

↓

Entrar al Pasillo

↓

Activar SHIFT

↓

Cambio de realidad

↓

Cambio visible del escenario

↓

Obstáculo desaparece

Si este flujo funciona de principio a fin:

✅ Arquitectura validada

✅ Vertical Slice viable

✅ Riesgo técnico principal eliminado

✅ Comienza el desarrollo real del juego
