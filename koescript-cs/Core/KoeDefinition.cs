using System;
using System.Text.RegularExpressions;

namespace KoeScript {
    /// <summary>
    /// A definition is a representation of a language syntax or grammar point, used by
    /// the KoeScript interpreter to recognize them while reading a text line
    /// (or a part of a line) that's written in KoeScript.
    /// </summary>
    public class KoeDefinition {
        /// <summary>
        /// A regex to match the relevant syntax or grammar point targeted by this definition.
        /// </summary>
        public Regex Regex;
        /// <summary>
        /// What should be done when the regex is matched with a given text line (or line part).
        /// A blank KoeToken (or an existing KoeToken if any) is passed as an argument, as well as
        /// the match results from the regex.
        /// </summary>
        public Action<KoeToken, GroupCollection> Callback;
        /// <summary>
        /// If this definition isn't made to define the type of a text line (and therefore its token),
        /// then it can target only certain types of tokens.
        /// This field contain a KoeToken type which is a power of two, so bitwise operations are possible
        /// in order to target one or multiple types.
        /// </summary>
        public KoeToken.Type TargetType;
        public KoeDefinition(string regex, Action<KoeToken, GroupCollection> callback, KoeToken.Type target=KoeToken.Type.Undefined) {
            this.Regex = new Regex(regex);
            this.Callback = callback;
            this.TargetType = target;
        }
    }
}
