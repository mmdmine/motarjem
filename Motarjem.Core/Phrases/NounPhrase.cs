using System.Linq;
using System.Collections.Generic;
using Motarjem.Core.Dictionary;

namespace Motarjem.Core.Phrases
{
    public abstract class NounPhrase : Phrase
    {
        internal static NounPhrase ParseEnglish(Queue<Word[]> words, bool child = false)
        {
            NounPhrase result;
            switch (words.Peek()[0].pos)
            {
                // Det. + Noun e.g. the book
                case PartsOfSpeech.Determiner:
                    result = DeterminerNoun.ParseEnglish(words);
                    break;
                // Adjective + Noun e.g. small book
                case PartsOfSpeech.Adjective:
                    result = AdjectiveNoun.ParseEnglish(words);
                    break;
                case PartsOfSpeech.Pronoun:
                case PartsOfSpeech.Noun:
                case PartsOfSpeech.ProperNoun:
                    result = Noun.ParseEnglish(words);
                    break;
                default:
                    throw new UnexpectedWord(words.Dequeue()[0].english);
            }
            // Noun + Conj. + Noun e.g. Ali and Reza
            if (words.Any() &&
                words.Peek()[0].pos == PartsOfSpeech.Conjunction &&
                !child)
            {
                // TODO: Ambigous:
                // [Ali wrote [books and letters]] and [Reza read them].
                result = ConjNoun.ParseEnglish(result, words);
            }
            return result;
        }
    }
}
