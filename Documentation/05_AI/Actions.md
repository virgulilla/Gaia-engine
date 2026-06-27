# AI-005 — Actions

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the executable actions available to organisms.

Actions are requests produced by the AI.

Simulation Systems execute those requests.

---

# Scope

This specification defines:

- action definition
- action lifecycle
- action requirements
- action interruption
- action completion

It does not define:

- decision making
- movement
- animation
- rendering

---

# Philosophy

An Action represents an intention.

It does not perform work.

Dedicated Simulation Systems execute Actions.

---

# Responsibilities

Actions are responsible for:

- describing intended behaviour
- exposing execution requirements
- defining completion rules

Actions never:

- execute themselves
- modify the World
- update organisms
- perform rendering

---

# Action Lifecycle

```text
Action Selected

↓

Validation

↓

Execution Requested

↓

Running

↓

Completed

↓

Removed
```

---

# Action Structure

Every Action contains:

```text
Action

├── Action Id
├── Action Type
├── Target
├── Priority
├── Status
├── Estimated Duration
├── Interruptible
└── Completion Rules
```

---

# Action Types

Core actions include:

- Idle
- Move
- Eat
- Drink
- Sleep
- Explore
- Rest
- Reproduce
- Attack
- Escape
- Follow
- Search
- Harvest
- Interact

New actions may be introduced without modifying existing ones.

---

# Target

Actions may reference:

- Organism
- Resource
- Position
- Chunk
- Structure

Targets are referenced by identifiers.

Direct object references are forbidden.

---

# Status

Possible values:

- Pending
- Running
- Completed
- Cancelled
- Failed

Status changes are managed by Simulation Systems.

---

# Validation

Every Action must validate:

- Target Exists
- Requirements Met
- Resources Available
- Action Allowed

Invalid Actions are cancelled.

---

# Execution

Actions are executed by specialized systems.

Examples:

Move → Movement System

Eat → Feeding System

Sleep → Rest System

Reproduce → Reproduction System

---

# Duration

Every Action defines an estimated duration.

Duration may be:

- Instant
- Timed
- Continuous

---

# Interruptibility

Actions define whether they can be interrupted.

Examples:

Interruptible:

- Explore
- Wander
- Search

Non-interruptible:

- Birth
- Death
- Digest Initial Meal

---

# Completion

An Action completes when:

- objective achieved
- cancelled
- failed
- timeout reached

Completion generates an Event.

---

# Failure

Actions may fail.

Examples:

- Target disappeared
- Resource depleted
- Predator arrived
- Path blocked

Failure never crashes the simulation.

---

# Determinism

Action execution requests must be deterministic.

Identical decisions always produce identical Action Requests.

---

# Serialization

Current Action state must be serializable.

Loading restores pending and running Actions.

---

# Design Constraints

Actions must remain:

- deterministic
- data-driven
- lightweight
- renderer independent

---

# Related Documents

AI-001 — Utility AI

AI-004 — Decision Making

AI-006 — Behaviour Execution

SIM-003 — Event Bus

---

# Acceptance Criteria

- [ ] Actions describe intent only.
- [ ] Actions never execute themselves.
- [ ] Supports interruption.
- [ ] Supports validation.
- [ ] Fully serializable.

---

# Revision History

## 1.0.0

Initial version.
