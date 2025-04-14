using System.IO;

namespace UnityEditorMods.Folders
{
    internal static class FileInteract
    {
        public static string ReadAndReplace(string fullPath, string with, string replace = "{{Name}}")
        {
            var content = File.ReadAllText(fullPath);
            return content.Replace(content,with);
        }
    }
}