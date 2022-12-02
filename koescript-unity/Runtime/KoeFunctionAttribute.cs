using System;

namespace KoeScript {
    /// <summary>
    /// This attribute is used to mark class methods as being callable by a KoeScript dialogue.
    /// 
    /// For example, the following method can be called as @character :
    /// [KoeFunction]
    /// void Character() {
    /// }
    /// 
    /// You can also specify the name from which the method can be called from the KoeScript dialogue
    /// by using the "name" argument on the system attribute.
    /// For example, the following method can be called as @bgm :
    /// [KoeFunction(name="bgm")]
    /// void BackgroundMusic() {
    /// }
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class KoeFunctionAttribute : System.Attribute {
        
        /// <summary>
        /// Name of the function when called from a KoeScript dialogue (optional).
        /// </summary>
        public string name = null;
    }
}
