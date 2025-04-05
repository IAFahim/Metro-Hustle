using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using Object = UnityEngine.Object;

namespace UnityEditorMods.Folders
{
    public static class FolderUtil
    {
        public static string CreateAsmdef(
            string folderPath,
            string name,
            string[] references,
            bool isEditor
        )
        {
            var asmdefRuntimeName = $"{name}.Runtime";
            var asmdefEditorName = $"{name}.Editor";
            var asmdefName = asmdefRuntimeName;
            if (isEditor)
            {
                asmdefName = asmdefEditorName;
                Array.Resize(ref references, references.Length + 1);
                references[^1] = asmdefRuntimeName;
            }

            var json = CreateAsmdefFileContent(asmdefName, asmdefRuntimeName, references, isEditor);
            var fullPath = Path.Combine(folderPath, asmdefName);
            File.WriteAllText(fullPath += ".asmdef", json.ToString());
            return fullPath;
        }

        public static StringBuilder CreateAsmdefFileContent(string asmdefName, string runtimeAsmdefName,
            string[] references, bool isEditor)
        {
            StringBuilder json = new StringBuilder();
            json.AppendLine("{");
            json.AppendLine($"    \"name\": \"{asmdefName}\",");
            json.AppendLine($"    \"rootNamespace\": \"\",");

            json.AppendLine("    \"references\": [");
            var allReferences = new List<string>(references ?? Array.Empty<string>());
            if (isEditor && !allReferences.Contains(runtimeAsmdefName)) allReferences.Add(runtimeAsmdefName);
            for (int i = 0; i < allReferences.Count; i++)
            {
                json.Append($"        \"{allReferences[i]}\"");
                if (i < allReferences.Count - 1) json.Append(",");
                json.AppendLine();
            }

            json.AppendLine("    ],");

            json.AppendLine("    \"includePlatforms\": [");
            if (isEditor) json.AppendLine("        \"Editor\"");
            json.AppendLine("    ],");

            json.AppendLine("    \"excludePlatforms\": [],");
            json.AppendLine("    \"allowUnsafeCode\": false,");
            json.AppendLine("    \"overrideReferences\": false,");
            json.AppendLine("    \"precompiledReferences\": [],");
            var autoReferenced = isEditor ? "false" : "true";
            json.AppendLine($"    \"autoReferenced\": {autoReferenced},");
            json.AppendLine("    \"defineConstraints\": [],");
            json.AppendLine("    \"versionDefines\": [],");
            json.AppendLine("    \"noEngineReferences\": false");
            json.AppendLine("}");
            return json;
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