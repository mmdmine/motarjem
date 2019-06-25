using System.Linq;
using System.Collections.Generic;
using System;

namespace Motarjem.Core
{
    public static partial class Dictionary
    {
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
