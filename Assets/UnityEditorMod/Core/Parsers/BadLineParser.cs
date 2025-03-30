namespace UnityEditorMod.Core.Parsers
{
    public static class BadLineParser
    {
        public static bool TryGetVariable(string line, char activator, char opening, char closing,
            out int startIndex,
            out int length
        )
        {
            var activatorIndex = line.IndexOf(activator);
            startIndex = 0;
            length = 0;
            var openingIndex = activatorIndex + 1;
            if (activatorIndex >= 0 && openingIndex < line.Length && line[openingIndex] == opening)
            {
                for (int i = openingIndex; i < line.Length; i++)
                {
                    if (line[i] != closing)
                    {
                        length++;
                        continue;
                    }

                    startIndex = openingIndex;
                    return true;
                }
            }

            return false;
        }
    }
}