using UnityEngine;

using System.IO;

namespace KoeScript {

    /// <summary>
    /// Lets the Unity Editor import ".koe" KoeScript files as text files.
    /// </summary>
    [UnityEditor.AssetImporters.ScriptedImporter(1, "koe")]
    public class KoeImporter : UnityEditor.AssetImporters.ScriptedImporter {
        public override void OnImportAsset(UnityEditor.AssetImporters.AssetImportContext context) {
            TextAsset subAsset = new TextAsset(File.ReadAllText(context.assetPath));
            context.AddObjectToAsset("text", subAsset);
            context.SetMainObject(subAsset);
        }
    }
}
