using System;
using System.Collections.Generic;
using System.Linq;
using Motarjem.Core.Dictionary;
using Motarjem.Core.Sentences;

namespace Motarjem.Core
{
    public enum Language
    {
        English,
        Persian
    }

    public abstract class Sentence
    {
        protected Language Language;

        public abstract void Display(IDisplay display);
        public abstract Sentence Translate();

        public static IEnumerable<Sentence> ParseEnglish(IEnumerable<char> text)
        {
            var tokens = new Queue<Token>(Token.Tokenize(text));
            var words = new Queue<Word[]>();
            while (tokens.Any())
                switch (tokens.Peek().TokenType)
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

        internal static Sentence ParseEnglish(Queue<Word[]> words)
        {
            // TODO: if + S1 + ',' + S2

            // Simple Sentence: Noun Phrase + Verb Phrase
            Sentence sentence = SimpleSentence.ParseEnglish(words);

            // Conjunction Sentence: S1 + and + S2
            if (words.Any() && words.Peek()[0].Pos == PartsOfSpeech.Conjunction)
                sentence = ConjSentence.ParseEnglish(sentence, words);

            // Is any word left?
            if (words.Any())
                throw new UnexpectedWord(words.Dequeue()[0].English);

            return sentence;
        }
    }
}