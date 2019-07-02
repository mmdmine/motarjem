using System.Linq;
using System.Collections.Generic;
using Motarjem.Core.Dictionary;

namespace Motarjem.Core.Phrases
{
    internal class DeterminerNoun : NounPhrase
    {
        public Word Determiner;
        public NounPhrase Right;

        protected override void DisplayEnglish(IDisplay display)
        {
            display.Print(Determiner.English, FontColor.LightBlue);
            display.PrintSpace();

            Right.Display(display, Language.English);
        }

        protected override void DisplayPersian(IDisplay display)
        {
            display.Print(Determiner.Persian, FontColor.LightBlue);
            display.PrintSpace();

            Right.Display(display, Language.Persian);
        }

        internal static DeterminerNoun ParseEnglish(Queue<Word[]> words)
        {
            return new DeterminerNoun
            {
                Determiner = words.Dequeue().First(a => a.Pos == PartsOfSpeech.Determiner),
                Right = ParseEnglish(words, true)
            };
        }
    }
}
