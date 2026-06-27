# WRLD-003 — Terrain

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the physical surface of the World.

Terrain represents the permanent geological characteristics of each Chunk.

Terrain is immutable during normal simulation unless modified by dedicated geological systems.

---

# Scope

This specification defines:

- elevation
- slope
- soil
- geological composition
- terrain modifiers

It does not define:

- climate
- vegetation
- organisms
- resources
- rendering

---

# Philosophy

Terrain is the foundation of the simulation.

Everything else depends on it.

Terrain changes very slowly compared to biological systems.

---

# Responsibilities

Terrain is responsible for:

- elevation
- geological properties
- movement cost
- drainage potential
- fertility modifiers

Terrain never:

- grows vegetation
- generates weather
- stores organisms
- executes simulation logic

---

# Terrain Structure

```text
Terrain

├── Elevation
├── Slope
├── Soil
├── Surface
├── Geology
└── Modifiers
```

---

# Elevation

Represents terrain height.

Fields:

- Height
- Relative Height
- Sea Level Offset

Elevation is immutable after world generation.

---

# Slope

Represents terrain inclination.

Fields:

- Gradient
- Aspect
- Traversal Cost

Slope is calculated from neighbouring cells.

---

# Soil

Defines ground properties.

Fields:

- Soil Type
- Fertility
- Drainage
- Moisture Capacity
- Organic Matter

---

# Surface

Represents the visible ground layer.

Examples:

- Rock
- Sand
- Grass
- Mud
- Snow
- Ice

Surface may change over time.

Underlying geology does not.

---

# Geology

Represents permanent underground composition.

Examples:

- Granite
- Limestone
- Clay
- Volcanic Rock

Geology affects:

- mineral generation
- water movement
- fertility

---

# Terrain Modifiers

Temporary or long-term changes.

Examples:

- Landslide
- Flood Damage
- Erosion
- Lava Flow
- Crater

Modifiers never alter the original world seed.

---

# Movement Cost

Terrain provides movement multipliers.

Examples:

Road

0.80

Grass

1.00

Mud

1.60

Rock

1.35

Water

Not Traversable

Movement Systems interpret these values.

---

# Fertility Modifier

Terrain contributes to biological productivity.

Higher fertility increases:

- plant growth
- food availability
- biodiversity

Terrain does not grow plants directly.

---

# Water Interaction

Terrain influences:

- river formation
- drainage
- flooding
- groundwater

Water Systems perform the calculations.

Terrain provides the physical constraints.

---

# Climate Interaction

Terrain influences:

- local temperature
- humidity
- rainfall
- wind

Climate Systems consume terrain information.

---

# Determinism

Terrain generation must be deterministic.

Given the same:

- Seed
- Configuration
- Engine Version

the generated terrain must always be identical.

---

# Serialization

Terrain must support complete serialization.

Generated terrain may optionally be regenerated from the original seed instead of stored.

---

# Design Constraints

Terrain must remain:

- deterministic
- immutable by default
- cache-friendly
- data-oriented
- renderer independent

---

# Related Documents

WRLD-001 — World

WRLD-002 — Chunk System

WRLD-004 — Biome

WRLD-005 — Climate

WRLD-006 — Water

---

# Acceptance Criteria

- [ ] Terrain stores geological information only.
- [ ] Supports deterministic generation.
- [ ] Provides movement modifiers.
- [ ] Supports fertility calculations.
- [ ] Independent from organisms and rendering.

---

# Revision History

## 1.0.0

Initial version.
