using System.Collections.Generic;
using System.Linq;
using Motarjem.Core.Sentences;

namespace Motarjem.Core
{
    public static class Parser
    {
        public static IEnumerable<Sentence> Parse(IEnumerable<char> text)
        {
            var tokens = new Queue<Token>(Token.Tokenize(text));
            while (tokens.Any())
            {
                var result = Sentence.ParseEnglish(tokens);
                if (result != null)
                    yield return result;
            }
        }
    }
}