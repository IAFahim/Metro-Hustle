using UnityEditor;
using UnityEngine;

namespace UnityEditorMods.Folders
{
    [CreateAssetMenu(menuName = "Create/Mod/Folder/SO")]
    public class ModuleConfig : ScriptableObject
    {
        [Header("Remote Configuration")]
        [Tooltip("Base URL where module definition and templates are hosted. MUST end with '/'")]
        public string baseUrl = "YOUR_SERVER_BASE_URL_HERE/"; // <-- IMPORTANT: SET THIS

        [Tooltip("Filename of the main JSON definition file relative to the Base URL.")]
        public string definitionFileName = "module_definition.json";

        // --- Caching ---
        // Static cache persists across creation actions within the same editor session
        [HideInInspector] // No need to show this complex type in Inspector
        public static readonly System.Collections.Generic.Dictionary<string, string> WebContentCache =
            new System.Collections.Generic.Dictionary<string, string>();

        // Find the config asset easily
        public static ModuleConfig GetConfig()
        {
            var guids = AssetDatabase.FindAssets($"t:{nameof(ModuleConfig)}");
            if (guids.Length == 0)
            {
                Debug.LogWarning($"Module Creator Config asset not found. Creating default one in 'Assets/Editor/Settings'. Please configure it.");
                string path = "Assets/Editor/Settings";
                if (!AssetDatabase.IsValidFolder(path))
                {
                    AssetDatabase.CreateFolder("Assets", "Editor");
                    AssetDatabase.CreateFolder("Assets/Editor", "Settings");
                }
                ModuleConfig newConfig = CreateInstance<ModuleConfig>();
                AssetDatabase.CreateAsset(newConfig, $"{path}/ModuleCreatorConfig.asset");
                AssetDatabase.SaveAssets();
                return newConfig;
            }
            if (guids.Length > 1)
            {
                Debug.LogWarning("Multiple Module Creator Config assets found. Using the first one.");
            }
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            return AssetDatabase.LoadAssetAtPath<ModuleConfig>(assetPath);
        }

        // Utility to add to cache
        public static void AddToCache(string url, string content)
        {
            WebContentCache[url] = content;
        }

        // Utility to get from cache
        public static bool TryGetFromCache(string url, out string content)
        {
            return WebContentCache.TryGetValue(url, out content);
        }

        // Utility to clear cache
        public static void ClearCache()
        {
            WebContentCache.Clear();
            Debug.Log("Module creator web cache cleared.");
        }
    }
}