# Spherical Gravity
Planet---) <-r- [Player]
Player strongest attractor, calculates Gravity with:
g/r^2 where g is variable for each planet, and r is the distance to the centre of the planet.  
Then adds a force in the direction between the player and the centre of the planet  
For the player, they snap automatically to the normal of the planet's spherical surface.
Get the rotation between the player's up vector and the planet normal closest to the player's feet.
Add that to the player's current rotation to snap!
And then add the player's local y rotation to get yaw turning on a spherical surface.
