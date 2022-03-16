# 6CCS3PRJ Individual Project - Unity's A.I. Planner vs Behaviour Trees
---
Unity version: 2020.3.21f1

This is a stealth based game where the agent is the player and is implemented using either:
- Unity's A.I. Planner
- Behaviour tree asset

## Base Game
Stealthy agent is given a path with a goal location to reach. Along the path there are bins which provide cover for
the agent to avoid being detected by cyclic patrolling agents. The aim of the game is to reach the goal location 
without being detected.

## Project Description
The purpose of this project is to evaluate common reactive approaches (behaviour trees) to deliberative ones (planner).
There will be 2 scenes of the game - one for the planner agent and one for the behaviour tree agent. Both agents will 
be the stealthy players of the game and their performance will be benchmarked and analysed in the project report. 
Different environment configurations have been chosen to push the limits of both implementations of the stealthy agent.

## Environment Configurations
\# | Configuration
-- | --
`1`| Stamina mechanic introduced
`2`| Complex paths and junctions added
