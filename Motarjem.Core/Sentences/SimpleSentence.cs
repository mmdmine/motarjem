using System;
using System.Collections.Generic;
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
                        _vp = _vp, //vp.Translate()
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
                _np = NounPhrase.ParseEnglish(words)
            };
            sentence._vp = VerbPhrase.ParseEnglish(words, FindSubject(sentence._np));
            return sentence;
        }
        
        private static Word FindSubject(NounPhrase np)
        {
            var noun = np as Noun;
            if (noun != null)
            {
                return new Word
                {
                    Count = noun.Word.Count,
                    Person = noun.Word.Pos == PartsOfSpeech.Pronoun ? noun.Word.Person : Person.Third,
                };
            }

            var dn = np as DeterminerNoun;
            if (dn != null)
                // TODO: 'a' or 'an' are singular, 'some' and ... are plural
                return FindSubject(dn.Right);

            var an = np as AdjectiveNoun;
            if (an != null)
                return FindSubject(an.Right);

            var cn = np as ConjNoun;
            if (cn == null) return null;
            var subject = new Word
            {
                Count = PersonCount.Plural,
            };
            var left = FindSubject(cn.Left);
            var right = FindSubject(cn.Right);
            if (left.Pos == PartsOfSpeech.Pronoun)
            {
                subject.Person = left.Person;
            }
            if (right.Pos == PartsOfSpeech.Pronoun)
            {
                subject.Person = right.Person;
            }
            if (subject.Person == Person.All)
            {
                subject.Person = Person.Third;
            }
            return subject;
        }
    }
}
