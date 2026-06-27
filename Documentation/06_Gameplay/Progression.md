# GAME-005 — Progression

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the long-term progression system available to the player.

Progression rewards knowledge, exploration and observation rather than repetitive actions.

---

# Scope

This specification defines:

- progression levels
- experience
- unlocks
- milestones
- progression persistence

It does not define:

- objectives
- achievements
- tutorials
- simulation rules

---

# Philosophy

Progression represents player knowledge.

It never represents character power.

The player becomes more capable of understanding the simulation, not stronger than it.

---

# Responsibilities

The Progression System is responsible for:

- tracking experience
- unlocking features
- tracking milestones
- exposing progression state

The Progression System never:

- modify simulation systems
- modify organisms
- change world generation
- affect AI

---

# Progression Pipeline

```text
Gameplay Event

↓

Experience Awarded

↓

Progress Updated

↓

Milestone Reached

↓

Feature Unlocked

↓

Player Profile Updated
```

---

# Experience Sources

Experience may be awarded for:

- Discovering Species
- Discovering Traits
- Exploring Biomes
- Completing Objectives
- Observing Rare Events
- Completing Encyclopedia Entries

Experience values are configurable.

---

# Progression Levels

Each level defines:

- Required Experience
- Unlocks
- Milestones

Levels have no maximum limit.

Future expansions may introduce additional progression paths.

---

# Unlock Categories

```text
Progression

├── Analysis Tools
├── Simulation Controls
├── Encyclopedia Features
├── Statistics
├── Visualization
└── Cosmetic Rewards
```

---

# Analysis Tools

Examples:

- Organism Inspector
- Trait Viewer
- Genome Viewer
- Population Analysis
- Evolution Timeline

---

# Simulation Controls

Examples:

- Time Acceleration
- Simulation Speed
- Advanced Filters
- World Statistics

Simulation determinism is never affected.

---

# Encyclopedia Features

Examples:

- Advanced Search
- Evolution Tree
- Trait Comparison
- Species Comparison

---

# Statistics

Examples:

- Population Graphs
- Birth Rate
- Death Rate
- Climate History
- Extinction Timeline

---

# Visualization

Examples:

- Heat Maps
- Migration Paths
- Resource Overlay
- Climate Overlay

---

# Cosmetic Rewards

Examples:

- Icons
- Themes
- Profile Badges
- World Naming Options

Cosmetic rewards never affect gameplay.

---

# Milestones

Examples:

- First Discovery
- First Speciation
- First Extinction
- 100 Species Discovered
- Encyclopedia Completed

Milestones are permanent.

---

# Progress Storage

The Player Profile stores:

- Experience
- Current Level
- Unlocks
- Completed Milestones

Progress is independent from World Saves.

---

# World Independence

Deleting a World never removes player progression.

Progress is shared across all Worlds.

---

# Determinism

Experience rewards are deterministic.

Identical gameplay events always award identical experience.

---

# Serialization

Progression data must be fully serializable.

Progression is stored in the Player Profile.

---

# Design Constraints

The Progression System must remain:

- deterministic
- profile-based
- configurable
- simulation independent

---

# Related Documents

GAME-001 — Player

GAME-002 — Discovery

GAME-003 — Encyclopedia

GAME-004 — Objectives

GAME-006 — Achievements

---

# Acceptance Criteria

- [ ] Progress is based on discoveries.
- [ ] Unlocks never modify simulation rules.
- [ ] Progress persists across worlds.
- [ ] Experience rewards are configurable.
- [ ] Fully serializable.

---

# Revision History

## 1.0.0

Initial version.
