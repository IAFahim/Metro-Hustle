using System.IO;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace UnityEditorMod.Folders.ECS
{
    internal static class MakeEcsFolder
    {
        private const string EcsAsmdefFileName = "ecs.template.asmdef";

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
                var createdFile = Path.GetFileName(pathName);
                var parentFolder = CreateFolder(pathName, createdFile);
                var runtimeFolder = CreateSubFolder(parentFolder.createdPath, "Runtime");
                var dataFolder = CreateSubFolder(runtimeFolder.createdPath, "Datas");
                var systemFolder = CreateSubFolder(runtimeFolder.createdPath, "Systems");
                ProjectWindowUtil.ShowCreatedAsset(dataFolder.createdObject);
                GenerateGuid();
            }

            private static void GenerateGuid()
            {
                var asmdefTemplate = AssetDatabase.FindAssets(EcsAsmdefFileName);
                if (asmdefTemplate.Length == 0) return;
                var assetGuid = AssetDatabase.GUIDToAssetPath(asmdefTemplate[0]);
                var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(assetGuid);
                Debug.Log(asset.text);
            }

            private static (Object createdObject, string createdPath) CreateFolder(string pathName,
                string fileName)
            {
                var directoryName = Path.GetDirectoryName(pathName);
                var createdFolder = AssetDatabase.CreateFolder(directoryName, fileName);
                var createdPath = AssetDatabase.GUIDToAssetPath(createdFolder);
                var createdObject = AssetDatabase.LoadAssetAtPath(createdPath, typeof(Object));
                return (createdObject, createdPath);
            }

            private (Object createdObject, string createdPath) CreateSubFolder(string pathName, string folderName)
            {
                var pathCombined = Path.Combine(pathName, folderName);
                return CreateFolder(pathCombined, folderName);
            }
        }
    }
}