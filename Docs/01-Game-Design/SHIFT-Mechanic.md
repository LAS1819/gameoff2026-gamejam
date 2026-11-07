# Game Off 2026 - SHIFT MECHANIC

- Versión 0.1 - Diseño de Interacción y Progresión
- Creado: 05/11/2026
- Actualizado: 05/11/2026

---

## 1. Propósito

La mecánica SHIFT es el corazón del MVP. Permite al jugador cambiar entre dos versiones de la realidad: la Realidad Normal y El Otro Lado.

Su propósito es:

- proporcionar una herramienta principal para resolver puzles;
- crear la sensación de que el jugador está descubriendo un aspecto oculto del edificio;
- permitir que el jugador progrese en zonas que parecen bloqueadas;
- reforzar la atmósfera de "no todo es lo que parece".

---

## 2. Activación

### Método 1: Tecla de Teclado

- **Tecla**: LEFT SHIFT
- **Comportamiento**: Presionar activa/desactiva el cambio de realidad
- **Respuesta**: Instantánea (sin animación de transición en MVP)

### Método 2: Icono de UI - "El Amuleto"

- **Elemento**: Icono interactivo en la pantalla
- **Posición**: Esquina superior derecha de la pantalla (esquina de UI)
- **Aspecto**: Figura de un encapuchado, como un amuleto que porta el protagonista
- **Clickabilidad**: El jugador puede hacer click en el icono para activar SHIFT
- **Equivalencia**: Hacer click en el icono = presionar LEFT SHIFT

---

## 3. Progresión de Disponibilidad

### Fase 1: Vestíbulo (Desactivado)

En el Lobby, el icono del Amuleto está siempre visible.

**Estado:**

- Visible pero inerte.
- No parpadea.
- No hay feedback de que se pueda usar.
- Si el jugador intenta usarlo (click o tecla), ocurre uno de estos:
  - No pasa nada (silencio).
  - Aparece un pequeño aviso en pantalla: "No es el momento" o "La energía aún no despierta".

**Propósito:**

- familiarizar al jugador con el elemento;
- preparar mentalmente su disponibilidad futura;
- evitar confusión cuando llegue al Hallway.

### Fase 2: Hallway (Activado)

En el Pasillo Principal, el icono del Amuleto pasa a estar activo.

**Estado:**

- Visible y funcional.
- Parpadea o emite un efecto visual sutil (brillo, pulsación).
- Click o tecla activa el cambio de realidad.
- Feedback inmediato: cambio visual en pantalla y en el propio icono.

**Propósito:**

- señalar visualmente que el cambio de mecánica es inminente;
- invitar al jugador a experimentar con la herramienta;
- marcar un punto de no retorno narrativo.

---

## 4. Estética del Icono

### Realidad Normal

- **Aspecto**: Figura de encapuchado de color blanco, gris o dorado claro.
- **Estilo**: Silueta simple pero reconocible.
- **Luminosidad**: Moderada, con contornos claros.
- **Animación**: Parpadeo sutil (si está activo en Hallway).
- **Sensación**: Protección, guía, esperanza contenida.

### El Otro Lado

- **Aspecto**: La misma figura pero con inversión de color y presencia.
- **Estilo**: Más oscuro, con posible aura o distorsión.
- **Color**: Morado oscuro, azul profundo o negra con tintes irreales.
- **Luminosidad**: Más oscuro, pero potencialmente con brillo antinatural (azul, morado).
- **Animación**: Parpadeo más agresivo o pulsante si está activo.
- **Sensación**: Corrupción, misterio, poder incontrolado.

---

## 5. Comportamiento del Cambio

### En el Lobby

Si el jugador intenta activar SHIFT:

- **Opción A (Silencio)**: No ocurre nada. El icono no responde.
- **Opción B (Feedback)**: Aparece un aviso breve: "La energía no responde aquí" o similar.
- **Recomendación**: Opción A es más inmersiva, pero Opción B es más clara para el primer jugador.

### En el Hallway

Cuando se activa SHIFT:

- La pantalla cambia inmediatamente entre ambas versiones del escenario.
- El icono refleja el estado actual (Normal o Otro Lado).
- El sonido (si existe) marca la transición.
- El jugador puede seguir haciendo click/pulsando SHIFT para cambiar entre realidades.

---

## 6. Progresión de Comprensión

### Acto 1: Introducción (Lobby)

El icono existe pero el jugador no comprende su función.

- Aprendizaje: "Hay algo aquí, pero aún no puedo usarlo".

### Acto 2: Despertar (Hallway)

El icono se activa y el jugador descubre la mecánica.

- Aprendizaje: "Puedo cambiar entre dos realidades".
- Momento clave: Primera activación de SHIFT.

### Acto 3: Maestría (Futuro)

En zonas posteriores, el jugador usa SHIFT como herramienta natural.

- Aprendizaje: "Sé cómo usar esto para resolver puzles".

---

## 7. Indicadores Visuales

El icono debe comunicarse claramente en cada estado:

| Estado | Lobby | Hallway |
| -------- | ------- | --------- |
| Visible | Sí | Sí |
| Clickeable | No (o sin efecto) | Sí |
| Parpadea | No | Sí |
| Feedback en click | Ninguno o aviso | Cambio inmediato |
| Estética | Normal (clara) | Cambia con realidad |
| Posición | Esquina superior derecha | Esquina superior derecha |

---

## 8. Notas de Implementación

Para el MVP:

- El icono debe ser pequeño y no ocupar más de 64x64 píxeles.
- Debe estar siempre visible excepto durante transiciones de escena.
- La lógica de "activo/inactivo" depende de la sala actual y si la flag `RealityUnlocked` está activada.
- El cambio entre realidades es instantáneo en MVP (sin animación de transición).
- El audio de transición es opcional en MVP, pero recomendado para refuerzo.

### Sistema de Input

La entrada del teclado (LEFT SHIFT) utiliza **Unity Input System Package** (no el antiguo Input.GetKeyDown):

```csharp
// InputHandler.cs usa:
using UnityEngine.InputSystem;

if (Keyboard.current != null && Keyboard.current.leftShiftKey.wasPressedThisFrame)
{
    RealityManager.Instance?.ToggleReality();
}
```

**Requisito en Player Settings**: El proyecto debe tener "Active Input Handling" configurado a "Input System Package".

**Ventajas**:

- Mejor rendimiento y arquitectura.
- Soporte para múltiples dispositivos (teclado, gamepad, etc.).
- Más flexible para futuros cambios de keybindings.

**Alternativa futura**: Si necesitamos remapear teclas o usar múltiples dispositivos, podemos expandir a un InputActionAsset completo. Por ahora, acceso directo es suficiente para MVP.

---

## 9. Relación con la Progresión de Puzles

El icono es también un indicador invisible de progresión:

- Su activación marca el paso de la zona segura (Lobby) a la zona desafiante (Hallway).
- Visual: el cambio de estética del icono refleja el cambio de realidad en la pantalla.
- Narrativo: el jugador "despierta" a la verdadera naturaleza del edificio a través de este amuleto.

---

## 10. Futuro Expandido

Más allá del MVP, el icono podría:

- obtener habilidades adicionales o variantes según el progreso;
- reaccionar a eventos especiales (radiación, corrupción, etc.);
- cambiar de estética según el final que esté "programado";
- ser un elemento de recompensa o castigo narrativo.

Para el MVP, permanece como herramienta funcional y narrativa sin complejidad adicional.
