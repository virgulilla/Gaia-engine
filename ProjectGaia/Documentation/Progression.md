# PG-009 — Progression

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the progression system of Project Gaia.

Progression is based on scientific understanding rather than traditional experience points, levels or resource grinding.

Knowledge is the primary measure of player advancement.

---

# Scope

This specification defines:

- player progression
- knowledge progression
- unlocks
- research milestones
- long-term objectives

It does not define:

- Gaia Engine simulation
- UI implementation
- save system

---

# Philosophy

Progression should feel like becoming a better scientist.

The player gains understanding, not power.

Every new discovery should expand the player's ability to interpret the living world.

---

# Core Principle

The progression loop is:

```text id="e7j1vf"
Observe

↓

Discover

↓

Understand

↓

Unlock New Tools

↓

Perform Better Observations

↓

Discover More
```

Knowledge continuously feeds future discoveries.

---

# Progression Sources

Players progress by:

- discovering species
- documenting behaviours
- understanding ecosystems
- observing evolution
- completing scientific studies
- recording rare events

Waiting alone never grants progression.

---

# Knowledge Levels

Player knowledge advances through stages.

```text id="g3x5qm"
Novice

↓

Observer

↓

Naturalist

↓

Researcher

↓

Biologist

↓

Evolutionary Scientist
```

These titles are descriptive only.

They do not modify simulation rules.

---

# Unlock Categories

Progression unlocks:

- analysis tools
- Encyclopedia sections
- advanced statistics
- environmental overlays
- research reports
- scientific experiments

Nothing is unlocked through combat or resource collection.

---

# Scientific Milestones

Major discoveries create milestones.

Examples include:

- first species discovered
- first extinction observed
- first evolutionary branch documented
- complete food web identified
- ecosystem fully catalogued

Milestones represent significant moments in the player's scientific journey.

---

# Research Objectives

Objectives guide the player without forcing a specific play style.

Examples:

- Observe five herbivore species.
- Document a migration event.
- Discover a mutualistic relationship.
- Identify a dominant genetic trait.
- Record a complete seasonal cycle.

Objectives are optional.

---

# Long-Term Progression

Over many worlds, the player gradually builds a comprehensive scientific archive.

Knowledge accumulates across all simulations.

Each new world offers opportunities to discover something previously unseen.

---

# Replayability

Different world seeds generate different ecosystems.

Replayability comes from:

- new evolutionary paths
- unexpected behaviours
- environmental variation
- emergent interactions

The player's accumulated knowledge improves their ability to understand new worlds.

---

# Failure

There is no permanent failure.

World collapse, extinction and ecological imbalance become research opportunities.

Even unsuccessful experiments contribute to player knowledge.

---

# Relationship With Gaia Engine

Gaia Engine generates deterministic simulation data.

Project Gaia interprets that data to determine progression.

Progression never modifies Gaia Engine rules.

---

# Related Documents

PG-002 — Core Loop

PG-005 — Player Powers

PG-006 — Discovery System

PG-007 — Encyclopedia Game Design

PG-008 — UI Flow

PG-010 — Vertical Slice Plan

---

# Acceptance Criteria

- [ ] Progression is based on knowledge.
- [ ] Unlocks improve understanding rather than power.
- [ ] Knowledge persists across worlds.
- [ ] Supports endless replayability.
- [ ] Fully compatible with Gaia Engine.

---

# Revision History

## 1.0.0

Initial version.
