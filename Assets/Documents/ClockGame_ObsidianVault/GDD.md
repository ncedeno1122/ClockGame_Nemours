# Title TBD - I stink at naming sometimes
# Concept
A 2D *Puzzle-Platformer* (possibly using the Corgi Engine) with a focus on solving platforming puzzles with pieces of an antique clock that can be found throughout each level. Upon starting each level, you will "lose" them - you'll platform through each level to find them, and **you cannot complete the level without each one**. Also throughout each level are some enemies who try to impede the player's progress, but can be defeated with some of the antique clock abilities found throughout the levels.
# Aesthetic / Themes
The levels may take place in clock-based levels, like grandfather clocks, modern clocks, and perhaps even digital clocks. With sprited assets, there will need to be certain badges indicating the player can use a certain ability on a certain element.
# Mechanics
Different clock parts augment the player's ability to interact with certain elements in each level. There will be 4:
## Pendulum
The clock's Pendulum ability allows you to "Tick" certain elements, or **slightly update** properties that respond to Ticking. The input for this is a simple Interact-button press. For example, perhaps a certain set of blocks retract and protrude alternatingly from a wall, forming a gap. The player can jump from one platform, use the Tick ability to pull out the next block, and land on it. They can repeat and progress.
## Hands
The clock's Hands ability allows the player to rotate certain objects throughout a level. The input for this is holding the Interact-button and inputting a left-right direction to adjust one way or the other. For example, a set of gears that can form a platform at a certain rotation stand before the player. The player can rotate them into proper rotation and then cross the obstacle by using the Hands ability. As well, perhaps a floating platform only moves via the Hands ability. You can jump on and hitch a ride with your fancy new Hands ability, with it taking you where you want to go.
## Chime
The clock's Chime ability can stun enemies, allowing you to get the upper hand and jump on them. It can also shatter glass panels that may block the player's way.
Chime works by SphereCasting and triggering any ChimeObserver scripts. 
## Cuckoo
The last ability is Cuckoo; the primary way of damaging enemies and pressing buttons throughout the level. The player uses a small, violent, and hand-carved bird to launch in front of them for a moment - it is essentially a punch attack.