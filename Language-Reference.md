
# KoeScript v1.0.0 - Language Reference

## Syntax

The main purpose of the language being the writing of dialogues, its syntax is kept as its bare minimum, and a line is a speech message unless proven otherwise. Therefore, each line have a "type". Here are all of them :

* **Message**
* **Comment**
* **Function** call
* **Variable** statement
* **Label**

When written in a file, it SHOULD be encoded in UTF-8.

_For developers_ : In interpreters, the abstract representation of a parsed line is called a "token". A token MUST represent a single statement, which usually means a single line of text.

### Special characters

They're used in the language either to specify the type of a line, or to add extra information to a "message". Here are some of them :

* `#` : Writing a comment
* `@` : Calling a function
* `$` : Using a context variable
* `~` : Defining a message as "side"
* `>` : Defining a message as "answer"
* `*` : Placing a label

### Reserved keywords

* `exit` : Reserved for quitting a KoeScript REPL. Has no effect when read by an interpreter.
* `nobody` : Can be used as a dummy character name in a message, equivalent of "null".

## Values types

* String : a text value, delimited by a single `'` or double `"` quote
* Integer : a whole number
* Float : a decimal, always delimited by a dot `.`
* Boolean : `true` or `false`
* Null : `null`

For simplicity purposes, you can't write escaped quotes (like `\"` or `\'`) inside a string value.

## Naming conventions

Any function, keyword or variable name SHOULD be only written using alphabetical characters and the underscore `_`. As a Regex characters group, it means `[A-Za-z_]`.

Labels IDs can also contain numerals. As a Regex characters group, it means `[A-Za-z0-9_]`.

## Comment

All comments are single-line only, and they always start with a `#`.

    # This is a comment.

They should be written on their own line, meaning you can't add a comment at the end of another line (a message, a function call...).

## Message

Any new line that isn't either a comment, a function call, a variable, etc... is considered as a message. Just type anything you want !

    This is a simple text message.

For implementation developers : Be careful as a message MAY contains quotes.

    "I hate it", he said.

If you want to indicate the character that will speaks the message, write the character ID followed by a `:` before writing your message. Any whitespace around the `:` will be removed so don't worry about it. A character ID should be made of letters, numbers and `_` only.

    tanaka : This is a message spoken by a Tanaka.

When a character speaks a message, all the following messages will be spoken by the same character, unless you indicate a different character at the start of a message line.

Please note that characters of side messages or answer messages won't follow to the next lines.

    tanaka : This message is defined as spoken by Tanaka.

    No character is indicated at the start of this line, so it will be spoken by Tanaka too.

    nakamura : But this message will me spoken by Nakamura.

If you'd like to show a message that isn't spoken by any character, you can use the special character "nobody".

    nakamura : I'm saying something as Nakamura

    nobody : This is spoken by a narrator or someone like this.

You can also change the "state" attribute of a character by adding an `@` mark after the character name. Thus, this line :

    nakamura@sad : I'm speaking while having a state that change the character sprite.

Is equivalent to the following lines :

    @cs "nakamura" state="sad"
    nakamura : I'm speaking but my state is set by the previous line.

### Side message

A message can be defined as a "side" one by adding a `~` character at the beginning of the line.

_Recommended implementation_ : A side message is displayed as a small aside message on the screen, like a hubbub or a murmur. The usual message dialog MAY be hidden when they're displayed. Many side messages MAY be displayed at once, and the last one MAY wait for player input to continue.

Here is an example of two side messages.

    ~ Have you the last game?
    ~ It was so cool!

### Answer message

A message can be defined as a "answer" one by adding a `>` character at the beginning of the line.

The result may depend on the game that will read the script, but a side message is usually displayed when the next message is shown.

    I love this food.
    > Really???

## Function call

A function is called by prefixing its name with a `@`. The function name may contain underscores in their name.

    @hideall

If the function accept them, positional arguments can be written right afterwards.

    @sfx "crash.wav"

If the function accept them, named arguments can be written after the positional arguments (if any). The argument name and the value are separated by an `=`.

    @bg "school" transition="up"

When a named argument accepts a boolean value, omitting its value is equal to writing `true`. Therefore, the following two lines are purely equal.

    @bg "classroom" fade
    @bg "classroom" fade=true

When the values are string, both positional and named arguments CAN be written using single `'` or double `"` quotes.

    @bg 'cafeteria'
    @bg "cafeteria"

You can use a value from the context variables as an argument of a function call, by preceding their name with a `$` char. It also works with named arguments.

    @sfx $player_clickeffect
    @sfx name=$player_clickeffect

## Labels

A label marks a position in a dialogue thanks to a short identifier. They're quite useful to make jumps inside a dialogue, or for keeping trace of the current scene, which can be useful il you want to have this information while creating a savefile or displaying a flowchart.

    *schoolfestival

You can also add a short description of the label next to it, which may be useful for savestates.

    *classenter "When entering the classroom"
