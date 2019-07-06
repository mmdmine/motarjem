using System.Collections.Generic;
using System.Linq;
using Motarjem.Core.Dictionary;
using Motarjem.Core.Sentences;

namespace Motarjem.Core.Phrases
{
    /// <summary>
    /// Two Noun Phrases connected to each other using Conjunction
    /// </summary>
    internal class PhraseConj : NounPhrase
    {
        public WordConj Conjunction { get; internal set; }
        public NounPhrase Left { get; internal set; }
        public NounPhrase Right { get; internal set; }

        protected override void DisplayEnglish(IDisplay display)
        {
            Left.Display(display, Language.English);

            display.Print(Conjunction.English, FontColor.Gray);
            display.PrintSpace();

            Right.Display(display, Language.English);
        }

        protected override void DisplayPersian(IDisplay display)
        {
            Left.Display(display, Language.Persian);

            display.Print(Conjunction.Persian, FontColor.Gray);
            display.PrintSpace();

            Right.Display(display, Language.Persian);
        }

        /// <summary>
        /// Parse a 'Conduction'-ed Phrase 
        /// </summary>
        /// <param name="left">Phrase before conjunction</param>
        /// <param name="words">Word Queue after conjuction</param>
        /// <returns>Parsed Conjunction Phrase</returns>
        internal static PhraseConj ParseEnglish(NounPhrase left, Queue<Word[]> words)
        {
            return new PhraseConj
            {
                Left = left,
                Conjunction = (WordConj)words.Dequeue().First(word => word is WordConj),
                Right = ParseEnglish(words, child: true)
            };
        }
    }
}