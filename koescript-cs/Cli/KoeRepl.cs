
using System;
using System.Linq;
using System.Text;

namespace KoeScript {
    public class KoeRepl {
        public void Start(KoeInterpreter interpreter) {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("┌── KoeScript REPL ───");
            Console.WriteLine("│Type \"exit\" or \"q\" to quit.");
            Console.ResetColor();
            Console.InputEncoding = Encoding.Unicode;

            string[] quitters = {"exit", "q"};
            String input;

            while (true) {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Magenta;
                Console.Write("声");
                Console.ResetColor();
                Console.Write(" ");
                input = Console.ReadLine();
                if (quitters.Contains(input.ToLower())) {
                    break;
                }
                if (input.Trim().Length == 0) {
                    continue;
                }

                KoeToken token = interpreter.ParseLine(input);
                object result = null;
                if (token != null) {
                    result = interpreter.ExecuteToken(token);
                }
                if (result != null) {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"-> {result}\n");
                    Console.ResetColor();
                }
            }
        }
    }
}
