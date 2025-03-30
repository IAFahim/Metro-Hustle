using System.IO;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace UnityEditorMod.Folders.ECS
{
    internal static class MakeEcsFolder
    {
        private const string EcsAsmdefFileName = "template.asmdef";

        [MenuItem("Assets/Create/ECS Util/Generate ECS Script Folder", priority = -1000)]
        private static void InitCreateEcs()
        {
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
                ScriptableObject.CreateInstance<DoCreateFolder>(), "New ECS",
                EditorGUIUtility.IconContent(EditorResources.emptyFolderIconName).image as Texture2D,
                null
            );
        }

        internal class DoCreateFolder : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string _)
            {
                var fileName = Path.GetFileName(pathName);
                var folderPath = CreateFolder(pathName, fileName);
                CreateChildrenAndParentFolders(folderPath, "Runtime", "Datas", "Systems");
                CreateFolder(folderPath, "Editor");
                AssetDatabase.Refresh();

                var createdObject = AssetDatabase.LoadAssetAtPath(folderPath, typeof(Object));
                ProjectWindowUtil.ShowCreatedAsset(createdObject);
                TryGetAssetByName<TextAsset>(EcsAsmdefFileName, out var ecsTemplate);
            }

            private string[] CreateChildrenAndParentFolders(string folderPath, string parentFolder,
                params string[] childrens)
            {
                var parentSubFolder = CreateSubFolder(folderPath, parentFolder);
                var childrenSubFolders = new string[childrens.Length];
                for (var i = 0; i < childrens.Length; i++)
                {
                    childrenSubFolders[i] = CreateSubFolder(parentSubFolder, childrens[i]);
                }

                return childrenSubFolders;
            }

            private static bool TryGetAssetByName<T>(string filter, out T asset) where T : Object
            {
                asset = null;
                var asmdefTemplate = AssetDatabase.FindAssets(filter);
                if (asmdefTemplate.Length == 0) return false;
                var assetGuid = AssetDatabase.GUIDToAssetPath(asmdefTemplate[0]);
                asset = AssetDatabase.LoadAssetAtPath<T>(assetGuid);
                return true;
            }

            private static string CreateFolder(string pathName,
                string fileName)
            {
                var directoryName = Path.GetDirectoryName(pathName);
                var createdFolder = AssetDatabase.CreateFolder(directoryName, fileName);
                return AssetDatabase.GUIDToAssetPath(createdFolder);
            }

            private string CreateSubFolder(string pathName, string folderName)
            {
                var pathCombined = Path.Combine(pathName, folderName);
                return CreateFolder(pathCombined, folderName);
            }
        }
    }
}