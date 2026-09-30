# Project Structure

Target engine: Unity (mobile).

Assets/
- Scenes/ — gameplay and test scenes
- Scripts/Core/ — shared game systems and interfaces
- Scripts/Player/ — player movement and interaction
- Scripts/Camera/ — isometric camera
- Scripts/Economy/ — money, prices and transactions
- Scripts/Production/ — machines, production cycles and production chains
- Scripts/Inventory/ — items and storage
- Scripts/World/ — terrain, unlockable areas and expansion
- Scripts/UI/ — HUD, buttons and mobile controls
- Scripts/Save/ — save/load
- Prefabs/Player/ — player prefabs
- Prefabs/Machines/ — production machines
- Prefabs/World/ — world objects
- Prefabs/UI/ — UI prefabs
- Art/ — models, materials, textures and VFX
- Audio/ — music and sound effects
- Resources/ — runtime resources when required

## Build order

1. Core game state
2. Player + joystick
3. Isometric camera
4. Basic production
5. Collection + inventory
6. Selling + economy
7. Machine upgrades
8. Land unlocking
9. Multiple machine types
10. Production chains
11. Level-based unlocks
12. Save system
13. UI polish and mobile optimization

## Design principle

The game should grow from a simple one-machine loop into a network of machines and resources. New areas should introduce meaningful production options rather than only increasing map size.
