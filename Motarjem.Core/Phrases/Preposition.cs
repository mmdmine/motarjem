using Motarjem.Core.Dictionary;
using System.Collections.Generic;
using System.Linq;

namespace Motarjem.Core.Phrases
{
    internal class Preposition : NounPhrase
    {
        public Word Prep;
        public NounPhrase Right;

        protected override void DisplayEnglish(IDisplay display)
        {
            throw new System.NotImplementedException();
        }

        protected override void DisplayPersian(IDisplay display)
        {
            throw new System.NotImplementedException();
        }

        public static Preposition ParseEnglish(Queue<Word[]> words)
        {
            return new Preposition
            {
                Prep = words.Dequeue().First(w => w.Pos == PartsOfSpeech.Preposition),
                Right = NounPhrase.ParseEnglish(words)
            };
        }
    }
}
