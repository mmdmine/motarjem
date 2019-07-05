using System;

namespace Motarjem.Core
{
    [Serializable]
    public abstract class MotarjemException : Exception
    {
        protected MotarjemException()
        {
        }

        protected MotarjemException(string english, string persian) : base(english)
        {
            MessageFa = persian;
        }

        public string MessageFa { get; }
    }

    [Serializable]
    internal class UnexpectedEnd : MotarjemException
    {
        public UnexpectedEnd() :
            base("Unexpected end of sentence", "پایان غیر منتظره جمله")
        {
        }
    }

    [Serializable]
    internal class UnexpectedWord : MotarjemException
    {
        public UnexpectedWord(string word) :
            base("Unexpected word '" + word + "'", "واژه غیرمنتظره '" + word + "'")
        {
        }
    }

    [Serializable]
    internal class UndefinedWord : MotarjemException
    {
        public UndefinedWord(string word) :
            base("Undefined word '" + word + "'", "واژه تعریف نشده '" + word + "'")
        {
        }
    }

    [Serializable]
    internal class GrammarError : MotarjemException
    {
        public GrammarError(string word) :
            base($"Grammer Error at '{word}'", $"خطای گرامری در '{word}'")
        {
        }
    }
}