## Dynamic Component Design

### 1. **What are the main features of the hidden objects in your game?**
- **How will you determine the properties of the hidden objects so the game is balanced?**  
  We will use a difficulty level system, where the properties of hidden objects will vary based on the stage's difficulty. In easier levels, there will be fewer objects, and their placement will be more accessible, while in harder levels, their placement will be more complex and challenging.

- **Provide logical starting numbers; explain your thought process to arrive at these numbers.**  
  - Easy houses: 1-2 ghosts per house.  
  - Medium houses: 3-4 ghosts.  
  - Hard houses: 5-6 ghosts.  
  This selection ensures a gradual challenge while maintaining an engaging experience for players of different skill levels.

### 2. **What are the locations of the main objects in your game?**
- **How did you determine the locations of each item so the game will feel balanced?**  
  Object placement is determined by the difficulty level and designed to integrate naturally into the map. For example:
  - Easy: Close to switches and easy-to-access areas.
  - Medium: Spread out and slightly hidden.
  - Hard: Hidden in difficult-to-reach areas or requiring puzzle-solving to access.

The map isn't finish, here is the map at the moment:
![Game Map](Images/map.png)


### 3. **What are the main behaviors of the objects in your game?**
- **Describe in detail how these objects behave, including interactions with non-player characters (NPCs).**  
  - Switches can be activated by players to turn lights on or off, open doors, or trigger traps.  
  - Ghosts move through the environment, obstructing the player's progress and sometimes chasing them.  
  - NPCs may provide hints or distractions, depending on the game's storyline.

### 4. Does your game have an economic system that can fit its design?

**Economic System:**  
- The game features an **internal economic system** based on coins collected during gameplay. Players can use these coins to purchase upgrades such as gadgets, skins, and additional abilities.  

**Pricing Details:**  
- Prices are determined by the item’s **rarity** and its **impact on gameplay**:  
  - **Cosmetic items** like skins are cheaper.  
  - **Ability-enhancing items** such as higher jumps or improved lighting are more expensive.  

### 5. **What and how much information do you check about the player's state?**
- **What checkpoints or indicators show the player’s progress?**  
  - Real-time HUD displays health, flashlight battery, coins, and houses left.  

### 6. **What is the player's control system for managing the game state?**
- **Explain the reasoning behind your control system.**  
  - Players control their character directly using keyboard and mouse.  
  - Time cannot be fast-forwarded, but the game can be paused.  
  - Inventory management is handled via an in-game menu.

### 7. **What decisions will players need to make during gameplay?**
- **Does your game require a strategy system? If so, what strategies can players use to progress?**  
  - Movement strategies: Avoid ghosts based on their positions and light conditions.  
  - Resource management: Use in-game currency to purchase items that enhance abilities (e.g., better jumps or stronger lighting).  
  - Level skipping: Players may purchase hints or skip levels at a cost, which introduces a risk-reward system to maintain competitiveness.
