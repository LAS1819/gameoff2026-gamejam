---
description: 'Unity + C# coding guidelines'
applyTo: '**/*.cs'
---

# Unity + C# Development

## General
- Use the C# language version already configured by the project. Do not force a newer version unless the project actually targets it.
- Prefer simple, readable C# over clever abstractions or over-engineered patterns.
- Follow the projects existing style and conventions before introducing new ones.
- Favor maintainability, clarity, and gameplay correctness over theoretical architecture purity.

## Naming and style
- Use PascalCase for types, methods, properties, and public members.
- Use camelCase for private fields, local variables, parameters, and method locals.
- Use [SerializeField] for serialized private fields instead of exposing them publicly unless there is a clear API reason.
- Use nameof(...) instead of string literals when referring to members.
- Keep formatting consistent with the projects .editorconfig and the surrounding codebase.

## Unity-specific conventions
- Prefer explicit references over runtime discovery. Avoid FindObjectOfType and repeated GetComponent calls in runtime loops.
- Respect Unity lifecycle methods and avoid doing heavy work in constructors.
- Keep gameplay logic small, focused, and composable.
- Prefer composition and small systems over one giant Manager.
- Use ScriptableObjects for data, configuration, and reusable gameplay rules.
- Avoid unnecessary Singletons; only use them where a truly global service is justified.
- Minimize coupling between systems and avoid magical registration patterns unless the project already relies on them.

## Nullability
- Enable nullable reference types and keep them consistent.
- Use is null / is not null instead of == null / != null.
- Declare values as non-nullable when they cannot be null and guard at the system boundaries.
- Do not add redundant null checks where the type system already guarantees safety.

## Comments
- Write comments only when they explain intent, invariants, non-obvious trade-offs, or domain-specific logic.
- Avoid comments for trivial code or for every method.
- If a dependency or external system is introduced, briefly explain why it is there and what problem it solves.

## Testing and validation
- Add tests for critical gameplay logic, edge cases, and bug-fix regressions.
- Prefer focused tests that validate actual behavior over broad test scaffolding.
- Keep validation simple and tied to real game rules.

## Scope and architecture
- Do not introduce ASP.NET Core, EF Core, JWT, Swagger, controller patterns, or other web-only abstractions unless the project explicitly requires them.
- Keep the solution scoped to the actual project and avoid adding enterprise patterns to a game project.
- Favor small reusable systems and readable gameplay code over “future-proof” abstractions.
- If a feature can be solved in a simpler way, prefer the simpler solution.

---
