using System.Collections.Generic;
using System.Linq;
using Motarjem.Core.Dictionary;

namespace Motarjem.Core.Phrases
{
    internal class DeterminerNominal : NounPhrase
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

        internal static DeterminerNominal ParseEnglish(Queue<Word[]> words)
        {
            return new DeterminerNominal
            {
                Determiner = words.Dequeue().First(a => a.Pos == PartsOfSpeech.Determiner),
                Right = words.Peek().Any(w => w.Pos == PartsOfSpeech.Adjective) ? 
                        (NounPhrase)AdjectiveNoun.ParseEnglish(words) :
                        Nominal.ParseEnglish(words)
            };
        }
    }
}