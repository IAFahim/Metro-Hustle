using System;
using System.IO;
using UnityEditor;

namespace unity.ecs.util.Editor
{
    internal class SelectionExtension
    {
        private const string AssetsRootPath = "Assets";
        
        public static string GetFolder()
        {
            Object[] selectedObjects = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
            if ((selectedObjects?.Length ?? 0) > 0)
            {
                string folderPath = AssetDatabase.GetAssetPath((UnityEngine.Object)selectedObjects[0]);
                return AssetDatabase.IsValidFolder(folderPath) ? folderPath : Path.GetDirectoryName(folderPath);
            }

            return AssetsRootPath;
        }
    }
}