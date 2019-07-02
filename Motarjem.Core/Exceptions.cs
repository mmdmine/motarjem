using System;

namespace Motarjem.Core
{
    public abstract class MotarjemException : Exception
    {
        protected MotarjemException(string english, string persian) : base(english)
        {
            MessageFa = persian;
        }

        public string MessageFa { get; }
    }

    internal class UnexpectedEnd : MotarjemException
    {
        public UnexpectedEnd() :
            base("Unexpected end of sentence", "پایان غیر منتظره جمله")
        {
        }
    }

    internal class UnexpectedWord : MotarjemException
    {
        public UnexpectedWord(string word) :
            base("Unexpected word '" + word + "'", "واژه غیرمنتظره '" + word + "'")
        {
        }
    }

    internal class UndefinedWord : MotarjemException
    {
        public UndefinedWord(string word) :
            base("Undefined word '" + word + "'", "واژه تعریف نشده '" + word + "'")
        {
        }
    }

    internal class GrammarError : MotarjemException
    {
        public GrammarError(string word) :
            base($"Grammer Error at '{word}'", $"خطای گرامری در '{word}'")
        {
        }
    }
}