using System.Linq;
using System.Collections.Generic;
using Motarjem.Core.Dictionary;

namespace Motarjem.Core.Phrases
{
    internal class Noun : NounPhrase
    {
        public Word Word;

        protected override void DisplayEnglish(IDisplay display)
        {
            if (Word.Pos == PartsOfSpeech.Pronoun)
            {
                display.Print(Word.English, FontColor.Blue, FontStyle.Italic);
            }
            else if (Word.Pos == PartsOfSpeech.ProperNoun)
            {
                display.Print(Word.English, FontColor.Black, FontStyle.Bold);
            }
            else
            {
                display.Print(Word.English);
            }
            display.PrintSpace();
        }

        protected override void DisplayPersian(IDisplay display)
        {
            if (Word.Pos == PartsOfSpeech.Pronoun)
            {
                display.Print(Word.Persian, FontColor.Blue, FontStyle.Italic);
            }
            else if (Word.Pos == PartsOfSpeech.ProperNoun)
            {
                display.Print(Word.Persian, FontColor.Black, FontStyle.Bold);
            }
            else
            {
                display.Print(Word.Persian);
            }
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
