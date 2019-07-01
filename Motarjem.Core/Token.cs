using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            Undefined,
        }

        public Type type;
        public char charactor;

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
            }
            if ((c >= 'a' && c <= 'z') ||
                (c >= 'A' && c <= 'Z'))
                return Type.Alphabet;
            if (c >= '0' && c <= '9')
                return Type.Digit;
            return Type.Undefined;
        }

        public static IEnumerable<Token> Tokenize(IEnumerable<char> chars) => from c in chars select new Token { type = GetTokenType(c), charactor = c };
    }
}
