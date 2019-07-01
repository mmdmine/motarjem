using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motarjem.Core
{
    public class Word
    {
        public string english = "";
        public string persian = "";
        public string persian_2 = "";
        public string persian_verb_identifier = "";
        public PartOfSpeech pos = PartOfSpeech.Noun;
        public Person person = Person.All;
        public PersonCount count = PersonCount.All;
        public PersonSex sex = PersonSex.All;
        public PronounType pronounType = PronounType.None;
        public VerbTense tense = VerbTense.None;

        public bool IsNoun
        {
            get
            {
                return pos == PartOfSpeech.Noun || pos == PartOfSpeech.Pronoun || pos == PartOfSpeech.ProperNoun;
            }
        }

        public bool IsVerb
        {
            get
            {
                return pos == PartOfSpeech.Verb || pos == PartOfSpeech.AuxiliaryVerb || pos == PartOfSpeech.ToBe;
            }
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
            var matches = Dictionary.LookupEn(str.ToString());
            if (!matches.Any())
                throw new UndefinedWord(str.ToString());
            return matches.ToArray();
        }
    }

    public enum PartOfSpeech
    {
        Noun, Pronoun, ProperNoun,
        Verb, AuxiliaryVerb, ToBe,
        Adjective, Adverb,
        Conjunction, Determiner,
        Preposition,
        Number,
    }

    public enum Person
    {
        All, First, Second, Third
    }

    public enum PersonCount
    {
        All, Singular, Plural
    }

    public enum PersonSex
    {
        All, Male, Female
    }

    public enum VerbTense
    {
        None,
        Present,
        Past,
        PastParticiple,
    }

    public enum PronounType
    {
        None,
        Subjective,
        Objective
    }
}
