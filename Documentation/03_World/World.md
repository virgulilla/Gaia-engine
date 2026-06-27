# WRLD-001 — World

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the simulated world that contains every entity, organism and environmental system.

The World is the root container of the entire simulation.

---

# Scope

This specification defines:

- world boundaries
- world properties
- world seed
- world dimensions
- world metadata
- chunk ownership

It does not define:

- terrain generation
- climate
- organisms
- rendering

---

# Philosophy

The World is passive.

It stores environmental state.

Simulation Systems modify the World.

The World never updates itself.

---

# Responsibilities

The World is responsible for:

- owning Chunks
- storing global metadata
- exposing spatial queries
- providing deterministic initialization

The World never:

- executes simulation logic
- performs rendering
- updates organisms
- calculates AI

---

# World Structure

```text
World

├── Metadata
├── Chunks
├── Global Climate
├── Global Resources
├── Simulation Settings
└── Statistics
```

---

# Metadata

Stores immutable information.

Fields:

- WorldId
- WorldName
- Seed
- CreationDate
- EngineVersion
- ConfigurationVersion

---

# World Dimensions

Fields:

- Width
- Height
- Chunk Size
- Chunk Count
- Maximum Elevation

Dimensions are immutable after creation.

---

# Simulation Settings

Stores world-specific simulation configuration.

Examples:

- Sea Level
- Initial Temperature
- Initial Humidity
- World Age
- Starting Season

---

# Global References

The World stores references to:

- Chunk Collection
- Climate System
- Resource System
- Statistics System

The World never owns simulation behaviour.

---

# World Age

Every world stores:

- Current Tick
- Current Day
- Current Season
- Current Year

Simulation Time belongs to the Simulation module.

The World only stores its current state.

---

# Spatial Ownership

Every spatial object belongs to exactly one Chunk.

Every Chunk belongs to exactly one World.

Ownership is never shared.

---

# Deterministic Generation

A World is completely defined by:

- Seed
- Configuration
- Engine Version

Generating the same World twice must produce identical results.

---

# Serialization

The World must support complete serialization.

Saving and loading must preserve:

- Seed
- Current Time
- Chunks
- Organisms
- Resources
- Climate State

---

# Design Constraints

The World must remain:

- deterministic
- platform independent
- serializable
- data-oriented

---

# Related Documents

WRLD-002 — Chunk System

WRLD-003 — Terrain

SIM-002 — Simulation Loop

---

# Acceptance Criteria

- [ ] Owns every Chunk.
- [ ] Stores immutable metadata.
- [ ] Supports deterministic generation.
- [ ] Fully serializable.
- [ ] Contains no simulation logic.

---

# Revision History

## 1.0.0

Initial version.
