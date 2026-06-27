# WRLD-002 — Chunk System

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines how the World is spatially divided into deterministic simulation regions.

Chunks are the fundamental spatial unit used by Gaia Engine.

---

# Scope

This specification defines:

- chunk structure
- chunk ownership
- chunk lifecycle
- chunk loading
- chunk activation
- spatial partitioning

It does not define:

- terrain generation
- pathfinding
- rendering
- AI

---

# Philosophy

The World is never processed as a single object.

Simulation always operates on Chunks.

Chunks improve scalability, determinism and future parallelization.

---

# Responsibilities

The Chunk System is responsible for:

- dividing the world
- storing local references
- exposing spatial queries
- supporting chunk activation

The Chunk System never:

- updates organisms
- generates terrain
- executes AI
- performs rendering

---

# Chunk Structure

```text
Chunk

├── Metadata
├── Terrain
├── Climate
├── Water
├── Resources
├── Organisms
└── Neighbours
```

---

# Metadata

Stores immutable information.

Fields:

- ChunkId
- WorldId
- Coordinates
- Seed
- Size

---

# Terrain Reference

Each Chunk owns exactly one Terrain section.

Terrain data is stored locally.

---

# Climate Reference

Each Chunk stores local climate values.

Examples:

- Temperature
- Humidity
- Rainfall
- Wind

---

# Water Reference

Stores local water information.

Examples:

- Rivers
- Lakes
- Ground Water
- Water Table

---

# Resource Reference

Stores renewable and non-renewable resources.

Examples:

- Vegetation
- Minerals
- Food Sources

---

# Organism Reference

Chunks contain references to every organism currently inside their boundaries.

References use OrganismId only.

---

# Neighbours

Every Chunk stores references to adjacent Chunks.

Neighbour references accelerate:

- movement
- climate propagation
- water flow
- resource spreading

---

# Chunk States

Each Chunk has one runtime state.

Possible values:

- Active
- Dormant
- Sleeping
- Unloaded (future)

---

# Active

Simulation executes normally.

All systems update.

---

# Dormant

Simulation executes at reduced frequency.

Used for distant regions.

---

# Sleeping

Simulation executes only when required.

Long-term environmental changes may still occur.

---

# Chunk Coordinates

Chunks use integer coordinates.

Example:

X = 42

Y = -17

Coordinates never change.

---

# Chunk Size

Chunk dimensions are configurable.

All Chunks share the same size within a World.

---

# Spatial Queries

The Chunk System provides:

- Find Chunk
- Adjacent Chunks
- Radius Search
- Bounding Area
- Region Enumeration

---

# Determinism

Chunk generation must be deterministic.

Chunk order must never influence simulation results.

---

# Serialization

Each Chunk is serialized independently.

This enables:

- partial loading
- debugging
- future streaming

---

# Design Constraints

Chunks must remain:

- deterministic
- independent
- serializable
- cache-friendly

---

# Related Documents

WRLD-001 — World

WRLD-003 — Terrain

WRLD-004 — Biome

SIM-002 — Simulation Loop

---

# Acceptance Criteria

- [ ] Every position belongs to exactly one Chunk.
- [ ] Chunk coordinates are immutable.
- [ ] Chunk ownership is unique.
- [ ] Supports deterministic loading.
- [ ] Supports independent serialization.

---

# Revision History

## 1.0.0

Initial version.
