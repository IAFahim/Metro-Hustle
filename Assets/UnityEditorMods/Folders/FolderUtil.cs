using System.IO;
using UnityEditor;
using Object = UnityEngine.Object;

namespace UnityEditorMods.Folders
{
    public static class FolderUtil
    {
        public static void CreateFile(
            string folderPath,
            string fileName,
            string contents
        )
        {
            var fullPath = Path.Combine(folderPath, fileName);
            File.WriteAllText(fullPath, contents);
        }

        public static (string parentFolder, string[] subFolders) MakeFolderAndSubFolder(string folderPath,
            string parentFolderName, params string[] childrens)
        {
            var parentFolder = CreateFolderInPath(folderPath, parentFolderName);
            var childrenSubFolders = new string[childrens.Length];
            for (var i = 0; i < childrens.Length; i++)
            {
                childrenSubFolders[i] = CreateFolderInPath(parentFolder, childrens[i]);
            }

            return (parentFolder, childrenSubFolders);
        }

        public static bool TryGetAssetByName<T>(string filter, out T asset) where T : Object
        {
            asset = null;
            var asmdefTemplate = AssetDatabase.FindAssets(filter);
            if (asmdefTemplate.Length == 0) return false;
            var assetGuid = AssetDatabase.GUIDToAssetPath(asmdefTemplate[0]);
            asset = AssetDatabase.LoadAssetAtPath<T>(assetGuid);
            return true;
        }

        public static string CreateFolderAtPath(string pathName, string newFolderName)
        {
            var directoryName = Path.GetDirectoryName(pathName);
            var createdFolder = AssetDatabase.CreateFolder(directoryName, newFolderName);
            return AssetDatabase.GUIDToAssetPath(createdFolder);
        }

        public static string CreateFolderInPath(string pathName, string newFolderName)
        {
            var pathCombined = Path.Combine(pathName, newFolderName);
            return CreateFolderAtPath(pathCombined, newFolderName);
        }
    }
}