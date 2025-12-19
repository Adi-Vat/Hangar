# Ship Building
## Overview
I want the ships to feel your own. Each player should be able to mess around with their ship design (to a certain extent).
Most importantly the player should be able to make a ship that **doesn't work**. We must tow the line between frustration
and spoon-feeding. The player should feel like a genius for making their ship work. And like DaVinci for designing something
on their own.

## Features
Ships are not realistic rockets. NO AERODYNAMICAL SHIT! I'm not an Aerospace Engineer. And I'm not making Kerbal. leave some whimsy
for the player :)

To get started:

- Ships have a mass (and so a centre of mass)
- Ships have thrusters (and so can accelerate)
- Ships have controls (to steer, and so accelerate)

Everything else is optional!

The player places blocks together in a certain framework to create a "ship response", a list of variables that determine how
any ship will respond to any input. For example, a ship could be:  
Throttle * sensitivity -> Thruster  
or  
Throttle * sensitivity -> PID Controller -> Thruster -> Sensor -> Back to PID Controller

etc etc

And a ship's mass, centre of gravity etc affect the handling of the ship.

Ship locomotion is purely acceleration based.  
Throttle [0 - x] -> sets the acceleration of the ship ->  ship's velocity increases a certain amount every frame

If there's a PID controller instead of direct acceleration throttle, then
Throttle [0 - x]  -> (sets VELOCITY instead) -> PID Controller -> Sets accel. of the ship -> velocity sensors feedback to PID system

The base controls to make things feel "good" are,
forward thrust  
up thrust  
pitch, yaw, roll  
But the non-forward vectors are slowly cancelled out by applying a reverse force over time, this lets the user turn to move rather
than just float in a different direction.  
