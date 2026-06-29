# ORG-000 — Organism Module

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-29

---

# Purpose

The Organism module defines living entities participating in the simulation.

It provides the biological runtime state consumed by AI, interaction systems and future genetics pipelines.

---

# Scope

This module defines:

- organism identity
- physiology
- needs
- lifecycle
- health
- organism bootstrap state

It does not define:

- AI decisions
- movement
- reproduction logic
- genetics generation
- rendering

---

# Module Architecture

```text
Organism

├── Organism
├── Physiology
├── Needs
├── Lifecycle
├── Health
└── Species References
```

---

# Design Principles

## Living State

Organisms are the smallest living simulation entities.

Every organism owns its own biological state.

---

## Components Are Data

Organism components store deterministic data only.

Systems update those components during the Organism Update phase.

---

## Explicit References

Organisms reference:

- ChunkId
- SpeciesId
- GenomeId

Shared biological information is never duplicated through hidden links.

---

# Dependencies

Incoming:

- Simulation
- World

Outgoing:

- AI
- Gameplay
- Audio
- UI

---

# Folder Structure

```text
Organism/

README.md

Organism.md

Physiology.md

Needs.md

Lifecycle.md
```

---

# Related Documents

ENG-002 — Domain Model

ENG-003 — Component System

SIM-003 — Tick Pipeline

GEN-002 — Genome

GEN-006 — Species Formation

---

# Acceptance Criteria

- [ ] Organisms define deterministic runtime state.
- [ ] Components contain data only.
- [ ] Organisms reference Species and Genome through immutable identifiers.
- [ ] Organism state is fully serializable.
- [ ] Organism systems remain separate from AI and rendering.

---

# Revision History

## 1.0.0

Initial version.
