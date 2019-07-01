using System;

namespace Motarjem.Core
{
    public abstract class MotarjemException : Exception
    {
        public string MessageFa { get; }

        public MotarjemException(string english, string persian) : base(english)
        {
            MessageFa = persian;
        }
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

    internal class GrammerError : MotarjemException
    {
        public GrammerError(string word) :
            base($"Grammer Error at '{word}'", $"خطای گرامری در '{word}'")
        {
        }
    }
}
