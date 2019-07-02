using System.Linq;
using System.Collections.Generic;
using Motarjem.Core.Dictionary;

namespace Motarjem.Core.Phrases
{
    internal class AdjectiveNoun : NounPhrase
    {
        public Word Adjective;
        public NounPhrase Right;

        protected override void DisplayEnglish(IDisplay display)
        {
            display.Print(Adjective.English, FontColor.LightRed);
            display.PrintSpace();

            Right?.Display(display, Language.English);
        }

        protected override void DisplayPersian(IDisplay display)
        {
            Right?.Display(display, Language.Persian);

            display.Print(Adjective.Persian, FontColor.LightRed);
            display.PrintSpace();
        }

        internal static AdjectiveNoun ParseEnglish(Queue<Word[]> words)
        {
            var adj = new AdjectiveNoun { Adjective = words.Dequeue().First(a => a.Pos == PartsOfSpeech.Adjective) };
            if (words.Any()
                && words.Peek().Any(a => a.IsNoun))
                adj.Right = ParseEnglish(words, true);
            return adj;
        }
    }
}
