# PG-008 — UI Flow

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the navigation flow and user interface structure of Project Gaia.

The UI should help players observe, understand and interact with the simulation without distracting from it.

The interface exists to expose knowledge, not to dominate the screen.

---

# Scope

This specification defines:

- screen hierarchy
- navigation flow
- HUD philosophy
- interaction patterns
- contextual interfaces

It does not define:

- visual style
- artwork
- implementation details

---

# Philosophy

The world is the primary interface.

UI elements should appear only when they provide meaningful information.

The player should spend more time observing the simulation than interacting with menus.

---

# Navigation Structure

```text id="93xtvn"
Main Menu

↓

World Selection

↓

Simulation

├── HUD
├── Encyclopedia
├── Organism Inspector
├── World Inspector
├── Discovery Panel
├── Settings
└── Pause Menu
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

The Main Menu contains no gameplay information.

---

# World Selection

The player may:

- create a new world
- select an existing world
- delete worlds
- duplicate worlds

Each world displays:

- world seed
- creation date
- total play time
- biodiversity score
- completion percentage

---

# Simulation HUD

The HUD remains minimal.

Persistent elements include:

- simulation speed
- current date
- pause indicator
- active notifications

Optional overlays are hidden until enabled.

---

# Organism Inspector

Selecting an organism opens a contextual panel.

Information may include:

- species
- age
- health
- energy
- current behaviour
- discovered traits
- genealogy (if unlocked)

Unknown data appears as hidden.

---

# World Inspector

Displays world-level information.

Examples:

- biome
- temperature
- humidity
- available resources
- biodiversity
- population density

---

# Discovery Panel

Appears only when a new discovery occurs.

Displays:

- discovery title
- category
- short description
- Encyclopedia shortcut

The panel dismisses automatically after a short time.

---

# Encyclopedia

Accessible at any time.

Supports:

- search
- filtering
- favorites
- recently discovered
- completion statistics

Opening the Encyclopedia pauses the simulation by default.

---

# Overlays

Players may enable optional overlays.

Examples:

- temperature map
- rainfall map
- migration paths
- food chains
- biodiversity
- genetics
- territories

Only one major overlay should be active at a time.

---

# Pause Menu

Provides:

- Resume
- Save
- Load
- Encyclopedia
- Settings
- Return to Main Menu

Simulation stops while paused.

---

# Notifications

Notifications should be informative rather than intrusive.

Examples:

- new species discovered
- extinction
- mutation detected
- climate event
- ecosystem milestone

Notifications never block gameplay.

---

# Mobile Design

The interface is designed for touch devices.

Requirements:

- large touch targets
- minimal text
- contextual panels
- gesture navigation
- one-handed usability where practical

---

# Accessibility

The UI should support:

- scalable text
- colorblind-friendly palettes
- high contrast mode
- configurable notification levels

---

# Relationship With Gaia Engine

The UI never modifies simulation directly.

It displays data exposed by Gaia Engine and sends player requests through Project Gaia systems.

---

# Related Documents

PG-002 — Core Loop

PG-005 — Player Powers

PG-006 — Discovery System

PG-007 — Encyclopedia Game Design

PG-009 — Progression

---

# Acceptance Criteria

- [ ] Navigation is simple and intuitive.
- [ ] HUD remains minimal.
- [ ] Encyclopedia is always accessible.
- [ ] Designed for mobile-first interaction.
- [ ] UI never bypasses Gaia Engine.

---

# Revision History

## 1.0.0

Initial version.
