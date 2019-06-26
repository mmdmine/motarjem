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

    public class Sentence
    {
        public Language language;

        public void Display(IDisplay display)
        {
            if (this is ConjSentence)
            {
                var conj = this as ConjSentence;
                conj.left.Display(display);
                display.Print(language == Language.English ? conj.conj.english : conj.conj.persian, FontColor.LightBlue);
                conj.right.Display(display);
            }
            else if (this is SimpleSentence)
            {
                var ss = this as SimpleSentence;
                ss.np.Display(display, language);
                ss.vp.Display(display, language);
                display.Print(".");
                display.PrintSpace();
            }
            else
            {
                display.Print("Undefined Sentence: " + GetType().FullName, FontColor.Red);
            }
        }
    }

    public class SimpleSentence : Sentence
    {
        public NounPhrase np;
        public VerbPhrase vp;
    }

    public class ConjSentence : Sentence
    {
        public Sentence left;
        public Word conj;
        public Sentence right;
    }
}
