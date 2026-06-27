# AI-003 — Memory

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines how organisms store and use information acquired through experience.

Memory extends Perception by preserving relevant observations across Simulation Ticks.

Memory never makes decisions.

---

# Scope

This specification defines:

- memory storage
- memory decay
- memory confidence
- memory retrieval
- memory expiration

It does not define:

- decision making
- learning
- emotions
- rendering

---

# Philosophy

Perception represents the present.

Memory represents the past.

Utility AI evaluates both.

---

# Responsibilities

Memory is responsible for:

- storing observations
- tracking confidence
- forgetting obsolete information
- exposing remembered knowledge

Memory never:

- select actions
- modify the world
- modify organisms
- execute behaviour

---

# Memory Pipeline

```text
Perception

↓

Observation

↓

Memory Storage

↓

Memory Decay

↓

Memory Retrieval

↓

Utility AI
```

---

# Memory Categories

```text
Memory

├── Organisms
├── Resources
├── Locations
├── Hazards
└── Events
```

---

# Organism Memory

Stores known organisms.

Examples:

- Predator
- Mate
- Parent
- Offspring
- Rival

Each memory stores:

- OrganismId
- Last Known Position
- Last Seen Tick
- Confidence

---

# Resource Memory

Stores known resources.

Examples:

- Water Source
- Fruit Tree
- Nest
- Shelter

Each memory stores:

- ResourceId
- Position
- Estimated Availability
- Last Visited Tick

---

# Location Memory

Stores relevant locations.

Examples:

- Home Area
- Nest
- Safe Zone
- Migration Route

---

# Hazard Memory

Stores dangerous locations.

Examples:

- Predator Territory
- Fire
- Flood Area
- Toxic Region

---

# Event Memory

Stores significant events.

Examples:

- Recent Attack
- Birth
- Death
- Storm
- Food Discovery

---

# Memory Entry

Every memory contains:

```text
Memory Entry

├── Identifier
├── Category
├── Position
├── Confidence
├── Creation Tick
├── Last Update Tick
└── Expiration Tick
```

---

# Confidence

Confidence decreases over time.

Range:

0.0 → 1.0

Older memories become less reliable.

---

# Forgetting

Memory automatically expires.

Expiration depends on:

- memory type
- organism intelligence
- configuration

Expired memories are removed.

---

# Capacity

Memory capacity is configurable.

Organisms may remember different amounts of information.

Capacity may depend on Genome.

---

# Updating

If a remembered object is perceived again:

- confidence increases
- position updates
- expiration resets

---

# Determinism

Memory updates must be deterministic.

Identical observations always produce identical memories.

---

# Serialization

Memory state must be fully serializable.

Loading restores every remembered observation.

---

# Design Constraints

Memory must remain:

- deterministic
- data-driven
- configurable
- renderer independent

---

# Related Documents

AI-001 — Utility AI

AI-002 — Perception

AI-004 — Decision Making

ORG-009 — Relationships

---

# Acceptance Criteria

- [ ] Stores observations only.
- [ ] Supports confidence decay.
- [ ] Supports configurable capacity.
- [ ] Fully serializable.
- [ ] Independent from decision making.

---

# Revision History

## 1.0.0

Initial version.
