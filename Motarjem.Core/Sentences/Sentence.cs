using System;
using System.Collections.Generic;
using System.Linq;
using Motarjem.Core.Dictionary;

namespace Motarjem.Core.Sentences
{
    /// <summary>
    /// A sentence is a collections of Phrases
    /// </summary>
    public abstract class Sentence
    {
        protected Language Language;

        public abstract void Display(IDisplay display);
        public abstract Sentence Translate();

        internal static Sentence ParseEnglish(Queue<Token> tokens)
        {
            var words = new Queue<Word[]>();
            while (tokens.Any() && tokens.Peek().TokenType == Token.Type.Alphabet)
            {
                words.Enqueue(Word.ParseEnglish(tokens));
                if (tokens.Peek().TokenType == Token.Type.Space)
                    tokens.Dequeue();
            }

            switch (tokens.Peek().TokenType)
            {
                case Token.Type.Dot:
                    return ParseEnglish(words);
                case Token.Type.Exclamation:
                    throw new NotImplementedException();
                case Token.Type.QuestionMark:
                    throw new NotImplementedException();
                
                case Token.Type.Comma:
                case Token.Type.Undefined:
                    throw new GrammarError(tokens.Dequeue().Character.ToString());
                case Token.Type.Alphabet:
                case Token.Type.Digit:
                case Token.Type.Space:
                default:
                    throw new Exception(); // TODO: Parser Error
            }
        }

        internal static Sentence ParseEnglish(Queue<Word[]> words)
        {
            // TODO: if + S1 + ',' + S2

            // Simple Sentence: Noun Phrase + Verb Phrase
            Sentence sentence = SentenceSimple.ParseEnglish(words);

            // Conjunction Sentence: S1 + and + S2
            if (words.Any() && words.Peek().Any(word => word is WordConj))
                sentence = SentenceMixed.ParseEnglish(sentence, words);

            // Is any word left?
            if (words.Any())
                throw new UnexpectedWord(words.Dequeue()[0].English);

            return sentence;
        }
    }

    public enum Language
    {
        English,
        Persian
    }
}