using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motarjem.Core.Dictionary
{
    public class Word
    {
        private static EnglishDictionary _dictionary;
        
        internal string English = "";
        internal string Persian = "";
        internal string Persian2 = "";
        internal string PersianVerbIdentifier = "";
        internal PartsOfSpeech Pos = PartsOfSpeech.Noun;
        internal Person Person = Person.All;
        internal PersonCount Count = PersonCount.All;
        internal PersonSex Sex = PersonSex.All;
        internal PronounType PronounType = PronounType.None;
        internal VerbTense Tense = VerbTense.None;

        public bool IsNoun =>
            Pos == PartsOfSpeech.Noun ||
            Pos == PartsOfSpeech.Pronoun ||
            Pos == PartsOfSpeech.ProperNoun;

        public bool IsVerb =>
            Pos == PartsOfSpeech.Verb ||
            Pos == PartsOfSpeech.AuxiliaryVerb ||
            Pos == PartsOfSpeech.ToBe;

        public static void OpenDictionary(IDictionaryFile file)
        {
            _dictionary = new EnglishDictionary(file);
        }

        internal static Word[] ParseEnglish(Queue<Token> tokens)
        {
            var str = new StringBuilder();
            while (tokens.Any())
                if (tokens.Peek().TokenType == Token.Type.Alphabet ||
                    tokens.Peek().TokenType == Token.Type.Digit)
                {
                    str.Append(tokens.Dequeue().Character);
                }
                else
                {
                    if (tokens.Peek().TokenType == Token.Type.Space)
                        tokens.Dequeue();
                    break;
                }

            var matches = _dictionary.Lookup(str.ToString()).ToArray();
            if (!matches.Any())
                throw new UndefinedWord(str.ToString());
            return matches;
        }

        public Word Clone()
        {
            return new Word
            {
                English = English,
                Persian = Persian,
                Persian2 = Persian2,
                PersianVerbIdentifier = PersianVerbIdentifier,
                Pos = Pos,
                Person = Person,
                Count = Count,
                Sex = Sex,
                Tense = Tense,
            };
        }
    }

    internal enum PartsOfSpeech
    {
        Noun,
        Pronoun,
        ProperNoun,
        Verb,
        AuxiliaryVerb,
        ToBe,
        Adjective,
        Adverb,
        Conjunction,
        Determiner,
        Preposition,
        Number
    }

    internal enum Person
    {
        All,
        First,
        Second,
        Third
    }

    internal enum PersonCount
    {
        All,
        Singular,
        Plural
    }

    internal enum PersonSex
    {
        All,
        Male,
        Female
    }

    internal enum VerbTense
    {
        None,
        Present,
        Past,
        PastParticiple
    }

    internal enum PronounType
    {
        None,
        Subjective,
        Objective
    }
}