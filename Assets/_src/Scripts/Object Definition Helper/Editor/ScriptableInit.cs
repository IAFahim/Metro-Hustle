using BovineLabs.Core.Authoring.ObjectManagement;
using UnityEditor;
using UnityEditorMods.Selections;
using UnityEngine;

namespace _src.Scripts.Object_Definition_Helper.Editor
{
    internal static class ScriptableInit
    {
        [MenuItem("Assets/Mod/Asset/Object Definition/Default", priority = -999)]
        internal static void CreateObjectDefinition()
        {
            var asset = ScriptableObject.CreateInstance<ObjectDefinition>();
            SelectionExtension.CreateAsset(asset, "Default ObjDef.asset");
        }
    }
}