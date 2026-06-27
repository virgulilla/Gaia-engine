# WRLD-008 — Spatial Queries

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the spatial query system used by Gaia Engine.

Spatial Queries provide deterministic access to world objects without exposing internal storage structures.

---

# Scope

This specification defines:

- spatial searches
- area queries
- nearest-neighbour searches
- range queries
- chunk lookup

It does not define:

- movement
- pathfinding
- rendering
- AI decision making

---

# Philosophy

Simulation Systems never iterate over the entire World.

Every spatial lookup must use Spatial Queries.

---

# Responsibilities

The Spatial Query System is responsible for:

- locating organisms
- locating resources
- locating chunks
- finding nearby objects
- performing efficient area searches

The Spatial Query System never:

- modifies simulation state
- executes behaviour
- updates world data

---

# Query Categories

```text
Spatial Queries

├── Position
├── Radius
├── Area
├── Chunk
├── Line
└── Nearest
```

---

# Position Query

Returns the object located at a specific world position.

Example:

- Terrain Cell
- Chunk
- Water Cell

---

# Radius Query

Returns every object inside a circular radius.

Parameters:

- Origin
- Radius
- Filters

Typical use cases:

- smell
- hearing
- resource search

---

# Area Query

Returns every object inside a rectangular region.

Typical use cases:

- chunk updates
- ecosystem analysis
- save operations

---

# Chunk Query

Returns:

- current chunk
- neighbouring chunks
- chunk coordinates

Used by almost every simulation system.

---

# Line Query

Evaluates a straight path through the world.

Typical use cases:

- vision
- projectile simulation
- line of sight

---

# Nearest Query

Returns the closest valid object.

Supported targets:

- Organism
- Resource
- Water
- Shelter
- Food Source

---

# Query Filters

Every query supports filters.

Examples:

- Species
- Resource Type
- Organism State
- Biome
- Water Availability
- Elevation

---

# Result Ordering

Query results are deterministic.

Objects are always returned in a stable order.

---

# Performance

Queries should avoid:

- full world scans
- unnecessary allocations
- duplicate results

Spatial indexing is implementation specific.

---

# Determinism

Queries must always produce identical results given identical simulation state.

---

# Serialization

Spatial Queries are runtime services.

No persistent state is serialized.

---

# Design Constraints

Spatial Queries must remain:

- deterministic
- allocation-friendly
- thread-safe
- independent from rendering
- independent from AI

---

# Related Documents

WRLD-001 — World

WRLD-002 — Chunk System

ORG-001 — Organism

SIM-002 — Simulation Loop

---

# Acceptance Criteria

- [ ] Supports deterministic searches.
- [ ] Supports filtering.
- [ ] Supports nearest-neighbour queries.
- [ ] Contains no simulation logic.
- [ ] Contains no persistent state.

---

# Revision History

## 1.0.0

Initial version.
