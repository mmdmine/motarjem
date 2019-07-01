using System.Linq;
using System.Collections.Generic;

namespace Motarjem.Core
{
    public abstract class NounPhrase : Phrase
    {
        internal static NounPhrase ParseEnglish(Queue<Word[]> words, bool child = false)
        {
            NounPhrase result;
            switch (words.Peek()[0].pos)
            {
                // Det. + Noun e.g. the book
                case PartOfSpeech.Determiner:
                    result = DeterminerNoun.ParseEnglish(words);
                    break;
                // Adjective + Noun e.g. small book
                case PartOfSpeech.Adjective:
                    result = AdjectiveNoun.ParseEnglish(words);
                    break;
                case PartOfSpeech.Pronoun:
                case PartOfSpeech.Noun:
                case PartOfSpeech.ProperNoun:
                    result = Noun.ParseEnglish(words);
                    break;
                default:
                    throw new UnexpectedWord(words.Dequeue()[0].english);
            }
            // Noun + Conj. + Noun e.g. Ali and Reza
            if (words.Any() &&
                words.Peek()[0].pos == PartOfSpeech.Conjunction &&
                !child)
            {
                // TODO: Ambigous:
                // [Ali wrote [books and letters]] and [Reza read them].
                result = ConjNoun.ParseEnglish(result, words);
            }
            return result;
        }
    }

    public class Noun : NounPhrase
    {
        public Word word;

        protected override void DisplayEnglish(IDisplay display)
        {
            if (word.pos == PartOfSpeech.Pronoun)
            {
                display.Print(word.english, FontColor.Blue, FontStyle.Italic);
            }
            else if (word.pos == PartOfSpeech.ProperNoun)
            {
                display.Print(word.english, FontColor.Black, FontStyle.Bold);
            }
            else
            {
                display.Print(word.english);
            }
            display.PrintSpace();
        }

        protected override void DisplayPersian(IDisplay display)
        {
            if (word.pos == PartOfSpeech.Pronoun)
            {
                display.Print(word.persian, FontColor.Blue, FontStyle.Italic);
            }
            else if (word.pos == PartOfSpeech.ProperNoun)
            {
                display.Print(word.persian, FontColor.Black, FontStyle.Bold);
            }
            else
            {
                display.Print(word.persian);
            }
            display.PrintSpace();
        }

        internal static Noun ParseEnglish(Queue<Word[]> words)
        {
            return new Noun
            {
                word = words.Dequeue().First(a => a.IsNoun)
            };
        }
    }

    public class DeterminerNoun : NounPhrase
    {
        public Word determiner;
        public NounPhrase right;

        protected override void DisplayEnglish(IDisplay display)
        {
            display.Print(determiner.english, FontColor.LightBlue);
            display.PrintSpace();

            right.Display(display, Language.English);
        }

        protected override void DisplayPersian(IDisplay display)
        {
            display.Print(determiner.persian, FontColor.LightBlue);
            display.PrintSpace();

            right.Display(display, Language.Persian);
        }

        internal static DeterminerNoun ParseEnglish(Queue<Word[]> words)
        {
            return new DeterminerNoun
            {
                determiner = words.Dequeue().First(a => a.pos == PartOfSpeech.Determiner),
                right = ParseEnglish(words, true)
            };
        }
    }

    public class ConjNoun : NounPhrase
    {
        public NounPhrase left;
        public Word conjunction;
        public NounPhrase right;

        protected override void DisplayEnglish(IDisplay display)
        {
            left.Display(display, Language.English);

            display.Print(conjunction.english, FontColor.Gray);
            display.PrintSpace();

            right.Display(display, Language.English);
        }

        protected override void DisplayPersian(IDisplay display)
        {
            left.Display(display, Language.Persian);

            display.Print(conjunction.persian, FontColor.Gray);
            display.PrintSpace();

            right.Display(display, Language.Persian);
        }

        internal static ConjNoun ParseEnglish(NounPhrase left, Queue<Word[]> words)
        {
            return new ConjNoun
            {
                left = left,
                conjunction = words.Dequeue().First(a => a.pos == PartOfSpeech.Conjunction),
                right = ParseEnglish(words, true)
            };
        }
    }

    public class AdjectiveNoun : NounPhrase
    {
        public Word adjective;
        public NounPhrase right;

        protected override void DisplayEnglish(IDisplay display)
        {
            display.Print(adjective.english, FontColor.LightRed);
            display.PrintSpace();

            right?.Display(display, Language.English);
        }

        protected override void DisplayPersian(IDisplay display)
        {
            right?.Display(display, Language.Persian);

            display.Print(adjective.persian, FontColor.LightRed);
            display.PrintSpace();
        }

        internal static AdjectiveNoun ParseEnglish(Queue<Word[]> words)
        {
            var adj = new AdjectiveNoun { adjective = words.Dequeue().First(a => a.pos == PartOfSpeech.Adjective) };
            if (words.Any()
                && words.Peek().Any(a => a.IsNoun))
                adj.right = ParseEnglish(words, true);
            return adj;
        }
    }
}
