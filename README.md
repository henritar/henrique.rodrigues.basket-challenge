# Basketball Shooting Race  

## Overview  
Basketball Shooting Race is a Unity project inspired by basketball mini-games where the goal is to score as many baskets as possible from different positions on the court within a limited time.  

The gameplay focuses on responsive input, smooth physics, and polished user feedback, aiming to deliver an engaging and competitive experience.  

---

## Features  

### Core Gameplay  
- Single-player basketball shooting.  
- Camera tracking with smooth transitions.  
- Supports both **mouse input (PC)** and **touch input (mobile)**.  
- Score system:  
  - Perfect shots = 3 points  
  - Normal shots = 2 points  
- Random **Backboard Bonus**: 4, 6, or 8 points on successful rebound baskets.  
- Complete game flow: Main Menu → In-game → Reward Screen.  

### Extended Gameplay  
- **AI Opponent**: Computer-controlled player, configurable difficulty.  
- **Fireball Mode**: Consecutive baskets fill a bar; once full, all shots score double points until a miss.  
- **Visual & Audio Feedback**: Ball rotation, input power bar, score flyers, effects.  
- **Sound Effects**: Ball throw, goal scored, ambient sounds.  

### User Interface  
- Modular Canvas system (Main Menu, HUD, Timer UI, Reward Screen).  
- In-game HUD:  
  - Score counter  
  - Countdown timer  
  - Swipe feedbacks & power bar  
  - Fireball progress bar  
  - Active bonus indicator  

---

## Architecture & Tech Stack  

### Architecture Principles  
- **MVP (Model-View-Presenter)** for complex stateful entities (Ball, Player, UI).  
- **State Machine** for game flow (`Main Menu`, `Playing`, `Reward`).  
- **EventBus (UniRx)** for decoupled communication between managers.  
- **VContainer** for dependency injection.  
- **UniTask** for async handling (instead of coroutines).  
- **UniRx Observables** for UI binding and reactivity.  
- **Multiple Canvas** to minimize UI rebuilds.  
- **Object pooling** not required (fixed number of balls/players).  

### Tools & Frameworks  
- **Unity 2021.3.4f1**  
- **C#**  
- **VContainer** (DI)  
- **UniTask** (async/await for Unity)  
- **UniRx** (reactive event system)  
- **Cinemachine** (smooth camera control)  
- **TextMeshPro** (UI text)  

---

## Gameplay Flow  
1. **Main Menu** → Select time limit & difficulty.  
2. **Playing**  
   - Player alternates between shooting positions.  
   - NPC performs shots based on configured difficulty.  
   - Timer ticks down while scoring and bonuses are active.  
3. **Reward Screen** → Shows Player vs NPC score results.  

---

## Highlights  
- Physics-based shot trajectories with realistic arcs and spin.  
- Configurable shot types thresholds: perfect, backboard, rim touch, weak/strong miss.  
- AI opponent with probability-based difficulty system.  
- Fireball streak system with UI feedback.  
- HUD with responsive feedback across resolutions.  
- Optimized camera dynamics and environment details.  
