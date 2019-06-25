using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motarjem.Core
{
    public class Sentence
    {
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
