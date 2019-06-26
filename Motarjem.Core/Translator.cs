using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motarjem.Core
{
    public static class Translator
    {
        public static Sentence Translate(Sentence s)
        {
            if (s is SimpleSentence)
            {
                s.language = Language.Persian;
                return s;
            }
            else if (s is ConjSentence)
            {
                var ss = s as ConjSentence;
                ss.language = Language.Persian;
                ss.left = Translate(ss.left);
                ss.right = Translate(ss.right);
                return ss;
            }
            return null;
        }
    }
}
