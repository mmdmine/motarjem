using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motarjem.Core.Dictionary
{
    public abstract class Word
    {
        public abstract PartsOfSpeech Pos { get; }
        public string English { get; internal set; }
        public string Persian { get; internal set; }
        
        private static EnglishDictionary _dictionary;

        /// <summary>
        /// Open a Dictionary File to be used in Parsing Words
        /// </summary>
        /// <param name="file">the Dictionary File to be opened</param>
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
    }

    public class WordNoun : Word
    {
        public override PartsOfSpeech Pos => PartsOfSpeech.Noun;
        
        public PersonCount Count { get; internal set; }
        
        public WordNoun Clone()
        {
            return new WordNoun
            {
                English = English,
                Persian = Persian,
                Count = Count
            };
        }
    }
    
    public sealed class WordPronoun : WordNoun
    {
        public override PartsOfSpeech Pos => PartsOfSpeech.Pronoun;
        
        //public PronounType PronounType { get; internal set; }
        
        public Person Person { get; internal set; }
        public PersonSex Sex { get; internal set; }
    }

    public sealed class ProperNounWord : WordNoun
    {
        public override PartsOfSpeech Pos => PartsOfSpeech.ProperNoun;
    }

    public class WordVerb : Word
    {
        public override PartsOfSpeech Pos => PartsOfSpeech.Verb;
        
        public string Persian2 { get; internal set; }
        public string PersianVerbIdentifier { get; internal set; }
        
        public Person Person { get; internal set; }
        public PersonCount Count { get; internal set; }
        public PersonSex Sex { get; internal set; }
        
        public VerbType VerbType { get; internal set; }
        public VerbTense Tense { get; internal set; }

        public WordVerb Clone()
        {
            return new WordVerb
            {
                English = English,
                Persian = Persian,
                Persian2 = Persian2,
                PersianVerbIdentifier = PersianVerbIdentifier,
                Person = Person,
                Count = Count,
                Sex = Sex,
                VerbType = VerbType,
                Tense = Tense
            };
        }
    }

    public sealed class WordAdj : Word
    {
        public override PartsOfSpeech Pos => PartsOfSpeech.Adjective;
    }

    public sealed class WordConj : Word
    {
        public override PartsOfSpeech Pos => PartsOfSpeech.Conjunction;
    }

    public sealed class WordDet : Word
    {
        public override PartsOfSpeech Pos => PartsOfSpeech.Determiner;

        public PersonCount Count { get; internal set; }
    }

    /// <summary>
    /// Preposition
    /// </summary>
    public sealed class WordPrep : Word
    {
        public override PartsOfSpeech Pos => PartsOfSpeech.Preposition;
    }

    public sealed class WordProperNoun : Word
    {
        public override PartsOfSpeech Pos => PartsOfSpeech.ProperNoun;
    }

    public enum PartsOfSpeech
    {
        Noun,
        Pronoun,
        ProperNoun,
        Verb,
        Adjective,
        Adverb,
        Conjunction,
        Determiner,
        Preposition,
        Number
    }

    public enum Person
    {
        All,
        First,
        Second,
        Third
    }

    public enum PersonCount
    {
        All,
        Singular,
        Plural
    }

    public enum PersonSex
    {
        All,
        Male,
        Female
    }

    public enum VerbType
    {
        Action,
        ToBe,
        Auxiliary,
    }

    public enum VerbTense
    {
        None,
        Present,
        Past,
        PastParticiple
    }

    public enum PronounType
    {
        None,
        Subjective,
        Objective
    }
}