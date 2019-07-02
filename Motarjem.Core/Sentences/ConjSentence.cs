using System;
using System.Collections.Generic;
using System.Linq;
using Motarjem.Core.Dictionary;

namespace Motarjem.Core
{
    internal class ConjSentence : Sentence
    {
        public Sentence Left;
        public Word Conj;
        public Sentence Right;

        public override void Display(IDisplay display)
        {
            Left.Display(display);

            display.Print(Language == Language.English ? Conj.English : Conj.Persian, FontColor.Gray);
            display.PrintSpace();

            Right.Display(display);
        }

        public override Sentence Translate()
        {
            switch (Language)
            {
                case Language.English:
                    return new ConjSentence
                    {
                        Language = Language.Persian,
                        Left = Left.Translate(),
                        Right = Right.Translate(),
                        Conj = Conj
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
                Left = left,
                Conj = words.Dequeue().First(a => a.Pos == PartsOfSpeech.Conjunction),
            };
            conj.Right = ParseEnglish(words);
            return conj;
        }
    }
}
