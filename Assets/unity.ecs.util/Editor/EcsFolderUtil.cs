using System.IO;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace unity.ecs.util.Editor
{
    internal static class EcsFolderUtil
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
                ProjectWindowUtil.ShowCreatedAsset(AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.CreateFolder(Path.GetDirectoryName(pathName), Path.GetFileName(pathName))), typeof (Object)));
            }
        }
    }
}