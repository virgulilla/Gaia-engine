# UI-002 — Heads-Up Display (HUD)

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the in-game Heads-Up Display (HUD).

The HUD presents essential gameplay information while minimizing visual distraction from the simulation.

---

# Scope

This specification defines:

- HUD layout
- HUD widgets
- contextual information
- visibility rules
- HUD customization

It does not define:

- menus
- encyclopedia
- rendering
- gameplay logic

---

# Philosophy

The simulation is the main focus.

The HUD should provide information only when necessary.

Information density should remain low.

---

# Responsibilities

The HUD is responsible for:

- displaying current simulation status
- presenting player tools
- showing contextual information
- displaying selected object information

The HUD never:

- stores gameplay state
- modifies simulation
- performs calculations
- executes gameplay logic

---

# HUD Layout

```text
HUD

├── Top Bar
├── Left Panel
├── Right Panel
├── Bottom Toolbar
├── Context Panel
└── Notification Area
```

---

# Top Bar

Displays global information.

Examples:

- Simulation Speed
- Current Season
- Current Day
- Current Year
- Active World

---

# Left Panel

Displays contextual simulation information.

Examples:

- Selected Organism
- Selected Resource
- Selected Biome
- Selected Chunk

Hidden when nothing is selected.

---

# Right Panel

Displays world statistics.

Examples:

- Population
- Temperature
- Active Species
- Resources
- Performance

Panels are collapsible.

---

# Bottom Toolbar

Contains player tools.

Examples:

- Inspect
- Time Controls
- Filters
- Encyclopedia
- Statistics
- Settings

---

# Context Panel

Displays information related to the current selection.

Examples:

Organism

- Species
- Age
- Health
- Hunger

Biome

- Climate
- Resources
- Dominant Species

Resource

- Quantity
- Regeneration
- Quality

---

# Notification Area

Displays temporary notifications.

Examples:

- Discovery
- Achievement
- Objective Complete
- Warning

Notifications disappear automatically.

---

# HUD Visibility

HUD elements may be:

- Always Visible
- Contextual
- Hidden
- User Controlled

---

# Adaptive Interface

The HUD adapts to:

- Screen Size
- Orientation
- Input Device
- Accessibility Settings

---

# Customization

Players may configure:

- Scale
- Opacity
- Visible Panels
- Notification Duration
- HUD Position (where applicable)

Customization is stored in Player Settings.

---

# Interaction

HUD supports:

- Touch
- Mouse
- Keyboard
- Controller

Interaction behaviour must remain consistent.

---

# Performance

HUD updates only when observed data changes.

Continuous polling should be avoided.

---

# Determinism

HUD presentation never affects simulation.

The HUD is a read-only visualization layer.

---

# Serialization

HUD state is not stored in World Saves.

Customization belongs to the Player Profile.

---

# Design Constraints

The HUD must remain:

- responsive
- lightweight
- customizable
- accessible
- simulation independent

---

# Related Documents

UI-001 — Visual Language

UI-003 — Menus

UI-004 — Notifications

GAME-001 — Player

---

# Acceptance Criteria

- [ ] Displays only relevant information.
- [ ] Supports responsive layouts.
- [ ] Supports player customization.
- [ ] Never modifies simulation.
- [ ] Independent from gameplay logic.

---

# Revision History

## 1.0.0

Initial version.
