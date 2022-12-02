# KoeScript-CS

This is the official and original C# implementation of the KoeScript language.

It is built upon .NET Core 2.1, which is the cross-platform framework from Microsoft.

The command line interface (CLI) is a C# script that can be run by [dotnet script](https://github.com/filipw/dotnet-script).

## Using the CLI

To use the KoeScript REPL:

    dotnet script koecli.csx repl

To read a whole dialogue file:

    dotnet script koecli.csx readfile dialogue.koe

To run an analysis of a KoeScript dialogue file:

    dotnet script koecli.csx analysis dialogue.koe

## Building the project

To build the library:

    dotnet build --configuration Release

One-liners for both compilation and running without any cache:

    dotnet build --configuration Release && dotnet script koecli.csx --no-cache repl
    dotnet build --configuration Release && dotnet script koecli.csx --no-cache readfile disaster.koe
