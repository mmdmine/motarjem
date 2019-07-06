using System.Collections.Generic;
using System.Linq;
using Motarjem.Core.Dictionary;

namespace Motarjem.Core.Phrases
{
    internal class PhraseProperNoun : NounPhrase
    {
        public WordProperNoun ProperNoun { get; internal set; }

        protected override void DisplayEnglish(IDisplay display)
        {
            display.Print(ProperNoun.English, FontColor.Black, FontStyle.Bold);
            display.PrintSpace();
        }

        protected override void DisplayPersian(IDisplay display)
        {
            display.Print(ProperNoun.Persian, FontColor.Black, FontStyle.Bold);
            display.PrintSpace();
        }
        
        internal static PhraseProperNoun ParseEnglish(Queue<Word[]> words)
        {
            return new PhraseProperNoun
            {
                ProperNoun = (WordProperNoun)words.Dequeue().First(word => word is ProperNounWord)
            };
        }
    }
}
