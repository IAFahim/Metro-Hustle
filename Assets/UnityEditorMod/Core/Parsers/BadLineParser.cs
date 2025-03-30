namespace UnityEditorMod.Core.Parsers
{
    public static class BadLineParser
    {
        public static bool TryGetVariable(string line, char activator, char opening, char closing,
            out int startIndex,
            out int length
        )
        {
            startIndex = 0;
            length = 0;
            var activatorIndex = line.IndexOf(activator);
            if (activatorIndex < 0) return false;
            var openingIndex = line.IndexOf(opening, activatorIndex + 1);
            startIndex = openingIndex + 1;
            if (openingIndex < 0) return false;
            var closingIndex = line.IndexOf(closing, openingIndex);
            length = closingIndex - startIndex;
            return true;
        }
    }
}