using System.Linq;
using System.Collections.Generic;
using Motarjem.Core.Dictionary;

namespace Motarjem.Core.Phrases
{
    internal class ConjNoun : NounPhrase
    {
        public NounPhrase left;
        public Word conjunction;
        public NounPhrase right;

        protected override void DisplayEnglish(IDisplay display)
        {
            left.Display(display, Language.English);

            display.Print(conjunction.english, FontColor.Gray);
            display.PrintSpace();

            right.Display(display, Language.English);
        }

        protected override void DisplayPersian(IDisplay display)
        {
            left.Display(display, Language.Persian);

            display.Print(conjunction.persian, FontColor.Gray);
            display.PrintSpace();

            right.Display(display, Language.Persian);
        }

        internal static ConjNoun ParseEnglish(NounPhrase left, Queue<Word[]> words)
        {
            return new ConjNoun
            {
                left = left,
                conjunction = words.Dequeue().First(a => a.pos == PartsOfSpeech.Conjunction),
                right = ParseEnglish(words, true)
            };
        }
    }
}
