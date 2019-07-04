using Motarjem.Core.Dictionary;
using System.Collections.Generic;
using System.Linq;

namespace Motarjem.Core.Phrases
{
    internal class Nominal : NounPhrase
    {
        public Nominal Left;
        public NounPhrase Right;

        protected override void DisplayEnglish(IDisplay display)
        {
            if (Left != null)
                Left.Display(display, Language.English);
            Right.Display(display, Language.English);
        }

        protected override void DisplayPersian(IDisplay display)
        {
            if (Left != null)
                Left.Display(display, Language.Persian);
            Right.Display(display, Language.Persian);
        }

        public static Nominal ParseEnglish(Queue<Word[]> words)
        {
            // Noun
            var result = new Nominal { Right = Noun.ParseEnglish(words) };
            if (!words.Any()) return result;
            // Noun + Noun
            if (words.Peek().Any(w => w.IsNoun))
            {
                var result2 = ParseEnglish(words);
                result2.Left = result;
                return result2;
            }
            // Noun + Preposition + Noun Phrase
            else if (words.Peek().Any(w => w.Pos == PartsOfSpeech.Preposition))
            {
                result.Right = Preposition.ParseEnglish(words);
            }
            return result;
        }
    }
}
