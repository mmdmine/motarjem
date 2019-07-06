using System;
using System.Collections.Generic;
using System.Linq;
using Motarjem.Core.Dictionary;

namespace Motarjem.Core.Sentences
{
    /// <summary>
    /// Two sentence mixed together by Conjunction
    /// </summary>
    internal class SentenceMixed : Sentence
    {
        public WordConj Conj { get; internal set; }
        public Sentence Left { get; internal set; }
        public Sentence Right { get; internal set; }

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
                    return new SentenceMixed
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

        internal static SentenceMixed ParseEnglish(Sentence left, Queue<Word[]> words)
        {
            return new SentenceMixed
            {
                Left = left,
                Conj = (WordConj)words.Dequeue().First(word => word is WordConj),
                Right = ParseEnglish(words)
            };
        }
    }
}