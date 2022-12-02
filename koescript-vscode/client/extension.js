const vscode = require('vscode');
const commonFunctions = require("./common-functions");

const outputchannel = vscode.window.createOutputChannel('KoeScript');

const signatures = [];
const autocomplete = [];

for (let name in commonFunctions) {
    signatures.push(
        {
            label: commonFunctions[name]["usage"],
            documentation: new vscode.MarkdownString(commonFunctions[name]["desc-en"], true),
            parameters: []
        }
    );
    autocomplete.push(new vscode.CompletionItem(`@${name}`, vscode.CompletionItemKind.Method));
} 

vscode.languages.registerHoverProvider('koescript', {
    provideHover(document, position, token) {
        const wordRange = document.getWordRangeAtPosition(position);
        const wordRangeAfter = document.getWordRangeAtPosition(position.with(position.line, position.start + 1));
        const word = document.getText(wordRange);
        const wordAfter = document.getText(wordRangeAfter);

        // Hovering a function
        if (word.startsWith("@")) {
            const hoveredFunction = commonFunctions[word.substring(1)];

            const usage = new vscode.MarkdownString(`$(symbol-function) ${hoveredFunction['usage']}`, true);
            usage.isTrusted = true;
            const documentation = new vscode.MarkdownString(hoveredFunction["desc-en"], true);
            documentation.isTrusted = true;
            
            return {
                contents: [
                    usage,
                    documentation
                ]
            };
        }
    }
});

vscode.languages.registerSignatureHelpProvider('koescript', {
    provideSignatureHelp(document, position, token, context) {
        const wordRange = document.getWordRangeAtPosition(position);
        const word = document.getText(wordRange);

        const autocompleteWord = word.substring(1);

        const matchFunction = commonFunctions[autocompleteWord];
        if (matchFunction != undefined) {
            return {
                activeSignature: 0,
                activeParameter: 0,
                signatures: [
                    {
                        label: matchFunction["usage"],
                        documentation: new vscode.MarkdownString(matchFunction["desc-en"], true),
                        parameters: [
                            
                        ]
                    }
                ]
            } 
        } else if (word.startsWith("@")) {
            return {
                activeSignature: 0,
                activeParameter: 0,
                signatures: [
                    {
                        label: " ",
                        documentation: " ",
                        parameters: []
                    }
                ]
            };
        }
    }
}, '@', ' ');


vscode.languages.registerCompletionItemProvider('koescript', {
    provideCompletionItems(document, position) {
        const linePrefix = document.lineAt(position).text.substring(0, position.character);

        if (linePrefix.startsWith("@")) {
            return autocomplete;
        }
    }
}, ['@']);
