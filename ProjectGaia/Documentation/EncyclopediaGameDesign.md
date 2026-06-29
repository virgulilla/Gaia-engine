# PG-007 — Encyclopedia Game Design

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the Encyclopedia used by Project Gaia.

The Encyclopedia is the player's scientific journal.

It records every validated discovery made across every world.

It is the primary progression system of the game.

---

# Scope

This specification defines:

- encyclopedia structure
- knowledge organization
- unlock system
- player research
- long-term progression

It does not define:

- UI implementation
- Gaia Engine simulation
- save game format

---

# Philosophy

The Encyclopedia is not a collection log.

It is a living scientific database that grows alongside the player's understanding.

Every discovery should answer one question while creating several new ones.

---

# Goals

The Encyclopedia should:

- reward curiosity
- encourage experimentation
- preserve discoveries
- explain biological systems
- motivate exploration

---

# Structure

```text
Encyclopedia

├── Species
├── Organisms
├── Behaviours
├── Genetics
├── Ecosystems
├── Climate
├── Relationships
├── Discoveries
├── Statistics
└── Research Notes
```

---

# Species Section

Each species contains:

- scientific name
- common name (optional)
- classification
- habitat
- diet
- lifespan
- population history
- observed behaviours
- discovered traits
- evolution history

Unknown information appears as hidden.

---

# Organism Records

Important organisms may receive individual records.

Examples:

- oldest organism
- largest organism
- first discovered organism
- unique mutations

These records provide historical context.

---

# Behaviour Database

Behaviours are documented independently from species.

Examples:

- migration
- hunting
- cooperation
- parental care
- camouflage
- pollination
- scavenging

The player gradually learns which species exhibit each behaviour.

---

# Genetics Section

Stores discovered biological knowledge.

Examples:

- inherited traits
- dominant genes
- recessive genes
- mutations
- evolutionary adaptations

Only confirmed knowledge is displayed.

---

# Ecosystem Section

Documents ecosystem-level discoveries.

Examples:

- food webs
- biodiversity
- extinction events
- invasive species
- ecological succession
- predator-prey balance

---

# Climate Section

Records environmental observations.

Examples:

- seasonal cycles
- rainfall
- drought
- temperature changes
- biome comparisons

---

# Relationship Database

Stores known relationships between organisms.

Examples:

- predator → prey
- parasite → host
- pollinator → plant
- symbiosis
- competition

Relationships become more detailed over time.

---

# Statistics

Automatically generated information.

Examples:

- discovered species
- extinct species
- longest simulation
- oldest ecosystem
- biodiversity index
- largest population

Statistics are read-only.

---

# Research Notes

Players may optionally write personal notes.

Research Notes are stored per player profile.

They never affect gameplay.

---

# Knowledge Levels

Each entry progresses through stages.

```text
Unknown

↓

Observed

↓

Identified

↓

Studied

↓

Fully Documented
```

More observations reveal additional information.

---

# Search & Filters

The Encyclopedia supports:

- search
- filtering
- sorting
- favorites
- recently updated
- undiscovered entries

Players should quickly locate previously discovered knowledge.

---

# Persistence

The Encyclopedia belongs to the Player Profile.

It is shared across every world.

Deleting a world never deletes scientific knowledge.

---

# Relationship With Gaia Engine

Gaia Engine produces observable facts.

Project Gaia transforms those facts into scientific knowledge.

The Encyclopedia never modifies simulation state.

---

# Related Documents

PG-006 — Discovery System

PG-008 — UI Flow

PG-009 — Progression

---

# Acceptance Criteria

- [ ] Knowledge persists across worlds.
- [ ] Supports gradual information reveal.
- [ ] Organized by scientific categories.
- [ ] Searchable and filterable.
- [ ] Independent from Gaia Engine implementation.

---

# Revision History

## 1.0.0

Initial version.
