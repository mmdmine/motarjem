using Motarjem.Core.Dictionary;
using System.Collections.Generic;
using System.Linq;

namespace Motarjem.Core.Phrases
{
    internal class PhrasePronoun : NounPhrase
    {
        public WordPronoun Pronoun { get; internal set; }

        protected override void DisplayEnglish(IDisplay display)
        {
            display.Print(Pronoun.English, FontColor.Blue, FontStyle.Italic);
            display.PrintSpace();
        }

        protected override void DisplayPersian(IDisplay display)
        {
            display.Print(Pronoun.Persian, FontColor.Blue, FontStyle.Italic);
            display.PrintSpace();
        }

        public static PhrasePronoun ParseEnglish(Queue<Word[]> words)
        {
            return new PhrasePronoun { Pronoun = (WordPronoun)words.Dequeue().First(word => word is WordPronoun) };
        }
    }
}
