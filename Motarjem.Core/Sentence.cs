using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motarjem.Core
{
    public enum Language
    {
        English,
        Persian,
    };

    public abstract class Sentence
    {
        public Language language;

        public abstract void Display(IDisplay display);
        public abstract Sentence Translate();

        public static IEnumerable<Sentence> ParseEnglish(IEnumerable<char> text)
        {
            var tokens = new Queue<Token>(Token.Tokenize(text));
            var words = new Queue<Word[]>();
            while (tokens.Any())
            {
                switch (tokens.Peek().type)
                {
                    case Token.Type.Dot:
                        tokens.Dequeue();
                        yield return ParseEnglish(words);
                        words.Clear();
                        break;
                    case Token.Type.QuestionMark:
                        tokens.Dequeue();
                        throw new NotImplementedException();
                    case Token.Type.Exclamation:
                        tokens.Dequeue();
                        throw new NotImplementedException();
                    case Token.Type.Space:
                        tokens.Dequeue();
                        break;
                    default:
                        words.Enqueue(Word.ParseEnglish(tokens));
                        break;
                }
            }
        }

        internal static Sentence ParseEnglish(Queue<Word[]> words)
        {
            // TODO: if + S1 + ',' + S2
            
            // Simple Sentence: Noun Phrase + Verb Phrase
            Sentence sentence = SimpleSentence.ParseEnglish(words);
            
            // Conjunction Sentence: S1 + and + S2
            if (words.Any() && words.Peek()[0].pos == PartOfSpeech.Conjunction)
            {
                sentence = ConjSentence.ParseEnglish(sentence, words);
            }
            
            // Is any word left?
            if (words.Any())
                throw new UnexpectedWord(words.Dequeue()[0].english);

            return sentence;
        }
    }

    public class SimpleSentence : Sentence
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
                        person = noun.word.pos == PartOfSpeech.Pronoun ? noun.word.person : Person.Third,
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
                    if (left.pos == PartOfSpeech.Pronoun)
                    {
                        subject.person = left.person;
                    }
                    if (right.pos == PartOfSpeech.Pronoun)
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

    public class ConjSentence : Sentence
    {
        public Sentence left;
        public Word conj;
        public Sentence right;

        public override void Display(IDisplay display)
        {
            left.Display(display);

            display.Print(language == Language.English ? conj.english : conj.persian, FontColor.Gray);
            display.PrintSpace();

            right.Display(display);
        }

        public override Sentence Translate()
        {
            switch (language)
            {
                case Language.English:
                    return new ConjSentence
                    {
                        language = Language.Persian,
                        left = left.Translate(),
                        right = right.Translate(),
                        conj = conj
                    };
                case Language.Persian:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal static ConjSentence ParseEnglish(Sentence left, Queue<Word[]> words)
        {
            var conj = new ConjSentence
            {
                left = left,
                conj = words.Dequeue().First(a => a.pos == PartOfSpeech.Conjunction),
            };
            conj.right = ParseEnglish(words);
            return conj;
        }
    }
}
