using Motarjem.Core.Dictionary;
using Motarjem.Core.Sentences;
using System.Collections.Generic;
using System.Linq;

namespace Motarjem.Core.Phrases
{
    /// <summary>
    /// Nominal Phrases contains Nouns and Prepositions
    /// </summary>
    internal class PhraseNominal : NounPhrase
    {
        public PhraseNominal Left;
        public NounPhrase Right;

        protected override void DisplayEnglish(IDisplay display)
        {
            Left?.Display(display, Language.English);
            Right.Display(display, Language.English);
        }

        protected override void DisplayPersian(IDisplay display)
        {
            Left?.Display(display, Language.Persian);
            Right.Display(display, Language.Persian);
        }

        public static PhraseNominal ParseEnglish(Queue<Word[]> words)
        {
            // Noun
            var result = new PhraseNominal { Right = PhraseNoun.ParseEnglish(words) };
            
            if (!words.Any()) return result;
            
            // Noun + Noun
            if (words.Peek().Any(word => word is WordNoun))
            {
                var result2 = ParseEnglish(words);
                result2.Left = result;
                return result2;
            }
            
            // Noun + Preposition + Noun Phrase
            if (words.Peek().Any(word => word is WordPrep))
            {
                result.Right = PhrasePrepNoun.ParseEnglish(words);
            }
            
            return result;
        }
    }
}
