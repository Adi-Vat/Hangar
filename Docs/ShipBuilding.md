# Ship Building
## Overview
Ships can be stripped down to empty shells. The player is able to populate their ship with modules of their choosing.

## Modules
Modules can be inputs or outputs, so they can take data from the outside world or they can affect the world in some way.  
Connectors/cables are routed from inputs to outputs however this is just visual. The real data direction is such:
<pre>
                                          |--float--> output module     
Input module --Vector3--> ship computer --|--float--> output module  
                                          |--float--> output module  
</pre>
The ship's computer stores relationships between input and output modules.  
The individual behaviour scripts for input modules can generate bools, floats, vector2s, vector3s: depending on the ControlInputValueOption selected (Button, OneAxis, TwoAxis, ThreeAxis). This value is then automatically fit to a Vector3 which is passed to the `ShipInputComponent` class.
`ShipModuleManager` then checks in a list of `InputOutputConnector`s and gathers data from the inputs, splits it, and sends it to each output.

