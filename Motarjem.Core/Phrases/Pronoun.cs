using Motarjem.Core.Dictionary;
using System.Collections.Generic;
using System.Linq;

namespace Motarjem.Core.Phrases
{
    internal class PronounPhrase : NounPhrase
    {
        public Word Pronoun;

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

        public static PronounPhrase ParseEnglish(Queue<Word[]> words)
        {
            return new PronounPhrase { Pronoun = words.Dequeue().First(w => w.Pos == PartsOfSpeech.Pronoun) };
        }
    }
}
