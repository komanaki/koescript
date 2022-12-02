
# KoeScript v1.0.0 - Common Tags

To have rich-text and other useful features inside messages, the use of "tags" will let the KoeScript parser understand them and transform them if needed before display.

Many tags are actually inspired by the Markdown syntax.

## Bold

Surround the part of a sentence with `**` to display it in bold.

    The following **word** is in bold.

## Italic

Surround the part of a sentence with `_` to display it in italic.

    The following _word_ is in italic.

## Underline

Surround the part of a sentence with `__` to display it as underlined.

    The following __word__ is underlined.

## Strikethrough

Surround the part of a sentence with `~~` to display it as striked through.

    The following ~~word~~ is striked.

## Links

Surround the part of a sentence with `[]` immediately followed by `(#xxxx)` where `xxxx` is your link.

    The following [word](#test) is actually a link.

## String interpolation

Inside messages, you can write the value of a context variable by writing its name inside the `{}` characters.

    So your name is {hero_name}, that's right ?

## Ruby tags

To display phonetic indications (furigana) over kanji characters, surround the kanji(s) with `[]` and immediately follow it with `()` containing its phonetic writing.

    ここは[日本](にほん)です。
