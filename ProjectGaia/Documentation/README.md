# PG-000 — Project Gaia Documentation

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the documentation module for Project Gaia.

Project Gaia is the first game built on top of Gaia Engine.

This documentation describes the game layer, not the engine.

---

# Scope

This module defines:

- game vision
- core gameplay loop
- player role
- MVP
- player powers
- discovery system
- encyclopedia design
- UI flow
- progression
- vertical slice plan

It does not define:

- Gaia Engine architecture
- low-level simulation
- engine systems
- reusable engine modules

---

# Relationship With Gaia Engine

Gaia Engine provides:

- simulation
- world state
- organisms
- genetics
- AI
- events
- persistence

Project Gaia provides:

- player experience
- game objectives
- UI flow
- progression
- discoveries
- presentation rules

---

# Folder Structure

```text
ProjectGaia/
└── Documentation/
    ├── README.md
    ├── GameVision.md
    ├── CoreLoop.md
    ├── PlayerRole.md
    ├── MVP.md
    ├── PlayerPowers.md
    ├── DiscoverySystem.md
    ├── EncyclopediaGameDesign.md
    ├── UIFlow.md
    ├── Progression.md
    └── VerticalSlicePlan.md
```

---

# Design Rule

Project Gaia must never duplicate Gaia Engine logic.

If a feature belongs to reusable simulation, it belongs in Gaia Engine.

If a feature belongs to player experience, it belongs in Project Gaia.

---

# Related Documents

FOUND-001 — Vision

SIM-001 — Simulation Philosophy

GAME-000 — Gameplay Module

ENG-002 — Domain Model

---

# Acceptance Criteria

- [ ] Defines the Project Gaia documentation scope.
- [ ] Separates game layer from engine layer.
- [ ] References Gaia Engine as dependency.
- [ ] Provides a clear reading order.

---

# Revision History

## 1.0.0

Initial version.
