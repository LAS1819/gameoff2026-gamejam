---
description: 'Unity + C# coding guidelines'
applyTo: '**/*.cs'
---

# Unity + C# Development

## General
- Use the C# language version already configured by the project. Do not force a newer version unless the project actually targets it.
- Prefer simple, readable C# over clever abstractions or over-engineered patterns.
- Follow the project's existing style and conventions before introducing new ones.
- Favor maintainability, clarity, and gameplay correctness over theoretical architecture purity.

## Unity Version
- Target Unity version: 6000.6.8f1.
- Generate code compatible with Unity 6000.6.8f1 APIs.
- Avoid using deprecated Unity APIs, classes, methods, or properties.
- When suggesting or generating code, always prefer the current recommended Unity replacement APIs.
- If a Unity API is obsolete or marked with `[Obsolete]`, use the recommended alternative.
- Unity 6000.6.8f1 uses the Input System package as the default input solution.
- Never generate new code based on the legacy Input Manager unless explicitly requested.

Examples:
- Prefer `Object.FindObjectsByType<T>()` over deprecated `Object.FindObjectsOfType<T>()`.
- Prefer `FindObjectsByType<T>(FindObjectsSortMode.None)` unless deterministic sorting is explicitly required.
- Do not introduce new usages of Unity APIs that generate CS0618 warnings.

## Naming and Style
- Use PascalCase for types, methods, properties, and public members.
- Use camelCase for private fields, local variables, parameters, and method locals.
- Use `[SerializeField]` for serialized private fields instead of exposing them publicly unless there is a clear API reason.
- Use `nameof(...)` instead of string literals when referring to members.
- Keep formatting consistent with the project's `.editorconfig` and the surrounding codebase.

## Unity-Specific Conventions
- Prefer explicit references over runtime discovery.
- Avoid `FindObjectOfType`, `FindObjectsOfType`, and repeated `GetComponent` calls in runtime loops.
- Respect Unity lifecycle methods and avoid doing heavy work in constructors.
- Keep gameplay logic small, focused, and composable.
- Prefer composition and small systems over one giant Manager.
- Use ScriptableObjects for data, configuration, and reusable gameplay rules.
- Avoid unnecessary Singletons; only use them where a truly global service is justified.
- Minimize coupling between systems and avoid magical registration patterns unless the project already relies on them.

## Null Handling
- Do not use null-propagation operators (`?.`).
- Do not use null-coalescing assignment operators (`??=`).
- Prefer explicit null checks with early returns.
- Keep null handling straightforward and easy to debug.
- Use Unity's null comparison semantics when working with `UnityEngine.Object` derived types.

Preferred:

```csharp
if (target == null)
{
    return;
}

target.Execute();
```

Avoid:

```csharp
target?.Execute();
```

## Comments
- Write comments only when they explain intent, invariants, non-obvious trade-offs, or domain-specific logic.
- Avoid comments for trivial code or for every method.
- If a dependency or external system is introduced, briefly explain why it is there and what problem it solves.

## Testing and Validation
- Add tests for critical gameplay logic, edge cases, and bug-fix regressions.
- Prefer focused tests that validate actual behavior over broad test scaffolding.
- Keep validation simple and tied to real game rules.

## Scope and Architecture
- Do not introduce ASP.NET Core, EF Core, JWT, Swagger, controller patterns, or other web-only abstractions unless the project explicitly requires them.
- Keep the solution scoped to the actual project and avoid adding enterprise patterns to a game project.
- Favor small reusable systems and readable gameplay code over future-proof abstractions.
- If a feature can be solved in a simpler way, prefer the simpler solution.

## AI Assistant Rules
- Never generate code using deprecated Unity APIs.
- Always target Unity 6000.6.8f1 compatibility.
- Always use the Input System package for player input.
- Never use the legacy Input Manager APIs.
- Prefer FindObjectsByType<T>() over deprecated FindObjectsOfType<T>().
- Do not use null-propagation operators (`?.`).
- Do not introduce compiler warnings, obsolete API usage, or deprecated patterns.

## Input System

- This project uses Unity Input System exclusively.
- Assume Player Settings > Active Input Handling = Input System Package.
- Never use UnityEngine.Input.
- Never generate code using:
  - Input.mousePosition
  - Input.GetKey(...)
  - Input.GetKeyDown(...)
  - Input.GetKeyUp(...)
  - Input.GetMouseButton(...)
  - Input.GetMouseButtonDown(...)
  - Input.GetMouseButtonUp(...)
  - Input.GetAxis(...)
  - Input.GetAxisRaw(...)

- Use UnityEngine.InputSystem APIs instead.
- Use Mouse.current, Keyboard.current, Gamepad.current, InputAction, InputActionReference, and PlayerInput where appropriate.
- Any generated code must be compatible with Input System without requiring Legacy Input Manager support.

Preferred:

```csharp
using UnityEngine.InputSystem;

Vector2 mousePosition = Mouse.current.position.ReadValue();
```

Avoid:

```csharp
Vector3 mousePosition = Input.mousePosition;
```

---