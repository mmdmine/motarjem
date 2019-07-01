using System.Linq;
using System.Collections.Generic;
using Motarjem.Core.Dictionary;

namespace Motarjem.Core.Phrases
{
    internal class AdjectiveNoun : NounPhrase
    {
        public Word adjective;
        public NounPhrase right;

        protected override void DisplayEnglish(IDisplay display)
        {
            display.Print(adjective.english, FontColor.LightRed);
            display.PrintSpace();

            right?.Display(display, Language.English);
        }

        protected override void DisplayPersian(IDisplay display)
        {
            right?.Display(display, Language.Persian);

            display.Print(adjective.persian, FontColor.LightRed);
            display.PrintSpace();
        }

        internal static AdjectiveNoun ParseEnglish(Queue<Word[]> words)
        {
            var adj = new AdjectiveNoun { adjective = words.Dequeue().First(a => a.pos == PartsOfSpeech.Adjective) };
            if (words.Any()
                && words.Peek().Any(a => a.IsNoun))
                adj.right = ParseEnglish(words, true);
            return adj;
        }
    }
}
