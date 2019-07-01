using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Motarjem.Core
{
    public static class Dictionary
    {
        private static IEnumerable<XElement> Words = XDocument.Load(".\\words.xml").Elements().First(a => a.Name == "words").Elements();

        private static IEnumerable<Word> Pronouns = from word in Words where word.Name.LocalName == "pronoun" select ParseWord(word);
        private static IEnumerable<Word> Verbs = from word in Words where word.Name.LocalName == "verb" select ParseWord(word);
        private static IEnumerable<Word> Conjs = from word in Words where word.Name.LocalName == "conjunction" select ParseWord(word);
        private static IEnumerable<Word> Determiners = from word in Words where word.Name.LocalName == "determiner" select ParseWord(word);
        private static IEnumerable<Word> Adjectives = from word in Words where word.Name.LocalName == "adjective" select ParseWord(word);
        private static IEnumerable<Word> Nouns = from word in Words where word.Name.LocalName == "noun" select ParseWord(word);

        private static Word ParseWord(XElement x)
        {
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

            var word = new Word
            {
                pos = x.Attributes().Any(a => a.Name == "pos") ?
                    GetEnum<PartOfSpeech>("pos") :
                    (PartOfSpeech)Enum.Parse(typeof(PartOfSpeech), x.Name.LocalName, true),
                english = GetAttribute("en"),
                persian = GetAttribute("fa"),
                persian_2 = GetAttribute("fav"),
                persian_verb_identifier = GetAttribute("fai"),
                person = GetEnum<Person>("person"),
                count = GetEnum<PersonCount>("count"),
                sex = GetEnum<PersonSex>("sex"),
                tense = GetEnum<VerbTense>("tense")
            };
            if (word.count == PersonCount.All &&
                word.pos == PartOfSpeech.Noun)
                word.count = PersonCount.Singular;
            return word;
        }

        public static IEnumerable<Word> LookupEn(string query)
        {
            var matches = new List<Word>();
            try
            {
                matches.AddRange(LookupPronounEn(query));
                matches.AddRange(LookupVerbEn(query));
                matches.AddRange(LookupConjEn(query));
                matches.AddRange(LookupDetEn(query));
                matches.AddRange(LookupAdjEn(query));
                matches.AddRange(LookupNounEn(query));
                matches.AddRange(LookupProperNounEn(query));
            }
            catch (Exception)
            {
                // ignore
            }
            return matches;
        }

        public static IEnumerable<Word> LookupPronounEn(string query) => from pronoun in Pronouns
                                                                         where pronoun.english.Equals(query, StringComparison.CurrentCultureIgnoreCase)
                                                                         select pronoun;

        public static IEnumerable<Word> LookupVerbEn(string query)
        {
            foreach (var verb in from verb in Verbs where verb.english.Equals(query, StringComparison.CurrentCultureIgnoreCase) select verb)
                yield return verb;

            //  Past Verbs
            if (query.EndsWith("ed"))
            {
                foreach (var verb in Verbs.Where(verb => verb.english.Equals(query.Substring(0, query.Length - 2))))
                {
                    var result = verb;
                    result.english = query;
                    // result.persian = LookupPastFa(verb.persian)
                    result.tense = VerbTense.Past;
                    yield return result;
                }
            }
            //  Third Person -s ending Verbs
            Word generator(Word verb)
            {
                var result = verb;
                result.english = query;
                // verb.persian = ?
                result.person = Person.Third;
                result.count = PersonCount.Singular;
                result.tense = VerbTense.Present;
                result.persian_verb_identifier = "د";
                return result;
            }
            if (query.EndsWith("s"))
            {
                foreach (var verb in Verbs.Where(verb => verb.english.Equals(query.Substring(0, query.Length - 1))))
                {
                    yield return generator(verb);
                }
            }
            if (query.EndsWith("es"))
            {
                foreach (var verb in Verbs.Where(verb => verb.english.Equals(query.Substring(0, query.Length - 2))))
                {
                    yield return generator(verb);
                }
            }
            if (query.EndsWith("ies"))
            {
                foreach (var verb in Verbs.Where(verb => verb.english.Equals(query.Substring(0, query.Length - 3) + "y")))
                {
                    yield return generator(verb);
                }
            }
        }

        public static IEnumerable<Word> LookupConjEn(string query)
        {
            foreach (var conj in from conj in Conjs where conj.english.Equals(query, StringComparison.CurrentCultureIgnoreCase) select conj)
                yield return conj;
        }

        public static IEnumerable<Word> LookupDetEn(string query)
        {
            foreach (var det in from det in Determiners where det.english.Equals(query, StringComparison.CurrentCultureIgnoreCase) select det)
                yield return det;
        }

        public static IEnumerable<Word> LookupAdjEn(string query)
        {
            foreach (var adj in from adj in Adjectives where adj.english.Equals(query, StringComparison.CurrentCultureIgnoreCase) select adj)
                yield return adj;
        }

        public static IEnumerable<Word> LookupNounEn(string query)
        {
            foreach (var noun in from noun in Nouns where noun.english.Equals(query, StringComparison.CurrentCultureIgnoreCase) select noun)
                yield return noun;
            //  Third Person -s ending Verbs
            Word generator(Word noun)
            {
                var result = noun;
                result.english = query;
                result.persian = noun.persian + "ها";
                result.count = PersonCount.Plural;
                return result;
            }
            if (query.EndsWith("s"))
            {
                foreach (var noun in from noun in Nouns where noun.english.Equals(query.Substring(0, query.Length - 1)) select noun)
                {
                    yield return generator(noun);
                }
            }
            if (query.EndsWith("es"))
            {
                foreach (var noun in from noun in Nouns where noun.english.Equals(query.Substring(0, query.Length - 2)) select noun)
                {
                    yield return generator(noun);
                }
            }
            if (query.EndsWith("ies"))
            {
                foreach (var noun in from noun in Nouns where noun.english.Equals(query.Substring(0, query.Length - 3) + "y") select noun)
                {
                    yield return generator(noun);
                }
            }
        }

        public static IEnumerable<Word> LookupProperNounEn(string query)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<Word> TranslateProperNounEn(string query)
        {
            throw new NotImplementedException();
        }
    }

    public class Word
    {
        public string english = "";
        public string persian = "";
        public string persian_2 = "";
        public string persian_verb_identifier = "";
        public PartOfSpeech pos = PartOfSpeech.Noun;
        public Person person = Person.All;
        public PersonCount count = PersonCount.All;
        public PersonSex sex = PersonSex.All;
        public PronounType pronounType = PronounType.None;
        public VerbTense tense = VerbTense.None;

        public bool IsNoun
        {
            get
            {
                return pos == PartOfSpeech.Noun || pos == PartOfSpeech.Pronoun || pos == PartOfSpeech.ProperNoun;
            }
        }

        public bool IsVerb
        {
            get
            {
                return pos == PartOfSpeech.Verb || pos == PartOfSpeech.AuxiliaryVerb || pos == PartOfSpeech.ToBe;
            }
        }
    }

    public enum PartOfSpeech
    {
        Noun, Pronoun, ProperNoun,
        Verb, AuxiliaryVerb, ToBe,
        Adjective, Adverb,
        Conjunction, Determiner,
        Preposition,
        Number,
    }

    public enum Person
    {
        All, First, Second, Third
    }

    public enum PersonCount
    {
        All, Singular, Plural
    }

    public enum PersonSex
    {
        All, Male, Female
    }

    public enum VerbTense
    {
        None,
        Present,
        Past,
        PastParticiple,
    }

    public enum PronounType
    {
        None,
        Subjective,
        Objective
    }
}
