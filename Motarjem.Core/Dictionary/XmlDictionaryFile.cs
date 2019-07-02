using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Motarjem.Core.Dictionary
{
    public class XmlDictionaryFile : IDictionaryFile
    {
        public XmlDictionaryFile(string path)
        {
            Document = XDocument.Load(path);
        }

        private XDocument Document { get; }

        public IEnumerable<Word> Adjectives =>
            from word in Document
                .Element(XName.Get("words"))
                ?.Elements(XName.Get("adjective"))
            select ParseWord(word);

        public IEnumerable<Word> Conjunctions =>
            from word in Document
                .Element(XName.Get("words"))
                ?.Elements(XName.Get("conjunction"))
            select ParseWord(word);

        public IEnumerable<Word> Determiners =>
            from word in Document
                .Element(XName.Get("words"))
                ?.Elements(XName.Get("determiner"))
            select ParseWord(word);

        public IEnumerable<Word> Nouns =>
            from word in Document
                .Element(XName.Get("words"))
                ?.Elements(XName.Get("noun"))
            select ParseWord(word);

        public IEnumerable<Word> Pronouns =>
            from word in Document
                .Element(XName.Get("words"))
                ?.Elements(XName.Get("pronoun"))
            select ParseWord(word);

        public IEnumerable<Word> Verbs =>
            from word in Document
                .Element(XName.Get("words"))
                ?.Elements("verb")
            select ParseWord(word);


        private Word ParseWord(XElement x)
        {
            var word = new Word
            {
                // if there's a 'pos' attribute, use it
                // otherwise use tag name
                Pos = x.Attributes().Any(a => a.Name == "pos")
                    ? GetEnum<PartsOfSpeech>("pos", x)
                    : (PartsOfSpeech) Enum.Parse(typeof(PartsOfSpeech), x.Name.LocalName, true),
                English = GetAttribute("en", x),
                Persian = GetAttribute("fa", x),
                Persian2 = GetAttribute("fav", x),
                PersianVerbIdentifier = GetAttribute("fai", x),
                Person = GetEnum<Person>("person", x),
                Count = GetEnum<PersonCount>("count", x),
                Sex = GetEnum<PersonSex>("sex", x),
                Tense = GetEnum<VerbTense>("tense", x)
            };
            // Use PersonCount.Singular as default value for Nouns
            if (word.Count == PersonCount.All &&
                word.Pos == PartsOfSpeech.Noun)
                word.Count = PersonCount.Singular;
            return word;
        }

        private static string GetAttribute(string attribute, XElement x)
        {
            var matches = x.Attributes(XName.Get(attribute)).ToArray();
            if (matches.Length == 0)
                return null;
            if (matches.Length > 1)
                throw new Exception(); // data error
            return matches[0].Value;
        }

        private static T GetEnum<T>(string attribute, XElement x)
        {
            var value = GetAttribute(attribute, x);
            if (value == null)
                return default(T);
            return (T) Enum.Parse(typeof(T), value, true);
        }
    }
}