
# KoeScript v1.0.0 - Implementation Guide

This document aims to explain in the most simple (but precise) terms, how the Language Specification has been represented into source code for the original C# implementation. Of course, you're still free to implement the language as you'd like, but please follow this guide when working on official projects.

## Tokens

Except for anything that could be ignored by an Interpreter or a Transpiler (like comments and empty lines), ANY line that have an effect SHOULD be named a **token** inside the source code.

Any token has a **type**, which can be :

* Message
* Function
* Label

Any token will always store anything needed to keep the content and the context of the text line it represents : a message text and its character, the name and the arguments of a function that's been called, etc.

Any token that is created from a text line of a dialogue file SHOULD store its **position** in this file, even if the token represents a non-blocking event. Take note that its position is NOT and SHOULD NOT be its effective line number on the file, it is its appearance number. If a token is the 5th one inside a file, then its position should be "5". That position may then be used by a game or an application for many features, like save or load.

A token CAN store others tokens into its **queue**. It is a convenient way to stack multiple tokens that follow each other into a single token. The main purpose of using this queue, is to batch execute multiple tokens right before a blocking event (generally, a message). It is also useful for answer messages to be shown at the same time as its preceding message.

## Messages

A message is, unless an eventual "auto-reading mode" or "fast-forward mode" is enabled, a "blocking" event that awaits any input from the player in order to advance further into the dialogue.

Functions may be kept in the queue of a message token, to be batch executed right before the message appears on-screen.

Side messages does not requires any player input and MAY be show in the same time or with a small delay.

Answer message are shown at the end of the previous message, before waiting for a player input.

## Function

A function token means to call a function that has been registered to the interpreter beforehand.

It may contains some **positional arguments** or **named arguments** to pass to the called functions.

## Parsing KoeScript into tokens

A KoeScript dialogue file COULD be read line-by-line and executed at the same time, but by doing so some features WON'T work such as answer messages or eventual future language features (control blocks...). That's why it is recommended to parse a dialogue file all at once, so that some lines are aware of previous or follow lines when they're represented as a "token".

Here's what you could do while you parse a whole file into tokens for the first time, to get a list of tokens as a result :

* If the current token is an **answer message**, you COULD insert it on the queue of the previous token, if any
* If the current token is a **message** and it doesn't have any character, you COULD copy the "current speaking character" if it is defined
* If the current token is a **message** and neither a **side** nor an **answer** message, and if it has a character, define it as the "current speaking character" for future use by the next tokens
* If the current token is a **label**, store it as well in a list of the current dialogue labels to facilitate further references to it

# Executing KoeScript tokens into a "visual novel" game environment

Visual novels have a few features that are unique to them, and of course KoeScript was conceived while thinking about them.

Executing properly a list of tokens can still be a challenge depending on the wanted final features, so the following is an exhaustive example about how it could be done no matter the target language.

The main "gameplay loop", which consists of reading a dialogue by letting the player advance at its pace by waiting for its input at the end of each message, could be done on a single **for loop** on the tokens list, from the first to the last, where you could **pause** the execution when you want (eg. using coroutines awaits).

To match the most common features of a visual novel, we would need the following things to being with :

* A boolean to represent a **fast-forward mode**. It's used to display extremely quickly messages and events, without needing player interaction.
* A boolean to represent an **auto-reading mode**. It automatically skips a message to the next one after a few seconds to let the player read it, while then offering a hands-off experience.
* A boolean to represent a **silent execution mode**. It means that the function called by tokens will immediately apply their side effect, without any animation or transition on screen.

One important thing to think about beforehand, is the fact that your game could start midway into a dialogue file. It makes sense if you're debugging your game, if a player is loading its progression from a savestate, etc. Here as what's needed to make that possible :

* The **start token position** which is an integer, to know from which token we would like to start inside a dialogue.
* A **virtual queue** that is a list of function calls tokens to be executed at once, when we reach the start token position.
* A **virtual stage** of characters that retains which characters should be shown on screen, and with which clothes, face expression, etc.

Simply put, the **virtual queue** is here to keep aside every function call that can be useful (setting the current time or background image, putting background music...) but some of these function calls could be erased if we encounter `@clear` function calls, for example.

For its part, the **virtual stage** follows every time a character sprite is displayed, updated or removed on screen. As such, we'll have in the end the exact characters that should be displayed on screen.

The first behavior to do to deal with that would be to begin the previously mentioned **for loop**, but to do nothing until we've reached this **start token position**. But instead of doing nothing, there's actually a lot that can be done until we reach the targeted position.

For each token we're looping on, you can do as is :

* If the current token is a **message** and not a **side** message, lookup if it contains a character state change and edit the corresponding character inside the virtual stage, if any
* If the current token is a **message** and neither a **side** nor an **answer** message, and if it has a character, define it as the "current speaking character" for future use by the next tokens
* If the current token is a `@cs` **function** call, add the corresponding character to the virtual stage or update its data if it's already on the virtual stage
* If the current token is a `@hide` **function** call, remove the corresponding character from the virtual stage.
* If the current token is a `@hideall` **function** call, remove every character from the virtual stage.
* If the current token is a `@clear` **function** call, remove the existing relevant tokens from the virtual queue. As such, a `@clear bgm` will need you to clear every past `@bgm` function call from the virtual queue, etc.
* If the current token is a **function** call that doesn't have a long-lasting side effect, for example `@chapter_start` or `@sfx`, don't keep them in the virtual queue.
* If the current token is then a **function** call with a long-lasting side effect, insert it in the virtual queue.

When you've finally reached the start token position, you can execute every function call of the **virtual queue** and display every character accordingly to the **virtual stage**.
