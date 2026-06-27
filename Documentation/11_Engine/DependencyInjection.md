# ENG-008 — Dependency Injection

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the Dependency Injection architecture used throughout Gaia Engine.

Dependency Injection enables modularity, testability and loose coupling between engine systems.

---

# Scope

This specification defines:

- service registration
- dependency resolution
- service lifetime
- module composition
- dependency rules

It does not define:

- gameplay logic
- simulation
- serialization
- rendering

---

# Philosophy

Systems should depend on abstractions.

No module should instantiate its own dependencies.

The composition root is responsible for wiring the application.

---

# Responsibilities

The Dependency Injection System is responsible for:

- registering services
- resolving dependencies
- managing service lifetimes
- validating dependency graphs

The Dependency Injection System never:

- execute simulation logic
- contain gameplay state
- manage entity lifecycles

---

# Architecture

```text
Application

↓

Composition Root

↓

Service Registry

↓

Dependency Resolver

↓

Engine Systems
```

---

# Service Categories

```text
Services

├── Simulation
├── World
├── AI
├── Gameplay
├── Audio
├── UI
├── Persistence
└── Diagnostics
```

---

# Registration

Services are registered during engine startup.

Registration is centralized.

Runtime registration should be avoided.

---

# Resolution

Dependencies are resolved automatically.

Systems receive dependencies through constructor injection.

Property injection is forbidden.

---

# Service Lifetimes

Supported lifetimes:

- Singleton
- Scoped
- Transient

Simulation services are typically Singletons.

Runtime processing objects may be Transient.

---

# Dependency Rules

Dependencies must always point toward lower-level abstractions.

Circular dependencies are forbidden.

Engine modules should communicate through interfaces.

---

# Composition Root

The Composition Root:

- registers every service
- validates registrations
- builds the dependency graph

Only one Composition Root exists.

---

# Testing

Dependency Injection enables:

- mock services
- fake repositories
- isolated unit tests
- deterministic integration tests

---

# Validation

Startup validation checks:

- missing registrations
- circular dependencies
- invalid lifetimes

Startup must fail fast when configuration is invalid.

---

# Performance

Dependency resolution occurs primarily during startup.

Runtime resolution should be minimized.

Frequently used services should be cached.

---

# Determinism

Dependency Injection must not introduce nondeterministic behaviour.

Service resolution order must remain stable.

---

# Serialization

Dependency Injection state is never serialized.

Services are recreated during engine initialization.

---

# Design Constraints

The Dependency Injection System must remain:

- modular
- deterministic
- testable
- platform independent

---

# Related Documents

ENG-002 — Domain Model

ENG-007 — Configuration

ENG-009 — Plugin Architecture

---

# Acceptance Criteria

- [ ] Constructor injection only.
- [ ] Supports Singleton, Scoped and Transient services.
- [ ] Detects circular dependencies.
- [ ] Supports automated testing.
- [ ] Runtime state is not serialized.

---

# Revision History

## 1.0.0

Initial version.
