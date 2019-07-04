using System.Collections.Generic;
using System.Linq;
using Motarjem.Core.Dictionary;

namespace Motarjem.Core.Phrases
{
    internal class Noun : NounPhrase
    {
        public Word Word;

        protected override void DisplayEnglish(IDisplay display)
        {
            display.Print(Word.English);
            display.PrintSpace();
        }

        protected override void DisplayPersian(IDisplay display)
        {
            display.Print(Word.Persian);
            display.PrintSpace();
        }

        internal static Noun ParseEnglish(Queue<Word[]> words)
        {
            return new Noun
            {
                Word = words.Dequeue().First(a => a.IsNoun)
            };
        }
    }
}