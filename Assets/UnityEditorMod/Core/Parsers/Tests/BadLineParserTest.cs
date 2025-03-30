using NUnit.Framework;
using UnityEngine;

namespace UnityEditorMod.Core.Parsers.Tests
{
    internal class BadLineParserTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void BadParserTestSimplePasses()
        {
            const string line = "    \"name\": \"${NAME}\",";
            var found = BadLineParser.TryGetVariable(line, '$', '{', '}', out int startIndex, out int length);
            Assert.True(found);
            Debug.Log(startIndex);
            Debug.Log(length);
            Debug.Log(line.Substring(startIndex, length));
        }
    }
}