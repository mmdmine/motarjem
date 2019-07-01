using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motarjem.Core.Dictionary
{
    public class Word
    {
        internal string english = "";
        internal string persian = "";
        internal string persian_2 = "";
        internal string persian_verb_identifier = "";
        internal PartsOfSpeech pos = PartsOfSpeech.Noun;
        internal Person person = Person.All;
        internal PersonCount count = PersonCount.All;
        internal PersonSex sex = PersonSex.All;
        internal PronounType pronounType = PronounType.None;
        internal VerbTense tense = VerbTense.None;

        public bool IsNoun
        {
            get
            {
                return pos == PartsOfSpeech.Noun ||
                    pos == PartsOfSpeech.Pronoun ||
                    pos == PartsOfSpeech.ProperNoun;
            }
        }

        public bool IsVerb
        {
            get
            {
                return pos == PartsOfSpeech.Verb ||
                    pos == PartsOfSpeech.AuxiliaryVerb ||
                    pos == PartsOfSpeech.ToBe;
            }
        }

        public static void OpenDictionary(IDictionaryFile file)
        {
            _dictionary = new EnglishDictionary(file);
        }

        internal static Word[] ParseEnglish(Queue<Token> tokens)
        {
            var str = new StringBuilder();
            while (tokens.Any())
            {
                if (tokens.Peek().type == Token.Type.Alphabet ||
                    tokens.Peek().type == Token.Type.Digit)
                    str.Append(tokens.Dequeue().charactor);
                else
                {
                    if (tokens.Peek().type == Token.Type.Space)
                        tokens.Dequeue();
                    break;
                }
            }
            var matches = _dictionary.Lookup(str.ToString());
            if (!matches.Any())
                throw new UndefinedWord(str.ToString());
            return matches.ToArray();
        }

        private static EnglishDictionary _dictionary;
    }

    internal enum PartsOfSpeech
    {
        Noun, Pronoun, ProperNoun,
        Verb, AuxiliaryVerb, ToBe,
        Adjective, Adverb,
        Conjunction, Determiner,
        Preposition,
        Number,
    }

    internal enum Person
    {
        All, First, Second, Third
    }

    internal enum PersonCount
    {
        All, Singular, Plural
    }

    internal enum PersonSex
    {
        All, Male, Female
    }

    internal enum VerbTense
    {
        None,
        Present,
        Past,
        PastParticiple,
    }

    internal enum PronounType
    {
        None,
        Subjective,
        Objective
    }
}
