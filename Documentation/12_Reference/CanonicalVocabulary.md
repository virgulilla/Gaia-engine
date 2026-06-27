# REF-001 — Canonical Vocabulary

**Version:** 1.0.0

**Status:** Approved

**Owner:** Technical Director

**Last Updated:** 2026-06-27

---

# Purpose

This document defines the official terminology used throughout Gaia Engine.

Every document, source file, namespace, class, variable, commit message and prompt must follow this vocabulary.

The purpose is to ensure consistency between developers and AI assistants.

---

# Fundamental Rule

One concept.

One name.

Always.

Synonyms are forbidden.

---

# Core Concepts

## Engine

The reusable simulation framework.

Never refers to the game.

---

## Project

A game built using Gaia Engine.

Example:

Project Gaia

---

## Simulation

The deterministic execution of the virtual world.

Simulation is independent from rendering.

---

## Tick

The smallest deterministic simulation step.

Never use:

Frame

Cycle

Loop

Update

---

## Frame

Rendering update only.

Frames are not part of the simulation.

---

## World

The complete simulated environment.

Includes:

- terrain
- climate
- organisms
- resources

---

## Chunk

A spatial subdivision of the world.

Used for optimization.

---

## Organism

Any living entity.

Examples:

Plants

Animals

Fungi

Future microorganisms

Always use:

Organism

Never use:

Creature

Animal

Mob

NPC

Unit

Lifeform

---

## Species

Evolutionary classification.

Species never contain behaviour.

Species describe relationships.

---

## Genome

Complete genetic information of an organism.

Contains genes.

Never contains runtime state.

---

## Gene

A single inherited trait.

---

## Trait

The observable result of one or more genes.

---

## Mutation

A modification of genetic information.

Mutations are never guaranteed to be beneficial.

---

## Biome

A region sharing environmental characteristics.

Examples:

Forest

Desert

Swamp

Tundra

---

## Climate

Global environmental conditions.

Examples:

Temperature

Humidity

Wind

Rain

---

## Resource

Anything organisms consume or compete for.

Examples:

Food

Water

Minerals

Sunlight

---

## Need

An internal requirement of an organism.

Examples:

Hunger

Thirst

Energy

Safety

Reproduction

---

## Behaviour

The result of decision making.

Behaviour is never hardcoded inside organisms.

---

## Event

A fact describing something that already happened.

Example:

RainStarted

Never:

StartRain

---

## System

A processor operating on data.

Systems own behaviour.

---

## Component

A data container.

Components never contain simulation logic.

---

## Configuration

External data controlling engine behaviour.

Configurations are editable.

Configurations are not code.

---

## Discovery

New knowledge acquired by the player.

Examples:

New species

Rare mutation

Climate event

Discovery belongs to gameplay.

Not simulation.

---

## Encyclopedia

Persistent collection of discoveries shared across all worlds.

---

# Forbidden Terms

The following words must never appear in engine code or documentation.

Creature

Animal

Mob

NPC

Monster

Enemy

Friendly

Magic

Spell

Level Up

Experience Points

Quest

These belong to gameplay-specific projects, not to Gaia Engine.

---

# Naming Principles

Use nouns for data.

Genome

Species

Climate

Use verbs only for actions.

Mutate

Move

Eat

Never mix both.

---

# Consistency Rules

Every concept has exactly one official name.

Plural forms follow standard English grammar.

Abbreviations are avoided unless universally accepted.

---

# Related Documents

CORE-001

SIM-001

ARCH-001

---

# Acceptance Criteria

- [ ] Every concept has one official name.
- [ ] Synonyms are eliminated.
- [ ] Engine terminology is game-independent.
- [ ] Gameplay-specific words remain outside the engine.

---

# Revision History

## 1.0.0

Initial version.
