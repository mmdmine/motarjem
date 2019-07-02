using System.Collections.Generic;
using System.Linq;

namespace Motarjem.Core
{
    internal class Token
    {
        public enum Type
        {
            Alphabet,
            Digit,
            Space,
            Dot,
            Comma,
            Exclamation,
            QuestionMark,
            Undefined
        }

        public char Character;

        public Type TokenType;

        private static Type GetTokenType(char c)
        {
            switch (c)
            {
                case '\r':
                case '\n':
                case '\t':
                case ' ':
                    return Type.Space;
                case '.':
                    return Type.Dot;
                case '،':
                case ',':
                    return Type.Comma;
                case '!':
                    return Type.Exclamation;
                case '؟':
                case '?':
                    return Type.QuestionMark;
                default:
                    if (c >= 'a' && c <= 'z' ||
                        c >= 'A' && c <= 'Z')
                        return Type.Alphabet;
                    if (c >= '0' && c <= '9')
                        return Type.Digit;
                    return Type.Undefined;
            }
        }

        public static IEnumerable<Token> Tokenize(IEnumerable<char> chars)
        {
            return from c in chars select new Token {TokenType = GetTokenType(c), Character = c};
        }
    }
}