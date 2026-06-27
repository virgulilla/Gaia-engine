# README.md

**Document ID:** ENG-001

**Title:** Gaia Engine Domain Model

**Version:** 1.0.0

**Status:** Approved

---

# Purpose

This document defines every high-level domain that exists inside Gaia Engine.

Every object, feature and system belongs to one of these domains.

No additional top-level domains may be introduced without an approved ADR.

---

# Domain Philosophy

Gaia Engine is organized around domains instead of gameplay features.

Domains represent concepts that exist independently from any specific game.

Project Gaia is simply one possible implementation.

---

# Engine Domains

## Time

Responsible for:

- simulation time
- world time
- scheduling
- calendars

---

## Simulation

Responsible for:

- simulation lifecycle
- tick execution
- synchronization
- execution order

---

## World

Responsible for:

- maps
- chunks
- biomes
- spatial queries
- world generation

---

## Terrain

Responsible for:

- elevation
- soil
- water level
- terrain modifiers

---

## Climate

Responsible for:

- weather
- seasons
- temperature
- humidity
- wind

---

## Resources

Responsible for:

- food
- minerals
- water
- fertility
- renewable resources

---

## Organisms

Responsible for every living entity.

Examples:

- plants
- animals
- fungi

---

## Genome

Responsible for:

- genes
- inheritance
- mutation
- evolution

---

## Species

Responsible for:

- classification
- taxonomy
- lineage
- extinction

Species never define behaviour.

Species describe evolutionary relationships.

---

## Player

Responsible for:

- player powers
- progression
- discoveries
- encyclopedia
- achievements

---

## Events

Responsible for communication between systems.

Events never own behaviour.

---

## Statistics

Responsible for:

- metrics
- analytics
- history
- records
- discoveries

---

# Design Rules

Every system belongs to one domain.

Every feature belongs to one domain.

Every document belongs to one domain.

Cross-domain communication must happen through events or public interfaces.

---

# Forbidden Domains

The following are NOT engine domains:

- Animals
- Wolves
- Cities
- Buildings
- Villages
- Monsters

These belong to higher gameplay layers.

---

# Related Documents

ARCH-001

SIM-001

CORE-001

---

# Acceptance Criteria

- [ ] Every engine feature maps to one domain.
- [ ] Domains remain independent.
- [ ] Cross-domain communication is explicit.
- [ ] New domains require an ADR.

---

# Revision History

## 1.0.0

Initial version.
