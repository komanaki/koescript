using System;

#if (UNITY_EDITOR || UNITY_STANDALONE)
using UnityEngine;
#endif

namespace KoeScript {
    /// <summary>
    /// A custom log class as we may use and debug the KoeScript Interpreter inside
    /// games engines in addition to a classic command line
    /// </summary>
    public static class KoeLog {
        public enum LogLevel { None, Error, Info } ;
        /// <summary>
        /// The current log level. By default, we don't want any log
        /// </summary>
        public static LogLevel Level = LogLevel.None;
        public static void Info(object message) {
            if (Level < LogLevel.Info) {
                return;
            }
            message = "KoeScript | " + message;
            #if (UNITY_EDITOR || UNITY_STANDALONE)
            Debug.Log(message);
            #else
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ResetColor();
            #endif
        }

        public static void Error(object message) {
            if (Level < LogLevel.Error) {
                return;
            }
            message = "KoeScript | " + message;
            #if (UNITY_EDITOR || UNITY_STANDALONE)
            Debug.LogError(message);
            #else
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(message);
            Console.ResetColor();
            #endif
        }
    }
}
