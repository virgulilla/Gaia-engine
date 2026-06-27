# WRLD-004 — Biome

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines ecological regions sharing similar environmental characteristics.

Biomes classify areas of the World according to climate, terrain and resource distribution.

---

# Scope

This specification defines:

- biome classification
- biome properties
- biome transitions
- environmental influence
- biome identity

It does not define:

- climate simulation
- terrain generation
- organism behaviour
- rendering

---

# Philosophy

A Biome is a classification.

It is not a simulation system.

Biomes describe environmental conditions.

Simulation Systems use biome information.

---

# Responsibilities

Biomes are responsible for:

- environmental classification
- ecological modifiers
- species affinity
- resource profiles

Biomes never:

- generate weather
- update organisms
- grow vegetation
- execute simulation logic

---

# Biome Structure

```text
Biome

├── Identity
├── Climate Profile
├── Terrain Profile
├── Resource Profile
├── Vegetation Profile
└── Species Affinity
```

---

# Identity

Stores immutable information.

Fields:

- BiomeId
- Name
- Category
- Description

---

# Climate Profile

Typical environmental values.

Examples:

- Average Temperature
- Average Rainfall
- Humidity
- Wind Intensity
- Seasonal Variation

---

# Terrain Profile

Typical geological properties.

Examples:

- Elevation Range
- Dominant Soil
- Surface Type
- Drainage

---

# Resource Profile

Typical resource availability.

Examples:

- Water
- Food
- Minerals
- Biomass

Each value represents average abundance.

---

# Vegetation Profile

Defines dominant vegetation types.

Examples:

- Forest
- Grassland
- Shrubs
- Moss
- None

Vegetation growth is handled by dedicated systems.

---

# Species Affinity

Defines environmental suitability.

Examples:

- Herbivore Affinity
- Carnivore Affinity
- Plant Diversity
- Aquatic Suitability

Affinity values influence ecosystem formation.

---

# Biome Examples

Supported biomes include:

- Ocean
- Coast
- Beach
- Grassland
- Forest
- Rainforest
- Swamp
- Desert
- Tundra
- Mountain
- Alpine
- Volcanic

Additional biomes may be introduced through configuration.

---

# Biome Transitions

Biome borders are gradual.

Neighbouring biomes blend through configurable transition zones.

Hard borders should be avoided whenever possible.

---

# Environmental Influence

Biomes influence:

- resource regeneration
- vegetation growth
- species distribution
- climate modifiers
- ecosystem diversity

Simulation Systems interpret these values.

---

# Determinism

Biome generation must be deterministic.

The same world seed always produces identical biome distribution.

---

# Serialization

Biome assignments must be serializable.

Procedurally generated worlds may regenerate biome assignments from the original seed.

---

# Design Constraints

Biomes must remain:

- deterministic
- data-driven
- configurable
- renderer independent

---

# Related Documents

WRLD-001 — World

WRLD-003 — Terrain

WRLD-005 — Climate

WRLD-007 — Resources

---

# Acceptance Criteria

- [ ] Biomes classify environmental regions.
- [ ] Biomes contain no simulation logic.
- [ ] Supports configurable biome definitions.
- [ ] Supports smooth biome transitions.
- [ ] Fully deterministic.

---

# Revision History

## 1.0.0

Initial version.
