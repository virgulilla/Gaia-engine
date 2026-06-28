# SIM-004 — Time System

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the Time System responsible for advancing simulation time.

The Time System provides a deterministic temporal model shared by every subsystem.

---

# Scope

This specification defines:

- simulation time
- ticks
- calendars
- seasons
- temporal progression
- time scaling

It does not define:

- gameplay speed
- rendering frame rate
- animations

---

# Philosophy

Time exists only inside the simulation.

Rendering speed must never affect simulation time.

Every subsystem observes the same simulation clock.

---

# Responsibilities

The Time System is responsible for:

- advancing simulation time
- maintaining the simulation calendar
- exposing temporal information
- notifying temporal transitions

The Time System never:

- execute gameplay logic
- modify rendering
- control frame rate

---

# Time Hierarchy

```text
Simulation Time

├── Tick
├── Minute
├── Hour
├── Day
├── Season
└── Year
```

---

# Tick

The Tick is the smallest deterministic unit of time.

Every simulation update occurs inside a Tick.

Ticks are sequential.

Ticks are never skipped.

---

# Calendar

Simulation time is represented using a virtual calendar.

Calendar units include:

- Day
- Season
- Year

The calendar is independent from real-world time.

---

# Seasons

Supported seasons:

- Spring
- Summer
- Autumn
- Winter

Season transitions occur deterministically.

---

# Time Scale

Simulation speed may be adjusted.

Examples:

- Pause
- Normal
- ×2
- ×4
- ×8

Changing simulation speed never changes simulation results.

It only changes how quickly Ticks are processed.

---

# Time Events

The Time System publishes events for:

- New Day
- New Season
- New Year

Subscribers react through the Event Bus.

---

# Temporal Queries

Systems may query:

- Current Tick
- Current Day
- Current Season
- Current Year
- Elapsed Time

The Time System is the only authoritative source for temporal information.

---

# Performance

The Time System should:

- avoid allocations
- update using fixed-size arithmetic
- expose cached temporal values

---

# Determinism

Simulation time must always advance identically under identical conditions.

Real-world clocks must never influence simulation.

---

# Serialization

Simulation time is serialized.

Loading restores the exact temporal state.

---

# Design Constraints

The Time System must remain:

- deterministic
- lightweight
- platform independent
- simulation driven

---

# Related Documents

SIM-002 — Simulation Loop

SIM-003 — Tick Pipeline

FOUND-003 — Determinism

ENG-001 — Event Bus

---

# Acceptance Criteria

- [ ] Defines deterministic simulation time.
- [ ] Supports seasons and calendar.
- [ ] Independent from rendering frame rate.
- [ ] Fully serializable.
- [ ] Acts as the unique temporal authority.

---

# Revision History

## 1.0.0

Initial version.
