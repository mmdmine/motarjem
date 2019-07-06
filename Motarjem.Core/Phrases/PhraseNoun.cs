using System.Collections.Generic;
using System.Linq;
using Motarjem.Core.Dictionary;

namespace Motarjem.Core.Phrases
{
    internal class PhraseNoun : NounPhrase
    {
        public WordNoun Noun;

        protected override void DisplayEnglish(IDisplay display)
        {
            display.Print(Noun.English);
            display.PrintSpace();
        }

        protected override void DisplayPersian(IDisplay display)
        {
            display.Print(Noun.Persian);
            display.PrintSpace();
        }

        internal static PhraseNoun ParseEnglish(Queue<Word[]> words)
        {
            return new PhraseNoun
            {
                Noun = (WordNoun)words.Dequeue().First(a => a is WordNoun)
            };
        }
    }
}