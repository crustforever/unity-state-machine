# Enumerated State Machine

Unity 2017.3.1f1

4/22/2018, Jack McCarron, jack.g.mccarron@gmail.com

## What's included

* My code
    
    * *EnumeratedStateMachine* - A generic state machine that I built for Caravan. The goal is to enforce a separation of machine-wide shared data and local state-specific data. The context object acts as an API for the states to interact with shared data (e.g. the RUN state object and the JUMP state object both have access to the player object's position, but the airborne velocity vector is owned by the JUMP state object). This separation has the added benefit of making the management of data serialization really simple. The context can be anything, but in practice I use an interface with functions to Serialize and Deserialize the object. All of the "save data" in Caravan lives in these state machine context objects. That way I never have to worry about what to save, just when to save it. Since Caravan is a roguelike with permanent consequences there is a lot of saving going on; some portion of the game state is serialized after every player action that mutates it. I found this state machine architecture is very nice for my game, but its wholly generic and can be used for pretty much anything.
    
    * *EnumeratedState* - The states of the machine are object instances of a class that inherits from EnumeratedState. When the machine is instantiated they are defined by and mapped to the enum type parameter and can be referenced with that enum (e.g. MyCharacterState.WALK, MyCharacterState.JUMP, MyCharacterState.ATTACK).
    
    * *MecanimStateMachine* - Since this came up during the interview I thought it would be fun to see if I find a simple way to couple my state machine to an animator controller for mecanim.

    * *EZStateMAchine* - This is the companion editor script that adds a menu item Window -> EZ State Machine. It has a button to generate an animator controller and animation clip stubs given the list of state enums in your MecanimStateMachine. The Mouse game object uses an animator controller built with this script that receives state change triggers from its MecanimStateMachine.

    * *Mouse* - The Monobehavior script that owns the MecanimStateMachine that drives the mouse's behavior.

    * *MouseContext* - The context object for the MecanimStateMachine. Has references to all the data that defines the mouse that the states access to.

    * *Follow* - The state that controls the behavior of the mouse while FOLLOWing and whether or not to change state to EAT.

    * *Eat* - The state that controls the behavior of the mouse while EATing and whether or not to change state to FOLLOW.

    * *Cheese* - Positions the cheese on top of the cursor.

    * *MouseTracker* - A script that tracks how many cheeses the mouse has eaten and what state the mouse is currently in. It serves as an example of how to react to state changes from outside of the EnumeratedStateMachine by subscribing to its generic state change event or to a specific state transition event.

* Demo Scene - the playable scene where you can see the state machine in action.

* Dependencies (not my code!)
    
    * PixelPerfectCamera - a nice utility script that automatically sizes an orthographic camera to the viewport dimensions and scales up base resolution when possible. See the README for author details.
    
    * Unitilities Tuples - lightweight generic implementation of tuples with comparison operator. I believe .Net 4 has tuples but the current Unity Mono version does not. See file header for author details.
