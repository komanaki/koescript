
using System.Reflection;

namespace KoeScript {

        /// <summary>
        /// Representation of a function registered against the interpreter,
        /// for it to be eventually called from a KoeScript dialogue line.
        /// </summary>
        class KoeFunction {
            /// <summary>
            /// The metadata of the registered method, used to invoke it when needed
            /// </summary>
            public MethodInfo MethodInfo;
            /// <summary>
            /// The class instance from which the method comes from
            /// </summary>
            public object MethodClassInstance;
            public ParameterInfo[] ParametersInfos;
            public KoeFunction(MethodInfo methodInfo, object methodClassInstance) {
                this.MethodInfo = methodInfo;
                this.MethodClassInstance = methodClassInstance;
                this.ParametersInfos = methodInfo.GetParameters();
            }
        }
}
