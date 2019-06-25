using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motarjem.Core
{
    public class Phrase
    {
    }

    public class NounPhrase : Phrase
    {
    }

    public class Noun : NounPhrase
    {
        public Word word;
    }

    public class DeterminerNoun : NounPhrase
    {
        public Word determiner;
        public NounPhrase right;
    }

    public class ConjNoun : NounPhrase
    {
        public NounPhrase left;
        public Word conjunction;
        public NounPhrase right;
    }

    public class AdjectiveNoun : NounPhrase
    {
        public Word adjective;
        public NounPhrase right;
    }

    public class VerbPhrase : Phrase
    {
    }

    public class Verb : VerbPhrase
    {
        public Word word;
    }

    public class PhrasalVerb : VerbPhrase
    {
        public VerbPhrase action;
        public Word preposition;
    }

    public class ObjectiveVerb : VerbPhrase
    {
        public VerbPhrase action;
        public NounPhrase objectNoun;
    }

    public class SubjectiveVerb : VerbPhrase
    {
        public VerbPhrase toBe;
        public Word status;
    }
}
