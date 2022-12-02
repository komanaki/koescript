#!/usr/bin/env dotnet-script
#r "bin/Release/netcoreapp2.1/koescript-cs.dll"

using System;
using KoeScript;

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("┌──────────────────┐");
Console.WriteLine("│ KoeScript C# CLI │");
Console.WriteLine("└──────────────────┘");
Console.ResetColor();

if (Args.Count > 0 && Args[0].ToLower() == "repl") {
    var interpreter = new KoeInterpreter();
    var repl = new KoeRepl();
    repl.Start(interpreter);

} else if (Args.Count > 0 && Args[0].ToLower() == "readfile") {
    var interpreter = new KoeInterpreter();
    interpreter.ReadFile(Args[1]);

} else if (Args.Count > 0 && Args[0].ToLower() == "analysis") {
    Console.WriteLine("Performing a deep analysis of the given file...\n");
    var interpreter = new KoeInterpreter();
    interpreter.SetLogLevel(KoeInterpreter.LogLevel.None);
    List<KoeToken> tokens = interpreter.ReadFileTokens(Args[1]);
    char[] wordDelimiters = new char[] { ' ', ',', ';', '.', '!', '"', '(', ')', '?', ':' };

    // stats
    int tokensCount = tokens.Count;
    int messageCount = 0;
    int messageCharCount = 0;
    int messageWordsCount = 0;
    Dictionary<string, int> bgmUsage = new Dictionary<string, int>();

    foreach (KoeToken token in tokens) {
        if (token.type == KoeToken.Type.Message) {
            messageCount += 1;
            messageCharCount += token.messageText.Trim().Length;
            messageWordsCount += token.messageText.Trim().Split(wordDelimiters, StringSplitOptions.RemoveEmptyEntries).Length;  
        }
        if (token.type == KoeToken.Type.Function) {
            if (token.functionName == "bgm" && token.functionArgs.Count > 0) {
                string bgmFile = token.functionArgs[0] as String;
                if (!bgmUsage.ContainsKey(bgmFile)) {
                    bgmUsage[bgmFile] = 0;
                }
                bgmUsage[bgmFile] += 1;
            }
        }
    }

    Console.WriteLine($"┌── GENERAL ────");
    Console.WriteLine($"│-> There is {tokensCount} tokens");
    Console.WriteLine($"\n┌── MESSAGES ───");
    Console.WriteLine($"│-> There is {messageCount} message lines");
    Console.WriteLine($"│-> Message characters count : {messageCharCount}");
    Console.WriteLine($"│-> Message words count (whitespace-based) : {messageWordsCount}");
    Console.WriteLine($"\n┌── USAGE ───");
    if (bgmUsage.Count > 0) {
        Console.WriteLine($"│-> BGM used :");
        foreach (KeyValuePair<string, int> kv in bgmUsage.OrderByDescending(x => x.Value)) {
            Console.WriteLine($"│->    {kv.Value}x  {kv.Key}");
        }
    }



} else {
    Console.WriteLine("CLI Usage");
    Console.WriteLine("    repl                  : Launches the KoeScript REPL.");
    Console.WriteLine("    readfile [filename]   : Interprets a KoeScript file and outputs the result of each token.");
    Console.WriteLine("    analysis [filename]   : Interprets a KoeScript file and display a usage analysis.");
}
