using System.Linq;
using System.Collections.Generic;
using Motarjem.Core.Dictionary;

namespace Motarjem.Core.Phrases
{
    internal class Noun : NounPhrase
    {
        public Word word;

        protected override void DisplayEnglish(IDisplay display)
        {
            if (word.pos == PartsOfSpeech.Pronoun)
            {
                display.Print(word.english, FontColor.Blue, FontStyle.Italic);
            }
            else if (word.pos == PartsOfSpeech.ProperNoun)
            {
                display.Print(word.english, FontColor.Black, FontStyle.Bold);
            }
            else
            {
                display.Print(word.english);
            }
            display.PrintSpace();
        }

        protected override void DisplayPersian(IDisplay display)
        {
            if (word.pos == PartsOfSpeech.Pronoun)
            {
                display.Print(word.persian, FontColor.Blue, FontStyle.Italic);
            }
            else if (word.pos == PartsOfSpeech.ProperNoun)
            {
                display.Print(word.persian, FontColor.Black, FontStyle.Bold);
            }
            else
            {
                display.Print(word.persian);
            }
            display.PrintSpace();
        }

        internal static Noun ParseEnglish(Queue<Word[]> words)
        {
            return new Noun
            {
                word = words.Dequeue().First(a => a.IsNoun)
            };
        }
    }
}
