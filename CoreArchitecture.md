CoreArchitecture.md

Version: 1.0.0

Status: Approved

Owner: Technical Director

---

Purpose

Definir los componentes fundamentales de Gaia Engine.

Todos los sistemas del motor dependen del Core.

El Core nunca depende de ningún sistema del motor.

---

Core Responsibilities

El Core proporciona únicamente infraestructura.

Nunca contiene lógica de simulación.

Nunca conoce organismos.

Nunca conoce biomas.

Nunca conoce clima.

Nunca conoce IA.

---

Core Modules

Core se divide en nueve módulos.

Core

├── Time

├── Scheduler

├── Events

├── Random

├── Configuration

├── Serialization

├── Logging

├── Profiling

└── Debug

Cada módulo debe ser completamente independiente.

---

Time Module

Responsabilidad

Gestionar el tiempo del motor.

Funciones

Simulation Tick

Delta Time

Tick Count

Simulation Speed

Pause

Resume

Nunca utiliza Time.deltaTime directamente.

Unity adapta su tiempo al tiempo del motor.

---

Scheduler Module

Responsabilidad

Ejecutar tareas periódicas.

Ejemplos

Actualizar clima.

Actualizar plantas.

Guardar partida.

Actualizar estadísticas.

Cada tarea posee una frecuencia independiente.

Nunca todo se ejecuta en cada Tick.

---

Event Module

Responsabilidad

Comunicación entre sistemas.

Todos los eventos son inmutables.

Un evento nunca modifica otro.

Los sistemas nunca conocen quién genera un evento.

Solo saben que ocurrió.

---

Random Module

Responsabilidad

Toda aleatoriedad.

Debe ser determinista.

La misma semilla siempre genera el mismo resultado.

Funciones

Seed

Streams independientes

Noise

Probability

Weighted Selection

Shuffle

---

Configuration Module

Toda configuración procede de archivos externos.

Nunca valores mágicos.

Tipos

EngineConfig

SimulationConfig

ClimateConfig

GenomeConfig

WorldConfig

AIConfig

---

Serialization Module

Responsabilidad

Guardar y cargar estado.

Debe poder serializar:

World

Organisms

Genome

Climate

Simulation

Statistics

No depende de Unity.

---

Logging Module

Responsabilidad

Registro de actividad.

Niveles

Info

Warning

Error

Critical

Debug

El Logging nunca contiene lógica.

---

Profiling Module

Responsabilidad

Medir.

Nunca optimizar.

Debe registrar

Tiempo por sistema

Tiempo por Tick

Consumo memoria

Eventos por segundo

Criaturas

Chunks

---

Debug Module

Responsabilidad

Visualizar el estado del motor.

Debe poder mostrar

Tick actual

Eventos

Colas

Temperatura

Humedad

Genes

Necesidades

Rutas IA

---

Dependency Rules

Todos los sistemas pueden utilizar Core.

Core nunca utiliza sistemas.

Nunca existe dependencia inversa.

---

Engine Startup

Load Config

↓

Initialize Logging

↓

Initialize Random

↓

Initialize Events

↓

Initialize Scheduler

↓

Initialize Serialization

↓

Initialize Simulation

↓

Ready

---

Engine Shutdown

Stop Simulation

↓

Flush Events

↓

Save

↓

Close Logging

↓

Shutdown

---

Thread Safety

Todo módulo Core debe estar preparado para funcionar en múltiples hilos.

Aunque inicialmente se ejecute en uno solo.

---

Performance

Core nunca debe generar Garbage Collection innecesaria.

Evitar:

LINQ en tiempo crítico.

Asignaciones repetidas.

Colecciones temporales.

Boxing.

Reflection en tiempo de ejecución.

---

Success Criteria

Core debe poder reutilizarse íntegramente en cualquier simulador.

Sin conocer absolutamente nada del juego.

---

Revision History

v1.0.0

Documento inicial.
