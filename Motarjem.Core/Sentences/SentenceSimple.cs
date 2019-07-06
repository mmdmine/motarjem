using System;
using System.Collections.Generic;
using System.Linq;
using Motarjem.Core.Dictionary;
using Motarjem.Core.Phrases;

namespace Motarjem.Core.Sentences
{
    /// <summary>
    /// Simple Sentence: Noun Phrase + Verb Phrase
    /// </summary>
    internal class SentenceSimple : Sentence
    {
        public NounPhrase NounPhrase { get; internal set; }
        public VerbPhrase VerbPhrase { get; internal set; }

        public override void Display(IDisplay display)
        {
            NounPhrase?.Display(display, Language);
            VerbPhrase.Display(display, Language);

            display.Print(".");
            display.PrintSpace();
        }

        public override Sentence Translate()
        {
            switch (Language)
            {
                case Language.English:
                    return new SentenceSimple
                    {
                        Language = Language.Persian,
                        NounPhrase = NounPhrase, // TODO: call NounPhrase.Translate() 
                        VerbPhrase = VerbPhrase, // TODO: call VerbPhrase.Translate()
                    };
                case Language.Persian:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal new static SentenceSimple ParseEnglish(Queue<Word[]> words)
        {
            var sentence = new SentenceSimple
            {
                Language = Language.English,
            };
            if (words.Peek().Any(word => word is WordNoun ||
                                         word is WordAdj  ||
                                         word is WordDet))
                sentence.NounPhrase = NounPhrase.ParseEnglish(words);
            sentence.VerbPhrase = VerbPhrase.ParseEnglish(words, FindSubject(sentence.NounPhrase));
            return sentence;
        }

        /// <summary>
        /// Find subject of sentence by giving sentence's Noun Phrase
        /// </summary>
        /// <param name="np">Noun Phrase of the sentence</param>
        /// <returns>Subject of sentence</returns>
        /// <exception cref="GrammarError">For unexpected Noun Phrase type</exception>
        private static WordPronoun FindSubject(NounPhrase np)
        {
            switch (np)
            {
                case null:
                    // Sentence is a command
                    // TODO: Is subject plural or singular?
                    return new WordPronoun { Person = Person.Second };
                case PhraseProperNoun _:
                    return new WordPronoun { Count = PersonCount.Singular, Person = Person.Third };
                case PhrasePronoun pronoun:
                    return pronoun.Pronoun;
                case PhraseDet det:
                    return FindSubject(det.Right);
                case PhraseAdjective adj:
                    return FindSubject(adj.Right);
                case PhraseNoun noun:
                    return new WordPronoun
                    {
                        Person = Person.Third,
                        Count = noun.Noun.Count,
                    };
                case PhraseConj conj:
                {
                    var left = FindSubject(conj.Left);
                    var right = FindSubject(conj.Right);
                    // 1st Person:
                    if (left.Person == Person.First ||
                        right.Person == Person.First)
                        return new WordPronoun { Person = Person.First, Count = PersonCount.Plural };
                    // 2nd Person:
                    if (left.Person == Person.Second ||
                        right.Person == Person.Second)
                        return new WordPronoun { Person = Person.Second, Count = PersonCount.Plural };
                    // 3rd Person:
                    return new WordPronoun { Person = Person.Third, Count = PersonCount.Plural };
                }
                case PhraseNominal nominal:
                    return FindSubject(nominal.Left ?? nominal.Right);
                default:
                    throw new GrammarError("Finding Subject");
            }
        }
    }
}