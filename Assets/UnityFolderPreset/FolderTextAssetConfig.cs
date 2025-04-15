using System;
using UnityEngine;
using UnityEngine.Search;

namespace UnityFolderPreset
{
    [CreateAssetMenu(fileName = "FolderTextAssetConfig.assets", menuName = "Config/FolderTextAsset Config")]
    public class FolderTextAssetConfig : ScriptableObject
    {
        public FolderTextAsset[] folderTextAssets;

        private void OnEnable()
        {
            hideFlags = HideFlags.DontUnloadUnusedAsset;
        }

        private void OnValidate()
        {
            foreach (var folderTextAsset in folderTextAssets)
            {
                folderTextAsset.path = folderTextAsset.path.Replace('\\', '/');
            }
        }
    }

    [Serializable]
    public class FolderTextAsset
    {
        public string path;

        [Tooltip(
            "If True (New Folder, Runtime.asmdef) becomes -> NewFolder.Runtime.asmdef\n" +
            "If False (New Folder, package.json) Remains -> package.json"
        )]
        public bool addFolderName = true;

        [SearchContext("t:TextAsset .txt")] public TextAsset[] textAssets;
    }
}