using System.IO;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;

namespace UnityFolderPreset
{
    public class DoCreateFolder : EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string _)
        {
            var fileName = Path.GetFileName(pathName);
            var fileNameWithoutSpace = fileName.Replace(" ", "");
            var directoryName = Path.GetDirectoryName(pathName);
            var guid = AssetDatabase.CreateFolder(directoryName, fileName);
            var parentDirectory = AssetDatabase.GUIDToAssetPath(guid);

            var findAssets = AssetDatabase.FindAssets("t:FolderTextAssetConfig");
            string assetPath = AssetDatabase.GUIDToAssetPath(findAssets[0]);
            var folderTextAssetConfig = AssetDatabase.LoadAssetAtPath<FolderTextAssetConfig>(assetPath);
            foreach (var folderTextAsset in folderTextAssetConfig.folderTextAssets)
            {
                var endPath = folderTextAsset.path;
                TraverseByAddMissingFolder(parentDirectory, endPath);
                AddFiles(parentDirectory, fileNameWithoutSpace, folderTextAsset);
            }
        }

        private static void TraverseByAddMissingFolder(string pathName, string endPath)
        {
            string subFolderCursor = pathName;
            if (endPath.Length == 0) return;
            foreach (var folder in endPath.Split('/'))
            {
                var currentFolder = Path.Combine(subFolderCursor, folder);
                if (!AssetDatabase.IsValidFolder(currentFolder))
                    AssetDatabase.CreateFolder(subFolderCursor, folder);
                subFolderCursor = currentFolder;
            }
        }

        private static void AddFiles(string parentDirectory, string fileNameWithoutSpace,
            FolderTextAsset folderTextAsset, string tagToReplaceInFileName = "{{Name}}")
        {
            if (folderTextAsset.textAssets == null) return;
            foreach (var textAsset in folderTextAsset.textAssets)
            {
                // {fileNameWithoutSpace} + "Runtime.asmdef"
                var newFileName = textAsset.name.Replace(tagToReplaceInFileName, fileNameWithoutSpace);
                var relativePath = Path.Combine(parentDirectory, folderTextAsset.path);
                var relativeFullFilePath = Path.Combine(relativePath, newFileName);
                // {{Name}} -> {fileNameWithoutSpace}
                var replaceAllCapNames = textAsset.text.Replace("{{Name}}", fileNameWithoutSpace);
                // {{name}} -> {fileNameWithoutSpace.Lower}
                var replacedAllNames = replaceAllCapNames.Replace("{{name}}", fileNameWithoutSpace.ToLower());
                File.WriteAllText(relativeFullFilePath, replacedAllNames);
            }
        }
    }
}