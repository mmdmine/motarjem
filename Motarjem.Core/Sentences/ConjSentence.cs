using System;
using System.Collections.Generic;
using System.Linq;
using Motarjem.Core.Dictionary;

namespace Motarjem.Core
{
    internal class ConjSentence : Sentence
    {
        public Sentence left;
        public Word conj;
        public Sentence right;

        public override void Display(IDisplay display)
        {
            left.Display(display);

            display.Print(language == Language.English ? conj.english : conj.persian, FontColor.Gray);
            display.PrintSpace();

            right.Display(display);
        }

        public override Sentence Translate()
        {
            switch (language)
            {
                case Language.English:
                    return new ConjSentence
                    {
                        language = Language.Persian,
                        left = left.Translate(),
                        right = right.Translate(),
                        conj = conj
                    };
                case Language.Persian:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal static ConjSentence ParseEnglish(Sentence left, Queue<Word[]> words)
        {
            var conj = new ConjSentence
            {
                left = left,
                conj = words.Dequeue().First(a => a.pos == PartsOfSpeech.Conjunction),
            };
            conj.right = ParseEnglish(words);
            return conj;
        }
    }
}
