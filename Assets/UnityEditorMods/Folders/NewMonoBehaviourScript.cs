using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace UnityEditorMods.Folders
{
    // Place this asset in an Editor-accessible folder like Assets/Editor/Settings
// Create it via Assets -> Create -> Mod -> Module Creator Config
// [CreateAssetMenu(fileName = "ModuleCreatorConfig", menuName = "Mod/Module Creator Config", order = 1)]

// Add a menu item to easily clear the cache
    public static class ModuleCacheMenu
    {
        [MenuItem("Assets/Mod/Clear Creator Cache", priority = -900)]
        private static void ClearWebCache()
        {
            ModuleConfig.ClearCache();
        }
    }

// Classes to represent the structure of module_definition.json
// Must be Serializable for JsonUtility

    [Serializable]
    public class ModuleJsonDefinition
    {
        public string description = "";
        public List<string> folders = new List<string>();
        public List<FileEntry> files = new List<FileEntry>();
    }

    [Serializable]
    public class FileEntry
    {
        public string templateUrlPath = ""; // Relative path on server
        public string targetPath = "";      // Relative path in project module folder
        public bool isTemplate = false;   // Needs {{Name}} replacement?
    }

    public static class ModuleCreatorCore
    {
        private const string NamePlaceholder = "{{Name}}";

        // Main async function called after user provides the name
        public static async Awaitable CreateModuleAsync(string moduleName, string moduleRootPath)
        {
            ModuleConfig config = ModuleConfig.GetConfig();
            if (config == null || string.IsNullOrWhiteSpace(config.baseUrl) || string.IsNullOrWhiteSpace(config.definitionFileName))
            {
                Debug.LogError("Module Creator configuration is missing or invalid. Please create and configure the ModuleConfig asset.");
                return;
            }

            // Basic Validation
            if (string.IsNullOrWhiteSpace(moduleName) || moduleName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 || moduleName.Contains(" "))
            {
                Debug.LogError($"Invalid module name: '{moduleName}'. Cannot contain invalid path characters or spaces.");
                return;
            }
            if (!config.baseUrl.EndsWith("/"))
            {
                Debug.LogError($"Base URL '{config.baseUrl}' must end with a forward slash '/'.");
                return;
            }

            // --- 1. Fetch Module Definition ---
            string definitionUrl = config.baseUrl + config.definitionFileName;
            EditorUtility.DisplayProgressBar("Creating Module", $"Fetching definition: {config.definitionFileName}...", 0.1f);

            string definitionJson = await GetTextContentAsync(definitionUrl, config);
            if (definitionJson == null)
            {
                EditorUtility.ClearProgressBar();
                Debug.LogError($"Failed to fetch module definition from: {definitionUrl}");
                return;
            }

            // --- 2. Parse Definition ---
            ModuleJsonDefinition definition;
            try
            {
                definition = JsonUtility.FromJson<ModuleJsonDefinition>(definitionJson);
                if (definition == null || (definition.folders == null && definition.files == null))
                {
                    throw new InvalidDataException("Parsed JSON definition is null or empty.");
                }
                Debug.Log($"Parsed module definition: {definition.description}");
            }
            catch (Exception e)
            {
                EditorUtility.ClearProgressBar();
                Debug.LogError($"Failed to parse module definition JSON from {definitionUrl}: {e.Message}\nContent:\n{definitionJson}");
                return;
            }

            // --- 3. Create Folders ---
            EditorUtility.DisplayProgressBar("Creating Module", "Creating folder structure...", 0.3f);
            try
            {
                // Root folder should already exist from EndNameEditAction
                if (!AssetDatabase.IsValidFolder(moduleRootPath))
                {
                    EditorUtility.ClearProgressBar();
                    Debug.LogError($"Module root path '{moduleRootPath}' is not a valid folder. Aborting.");
                    return;
                }

                if (definition.folders != null)
                {
                    foreach (string relativePath in definition.folders)
                    {
                        if (string.IsNullOrWhiteSpace(relativePath)) continue;
                        string fullPath = Path.Combine(moduleRootPath, relativePath).Replace("\\", "/");
                        EnsureFolderExists(fullPath); // Use AssetDatabase to create folders
                    }
                }
            }
            catch (Exception e)
            {
                EditorUtility.ClearProgressBar();
                Debug.LogError($"Failed to create folder structure for module '{moduleName}': {e}");
                // Consider cleanup or rollback if needed
                return;
            }


            // --- 4. Create Files ---
            float progressStep = 0.6f / (definition.files?.Count ?? 1); // Allocate 60% of progress bar to files
            float currentProgress = 0.4f;

            if (definition.files != null)
            {
                foreach (FileEntry fileEntry in definition.files)
                {
                    currentProgress += progressStep;
                    EditorUtility.DisplayProgressBar("Creating Module", $"Processing file: {fileEntry.targetPath}...", currentProgress);

                    if (string.IsNullOrWhiteSpace(fileEntry.templateUrlPath) || string.IsNullOrWhiteSpace(fileEntry.targetPath))
                    {
                        Debug.LogWarning($"Skipping file entry with missing paths: {JsonUtility.ToJson(fileEntry)}");
                        continue;
                    }

                    string templateUrl = config.baseUrl + fileEntry.templateUrlPath;
                    string targetPathRelative = fileEntry.targetPath.Replace(NamePlaceholder, moduleName);
                    string targetFullPath = Path.Combine(moduleRootPath, targetPathRelative).Replace("\\", "/");

                    // Fetch template content (uses cache)
                    string fileContent = await GetTextContentAsync(templateUrl, config);
                    if (fileContent == null)
                    {
                        Debug.LogWarning($"Failed to fetch template content from: {templateUrl}. Skipping file '{targetFullPath}'.");
                        continue; // Skip this file, but continue with others
                    }

                    // Replace placeholders if necessary
                    if (fileEntry.isTemplate)
                    {
                        fileContent = fileContent.Replace(NamePlaceholder, moduleName);
                        // Add more placeholder replacements here if needed in the future
                    }

                    // Ensure target directory exists before writing
                    string targetDirectory = Path.GetDirectoryName(targetFullPath);
                    EnsureFolderExists(targetDirectory);

                    // Write file asynchronously
                    try
                    {
                        await WriteTextToFileAsync(targetFullPath, fileContent);
                        Debug.Log($"Created file: {targetFullPath}");
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Failed to write file '{targetFullPath}': {e.Message}");
                        // Decide if failure here should stop the whole process
                    }
                }
            }

            // --- 5. Refresh Asset Database ---
            EditorUtility.DisplayProgressBar("Creating Module", "Refreshing Assets...", 0.95f);
            AssetDatabase.Refresh();

            // --- 6. Finish ---
            EditorUtility.ClearProgressBar();
            Debug.Log($"Successfully created module '{moduleName}' at '{moduleRootPath}'");

            // Select the created folder in the Project window
            Object obj = AssetDatabase.LoadAssetAtPath<Object>(moduleRootPath);
            if (obj != null)
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = obj;
            }
        }

        // --- Helper: Fetch Text Content with Cache and Awaitable ---
        private static async Awaitable<string> GetTextContentAsync(string url, ModuleConfig config)
        {
            if (ModuleConfig.TryGetFromCache(url, out string cachedContent))
            {
                Debug.Log($"Cache hit for: {url}");
                return cachedContent;
            }

            Debug.Log($"Fetching: {url}");
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                // Add cache control header if desired (optional)
                // request.SetRequestHeader("Cache-Control", "no-cache");

                try
                {
                    await request.SendWebRequest(); // Awaitable extension

#if UNITY_2020_2_OR_NEWER
                    if (request.result != UnityWebRequest.Result.Success)
#else
                if (request.isNetworkError || request.isHttpError)
#endif
                    {
                        Debug.LogError($"Error fetching {url}: {request.responseCode} - {request.error}");
                        return null; // Indicate failure
                    }
                    else
                    {
                        string content = request.downloadHandler.text;
                        ModuleConfig.AddToCache(url, content); // Add to cache on success
                        return content;
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Exception during web request for {url}: {ex.Message}\n{ex.StackTrace}");
                    return null; // Indicate failure
                }
            } // using ensures request is disposed
        }

        // --- Helper: Write Text File Async ---
        private static async Awaitable WriteTextToFileAsync(string filePath, string content)
        {
            try
            {
                // Using Task.Run to avoid blocking main thread for file I/O
                await Task.Run(() => File.WriteAllText(filePath, content, System.Text.Encoding.UTF8));
                // Alternative for newer .NET/Unity:
                // using (var writer = new StreamWriter(filePath, false, System.Text.Encoding.UTF8)) // false = overwrite
                // {
                //     await writer.WriteAsync(content);
                // }
            }
            catch (Exception ex)
            {
                // Log error but rethrow so the calling method knows about the failure
                Debug.LogError($"Error writing file '{filePath}': {ex.Message}");
                throw;
            }
        }

        // --- Helper: Ensure Folder Exists using AssetDatabase ---
        private static void EnsureFolderExists(string fullPath)
        {
            if (AssetDatabase.IsValidFolder(fullPath))
            {
                return;
            }

            // Need to create parent directories iteratively if they don't exist
            string parentPath = Path.GetDirectoryName(fullPath).Replace("\\", "/");
            string folderName = Path.GetFileName(fullPath);

            if (!AssetDatabase.IsValidFolder(parentPath))
            {
                EnsureFolderExists(parentPath); // Recurse upwards
            }

            // Now the parent should exist, create the target folder
            if (!AssetDatabase.IsValidFolder(fullPath)) // Double check after parent creation
            {
                string parentGuid = AssetDatabase.AssetPathToGUID(parentPath);
                if (!string.IsNullOrEmpty(parentGuid)) // Ensure parent is a valid asset path
                {
                    string guid = AssetDatabase.CreateFolder(parentPath, folderName);
                    if (string.IsNullOrEmpty(guid))
                    {
                        throw new IOException($"AssetDatabase.CreateFolder failed for '{folderName}' in '{parentPath}'");
                    }
                    Debug.Log($"Created folder: {fullPath}");
                }
                else
                {
                    // This might happen if the path is outside Assets/, though it shouldn't here.
                    Directory.CreateDirectory(fullPath);
                    Debug.LogWarning($"Created folder using System.IO: {fullPath} (outside AssetDatabase scope?)");
                }
            }
        }
    }

    public static class ModuleCreatorMenu
    {
        // Texture loading helper (cache icon)
        private static Texture2D _folderIcon;
        private static Texture2D FolderIcon => _folderIcon ??= EditorGUIUtility.IconContent(EditorResources.emptyFolderIconName).image as Texture2D;

        [MenuItem("Assets/Mod/Folder", priority = -1000)]
        private static void InitCreateModule()
        {
            // Use the project window naming utility
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
                0, // instanceID, 0 means create in selected folder
                ScriptableObject.CreateInstance<DoCreateModuleAction>(),
                "NewModule", // Default name suggestion
                FolderIcon, // Icon
                null // path, null uses current selection
            );
        }

        // Action executed after user confirms the name in the Project window
        internal class DoCreateModuleAction : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                // pathName is the full path where the user wants to create the item (e.g., Assets/Features/NewModuleName)
                // resourceFile is the template file, which we are not using here.

                string moduleName = Path.GetFileName(pathName);
                string parentDirectory = Path.GetDirectoryName(pathName).Replace("\\", "/");

                // 1. Create the root folder using AssetDatabase
                string moduleRootGuid = AssetDatabase.CreateFolder(parentDirectory, moduleName);
                if (string.IsNullOrEmpty(moduleRootGuid))
                {
                    Debug.LogError($"Failed to create root folder for module '{moduleName}' at '{parentDirectory}'.");
                    return;
                }
                string moduleRootPath = AssetDatabase.GUIDToAssetPath(moduleRootGuid);

                // 2. Trigger the async core logic (fire-and-forget style with discard _)
                // The core logic handles progress bar and completion logging.
                _ = ModuleCreatorCore.CreateModuleAsync(moduleName, moduleRootPath);

                // Note: We don't select the asset here immediately, the async method will do it upon completion.
            }

            public override void Cancelled(int instanceId, string pathName, string resourceFile)
            {
                // Optional: Add any cleanup logic if the user cancels the naming process
                Debug.Log("Module creation cancelled by user.");
            }
        }
    }
}