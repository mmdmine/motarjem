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

        public IEnumerable<Word> Adjs =>
            from word in Document
            .Element(XName.Get("words"))
            .Elements(XName.Get("adjective"))
            select ParseWord(word);

        public IEnumerable<Word> Conjs =>
            from word in Document
            .Element(XName.Get("words"))
            .Elements(XName.Get("conjunction"))
            select ParseWord(word);

        public IEnumerable<Word> Dets =>
            from word in Document
            .Element(XName.Get("words"))
            .Elements(XName.Get("determiner"))
            select ParseWord(word);

        public IEnumerable<Word> Nouns =>
            from word in Document
            .Element(XName.Get("words"))
            .Elements(XName.Get("noun"))
            select ParseWord(word);

        public IEnumerable<Word> Pronouns =>
            from word in Document
            .Element(XName.Get("words"))
            .Elements(XName.Get("pronoun"))
            select ParseWord(word);

        public IEnumerable<Word> Verbs =>
            from word in Document
            .Element(XName.Get("words"))
            .Elements("verb")
            select ParseWord(word);


        private Word ParseWord(XElement x)
        {
            var word = new Word
            {
                // if there's a 'pos' attribute, use it
                // otherwise use tag name
                pos = x.Attributes().Any(a => a.Name == "pos") ?
                    GetEnum<PartsOfSpeech>("pos") :
                    (PartsOfSpeech)Enum.Parse(typeof(PartsOfSpeech), x.Name.LocalName, true),
                english = GetAttribute("en"),
                persian = GetAttribute("fa"),
                persian_2 = GetAttribute("fav"),
                persian_verb_identifier = GetAttribute("fai"),
                person = GetEnum<Person>("person"),
                count = GetEnum<PersonCount>("count"),
                sex = GetEnum<PersonSex>("sex"),
                tense = GetEnum<VerbTense>("tense")
            };
            // Use PersonCount.Singular as default value for Nouns
            if (word.count == PersonCount.All &&
                word.pos == PartsOfSpeech.Noun)
                word.count = PersonCount.Singular;
            return word;

            string GetAttribute(string attribute)
            {
                var matches = x.Attributes(XName.Get(attribute));
                if (matches.Count() == 0)
                    return null;
                if (matches.Count() > 1)
                    throw new Exception(); // data error
                return matches.First().Value;
            }

            T GetEnum<T>(string attribute)
            {
                var value = GetAttribute(attribute);
                if (value == null)
                    return default(T);
                return (T)Enum.Parse(typeof(T), value, true);
            }
        }

        private XDocument Document { get; }
    }
}
