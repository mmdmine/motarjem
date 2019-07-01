using System;
using System.Collections.Generic;
using Motarjem.Core.Phrases;
using Motarjem.Core.Dictionary;

namespace Motarjem.Core
{
    internal class SimpleSentence : Sentence
    {
        public NounPhrase np;
        public VerbPhrase vp;

        public override void Display(IDisplay display)
        {
            np.Display(display, language);
            vp.Display(display, language);

            display.Print(".");
            display.PrintSpace();
        }

        public override Sentence Translate()
        {
            switch (language)
            {
                case Language.English:
                    return new SimpleSentence
                    {
                        language = Language.Persian,
                        np = np, //np.Translate()
                        vp = vp, //vp.Translate()
                    };
                case Language.Persian:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal new static SimpleSentence ParseEnglish(Queue<Word[]> words)
        {
            Word FindSubject(NounPhrase np)
            {
                if (np is Noun)
                {
                    var noun = np as Noun;
                    return new Word
                    {
                        count = noun.word.count,
                        person = noun.word.pos == PartsOfSpeech.Pronoun ? noun.word.person : Person.Third,
                    };
                }
                if (np is DeterminerNoun)
                {
                    // TODO: 'a' or 'an' are singular, 'some' and ... are plural
                    var dn = np as DeterminerNoun;
                    return FindSubject(dn.right);
                }
                if (np is AdjectiveNoun)
                {
                    var an = np as AdjectiveNoun;
                    return FindSubject(an.right);
                }
                if (np is ConjNoun)
                {
                    var cn = np as ConjNoun;
                    var subject = new Word
                    {
                        count = PersonCount.Plural,
                    };
                    var left = FindSubject(cn.left);
                    var right = FindSubject(cn.right);
                    if (left.pos == PartsOfSpeech.Pronoun)
                    {
                        subject.person = left.person;
                    }
                    if (right.pos == PartsOfSpeech.Pronoun)
                    {
                        subject.person = right.person;
                    }
                    if (subject.person == Person.All)
                    {
                        subject.person = Person.Third;
                    }
                    return subject;
                }
                return null;
            }

            var sentence = new SimpleSentence
            {
                language = Language.English,
                np = NounPhrase.ParseEnglish(words)
            };
            sentence.vp = VerbPhrase.ParseEnglish(words, FindSubject(sentence.np));
            return sentence;
        }
    }
}
