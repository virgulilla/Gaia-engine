# SIM-005 — Event Bus Integration

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines how the Simulation integrates with the Engine Event Bus.

The Event Bus provides communication between Simulation Systems while preserving deterministic execution.

Simulation Systems never communicate directly.

---

# Scope

This specification defines:

- event publication
- event consumption
- event ordering
- event lifecycle
- simulation integration

It does not define:

- Event Bus implementation
- gameplay events
- UI events
- audio events

---

# Philosophy

Simulation produces events.

Systems react to events.

No Simulation System should directly invoke another Simulation System.

---

# Responsibilities

Simulation is responsible for:

- publishing simulation events
- subscribing to required events
- preserving deterministic event ordering
- avoiding direct system dependencies

The Event Bus is responsible for delivery only.

---

# Event Flow

```text
Simulation System

↓

Publish Event

↓

Simulation Event Queue

↓

Event Bus

↓

Subscribers

↓

Simulation Continues
```

---

# Event Categories

Simulation publishes:

- World Events
- Climate Events
- Organism Events
- Resource Events
- Species Events
- Time Events

Each category uses strongly typed event definitions.

---

# Publication Rules

Simulation Systems publish events only after completing their internal update.

Systems must never modify previously published events.

Events are immutable.

---

# Subscription Rules

Systems subscribe only to required event types.

Subscriptions should remain explicit.

Wildcard subscriptions are discouraged.

---

# Event Timing

Events generated during a Tick are processed deterministically.

Processing order is identical across every simulation.

No event may be delayed beyond the current Tick unless explicitly scheduled.

---

# Event Ordering

Ordering follows:

1. Tick Number
2. Event Priority
3. Publication Order

No subscriber may alter ordering.

---

# Event Lifetime

Simulation events exist only during processing.

Persistent information belongs to:

- World State
- Statistics
- Save Games

Events are transient.

---

# Deferred Events

Some events may be scheduled for future Ticks.

Deferred events must include:

- Target Tick
- Event Type
- Payload

Scheduling remains deterministic.

---

# Performance

Simulation event handling should:

- minimize allocations
- reuse event buffers
- batch dispatch operations
- support pooling

---

# Determinism

Event publication and processing must always produce identical execution for identical simulations.

Event processing must never depend on:

- hardware
- operating system
- execution timing

---

# Serialization

Simulation events are runtime objects.

Only persistent world state is serialized.

Deferred scheduled events are serialized when necessary.

---

# Design Constraints

The Simulation Event Integration must remain:

- deterministic
- event-driven
- modular
- lightweight

---

# Related Documents

SIM-002 — Simulation Loop

SIM-003 — Tick Pipeline

ENG-001 — Event Bus

FOUND-003 — Determinism

---

# Acceptance Criteria

- [ ] Simulation communicates exclusively through events.
- [ ] Events are immutable.
- [ ] Processing order is deterministic.
- [ ] Runtime events are not serialized.
- [ ] Deferred events are supported.

---

# Revision History

## 1.0.0

Initial version.
