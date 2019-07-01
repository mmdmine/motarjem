using System.Linq;
using System.Collections.Generic;
using Motarjem.Core.Dictionary;

namespace Motarjem.Core.Phrases
{
    internal class DeterminerNoun : NounPhrase
    {
        public Word determiner;
        public NounPhrase right;

        protected override void DisplayEnglish(IDisplay display)
        {
            display.Print(determiner.english, FontColor.LightBlue);
            display.PrintSpace();

            right.Display(display, Language.English);
        }

        protected override void DisplayPersian(IDisplay display)
        {
            display.Print(determiner.persian, FontColor.LightBlue);
            display.PrintSpace();

            right.Display(display, Language.Persian);
        }

        internal static DeterminerNoun ParseEnglish(Queue<Word[]> words)
        {
            return new DeterminerNoun
            {
                determiner = words.Dequeue().First(a => a.pos == PartsOfSpeech.Determiner),
                right = ParseEnglish(words, true)
            };
        }
    }
}
