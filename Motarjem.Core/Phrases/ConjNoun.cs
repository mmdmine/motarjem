using System.Collections.Generic;
using System.Linq;
using Motarjem.Core.Dictionary;

namespace Motarjem.Core.Phrases
{
    internal class ConjNoun : NounPhrase
    {
        public Word Conjunction;
        public NounPhrase Left;
        public NounPhrase Right;

        protected override void DisplayEnglish(IDisplay display)
        {
            Left.Display(display, Language.English);

            display.Print(Conjunction.English, FontColor.Gray);
            display.PrintSpace();

            Right.Display(display, Language.English);
        }

        protected override void DisplayPersian(IDisplay display)
        {
            Left.Display(display, Language.Persian);

            display.Print(Conjunction.Persian, FontColor.Gray);
            display.PrintSpace();

            Right.Display(display, Language.Persian);
        }

        internal static ConjNoun ParseEnglish(NounPhrase left, Queue<Word[]> words)
        {
            return new ConjNoun
            {
                Left = left,
                Conjunction = words.Dequeue().First(a => a.Pos == PartsOfSpeech.Conjunction),
                Right = ParseEnglish(words, true)
            };
        }
    }
}