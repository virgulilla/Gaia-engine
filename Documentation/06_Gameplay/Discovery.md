# GAME-002 — Discovery

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the Discovery System used to reveal information to the player during gameplay.

Discoveries transform simulation events into permanent player knowledge.

---

# Scope

This specification defines:

- discoveries
- discovery conditions
- discovery progression
- discovery persistence
- discovery rewards

It does not define:

- achievements
- tutorials
- encyclopedia content
- UI presentation

---

# Philosophy

Players should learn by observing the simulation.

Nothing should be unlocked arbitrarily.

Every discovery originates from actual gameplay.

---

# Responsibilities

The Discovery System is responsible for:

- detecting discoveries
- unlocking new knowledge
- notifying Gameplay
- updating player progression

The Discovery System never:

- modify the simulation
- generate organisms
- execute AI
- change world state

---

# Discovery Pipeline

```text
Simulation Event

↓

Discovery Rules

↓

Discovery Validation

↓

Player Unlock

↓

Knowledge Updated

↓

Encyclopedia Updated

↓

Player Notification
```

---

# Discovery Categories

```text
Discoveries

├── Species
├── Traits
├── Biomes
├── Resources
├── Behaviours
├── Climate
└── World Events
```

---

# Species Discoveries

Unlocked when the player observes a species for the first time.

Examples:

- New Herbivore
- New Predator
- New Plant

---

# Trait Discoveries

Unlocked after observing specific characteristics.

Examples:

- Night Vision
- Camouflage
- Thick Fur
- Venom
- Large Antlers

---

# Biome Discoveries

Unlocked when visiting or observing a new biome.

Examples:

- Desert
- Swamp
- Alpine
- Rainforest

---

# Resource Discoveries

Unlocked when observing resources.

Examples:

- Fresh Water
- Berry Bush
- Iron Deposit
- Medicinal Plant

---

# Behaviour Discoveries

Unlocked after witnessing specific behaviours.

Examples:

- Hunting
- Migration
- Nest Building
- Courtship
- Cooperative Hunting

---

# Climate Discoveries

Unlocked through environmental observation.

Examples:

- Blizzard
- Drought
- Storm
- Heat Wave

---

# World Event Discoveries

Unlocked after important simulation events.

Examples:

- Extinction
- Speciation
- Wildfire
- Flood
- Disease Outbreak

---

# Discovery Entry

Every discovery stores:

```text
Discovery

├── DiscoveryId
├── Category
├── Name
├── Description
├── Unlock Tick
├── WorldId
└── PlayerId
```

---

# Discovery Conditions

Every discovery defines one or more conditions.

Examples:

- Observe organism
- Observe interaction
- Observe biome
- Complete objective
- Trigger event

Conditions are data-driven.

---

# Persistence

Discoveries belong to the Player Profile.

Deleting a world never removes discoveries.

---

# Duplicate Discoveries

A discovery may only be unlocked once.

Subsequent observations update statistics only.

---

# Notifications

When a discovery is unlocked:

- Player Progression is updated
- Encyclopedia entry becomes available
- Optional UI notification is generated

Notification presentation belongs to the UI module.

---

# Determinism

Discovery rules must be deterministic.

Identical simulation events must always produce identical discovery results.

---

# Serialization

Discovery data is serialized with the Player Profile.

---

# Design Constraints

The Discovery System must remain:

- deterministic
- data-driven
- profile-based
- simulation independent

---

# Related Documents

GAME-001 — Player

GAME-003 — Encyclopedia

GAME-004 — Objectives

GAME-005 — Progression

---

# Acceptance Criteria

- [ ] Discoveries originate from simulation events.
- [ ] Discoveries persist across worlds.
- [ ] Duplicate discoveries are ignored.
- [ ] Discovery rules are configurable.
- [ ] Fully serializable.

---

# Revision History

## 1.0.0

Initial version.
