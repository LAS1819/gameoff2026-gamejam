# Technical Design - MVP

**Quick Links:**

- **[MVP-SPECIFICATION.md](./MVP-SPECIFICATION.md)** ← START HERE (contains everything)
- [Technical-Design.md](./Technical-Design.md) (high-level design reference only)
- [MVP-Sprint-1.md](./MVP-Sprint-1.md) (sprint goals)
- [GameObject-Hierarchy.md](./GameObject-Hierarchy.md) (general patterns reference)

---

## 📖 How to Use This Folder

### For Building the MVP

**Open [MVP-SPECIFICATION.md](./MVP-SPECIFICATION.md) and follow the 5 phases:**

1. **Phase 1:** Create scenes & hierarchy (30 min)
2. **Phase 2:** Add interactables with data (45 min)
3. **Phase 3:** Attach scripts (30 min)
4. **Phase 4:** Initialize data in GameManager (20 min)
5. **Phase 5:** Validate in Play Mode (30 min)

**Total: 2.5-3 hours to working MVP**

---

### For Understanding Architecture

Read [Technical-Design.md](./Technical-Design.md) for high-level philosophy.

---

## 📋 Files

| File | Purpose |
| ------ | --------- |
| **MVP-SPECIFICATION.md** | Complete MVP spec with 5 phases, data tables, validation checklist |
| **MVP-Sprint-1.md** | Sprint goals and acceptance criteria for the base playable flow |
| **MVP-Sprint-2.md** | Recommended next sprint priorities for Bedroom, light gating and room flow continuity |
| **Room-Architecture.md** | Recommended room construction pattern to keep GameManager lean and data-driven |
| Technical-Design.md | High-level architecture philosophy |
| TD-HUD.md | Runtime HUD structure using Panel Renderer for the MVP polish pass |
| GameObject-Hierarchy.md | General hierarchy patterns |
| Gameplay/Rooms/Room-Lobby.md | Technical room spec for the Lobby: interactables, items, flags and flow |
| Gameplay/Rooms/Room-Hallway.md | Technical room spec for the Hallway: SHIFT, reality gating and transition logic |
| Gameplay/Rooms/Room-Bedroom.md | Technical room spec for the Bedroom: light, flashlight, key and threat system |
| Gameplay/Rooms/Room-Bedroom-Setup.md | Unity hierarchy and naming blueprint for the Bedroom room setup |

**That's it. No more.**
