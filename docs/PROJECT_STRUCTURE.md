# Project Structure

Target engine: Unity (mobile).

Assets/
- Scenes/ — gameplay and test scenes
- Scripts/Core/ — shared game systems and interfaces
- Scripts/Player/ — player movement and interaction
- Scripts/Camera/ — isometric camera
- Scripts/Economy/ — money, prices and transactions
- Scripts/Production/ — machines and production cycles
- Scripts/Inventory/ — items and storage
- Scripts/World/ — terrain and unlockable areas
- Scripts/UI/ — HUD, buttons and mobile controls
- Scripts/Save/ — save/load
- Prefabs/Player/ — player prefabs
- Prefabs/Machines/ — production machines
- Prefabs/World/ — world objects
- Prefabs/UI/ — UI prefabs
- Art/ — models, materials, textures and VFX
- Audio/ — music and sound effects
- Resources/ — runtime resources when required

ProjectSettings/ will be added when the Unity project is initialized.

## Build order
1. Core data and game state
2. Player + joystick
3. Isometric camera
4. Production machine
5. Collection
6. Selling/economy
7. Upgrades
8. Land unlocking
9. Save system
10. UI polish and mobile optimization
