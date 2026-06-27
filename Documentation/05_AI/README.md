# AI-000 — Artificial Intelligence Module

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

The Artificial Intelligence module transforms organism state into behaviour.

AI does not create needs.

AI does not modify physiology.

AI evaluates the current world state and selects the most appropriate action.

---

# Scope

This module defines:

- utility evaluation
- decision making
- action selection
- memory
- perception
- navigation requests

It does not define:

- movement
- pathfinding
- physiology
- genetics
- rendering

---

# Module Architecture

```text
AI

├── Perception
├── Memory
├── Utility Evaluation
├── Decision Making
├── Action Planning
├── Behaviour Execution
└── Learning (Future)
```

---

# Design Principles

## State Driven

AI never owns biological state.

AI consumes:

- Needs
- Physiology
- Health
- Relationships
- World State

---

## Stateless Decisions

Whenever possible, AI should evaluate the current situation rather than relying on persistent state.

---

## Utility Based

Gaia Engine uses Utility AI.

Behaviour Trees are not used.

Finite State Machines are only allowed for simple internal behaviours.

---

## Deterministic

AI decisions must be deterministic.

Given identical:

- World
- Needs
- Organism
- Seed

the selected action must always be identical.

---

# Dependencies

Incoming:

- World
- Organism
- Simulation

Outgoing:

- Movement
- Interaction
- Reproduction
- Statistics

---

# Folder Structure

```text
AI/

README.md

UtilityAI.md

Perception.md

Memory.md

DecisionMaking.md

Actions.md

BehaviourExecution.md
```

---

# Related Documents

ORG-007 — Needs

ORG-009 — Relationships

SIM-001 — Simulation Philosophy

PAT-001 — System Pattern

---

# Acceptance Criteria

- [ ] Utility AI is the only decision model.
- [ ] AI contains no biological state.
- [ ] AI is deterministic.
- [ ] AI is independent from rendering.

---

# Revision History

## 1.0.0

Initial version.
