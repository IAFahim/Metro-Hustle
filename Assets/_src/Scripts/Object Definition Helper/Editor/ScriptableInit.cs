using BovineLabs.Core.Authoring.ObjectManagement;
using UnityEditor;
using UnityEditorMods.Selections;
using UnityEngine;

namespace _src.Scripts.Object_Definition_Helper.Editor
{
    internal static class ScriptableInit
    {
        [MenuItem("Assets/Mod/Asset/Object Definition/New", priority = -1000)]
        internal static void CreateObjectDefinition()
        {
            var asset = ScriptableObject.CreateInstance<ObjectDefinition>();
            SelectionExtension.CreateAsset(asset, "New ObjDef.asset");
        }
    }
}