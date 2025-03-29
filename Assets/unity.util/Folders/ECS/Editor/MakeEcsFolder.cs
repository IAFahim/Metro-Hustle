using System.IO;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace unity.util.Folders.ECS.Editor
{
    internal static class MakeEcsFolder
    {
        [MenuItem("Assets/Create/ECS Util/Make Script Folder", priority = -1000)]
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
                var fileName = Path.GetFileName(pathName);
                var directoryName = Path.GetDirectoryName(pathName);
                var createdFolder = AssetDatabase.CreateFolder(directoryName, fileName);
                var createdFolderAssetGuid = AssetDatabase.GUIDToAssetPath(createdFolder);
                var loadCreatedAsset = AssetDatabase.LoadAssetAtPath(createdFolderAssetGuid, typeof(Object));
                ProjectWindowUtil.ShowCreatedAsset(loadCreatedAsset);
            }
        }
    }
}