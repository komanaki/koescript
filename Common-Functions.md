
# KoeScript v1.0.0 - Common Functions

Although KoeScript is purely a scripting language, an optimal usage in video games (and furthermore in visual novels) implies the presence of functions that let us control what's happening along the dialogue.

Even if the side-effects of these functions are up to the games using KoeScript, it's better to follow a common set of functions definition to ensure a comprehensible usage of the language across many different projects.

When implementing the KoeScript language inside your game, keep track of the "common functions" that you've implemented, along your custom functions if you've made any.

## 🔁 Flow control

### @wait

    @wait (float)duration

Forcibly waits for a certain duration before executing the next line.

### @start_auto

    @start_auto

Force the auto-play mode. May be useful for dramatic dialogues, but please make it optional as an accessibility setting.

### @stop_auto

    @stop_auto

Stops the auto-play mode. May be useful right before an important choice, or when you want to make sure that the player reads the next message. Please use with moderation.

### @warning

    @warning (string)name (string)shortcut=""

Displays a content warning to the player about the upcoming dialogue lines. It may also (and that's actually recommended) give the choice for the player skip the upcoming dialogue and resume from a safer point ahead in the dialogue, thanks to a label.

### @goto

    @goto (string)label

Continue the execution from a label forwards in the current dialogue.

### @dialogue

    @dialogue (string)name

Quits the current dialogue and tell the game to load another dialogue.

### @signal

    @signal (string)name

Trigger a certain event that can makes the game react in a certain way.

### @chapter_start

    @chapter_start (string)id

Marks a new chapter from this point, usually used at the start of a dialogue. Chapter ID is usually a number, but making it a string lets you use letter for special chapters or routes, for example.

### @chapter_end

    @chapter_end (string)id

Ends a game chapter.

### @ending

    @ending (string)name

Directly goes to a certain game ending.

## 🖥️ Display

### @display

    @display (string)mode

Changes the display mode of the dialogue. A display with characters sprites and a message box is usually called "adventure" or "ADV", while a full-screen message box is called "visual novel" or "NVL".

### @bg

    @bg (string)name (boolean)fade=false (string)translate=null (string)color=null

Changes the background of the game scene, eventually with a fade and/or an animation translation.

### @vfx

    @vfx (string)name

Apply a visual effect to the background and/or the camera. Here are some examples of VFX than you CAN implement in your game:

    * "darken" : Makes the background image way more darker, for example to emphasize the inner monologue of a character.
    * "invert" : Inverts the colors of the background image, maybe in a fashioned way.
    * "flash" : Makes the background image briefly flash in white. For obvious accessibility purposes, it may be good to let players soften or disable this effect.
    * "shake" : Shakes the background image. For obvious accessibility purposes, it may be good to let players soften or disable this effect.

### @closeup

    @closeup (string)name (string)description=null

Displays something, like an object or a picture, in a close-up box over the message box. Useful to put a visual emphasis on something.

### @clear

    @clear (boolean)bg=false (boolean)bgm=false (boolean)sfx=false (boolean)closeup=false

Clear one or multiple things at the same time : background image, background music, sound effect...

## 💾 Metadata

### @place

    @place (string)place (string)city=""

Sets the current place of the dialogue. May also set a bigger place, like a city.

### @date

    @date (string)date

Sets the current date of the dialogue. Can be written in any order that you'd want, like "8/12" for 12th August or "25/12" for December 25th.

### @time

    @time (string)time

Sets the approximate time of the dialogue, like "morning" or "afternoon".

### @hour

    @hour (string)hour

Sets the current precise hour of the dialogue.

## 🔊 Audio

### @bgm

    @bgm (string)name (boolean)additive=false (float)volume=1

Sets the current background music. May be added to the currently playing background music when used with the **additive** argument, if the game sound engine supports it.

### @sfx

    @sfx (string)name (boolean)blocking=false (boolean)loop=false (string)visual="" (string)cue=""

Plays a sound effect once, or in a loop. May wait for the end of the sound before executing the next line when used with the **blocking** argument. May be coupled with a visual representation of the sound with the **visual** argument. The **cue** argument may be a proper description of the sound for accessibility purposes.

### @cv

    @cv (string)clip

Plays a character voice clip.

## 😀 Characters

### @cs

    @cs (string)character (string)clothes (string)state (string)position

Sets a character sprite, making it appear if it isn't already shown. I can also set a specific **clothes** set, a specific **state** (eg. face expression), or a **position** on screen.

### @cf

    @cf (string)character (string)state

Sets a character face, usually inside the message box.

### @move

    @move (string)character (string)position

Moves a character sprite to a certain position on screen.

### @animate

    @animate (string)character (string)effect

Animate a character sprite with a certain effect : shake it, make it dodge, make it fall or fly, etc.

### @hide

    @hide (string)character (boolean)fade=false

Disappears a character sprite from the scene, with or without a fade effect.

### @hideall

    @hideall

Disappears all characters on screen.
