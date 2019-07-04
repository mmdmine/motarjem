using System.Collections.Generic;
using System.Linq;
using Motarjem.Core.Dictionary;

namespace Motarjem.Core.Phrases
{
    public abstract class NounPhrase : Phrase
    {
        internal static NounPhrase ParseEnglish(Queue<Word[]> words, bool child = false)
        {
            NounPhrase result;
            // Det. + Nominal
            if (words.Peek().Any(w => w.Pos == PartsOfSpeech.Determiner))
                result = DeterminerNominal.ParseEnglish(words);
            // Adjective + Noun e.g. small book
            else if (words.Peek().Any(w => w.Pos == PartsOfSpeech.Adjective))
                result = AdjectiveNoun.ParseEnglish(words);
            // Pronoun
            else if (words.Peek().Any(w => w.Pos == PartsOfSpeech.Pronoun))
                result = PronounPhrase.ParseEnglish(words);
            // Nominal
            else if (words.Peek().Any(w => w.Pos == PartsOfSpeech.Noun))
                result = Nominal.ParseEnglish(words);
            else // TODO: Proper Noun
                throw new UnexpectedWord(words.Dequeue()[0].English);

            // Noun + Conj. + Noun e.g. Ali and Reza
            if (words.Any() &&
                words.Peek()[0].Pos == PartsOfSpeech.Conjunction &&
                !child)
                // TODO: Ambigous:
                // [Ali wrote [books and letters]] and [Reza read them].
                result = ConjNoun.ParseEnglish(result, words);
            return result;
        }
    }
}