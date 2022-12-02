using System;
using System.Reflection;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace KoeScript {

    /// <summary>
    /// The interpreter represents raw text written in the KoeScript language into one or more code-friendly "tokens".
    /// It may also execute functions that are beforehand registered as reachable by a KoeScript dialogue.
    /// </summary>
    public class KoeInterpreter {
        /// <summary>
        /// The definitions that will be used by the interpreter when parsing a text line (or a line part)
        /// </summary>
        private List<KoeDefinition> _definitions = new List<KoeDefinition>();
        /// <summary>
        /// The registered functions that can be called from within a KoeScript dialogue
        /// </summary>
        private Dictionary<string, KoeFunction> _registeredFunctions = new Dictionary<string, KoeFunction>();
        /// <summary>
        /// The context holds variables which values can be used inside a dialogue.
        /// They can be used inside messages text content thanks to string interpolation,
        /// and as arguments values when calling a function.
        /// </summary>
        private Dictionary<string, object> _context = new Dictionary<string, object>();

        public KoeInterpreter() {
            // The language definitions are added in a certain order, so that the tokens are
            // gradually filled with all the data of the text line to which they correspond

            // Label
            _definitions.Add(new KoeDefinition("^\\*(?<id>[A-Za-z0-9_]+)[ ]?", (token, groups) => {
                token.type = KoeToken.Type.Label;
                token.labelId = groups["id"].Value;
                KoeLog.Info($"---- Label : {token.labelId}");
            }, KoeToken.Type.Undefined));

            // Description of a label
            _definitions.Add(new KoeDefinition("(?:\"(?<doublestr>[^\"]+)\"|\'(?<singlestr>[^\']+)\')", (token, groups) => {
                token.labelDescription = (groups["singlestr"].Value.Length > 0) ? groups["singlestr"].Value : groups["doublestr"].Value;
                KoeLog.Info($"---- Label Desc : {token.labelId} {token.labelDescription}");
            }, KoeToken.Type.Label));

            // Answer message
            _definitions.Add(new KoeDefinition("^>\\s*", (token, groups) => {
                token.type = KoeToken.Type.Message;
                token.messageIsAnswer = true;
                KoeLog.Info($"---- Answer message");
            }, KoeToken.Type.Undefined));

            // Side message
            _definitions.Add(new KoeDefinition("^~\\s*", (token, groups) => {
                token.type = KoeToken.Type.Message;
                token.messageIsSide = true;
                KoeLog.Info($"---- Side message");
            }, KoeToken.Type.Undefined));

            // Function Name on a function call
            _definitions.Add(new KoeDefinition("^@(?<function>[A-Za-z_]+)[ ]?", (token, groups) => {
                token.type = KoeToken.Type.Function;
                token.functionName = groups["function"].Value;
                KoeLog.Info($"---- Function name : {token.functionName}");
            }, KoeToken.Type.Undefined));

            // Named argument on a function call
            _definitions.Add(new KoeDefinition("^(?<key>[a-zA-Z_]+)=(?<value>(?:\"(?<doublestr>[^\"]+)\")|(?:\'(?<singlestr>[^\']+)\')|(?<context>\\$[^\\s]+)|(?:[^\\s]+))\\s*", (token, groups) => {
                string key = groups["key"].Value;
                // Keep as is if it's a string, or else convert it
                KoeLog.Info("converting value "+groups["context"].Value);
                object value = null;
                if (groups["singlestr"].Value.Length > 0) {
                    value = groups["singlestr"].Value;
                } else if (groups["doublestr"].Value.Length > 0) {
                    value = groups["doublestr"].Value;
                } else if (groups["context"].Value.Length > 0) {
                    value = ConvertValue(groups["context"].Value);
                } else {
                    value = ConvertValue(groups["value"].Value);
                }
                //object value = (groups["strvalue"].Value.Length > 0) ? groups["strvalue"].Value : ConvertValue(groups["value"].Value);
                token.functionKeywordArgs.Add(key, value);
                KoeLog.Info($"---- Function keyword argument : {key} = {value} ({value?.GetType()})");
            }, KoeToken.Type.Function));

            // Boolean or null as function argument value
            _definitions.Add(new KoeDefinition("^(?<keyword>true|false|null)\\s+", (token, groups) => {
                object value = ConvertValue(groups["keyword"].Value);
                token.functionArgs.Add(value);
                KoeLog.Info($"---- Function bool/null argument : {value}");
            }, KoeToken.Type.Function));

            // Keyword argument without value (= true)
            _definitions.Add(new KoeDefinition("^([a-zA-Z_]+)\\s+", (token, groups) => {
                token.functionKeywordArgs.Add(groups[1].Value, true);
                KoeLog.Info($"---- Function keyword argument without value : {groups[1].Value} = true");
            }, KoeToken.Type.Function));

            // String values (doubles or simple quotes)
            _definitions.Add(new KoeDefinition("(?:\"(?<doublestr>[^\"]+)\"|\'(?<singlestr>[^\']+)\')\\s*", (token, groups) => {
                string value = (groups["singlestr"].Value.Length > 0) ? groups["singlestr"].Value : groups["doublestr"].Value;
                token.functionArgs.Add(value);
                KoeLog.Info($"---- Function string argument : {value}");
            }, KoeToken.Type.Function));

            // Message character
            _definitions.Add(new KoeDefinition("^([A-Za-z0-9_]+)@?([A-Za-z0-9_]*)[ ]*:[ ]*", (token, groups) => {
                token.type = KoeToken.Type.Message;
                token.messageCharacter = groups[1].Value;
                token.messageCharacterState = groups[2].Value;
                KoeLog.Info($"---- Message character : {token.messageCharacter}");
            }, KoeToken.Type.Undefined | KoeToken.Type.Message)); // Have to include message for answer messages

            // Numeric value
            _definitions.Add(new KoeDefinition("^([0-9.]+)", (token, groups) => {
                object value = ConvertValue(groups[1].Value);
                token.functionArgs.Add(value);
                KoeLog.Info($"---- Function numeric argument : {value}");
            }, KoeToken.Type.Function));

            // Context variables
            _definitions.Add(new KoeDefinition("^(\\$[^\\s]+)", (token, groups) => {
                object value = ConvertValue(groups[1].Value);
                token.functionArgs.Add(value);
                KoeLog.Info($"---- Function context variable argument : {groups[1].Value} -> {value}");
            }, KoeToken.Type.Function));

            // Anything else (= message)
            _definitions.Add(new KoeDefinition("^(.*)\\s?$", (token, groups) => {
                if (token.type == KoeToken.Type.Undefined) {
                    token.type = KoeToken.Type.Message;
                }
                token.messageText = groups[1].Value;
            }, KoeToken.Type.Undefined | KoeToken.Type.Message));
        }

        /// <summary>
        /// Makes methods (with KoeFunction attribute) from a class callable from
        /// the KoeScript text that will be parsed
        /// </summary>
        /// <param name="classInstance"></param>
        public void RegisterFunctionsFromClass(object classInstance) {
            Type targetType = classInstance.GetType();
            MethodInfo[] targetMethods = targetType.GetMethods();
            foreach (MethodInfo methodInfo in targetMethods) {
                KoeFunctionAttribute attr = methodInfo.GetCustomAttribute<KoeFunctionAttribute>();
                if (attr != null) {
                    string functionName = (attr.name != null) ? attr.name : methodInfo.Name.ToLower();
                    if (_registeredFunctions.ContainsKey(functionName)) {
                        KoeLog.Error($"Function '{functionName}' is already registered.");
                        continue;
                    };
                    _registeredFunctions[functionName] = new KoeFunction(methodInfo, classInstance);
                }
            }
        }

        /// <summary>
        /// Add a value to the context, that may be used later for string interpolation
        /// </summary>
        /// <param name="key">Name of the context variable</param>
        /// <param name="value">Value of the context variable</param>
        public void AddToContext(string key, object value) {
            _context[key] = value;
        }

        /// <summary>
        /// Parse a KoeScript written text line into a token representation
        /// </summary>
        /// <param name="line">Text line</param>
        /// <returns>Token representing the line</returns>
        public KoeToken ParseLine(string line) {
            // Ignore empty and comment lines
            if (line.Trim().Length == 0 || line.TrimStart().StartsWith("#")) {
                return null;
            }

            KoeToken koeToken = new KoeToken();
            line = line.TrimStart() + "\n";
            (koeToken, line) = ParseLinePart(koeToken, line);

            return koeToken;
        }

        /// <summary>
        /// Execute the token wanted effect, depending on its type
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public object ExecuteToken(KoeToken token) {
            if (token.type == KoeToken.Type.Label) {
                // Don't have anything to do with a label
                return null;

            } else if (token.type == KoeToken.Type.Message) {
                // When "executing" a message, we just have to return the displayable message

                // String interpolation
                string message = Regex.Replace(token.messageText, @"\{(?<variable>[a-zA-Z_]+)\}", (match) => {
                    string key = match.Groups["variable"].Value;
                    if (_context.ContainsKey(key)) {
                        return _context[key].ToString();
                    } else {
                    KoeLog.Error($"Missing context variable on string interpolation : {key}");
                        return "";
                    }
                });
                token.messageTextFormatted = message;
                return message;

            } else if (token.type == KoeToken.Type.Function) {
                if (!_registeredFunctions.ContainsKey(token.functionName)) {
                    KoeLog.Error($"Trying to call an unregistered function : @{token.functionName}");
                    return null;
                }

                KoeLog.Info($"Calling function : @{token.functionName}");
                KoeFunction registeredFunc = _registeredFunctions[token.functionName];

                // As reflection makes it mandatory to invoke the targeted method with positional
                // arguments, we're gonna go through each of them in order and prepare their
                // values if available in the KoeToken
                ParameterInfo[] methodParameters = registeredFunc.ParametersInfos;
                object[] parameters = new object[methodParameters.Length];

                for (int i = 0; i < parameters.Length; i++) {
                    ParameterInfo info = methodParameters[i];
                    Type parameterType = info.ParameterType;
                    Type underlyingType = Nullable.GetUnderlyingType(parameterType);
                    if (underlyingType != null) {
                        parameterType = underlyingType;
                    }
                    KoeLog.Info($"-- Parameter n°{i} : {info.Name} ({parameterType})");

                    if (!info.HasDefaultValue) {
                        // Positional argument
                        if (token.functionArgs.Count >= i + 1) {
                            KoeLog.Info($"getting positional argument");
                            KoeLog.Info($"type : {token.functionArgs[i].GetType()}");
                            parameters[i] = token.functionArgs[i];
                        } else {
                            KoeLog.Error($"-- Missing positional argument '{info.Name}' when calling @{token.functionName}");
                            return null;
                        }
                    } else {
                        // Named argument
                        if (token.functionKeywordArgs.ContainsKey(info.Name)) {
                            object value = token.functionKeywordArgs[info.Name];
                            if (value != null && !parameterType.Equals(value.GetType())) {
                                KoeLog.Error($"-- Type mismatch with positional argument '{info.Name}' when calling @{token.functionName}");
                                return null;
                            }
                            parameters[i] = value;
                        } else if (token.functionArgs.Count >= i + 1) {
                            // When this argument is used like a positional argument without a name
                            parameters[i] = token.functionArgs[i];
                        } else {
                            parameters[i] = info.DefaultValue;
                        }
                    }
                }

                // Call the function and return its result
                return registeredFunc.MethodInfo.Invoke(registeredFunc.MethodClassInstance, parameters);
            }
            return null;
        }

        /// <summary>
        /// Tries to match a part of a line with the definitions representing the
        /// syntax of the KoeScript language
        /// </summary>
        /// <param name="token"></param>
        /// <param name="linePart"></param>
        /// <returns></returns>
        private (KoeToken, string) ParseLinePart(KoeToken token, string linePart) {
            linePart = linePart.TrimStart();

            // End the parsing if the line part is empty (nothing left to parse)
            if (linePart.TrimEnd().Length == 0) {
                return (token, "");
            }

            KoeLog.Info($"Parsing line part : {linePart.TrimEnd()}");

            foreach (KoeDefinition definition in _definitions) {
                // Don't bother with definitions that doesn't target this token's type
                if (!definition.TargetType.HasFlag(token.type)) {
                    continue;
                }

                var match = definition.Regex.Match(linePart);
                if (match.Success) {
                    KoeLog.Info($"Definition match : {definition.Regex}");
                    definition.Callback(token, match.Groups);

                    // End the parsing if the definition matches the whole line part
                    if (match.Length == linePart.Length) {
                        return (token, "");
                    }

                    // Else, parse the remaining line part
                    linePart = linePart.Substring(match.Length);
                    return ParseLinePart(token, linePart);
                }
            }

            // It is very unlikely to have a line part without any match,
            // but let's return everything as-is if it happens
            return (token, linePart);
        }

        private object ConvertValue(string value) {
            // Assure a strict equality when converting booleans and null
            if (value == "true") {
                return true;
            } else if (value == "false") {
                return false;
            } else if (value == "null") {
                return null;
            // Following KoeScript reference, only accept dot (.) as the comma separator for decimal numbers
            } else if (value.Contains(".")) {
                return Convert.ToSingle(value, CultureInfo.InvariantCulture);
            // Try to parse the value as an integer
            } else if (int.TryParse(value, out int number)) {
                return number;
            // Try to parse the value as a context variable name
            } else if (value.StartsWith("$")) {
                string contextKey = value.Substring(1);
                KoeLog.Info("converting a context variable : "+contextKey);
                return (_context.ContainsKey(contextKey)) ? _context[contextKey] : null;
            } else {
                return null;
            }
        }

    }
}
