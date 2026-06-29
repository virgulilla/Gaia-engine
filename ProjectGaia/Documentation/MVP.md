# PG-004 — Minimum Viable Product (MVP)

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the Minimum Viable Product (MVP) for Project Gaia.

The MVP represents the smallest complete version of the game capable of demonstrating the core player experience.

---

# Scope

This specification defines:

- MVP objectives
- mandatory features
- excluded features
- success criteria

It does not define:

- final content
- balancing
- engine implementation

---

# Philosophy

The MVP should prove that Project Gaia is fun before expanding its scope.

Every feature included in the MVP must reinforce the core gameplay loop.

---

# MVP Goal

Allow the player to:

- create a world
- observe life
- inspect organisms
- make discoveries
- record discoveries
- save and reload the world

If these actions are enjoyable, the project has a solid foundation.

---

# Included Features

## World Generation

The player can generate a new deterministic world.

The world includes:

- terrain
- climate
- vegetation
- organisms

---

## Simulation

The simulation runs continuously.

The player may:

- pause
- resume
- change simulation speed

---

## Camera

The player can:

- move
- zoom
- rotate (optional)

Navigation should be simple and intuitive.

---

## Organism Inspection

Selecting an organism displays:

- species
- age
- health
- needs
- current activity

Only discovered information is fully visible.

---

## Discovery System

Players can discover:

- species
- behaviours
- interactions
- environmental events

Discoveries are permanently recorded.

---

## Encyclopedia

The Encyclopedia stores:

- discovered species
- discovered behaviours
- player observations
- scientific statistics

---

## Save / Load

Players can:

- save the world
- reload the world
- continue observing

---

# Excluded Features

The MVP intentionally excludes:

- multiplayer
- missions
- achievements
- weather disasters
- advanced genetics editing
- terraforming
- mod support

These features belong to future milestones.

---

# Vertical Slice

The MVP should demonstrate the complete gameplay loop.

```text
Create World

↓

Observe

↓

Inspect

↓

Discover

↓

Record

↓

Save

↓

Reload

↓

Continue Exploring
```

---

# Success Criteria

The MVP is successful when a new player can:

- understand the world
- become curious
- discover something meaningful
- want to continue exploring

---

# Relationship With Gaia Engine

The MVP uses Gaia Engine exactly as provided.

Game-specific functionality belongs exclusively to Project Gaia.

---

# Related Documents

PG-001 — Game Vision

PG-002 — Core Loop

PG-005 — Player Powers

PG-006 — Discovery System

PG-010 — Vertical Slice Plan

---

# Acceptance Criteria

- [ ] World generation works.
- [ ] Simulation runs correctly.
- [ ] Organisms can be inspected.
- [ ] Discoveries are recorded.
- [ ] Encyclopedia functions.
- [ ] Save/Load works.
- [ ] Demonstrates the complete gameplay loop.

---

# Revision History

## 1.0.0

Initial version.
