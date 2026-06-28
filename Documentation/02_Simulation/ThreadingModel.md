# SIM-006 — Threading Model

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the threading architecture used by the Simulation.

The Threading Model allows Gaia Engine to take advantage of modern multi-core processors while guaranteeing deterministic execution.

Performance is important.

Determinism is mandatory.

---

# Scope

This specification defines:

- simulation threads
- worker jobs
- synchronization
- deterministic scheduling
- parallel execution rules

It does not define:

- operating system threads
- rendering threads
- networking
- asynchronous resource loading

---

# Philosophy

Parallelism is an optimization.

Simulation correctness is a requirement.

No amount of additional performance justifies non-deterministic execution.

---

# Responsibilities

The Threading Model is responsible for:

- distributing work across CPU cores
- synchronizing worker jobs
- preserving deterministic execution
- maximizing hardware utilization

The Threading Model never:

- modify simulation rules
- alter Tick order
- introduce race conditions
- change gameplay behaviour

---

# Architecture

```text
Simulation Thread

├── Scheduler
├── World Jobs
├── Chunk Jobs
├── Organism Jobs
├── Environment Jobs
└── Merge Phase
```

Only the Simulation Thread advances the Simulation Tick.

Worker Jobs execute isolated calculations.

---

# Simulation Thread

The Simulation Thread owns:

- Tick progression
- system ordering
- synchronization
- event dispatch
- final merge

It is the only thread allowed to modify authoritative simulation state.

---

# Worker Jobs

Worker Jobs execute independent tasks.

Typical examples include:

- chunk processing
- climate calculations
- vegetation updates
- organism physiology
- pathfinding preparation

Jobs operate only on isolated data.

---

# Job Scheduling

Jobs are created by the Scheduler.

Scheduling must remain deterministic.

Execution order may differ internally, but observable results must always remain identical.

---

# Synchronization Barriers

Synchronization occurs after major simulation phases.

Typical barriers:

- World Update Complete
- Organism Update Complete
- Interaction Complete
- Environment Complete
- Tick Complete

Every worker must finish before the next phase begins.

---

# Merge Phase

Parallel results are merged sequentially.

Merge order is fixed.

Merge operations always execute on the Simulation Thread.

---

# Shared State

Shared mutable state is forbidden.

Worker Jobs communicate using:

- immutable inputs
- local buffers
- deterministic merge operations

Workers never modify shared simulation objects directly.

---

# Thread Safety

Every simulation system must be thread-safe.

Systems should avoid:

- global mutable variables
- static runtime state
- uncontrolled locking

Ownership should always be explicit.

---

# Locking

Locking should be minimized.

Preferred techniques include:

- immutable data
- ownership boundaries
- work partitioning
- deterministic merge

Locks should never exist inside the simulation hot path unless unavoidable.

---

# Performance Goals

The Threading Model should:

- maximize CPU utilization
- minimize synchronization
- reduce cache misses
- avoid false sharing
- minimize allocations

---

# Failure Handling

If a worker fails:

- the current Tick is aborted safely
- diagnostics are recorded
- simulation integrity is preserved

Worker failures must never corrupt world state.

---

# Determinism

Parallel execution must produce identical simulation results regardless of:

- CPU core count
- execution timing
- operating system
- hardware platform

Deterministic scheduling has priority over maximum throughput.

---

# Serialization

Thread state is runtime only.

Worker Jobs are recreated every Tick.

No thread or scheduler state is serialized.

---

# Design Constraints

The Threading Model must remain:

- deterministic
- scalable
- thread-safe
- platform independent
- performance oriented

---

# Related Documents

SIM-002 — Simulation Loop

SIM-003 — Tick Pipeline

SIM-005 — Event Bus Integration

FOUND-003 — Determinism

ENG-001 — Event Bus

---

# Acceptance Criteria

- [ ] Supports deterministic parallel execution.
- [ ] Uses fixed synchronization barriers.
- [ ] Prevents race conditions.
- [ ] Merge operations are deterministic.
- [ ] Runtime thread state is never serialized.

---

# Revision History

## 1.0.0

Initial version.
