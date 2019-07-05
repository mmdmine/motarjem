using System;
using System.Collections.Generic;
using System.Linq;
using Motarjem.Core.Dictionary;
using Motarjem.Core.Phrases;

namespace Motarjem.Core.Sentences
{
    internal class SimpleSentence : Sentence
    {
        private NounPhrase _np;
        private VerbPhrase _vp;

        public override void Display(IDisplay display)
        {
            if (_np != null)
                _np.Display(display, Language);
            _vp.Display(display, Language);

            display.Print(".");
            display.PrintSpace();
        }

        public override Sentence Translate()
        {
            switch (Language)
            {
                case Language.English:
                    return new SimpleSentence
                    {
                        Language = Language.Persian,
                        _np = _np, //np.Translate()
                        _vp = _vp //vp.Translate()
                    };
                case Language.Persian:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal new static SimpleSentence ParseEnglish(Queue<Word[]> words)
        {
            var sentence = new SimpleSentence
            {
                Language = Language.English,
            };
            if (words.Peek().Any(w => 
                                w.IsNoun ||
                                w.Pos == PartsOfSpeech.Adjective ||
                                w.Pos == PartsOfSpeech.Determiner))
                sentence._np = NounPhrase.ParseEnglish(words);
            sentence._vp = VerbPhrase.ParseEnglish(words, FindSubject(sentence._np));
            return sentence;
        }

        private static Word FindSubject(NounPhrase np)
        {
            if (np == null) // It's a command
                            // TODO: Is it plural or singular?
                return new Word { Person = Person.Second };

            if (np is ProperNoun)
                return new Word { Count = PersonCount.Singular, Person = Person.Third };

            var pronoun = np as PronounPhrase;
            if (pronoun != null)
                return pronoun.Pronoun;

            var det = np as DeterminerNominal;
            if (det != null)
                return FindSubject(det.Right);

            var adj = np as AdjectiveNoun;
            if (adj != null)
                return FindSubject(adj.Right);

            var noun = np as Noun;
            if (noun != null)
                return new Word
                {
                    Count = noun.Word.Count,
                    Sex = noun.Word.Sex,
                    Person = Person.Third
                };

            var conj = np as ConjNoun;
            if (conj != null)
            {
                var left = FindSubject(conj.Left);
                var right = FindSubject(conj.Right);
                // 1st Person:
                if (left.Person == Person.First ||
                    right.Person == Person.First)
                    return new Word { Person = Person.First, Count = PersonCount.Plural };
                // 2nd Person:
                if (left.Person == Person.Second ||
                    right.Person == Person.Second)
                    return new Word { Person = Person.Second, Count = PersonCount.Plural };
                // 3rd Person:
                return new Word { Person = Person.Third, Count = PersonCount.Plural };
            }

            var nominal = np as Nominal;
            if (nominal == null)
                throw new Exception();
            if (nominal.Left != null) return FindSubject(nominal.Left);
            return FindSubject(nominal.Right);
        }
    }
}