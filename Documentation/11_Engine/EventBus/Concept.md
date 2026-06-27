# Concept.md

**Document ID:** CORE-002-A

---

# Why an Event Bus?

Without an Event Bus every system would directly reference every other system.

Example:

Climate

↓

Plants

↓

Resources

↓

Organisms

↓

AI

↓

Statistics

The number of dependencies grows exponentially.

The architecture becomes fragile.

---

# Event Driven Simulation

Gaia Engine follows an event-driven architecture.

Systems never ask:

"Who should I notify?"

Instead they publish facts.

Example:

RainStarted

TemperatureChanged

OrganismBorn

OrganismDied

SpeciesExtinct

PlantGrown

ResourceDepleted

Every interested system reacts independently.

---

# Events Represent Facts

Events describe something that already happened.

Events never describe intentions.

Correct:

OrganismDied

Incorrect:

KillOrganism

Correct:

RainStarted

Incorrect:

StartRain

---

# Communication Model

Producer

↓

Event Bus

↓

Subscribers

Neither side knows about the other.

This guarantees low coupling.

---

# Deterministic Behaviour

Events are processed in deterministic order.

Publishing order must always equal processing order.

No exceptions.

---

# Lifetime

Events exist only during processing.

After processing they are discarded.

The Event Bus is not an event history.

Historical data belongs to the Statistics System.

---

# Philosophy

Systems communicate through facts.

Never through commands.
