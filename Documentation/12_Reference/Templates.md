# REF-005 — Documentation Templates

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the standard templates used for all Gaia Engine documentation.

Templates ensure consistency, readability and maintainability across the entire repository.

---

# Scope

This specification defines:

- document templates
- section ordering
- formatting rules
- identifier conventions
- writing guidelines

It does not define:

- implementation details
- coding standards
- architectural decisions

---

# Philosophy

Every document should look familiar.

Readers should always know where to find information.

Consistency is more valuable than personal writing style.

---

# Responsibilities

Documentation Templates are responsible for:

- standardizing document structure
- reducing duplication
- improving readability
- simplifying maintenance

Templates never:

- define engine behaviour
- replace specifications
- duplicate implementation details

---

# Standard Document Structure

Every specification document follows the same structure.

```text
Header

Purpose

Scope

Philosophy

Responsibilities

Architecture (Optional)

Lifecycle (Optional)

Examples (Optional)

Performance

Determinism

Serialization

Design Constraints

Related Documents

Acceptance Criteria

Revision History
```

---

# Header

Every document begins with:

```text
Document ID

Title

Version

Status

Owner

Last Updated
```

---

# Purpose

Explains why the document exists.

Keep this section concise.

---

# Scope

Defines exactly what the document includes.

Also defines what it explicitly excludes.

---

# Philosophy

Explains the high-level design intent.

Focus on principles rather than implementation.

---

# Responsibilities

Clearly define what the described system is responsible for.

Also list responsibilities it explicitly does not own.

---

# Architecture

Include architecture diagrams whenever they improve understanding.

Prefer simple tree structures.

Example:

```text
Module

├── Subsystem A
├── Subsystem B
└── Subsystem C
```

---

# Lifecycle

Use lifecycle diagrams whenever objects change state.

Example:

```text
Created

↓

Initialized

↓

Active

↓

Destroyed
```

---

# Performance

Document relevant performance expectations.

Avoid implementation-specific optimizations.

---

# Determinism

Every simulation-related document must describe deterministic requirements.

If determinism is not applicable, explicitly state so.

---

# Serialization

Every persistent object must document:

- what is serialized
- what is reconstructed
- what is transient

---

# Design Constraints

Summarize the architectural restrictions.

Examples:

- deterministic
- platform independent
- modular
- scalable

---

# Related Documents

Reference only official specifications.

Avoid circular references whenever possible.

---

# Acceptance Criteria

Acceptance Criteria should use checklist format.

Example:

```text
- [ ] Supports deterministic execution.
- [ ] Fully serializable.
- [ ] Independent from rendering.
```

---

# Revision History

Every document maintains a revision history.

Example:

```text
## 1.0.0

Initial version.
```

---

# Formatting Rules

Use:

- ATX Markdown headings (`#`)
- fenced code blocks
- unordered lists
- short paragraphs

Avoid:

- HTML
- tables unless necessary
- excessive nesting

---

# Naming Rules

Document names follow:

```text
PascalCase.md
```

Examples:

```text
World.md

Genome.md

SimulationLoop.md
```

---

# Design Constraints

Documentation Templates must remain:

- consistent
- implementation independent
- easy to extend
- human readable

---

# Related Documents

REF-002 — Naming Conventions

REF-003 — Coding Standards

REF-004 — Architecture Decision Records

---

# Acceptance Criteria

- [ ] Every document follows the same structure.
- [ ] Templates remain implementation independent.
- [ ] Supports future documentation modules.
- [ ] Promotes consistent writing.
- [ ] Easy for automated tools to parse.

---

# Revision History

## 1.0.0

Initial version.
