using System.Collections.Generic;
using System.Linq;
using Motarjem.Core.Dictionary;
using Motarjem.Core.Sentences;

namespace Motarjem.Core.Phrases
{
    /// <summary>
    /// Determiner Noun Phrase used in Nominal Phrase
    /// </summary>
    internal class PhraseDet : NounPhrase
    {
        public WordDet Determiner { get; internal set; }
        public NounPhrase Right { get; internal set; }

        protected override void DisplayEnglish(IDisplay display)
        {
            display.Print(Determiner.English, FontColor.LightBlue);
            display.PrintSpace();

            Right.Display(display, Language.English);
        }

        protected override void DisplayPersian(IDisplay display)
        {
            if (!string.IsNullOrWhiteSpace(Determiner.Persian))
            {
                display.Print(Determiner.Persian, FontColor.LightBlue);
                display.PrintSpace();
            }

            Right.Display(display, Language.Persian);
        }

        internal static PhraseDet ParseEnglish(Queue<Word[]> words)
        {
            return new PhraseDet
            {
                Determiner = (WordDet)words.Dequeue().First(word => word is WordDet),
                Right = words.Peek().Any(w => w.Pos == PartsOfSpeech.Adjective) ? 
                        (NounPhrase)PhraseAdjective.ParseEnglish(words) :
                        PhraseNominal.ParseEnglish(words)
            };
        }
    }
}