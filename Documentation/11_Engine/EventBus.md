# ENG-001 — Event Bus

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the Event Bus used for communication between engine systems.

The Event Bus enables loose coupling between modules while preserving deterministic execution.

---

# Scope

This specification defines:

- event publishing
- event subscription
- event dispatch
- execution order
- event lifecycle

It does not define:

- gameplay logic
- rendering
- networking

---

# Philosophy

Systems never call other systems directly.

Instead, systems communicate through Events.

This minimizes dependencies and improves modularity.

---

# Responsibilities

The Event Bus is responsible for:

- receiving events
- dispatching events
- preserving execution order
- managing subscribers

The Event Bus never:

- execute gameplay logic
- modify simulation
- store persistent game state

---

# Event Pipeline

```text
Simulation System

↓

Publish Event

↓

Event Queue

↓

Dispatcher

↓

Subscribers

↓

Processing Complete
```

---

# Event Structure

Every Event contains:

```text
Event

├── EventId
├── EventType
├── Source
├── Timestamp
├── Priority
├── Payload
└── Tick
```

---

# Event Categories

Supported categories include:

- Simulation
- World
- Organism
- AI
- Gameplay
- UI
- Audio
- System

---

# Event Queue

Events are stored in a deterministic queue.

Queue ordering:

1. Tick
2. Priority
3. Creation Order

---

# Dispatch

Events are dispatched synchronously during the Simulation Tick.

Subscribers receive events in deterministic order.

---

# Subscriptions

Systems subscribe only to required event types.

Broadcast subscriptions should be avoided.

---

# Priorities

Supported priorities:

- Critical
- High
- Normal
- Low

Priority affects processing order only.

---

# Event Lifetime

Events exist only during processing.

Processed events are immediately discarded.

Persistent history belongs to Analytics.

---

# Error Handling

Subscriber failures must not interrupt event processing.

Errors are reported through diagnostic systems.

---

# Determinism

Given identical simulation history:

- identical events
- identical order
- identical subscribers

must always produce identical execution.

---

# Performance

The Event Bus should:

- avoid allocations
- reuse event buffers
- support pooling
- minimize copying

---

# Serialization

Runtime events are never serialized.

Persistent state belongs to simulation systems.

---

# Design Constraints

The Event Bus must remain:

- deterministic
- lightweight
- thread-safe
- modular

---

# Related Documents

SIM-002 — Simulation Loop

AI-006 — Behaviour Execution

AUD-001 — Audio Events

BAL-005 — Analytics

---

# Acceptance Criteria

- [ ] Supports deterministic dispatch.
- [ ] Supports priorities.
- [ ] Supports multiple subscribers.
- [ ] Runtime events are not serialized.
- [ ] Optimized for high event throughput.

---

# Revision History

## 1.0.0

Initial version.
