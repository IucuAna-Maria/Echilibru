# Echilibru
**Echilibru** is a 3D game developed in Unity as part of my bachelor's thesis. It features combat systems, respawn mechanics, real-time weather, and AI powered by Unity's NavMesh. The project is built with `C#` and makes use of `Newtonsoft.Json`, `OpenWeatherMap`, and `IPAPI`.

---

## 🎮 Features & Mechanics

### 🔹 Player Mechanics

- **Movement**: Full 3D movement with sprinting and jumping capabilities.
- **Combat**: The player can launch a projectile straight ahead or toward the nearest enemy. Projectiles self-destruct after a short duration.
- **Buff System**: Killing 5 enemies grants a temporary damage buff against bosses (+20 damage). This buff is lost upon death.
- **Health Management**:
  - Passive healing: If the player has at least 50 coins, they regenerate +2 HP per second.
  - HP upgrades can be purchased from the NPC using coins.
- **Armor**: Can be bought from the NPC using coins. It reduces incoming damage by 20%.
- **Death & Respawn**:
  - Coins are lost permanently on death.
  - The player respawns at the last checkpoint reached, or at a predefined spawn point if no checkpoint was activated.

### 🔹 Coin System

- **Procedural Generation**: 20 coins spawn every minute and are available for 30 seconds before disappearing.
- **Coin Usage**:
  - Upgrade HP
  - Purchase armor
  - Enable passive healing

### 🔹 Enemy AI

- **Basic Enemies**:
  - Spawn procedurally when the player screams or makes a loud sound detected via microphone.
  - Appear at fixed positions evenly spaced around the player in a circle at a constant spawn distance. If a spawn point is invalid, the next one is checked until a valid spot is found or no positions remain.
  - Deal damage on collision with the player.
  - Track and follow the player once inside detection range.
- **Bosses**:
  - There are 4 bosses in total.
  - Each defeated boss increases the difficulty of the next (HP +20, damage +5).
  - Bosses shoot projectiles aimed at the upper body of the player.
  - Boss progression is based on how many you've defeated, not the order in which you fight them.

### 🔹 Environment

- **Dynamic Weather**:
  - Weather conditions in the game mirror the player’s real-world location using live data. Rain and snow effects appear in the game exactly when detected outside.
- **Checkpoint System**:
  - Progress is saved at activated checkpoints.
  - The player respawns at the last checkpoint or the initial spawn point.

### 🔹 Fun Extras

- **Windows Error Screen Prank**:
  - When the player's HP falls below 20%, there's a 30% chance a fake Blue Screen of Death (BSOD) will appear for dramatic effect.

---

## 💡 Technologies Used

- **Engine**: Unity
- **Language**: `C#`
- **Libraries & APIs**:
  - `Newtonsoft.Json` – for JSON parsing
  - `OpenWeatherMap` – for real-time weather
  - `IPAPI` – for location-based weather detection
- **Assets**: Various 3D models, animations, and sound effects from Unity Asset Store
- **Audio**: Background music generated with Suno (2 tracks)
