using System;
using System.IO;
using UnityEditor;

namespace UnityEditorMods.Selections
{
    public static class SelectionExtension
    {
        private const string AssetsRootPath = "Assets";

        public static string GetFolder()
        {
            UnityEngine.Object[] selectedObjects = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
            if ((selectedObjects?.Length ?? 0) > 0)
            {
                string folderPath = AssetDatabase.GetAssetPath((UnityEngine.Object)selectedObjects[0]);
                return AssetDatabase.IsValidFolder(folderPath) ? folderPath : Path.GetDirectoryName(folderPath);
            }

            return AssetsRootPath;
        }

        public static string CreateAsset(UnityEngine.Object asset, string fileName)
        {
            var path = GetFolder();
            var assetPath = Path.Combine(path, fileName);
            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();
            Selection.activeObject = asset;
            return assetPath;
        }
    }
}