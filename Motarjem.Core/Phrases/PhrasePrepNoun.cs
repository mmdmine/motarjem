using Motarjem.Core.Dictionary;
using System.Collections.Generic;
using System.Linq;

namespace Motarjem.Core.Phrases
{
    internal class PhrasePrepNoun : NounPhrase
    {
        public WordPrep Prep { get; internal set; }
        public NounPhrase Right { get; internal set; }

        protected override void DisplayEnglish(IDisplay display)
        {
            throw new System.NotImplementedException();
        }

        protected override void DisplayPersian(IDisplay display)
        {
            throw new System.NotImplementedException();
        }

        public static PhrasePrepNoun ParseEnglish(Queue<Word[]> words)
        {
            return new PhrasePrepNoun
            {
                Prep = (WordPrep)words.Dequeue().First(word => word is WordPrep),
                Right = NounPhrase.ParseEnglish(words)
            };
        }
    }
}
