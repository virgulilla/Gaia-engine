# AI-006 — Behaviour Execution

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines how selected Actions are translated into executable requests for Simulation Systems.

Behaviour Execution is the bridge between Artificial Intelligence and the Simulation.

---

# Scope

This specification defines:

- action dispatch
- execution requests
- execution lifecycle
- interruption
- completion handling

It does not define:

- decision making
- movement
- rendering
- animation
- pathfinding

---

# Philosophy

AI decides.

Simulation executes.

Behaviour Execution connects both layers without introducing dependencies.

---

# Responsibilities

Behaviour Execution is responsible for:

- receiving selected Actions
- dispatching execution requests
- monitoring execution state
- reporting completion

Behaviour Execution never:

- make decisions
- execute simulation logic
- update physiology
- modify world state directly

---

# Execution Pipeline

```text
Decision Making

↓

Selected Action

↓

Behaviour Execution

↓

Simulation Request

↓

Simulation System

↓

Execution Result

↓

Event Bus

↓

Utility AI
```

---

# Execution Request

Every request contains:

- OrganismId
- ActionId
- TargetId
- StartTick
- ExpectedDuration
- Priority

Requests are immutable.

---

# Dispatch

Behaviour Execution routes requests to specialized systems.

Examples:

Move → Movement System

Eat → Feeding System

Drink → Hydration System

Sleep → Rest System

Attack → Combat System

Reproduce → Reproduction System

---

# Execution State

Possible states:

- Waiting
- Accepted
- Running
- Suspended
- Completed
- Cancelled
- Failed

---

# Monitoring

Behaviour Execution monitors:

- progress
- interruptions
- completion
- timeout
- cancellation

It does not evaluate success.

Simulation Systems report results.

---

# Interruptions

Execution may be interrupted when:

- organism dies
- higher priority action appears
- target disappears
- environment becomes invalid

Interrupted Actions return control to Decision Making.

---

# Completion

When an Action completes:

- execution state is updated
- completion event is published
- AI is allowed to evaluate a new decision

---

# Failure Handling

Failures include:

- unreachable target
- insufficient resources
- invalid state
- interrupted execution

Failures generate Events.

Failures never terminate the simulation.

---

# Event Integration

Typical published events:

- ActionStarted
- ActionCompleted
- ActionCancelled
- ActionFailed

Behaviour Execution never consumes its own events.

---

# Determinism

Execution dispatch must be deterministic.

The same selected Action must always generate the same execution request.

---

# Serialization

Pending and running execution requests must be serializable.

Completed requests are discarded after processing.

---

# Design Constraints

Behaviour Execution must remain:

- deterministic
- stateless where possible
- event-driven
- renderer independent
- platform independent

---

# Related Documents

AI-004 — Decision Making

AI-005 — Actions

SIM-003 — Event Bus

PAT-003 — Event Pattern

---

# Acceptance Criteria

- [ ] Dispatches Actions to Simulation Systems.
- [ ] Supports interruption.
- [ ] Publishes execution events.
- [ ] Fully deterministic.
- [ ] Fully serializable.

---

# Revision History

## 1.0.0

Initial version.
