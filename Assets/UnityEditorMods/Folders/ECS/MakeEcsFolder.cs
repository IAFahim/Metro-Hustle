using System.IO;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityEditorMods.Folders.ECS
{
    internal static class MakeEcsFolder
    {
        private const string EcsAsmdefFileName = "template.asmdef";

        [MenuItem("Assets/Mod/Folder/ECS", priority = -1000)]
        private static void InitCreateEcs()
        {
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
                ScriptableObject.CreateInstance<DoCreateFolder>(), "New Folder",
                EditorGUIUtility.IconContent(EditorResources.emptyFolderIconName).image as Texture2D,
                null
            );
        }

        internal class DoCreateFolder : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string _)
            {
                var newFolderName = Path.GetFileName(pathName);
                var folderPath = FolderUtil.CreateFolderAtPath(pathName, newFolderName);
                (string runtimeParentFolder, string[] _) =
                    FolderUtil.MakeFolderAndSubFolder(folderPath, "Runtime", "Datas", "Systems");
                string editorParentFolder = FolderUtil.CreateFolderInPath(folderPath, "Editor");
                CreateAsmdef_ECS(folderPath, newFolderName, runtimeParentFolder, editorParentFolder);
            }

            private void CreateAsmdef_ECS(string folderPath, string newFolderName, string runtimeParentFolder,
                string editorParentFolder)
            {
                var createdObject = AssetDatabase.LoadAssetAtPath(folderPath, typeof(Object));
                ProjectWindowUtil.ShowCreatedAsset(createdObject);
                var asmdefName = newFolderName.Replace(" ", "");
                var reference = new[]
                {
                    "Unity.Entities",
                    "Unity.Transforms",
                    "Unity.Entities.Hybrid",
                    "Unity.Burst",
                    "Unity.Collections",
                    "Unity.Mathematics"
                };
                FolderUtil.CreateAsmdef(runtimeParentFolder, asmdefName, reference, false);
                FolderUtil.CreateAsmdef(editorParentFolder, asmdefName, reference, true);
            }
        }
    }
}