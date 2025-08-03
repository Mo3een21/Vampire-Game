# Vampire Game – ReadMe

Welcome to **Vampire Game**! This document provides essential information about the game, including how to play, game mechanics, controls, and additional notes.

---

## Table of Contents  
1. [Game Overview](#game-overview)  
2. [How to Play](#how-to-play)  
3. [Controls](#controls)  
4. [Objectives & Goals](#objectives--goals)  
5. [Game Mechanics & Features](#game-mechanics--features)  
6. [System Requirements](#system-requirements)  
7. [Known Issues & Troubleshooting](#known-issues--troubleshooting)  
8. [Additional Notes](#additional-notes)  

---

## Game Overview  
**Genre:** Stealth Adventure (2D Top-Down)  
**Platform:** Windows Standalone (Unity)  
**Team:** Moeen Abu Katish, Mohammad Mohammad, Andre Tams, Tony Bajjali 

You are a vampire infiltrating ancient castles in search of treasure. Sneak past patrolling guards, avoid spike traps and enemy vision cones, collect keys and coins, unlock chests, and reach mystical statues to complete each level—all while racing the clock.

---

## How to Play  
**Gameplay Basics**  
- Navigate each room by moving your vampire avatar.  
- Press E to pick up keys/coins, open chests, or pull levers.  
- Avoid detection: if an enemy sees you, a detection meter fills; at 100% you die.  
- Spike traps only kill when their spikes are fully extended (animation timing matters).  
- Reach all statues to finish the level, then view your results screen.

**Levels/Stages**  
Main Menu → Level One → Level Two → Results

---

## Controls  
- **Move:** W / A / S / D  
- **Interact:** E  
- **Pause Menu:** Esc  
- **Menu Selection:** Mouse click

---

## Objectives & Goals  
**Main Objective:**  
Collect keys and coins, open chests for points, avoid detection and traps, then reach the golden statue to complete the level with the highest rank possible.  

**Secondary Goals:**  
- Finish under 6 minutes for a 2× time bonus (6–8 min = 1.5×; >8 min = 1×).  
- Gather every coin and chest.  
- Minimize deaths to avoid –200 pts penalty per death.
- Rank Systen goes like this: S Rank being the highest when you collect every treasure and beat the level as fast as possible. And C Rank being the lowest.  

---

## Game Mechanics & Features  
**Animations:**  
- Player: 10 unique clips (idle & walk in 4 directions, death, win).  
- Enemies: 2 visual variants × 8 clips each (idle & walk in 4 directions).  
- Enemy Patrol Variations: Vertical, horizontal, or stationary rotations.  

**Field of View & Detection Meter:** Enemies have a vision cone; meter rises when spotted.  

**Trap Timing:** Spike trap colliders toggle on/off via animation events.  

**Inventory System:** Keys collected with E, displayed in HUD, used on chests.  

**Pickups:**  
- Chests: +500 points  
- Coins: +100 points  
  On-screen message + SFX on pickup.  

**Score & Timer HUD:** Shows Keys, Score, and elapsed time (MM:SS:FF).  

**Audio Feedback:** Footsteps, ambient enemy sounds, trap warnings, pickup SFX, death & win cues.  

**Pause Menu:** Esc pauses gameplay; offers Resume or Main Menu.  

---

## System Requirements  
**Minimum:**  
- OS: Windows 10 (64-bit)  
- CPU: Intel Core i3-2100 or equivalent  
- Memory: 4 GB RAM  
- Graphics: DX11-capable GPU  
- Storage: 200 MB free  

**Recommended:**  
- OS: Windows 10/11  
- CPU: Intel Core i5-4590 or better  
- Memory: 8 GB RAM  
- Graphics: NVIDIA GTX 660 / AMD Radeon HD 7870 or better  
- Storage: 500 MB free  

---

## Known Issues & Troubleshooting  
- **Win Screen:** Sometimes the timer keeps going when we reach the win screen(the game doesn't get paused).  
- **Detection Meter Lag:** At very high FPS, meter may trail cone; reduce “Detection Rate” in Inspector.  
- **Audio Cut-off:** Ensure no other heavy audio apps are running.  

---

## Additional Notes  
- **Game Version:** 1.0  
- **Last Update:** July 31, 2025  
- **Contact:** moeen.2001@gmail.com  
- **Trailer & Demo:** [https://youtu.be/your_game_trailer](https://youtu.be/-ZI_x8ZtC5U)  

Enjoy Vampire Game—master the shadows and become the ultimate stealth vampire!
