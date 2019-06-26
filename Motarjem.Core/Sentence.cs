using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motarjem.Core
{
    public enum Language
    {
        English,
        Persian,
    };

    public abstract class Sentence
    {
        public Language language;

        public abstract void Display(IDisplay display);
    }

    public class SimpleSentence : Sentence
    {
        public NounPhrase np;
        public VerbPhrase vp;

        public override void Display(IDisplay display)
        {
            np.Display(display, language);
            vp.Display(display, language);

            display.Print(".");
            display.PrintSpace();
        }
    }

    public class ConjSentence : Sentence
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
    }
}
