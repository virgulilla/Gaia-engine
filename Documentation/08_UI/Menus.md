# UI-003 — Menus

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the menu system used throughout Gaia Engine.

Menus provide structured navigation to gameplay features without interrupting the simulation more than necessary.

---

# Scope

This specification defines:

- menu hierarchy
- navigation
- menu lifecycle
- modal windows
- interaction rules

It does not define:

- HUD
- rendering
- gameplay logic
- simulation

---

# Philosophy

Menus organize information.

Menus never become the primary gameplay experience.

Whenever possible, the simulation should remain visible.

---

# Responsibilities

The Menu System is responsible for:

- opening menus
- closing menus
- navigation
- focus management
- modal dialogs

The Menu System never:

- execute gameplay logic
- modify simulation state
- own persistent gameplay data

---

# Menu Hierarchy

```text
Menus

├── Main Menu
├── Pause Menu
├── World Creation
├── Encyclopedia
├── Statistics
├── Settings
├── Save / Load
└── Confirmation Dialogs
```

---

# Main Menu

Provides access to:

- Continue
- New World
- Load World
- Encyclopedia
- Settings
- Credits
- Exit

---

# Pause Menu

Provides access to:

- Resume
- Save
- Load
- Settings
- Main Menu

Simulation pauses according to game mode.

---

# World Creation

Allows configuration of:

- World Seed
- World Size
- Climate Preset
- Difficulty Preset
- Simulation Options

World generation begins only after confirmation.

---

# Encyclopedia

Provides access to:

- Species
- Traits
- Biomes
- Resources
- Evolution History
- Search

---

# Statistics

Displays:

- Population Graphs
- Species Count
- Climate History
- Extinction Events
- Discovery Progress

---

# Settings

Provides access to:

- Graphics
- Audio
- Controls
- Accessibility
- Language
- Gameplay

Settings are grouped into categories.

---

# Save / Load

Supports:

- Manual Save
- Auto Save
- Quick Save
- World Selection

Save previews may include:

- World Name
- Date
- Play Time
- Screenshot (Future)

---

# Confirmation Dialogs

Examples:

- Delete World
- Exit Game
- Overwrite Save
- Reset Settings

Destructive actions always require confirmation.

---

# Navigation

Supported input methods:

- Mouse
- Touch
- Keyboard
- Controller

Navigation order must remain deterministic.

---

# Focus Management

Only one interactive element may own focus at a time.

Focus changes must be predictable.

---

# Modal Windows

Modal dialogs block interaction with underlying menus.

Nested modals should be avoided.

---

# Performance

Menus should:

- load lazily
- reuse UI components
- avoid unnecessary allocations

---

# Determinism

Menu navigation never affects simulation determinism.

Menus are presentation only.

---

# Serialization

Menu state is not serialized.

User preferences are stored in the Player Profile.

---

# Design Constraints

The Menu System must remain:

- modular
- accessible
- responsive
- simulation independent

---

# Related Documents

UI-001 — Visual Language

UI-002 — HUD

UI-004 — Notifications

GAME-001 — Player

---

# Acceptance Criteria

- [ ] Supports all primary menus.
- [ ] Supports all input methods.
- [ ] Uses deterministic navigation.
- [ ] Destructive actions require confirmation.
- [ ] Independent from simulation logic.

---

# Revision History

## 1.0.0

Initial version.
