# PG-006 — Discovery System

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the Discovery System used by Project Gaia.

The Discovery System transforms observation into permanent player knowledge.

Discoveries are the primary source of progression.

---

# Scope

This specification defines:

- discoveries
- discovery categories
- discovery conditions
- discovery persistence
- player rewards

It does not define:

- Gaia Engine simulation
- achievements
- UI implementation

---

# Philosophy

Knowledge is the primary reward.

Players should feel like scientists documenting a living world.

Nothing is unlocked by grinding.

Everything is unlocked through understanding.

---

# Discovery Lifecycle

```text
Unknown

↓

Observed

↓

Validated

↓

Recorded

↓

Encyclopedia Updated

↓

New Knowledge Available
```

---

# Discovery Categories

```text
Discoveries

├── Species
├── Organisms
├── Behaviours
├── Genetics
├── Ecosystems
├── Climate
├── Relationships
├── Evolution
└── Rare Events
```

---

# Species Discoveries

A species is discovered when the player successfully observes it for the first time.

Recording a species unlocks:

- species entry
- distribution map
- population statistics
- future observations

---

# Behaviour Discoveries

Examples:

- hunting
- migration
- reproduction
- cooperation
- camouflage
- symbiosis

Behaviours become permanent scientific knowledge.

---

# Genetics Discoveries

The player gradually unlocks:

- dominant traits
- recessive traits
- mutations
- heredity
- adaptation

Not every genome is immediately visible.

Knowledge grows through observation.

---

# Ecosystem Discoveries

Examples:

- food chains
- predator-prey balance
- ecological collapse
- invasive species
- biodiversity hotspots

These discoveries describe systems rather than individuals.

---

# Rare Events

Examples:

- extinction
- speciation
- albino organism
- giant organism
- ecosystem recovery
- climate anomaly

Rare discoveries produce significant progression.

---

# Discovery Validation

A discovery becomes permanent only after meeting its validation criteria.

Examples:

- multiple observations
- observation duration
- sufficient scientific evidence

Validation prevents accidental discoveries.

---

# Persistence

Discoveries belong to the Player Profile.

They remain available across every future world.

Knowledge is permanent.

Simulation worlds are temporary.

---

# Rewards

Discoveries unlock:

- Encyclopedia entries
- Analysis tools
- New statistics
- Scientific reports
- Visualization modes

Discoveries never increase organism strength.

---

# Duplicate Discoveries

Repeated discoveries improve scientific confidence.

Duplicates may provide:

- additional statistics
- expanded descriptions
- improved biological understanding

They never replace the original discovery.

---

# Relationship With Gaia Engine

Gaia Engine produces simulation events.

Project Gaia evaluates those events.

The Discovery System determines whether new knowledge has been obtained.

---

# Related Documents

PG-002 — Core Loop

PG-005 — Player Powers

PG-007 — Encyclopedia Game Design

PG-009 — Progression

---

# Acceptance Criteria

- [ ] Discoveries are permanent.
- [ ] Knowledge persists across worlds.
- [ ] Discovery validation is deterministic.
- [ ] Rewards improve understanding.
- [ ] Fully integrated with Gaia Engine.

---

# Revision History

## 1.0.0

Initial version.
