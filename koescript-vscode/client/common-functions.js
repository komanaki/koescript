const list = {
    // Flow
    "wait": {
        "usage": `@wait (float)duration`,
        "desc-en": `Forcibly waits for a certain duration before executing the next line.`
    },
    "start_auto": {
        "usage": `@start_auto`,
        "desc-en": `Force the auto-play mode. May be useful for dramatic dialogues, but use with moderation.`
    },
    "stop_auto": {
        "usage": `@stop_auto`,
        "desc-en": `Stops the auto-play mode. May be useful right before an important choice, or when you want to make sure that the player reads the next message. Use also with moderation.`
    },
    "warning": {
        "usage": `@warning (string)name (string)shortcut=""`,
        "desc-en": `Displays a content warning to the player about the upcoming dialogue lines. It may also (and that's actually recommended) give the choice for the player skip the upcoming dialogue and resume from a safer point ahead in the dialogue thanks to a label.`
    },
    "goto": {
        "usage": `@goto (string)label`,
        "desc-en": `Continue the execution from a label forwards in the current dialogue.`
    },
    "dialogue": {
        "usage": `@dialogue (string)name`,
        "desc-en": `Quits the current dialogue and tell the game to load another dialogue.`
    },
    "signal": {
        "usage": `@signal (string)name`,
        "desc-en": `Trigger a certain event that can makes the game react in a certain way.`
    },
    "chapter_start": {
        "usage": `@chapter_start (string)id`,
        "desc-en": `Marks a new chapter from this point, usually used at the start of a dialogue. Chapter ID is usually a number, but making it a string lets you use letter for special chapters or routes, for example.`
    },
    "chapter_end": {
        "usage": `@chapter_end (string)id`,
        "desc-en": `Ends a game chapter.`
    },
    "ending": {
        "usage": `@ending (string)name`,
        "desc-en": `Directly goes to a certain game ending.`
    },
    "display": {
        "usage": `@display (string)mode`,
        "desc-en": `Changes the display mode of the dialogue. A display with characters sprites and a message box is usually called "adventure" or "ADV", while a full-screen message box is called "visual novel" or "NVL".`
    },
    "bg": {
        "usage": `@bg (string)name (boolean)fade=false (string)translate=null (string)color=null`,
        "desc-en": `Changes the background of the game scene, eventually with a fade and/or an animation translation.`
    },
    "vfx": {
        "usage": `@vfx (string)name`,
        "desc-en": `Apply a visual effect to the background and/or the camera. Here are some examples of VFX than you can implement in your game:

        * "darken" : Makes the background image way more darker, for example to emphasize the inner monologue of a character.
        * "invert" : Inverts the colors of the background image, maybe in a fashionated way.
        * "flash" : Makes the background image briefly flash in white. For obvious accessibility purposes, it may be good to let players soften or disable this effect.
        * "shake" : Shakes the background image. For obvious accessibility purposes, it may be good to let players soften or disable this effect.`
    },
    "closeup": {
        "usage": `@closeup (string)name (string)description=null`,
        "desc-en": `Displays something, like an object or a picture, in a close-up box over the message box. Useful to put a visual emphasis on something.`
    },
    "clear": {
        "usage": `@clear (boolean)bg=false (boolean)bgm=false (boolean)sfx=false (boolean)closeup=false`,
        "desc-en": `Clear one or multiple things at the same time : background image, background music, sound effect...`
    },
    // Metadata
    "place": {
        "usage": `@place (string)place (string)city=""`,
        "desc-en": `Sets the current place of the dialogue. May also set a bigger place, like a city.`
    },
    "date": {
        "usage": `@date (string)date`,
        "desc-en": `Sets the current date of the dialogue. Can be written in any order that you'd want, like "8/12" for 12th August or "25/12" for December 25th.`
    },
    "time": {
        "usage": `@time (string)time`,
        "desc-en": `Sets the approximate time of the dialogue, like "morning" or "afternoon".`
    },
    "hour": {
        "usage": `@hour (string)hour`,
        "desc-en": `Sets the current precise hour of the dialogue.`
    },
    // Audio
    "bgm": {
        "usage": `@bgm (string)name (boolean)additive=false (float)volume=1`,
        "desc-en": `Sets the current background music. May be added to the currently playing background music when used with the **additive** argument, if the game sound engine supports it.`
    },
    "amb": {
        "usage": `@amb (string)name`,
        "desc-en": `Sets the current ambiance track. Useful when you want to use both a music and an ambiance for the place where the dialogue takes place.`
    },
    "sfx": {
        "usage": `@sfx (string)name (boolean)blocking=false (boolean)loop=false (string)visual="" (string)cue=""`,
        "desc-en": `Plays a sound effect once, or in a loop. May wait for the end of the sound before executing the next line when used with the **blocking** argument. May be coupled with a visual representation of the sound with the **visual** argument. The **cue** argument may be a description of the sound for accessibility purposes.`
    },
    "cv": {
        "usage": `@cv (string)clip`,
        "desc-en": `Plays a character voice clip.`
    },
    // Characters
    "cs": {
        "usage": `@cs (string)character (string)clothes (string)state (string)position`,
        "desc-en": `Sets a character sprite, making it appear if it isn't already shown. I can also set a specific **clothes** set, a specific **state** (eg. face expression), or a **position** on screen.`
    },
    "cf": {
        "usage": `@cf (string)character (string)state`,
        "desc-en": `Sets a character face, usually inside the message box.`
    },
    "move": {
        "usage": `@move (string)character (string)position`,
        "desc-en": `Moves a character sprite to a certain position on screen.`
    },
    "animate": {
        "usage": `@animate (string)character (string)effect`,
        "desc-en": `Animate a character sprite with a certain effect : shake it, make it dodge, make it fall or fly, etc.`
    },
    "hide": {
        "usage": `@hide (string)character (boolean)fade=false`,
        "desc-en": `Disappears a character sprite from the scene, with or without a fade effect.`
    },
    "hideall": {
        "usage": `@hideall`,
        "desc-en": `Disappears all characters on screen.`
    },
};

module.exports = list;
