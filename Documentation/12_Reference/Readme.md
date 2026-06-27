# REF-000 — Reference Documentation

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the reference documentation used across Gaia Engine.

Reference documents provide implementation-independent information for developers, contributors and automated coding assistants.

They complement the engine specifications without defining engine behaviour.

---

# Scope

This module defines:

- terminology
- naming conventions
- file organization
- coding guidelines
- architectural decision records
- documentation templates

It does not define:

- gameplay
- simulation
- rendering
- engine implementation

---

# Philosophy

Documentation is part of the codebase.

Every architectural decision should be documented.

Documentation should always be more stable than implementation.

---

# Responsibilities

The Reference module is responsible for:

- documenting standards
- preserving architectural decisions
- defining terminology
- providing reusable templates

The Reference module never:

- define simulation behaviour
- replace technical specifications
- duplicate implementation details

---

# Module Architecture

```text
Reference

├── Glossary
├── Naming Conventions
├── Coding Standards
├── Architecture Decision Records
├── Templates
└── Specifications
```

---

# Design Principles

## Single Source of Truth

Each concept must be documented only once.

Other documents should reference it instead of duplicating it.

---

## Stable Documentation

Reference documentation changes less frequently than implementation.

Breaking documentation changes require review.

---

## Implementation Independent

Reference documents describe concepts rather than code.

Implementation details belong elsewhere.

---

## Developer Friendly

Documentation should support:

- contributors
- maintainers
- reviewers
- automated coding agents

---

# Folder Structure

```text
Reference/

README.md

Glossary.md

NamingConventions.md

CodingStandards.md

ADR.md

Templates.md

Specifications.md
```

---

# Related Documents

ARCH-001 — Architecture Overview

PAT-001 — Design Patterns

ENG-002 — Domain Model

---

# Acceptance Criteria

- [ ] Defines shared terminology.
- [ ] Defines coding conventions.
- [ ] Documents architectural decisions.
- [ ] Avoids implementation details.

---

# Revision History

## 1.0.0

Initial version.
