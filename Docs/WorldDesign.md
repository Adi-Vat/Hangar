# World Design  
## Zones
The larger solar system/galaxy is split up into multiple scenes for performance.
Each scene contains one or more main locations and multiple POIs.
The skybox for each zone reflects the zones around it that can be travelled to.  
## Warp points
To travel to other zones, warp points are used. These are toll-booth esque points at the edges of each zone that allow the player and NPCs to switch scenes.
Visually, they may look like a black hole, or be some encompassing build or something similar. To an observer, a ship/craft entering the warp point appears to stop, turn red, and fade away. To the people inside the ship, some 'warp' loading screen might play?  
## Edge of map conditions
The boundary of each zone may be defined as an oblate spheroid(?) with equation `{(x,y,z) in R+ : x^2/a + y^2/b + z^2/c = 1}`
As the player approaches the boundary, certain visual effects may be applied like gravitational lensing, fog(?), and static. Once the player crosses the boundary, their field of view is completely obscured (maybe just by darkness) and, given coordinates (x1, y1, z1) they are teleported to (-x1, -y1, -z1). 
Any maps the player might have stop working for a few seconds as they teleport.  
The zones are **continuous**. Lore explanation: Each zone sits in a gravitational well where space-time is bent into an upside-down rain drop shape. The edge of the zones are at the sharpest turn in the rain-drop. To escape the gravity well, a large amount of energy/wormhole is required, which is why ships must go to the warp points.