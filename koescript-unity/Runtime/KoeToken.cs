
using System.Collections.Generic;

namespace KoeScript {
    public class KoeToken {
        /// <summary>
        /// Type of the token. Value must be a power of 2.
        /// </summary>
        public enum Type : short {
            Undefined = 1,
            Message = 2,
            Function = 4,
            Label = 8
        };
        public Type type = Type.Undefined;
        /// <summary>
        /// When applies, position of this token inside a token list, a dialogue file, etc
        /// </summary>
        public int position;
        /// <summary>
        /// Name of the function called by the token
        /// </summary>
        public string functionName;
        /// <summary>
        /// List of positional arguments for the function called by the token
        /// </summary>
        public List<object> functionArgs;
        /// <summary>
        /// Dictionary of keyword (named) arguments for the function called by the token
        /// </summary>
        public Dictionary<string, object> functionKeywordArgs;
        /// <summary>
        /// Text content of the message
        /// </summary>
        public string messageText;
        /// <summary>
        /// Displayable text content of the message
        /// </summary>
        public string messageTextFormatted;
        /// <summary>
        /// Character that speaks the message
        /// </summary>
        public string messageCharacter = "";
        /// <summary>
        /// State (eg. face...) of the character that is linked to the message
        /// </summary>
        public string messageCharacterState = "";
        /// <summary>
        /// If the message is a side message
        /// </summary>
        public bool messageIsSide = false;
        /// <summary>
        /// If the message is an answer message
        /// </summary>
        public bool messageIsAnswer = false;
        /// <summary>
        /// ID of the label defined by the token
        /// </summary>
        public string labelId = "";
        /// <summary>
        /// Human-readable description of the label defined by the token
        /// </summary>
        public string labelDescription = "";
        /// <summary>
        /// Queue of tokens that may be executed before the current token
        /// </summary>
        public List<KoeToken> queue;

        public KoeToken(string functionName="") {
            this.functionName = functionName;
            functionArgs = new List<object>();
            functionKeywordArgs = new Dictionary<string, object>();
            queue = new List<KoeToken>();
            if (functionName.Length > 0) {
                type = KoeToken.Type.Function;
            }
        }

        public override string ToString() {
            string text = $"KoeToken {position} | ";
            if (type == Type.Function) {
                text += $"@{functionName} \"" + string.Join("\" \"", functionArgs) + "\" ";
                foreach (KeyValuePair<string, object> kv in functionKeywordArgs) {
                    text += kv.Key + "=" + kv.Value + " ";
                }
            } else if (type == Type.Message) {
                text += $"{messageCharacter} : {messageText} ";
            } else if (type == Type.Label) {
                text += $"*{labelId} : {labelDescription}";
            } else if (type == Type.Undefined) {
                text += "UNDEFINED TOKEN";
            }
            return text;
        }
    }
}
