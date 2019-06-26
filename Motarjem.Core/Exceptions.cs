using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motarjem.Core
{
    public class MotarjemException : Exception
    {
        public string MessageFa { get; }

        public MotarjemException(string english, string persian) : base(english)
        {
            MessageFa = persian;
        }
    }

    public class UnexpectedEnd : MotarjemException
    {
        public UnexpectedEnd() : 
            base("Unexpected end of sentence", "پایان غیر منتظره جمله")
        {
        }
    }

    public class UnexpectedWord : MotarjemException
    {
        public UnexpectedWord(string word) :
            base("Unexpected word '" + word + "'", "واژه غیرمنتظره '" + word + "'")
        {
        }
    }

    public class UndefinedWord : MotarjemException
    {
        public UndefinedWord(string word) :
            base("Undefined word '" + word + "'", "واژه تعریف نشده '" + word + "'")
        {
        }
    }

    public class GrammerError : MotarjemException
    {
        public GrammerError(string word) :
            base($"Grammer Error at '{word}'", $"خطای گرامری در '{word}'")
        {
        }
    }
}
