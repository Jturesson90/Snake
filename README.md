# Retro Snake

A Snake game made with unity.

## Play it here!

[Snake hosted at Unity Play](https://play.unity.com/mg/other/snake-3tl)

## Tech

### Architectural goals

The first goal was to have Game Manager and the board to be completely separated. We can have multiple boards listening to game changes from the Game Manager.

### Snake Logic Custom Package

- No Unity Engine Reference
- Tests
- Keeps all the logic safe from outside
- Outside will be responsible to call Update(Direction) and SpawnFood

### Game Manager

- Has a scriptable object reference with game setup data
- Creates an instance of Snake Logic
- Keeps Game State
- Listening on GameInput
- Responsible for calling SpawnFood and Update on Snake Logic instance

### Sprite Game Board

- Has a reference to Game Manager

### UI

- Listening on Game State changes
- Listening on Score changes

### Sound Manager

- Listening on Game events
- Has a scriptable object reference to audio clips

### Final thoughts

- Worm body should have some sort of direction
