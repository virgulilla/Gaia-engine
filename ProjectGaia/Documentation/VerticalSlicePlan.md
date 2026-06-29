# PG-010 — Vertical Slice Plan

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the implementation plan for the first playable version of Project Gaia.

The Vertical Slice demonstrates the complete gameplay experience using a minimal but fully functional feature set.

It validates both Gaia Engine and Project Gaia.

---

# Scope

This specification defines:

- development milestones
- implementation order
- MVP validation
- success criteria

It does not define:

- final game content
- production roadmap
- engine architecture

---

# Philosophy

The Vertical Slice is not a prototype.

It is a complete miniature version of the final game.

Every implemented system should be production quality.

Future development expands existing systems rather than replacing them.

---

# Primary Goal

A new player should be able to:

- create a world
- observe the ecosystem
- inspect organisms
- discover new knowledge
- record discoveries
- save the world
- continue exploring

This entire experience should require no placeholder mechanics.

---

# Gameplay Flow

```text
Create World

↓

Simulation Starts

↓

Explore World

↓

Observe Organisms

↓

Inspect Organism

↓

Make Discovery

↓

Encyclopedia Updated

↓

Continue Exploring

↓

Save World
```

---

# Milestone 1 — Engine Integration

Requirements:

- Gaia Engine integrated
- deterministic simulation
- world generation
- organism spawning
- save/load support

Deliverable:

A running simulation without gameplay.

---

# Milestone 2 — Camera & Navigation

Requirements:

- touch controls
- camera movement
- zoom
- world navigation

Deliverable:

Player can freely explore the world.

---

# Milestone 3 — Organism Interaction

Requirements:

- organism selection
- inspection panel
- live statistics
- current behaviour

Deliverable:

Player can study living organisms.

---

# Milestone 4 — Discovery System

Requirements:

- species discovery
- behaviour discovery
- rare event detection
- discovery notifications

Deliverable:

Scientific discoveries become part of gameplay.

---

# Milestone 5 — Encyclopedia

Requirements:

- persistent discoveries
- search
- categories
- statistics
- completion tracking

Deliverable:

Knowledge persists between worlds.

---

# Milestone 6 — Progression

Requirements:

- unlock new analysis tools
- unlock overlays
- unlock scientific reports

Deliverable:

Player progression driven entirely by knowledge.

---

# Milestone 7 — Polish

Requirements:

- UI improvements
- animations
- transitions
- sound effects
- ambient music
- optimization

Deliverable:

A polished vertical slice representative of the final game.

---

# Minimum Content

The Vertical Slice should include at least:

- 1 world
- 3 biomes
- 10 species
- 30 organisms
- day/night cycle
- seasonal progression
- weather variation

Content quantity is less important than systemic depth.

---

# Technical Goals

The Vertical Slice must demonstrate:

- deterministic simulation
- stable performance
- save/load reliability
- modular architecture
- mobile-first performance

---

# Success Criteria

The Vertical Slice is considered successful if a new player can:

- understand the basic mechanics without a tutorial
- become curious about the ecosystem
- make meaningful discoveries
- revisit previous knowledge
- continue playing voluntarily

---

# Future Expansion

After the Vertical Slice, future milestones expand:

- world size
- biodiversity
- climates
- organisms
- discoveries
- progression systems

The core gameplay loop should remain unchanged.

---

# Relationship With Gaia Engine

Gaia Engine provides every simulation system.

Project Gaia provides presentation, interaction and progression.

The Vertical Slice validates the integration of both projects.

---

# Related Documents

PG-001 — Game Vision

PG-002 — Core Loop

PG-004 — MVP

PG-006 — Discovery System

PG-007 — Encyclopedia Game Design

PG-009 — Progression

---

# Acceptance Criteria

- [ ] Demonstrates the complete gameplay loop.
- [ ] Fully playable from start to save/load.
- [ ] Knowledge-driven progression is functional.
- [ ] Integrates Gaia Engine without modifying its architecture.
- [ ] Represents the foundation of the final game.

---

# Revision History

## 1.0.0

Initial version.
