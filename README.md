# Disco-Lights
In this project, I made the lights(cells in the grid)  in such a way that the light checks the neighboring light's state and changes itself accordingly.
Made 2 enums for defining the direction of the diagonal (left, right, both, none) and the current state (alive, selected, reachable, or dead) of the Light.
Made a Grid Manager script that deals with the varying size of the grid as per the number of lights (cells) in the grid, along with this it instantiates the number of lights(cells) by using a prefab of type Button which has a "LightsController" Script attached to it along with 4 colliders, 2 of type "BoxCollider2D" along the left diagonal and 2 of type "CircleCollider2D" along the right diagonal.
If the Adjacent light along the left diagonal is either "Reachable" or "Selected" via the BoxColliders then we'll set the current state of the Light as "Reachable" and diagonal as "Left"
else if the adjacent light along the right diagonal is either "Reachable" or "Selected" via the CircleColliders then we'll set the current state of the Light as "Reachable" and diagonal as "Right"
and then then it Starts a Coroutine which enables us to wait for 2 seconds before it kills the "Reachable" or "Selected" lights and it'll call an event to disable the interactability of all the lights(which are of type Buttons).
and this goes on until all the lights turn red (or gets their state as Dead)
and the entire scene reloads
