using System;
using System.Linq;
using System.Collections.Generic;

namespace Motarjem.Core.Dictionary
{
    public class EnglishDictionary : Dictionary
    {
        public EnglishDictionary(IDictionaryFile file)
        {
            _file = file;
        }

        private readonly IDictionaryFile _file;

        protected override IEnumerable<Word> LookupPronoun(string query) =>
            from pronoun in _file.Pronouns
            where pronoun.English.Equals(query, StringComparison.CurrentCultureIgnoreCase)
            select pronoun;

        protected override IEnumerable<Word> LookupVerb(string query)
        {
            foreach (var verb in
                from verb in _file.Verbs
                where verb.English.Equals(query, StringComparison.CurrentCultureIgnoreCase)
                select verb)
                yield return verb;

            //  Past Verbs
            if (query.EndsWith("ed"))
            {
                foreach (var verb in
                    from verb in _file.Verbs
                    where verb.English.Equals(query.Substring(0, query.Length - 2))
                    select verb)
                {
                    var result = verb;
                    result.English = query;
                    // result.persian = LookupPastFa(verb.persian)
                    result.Tense = VerbTense.Past;
                    yield return result;
                }
            }
            //  Third Person -s ending Verbs
            var generator = new Func<Word, Word>(verb =>
            {
                var result = verb;
                result.English = query;
                // verb.persian = ?
                result.Person = Person.Third;
                result.Count = PersonCount.Singular;
                result.Tense = VerbTense.Present;
                result.PersianVerbIdentifier = "د";
                return result;
            });
            if (query.EndsWith("s"))
            {
                foreach (var verb in
                    from verb in _file.Verbs
                    where verb.English.Equals(query.Substring(0, query.Length - 1))
                    select verb)
                    yield return generator(verb);
            }
            if (query.EndsWith("es"))
            {
                foreach (var verb in
                    from verb in _file.Verbs
                    where verb.English.Equals(query.Substring(0, query.Length - 2))
                    select verb)
                    yield return generator(verb);
            }
            if (query.EndsWith("ies"))
            {
                foreach (var verb in
                    from verb in _file.Verbs
                    where verb.English.Equals(query.Substring(0, query.Length - 3) + "y")
                    select verb)
                    yield return generator(verb);
            }
        }

        protected override IEnumerable<Word> LookupConj(string query) =>
            from conj in _file.Conjs
            where conj.English.Equals(query, StringComparison.CurrentCultureIgnoreCase)
            select conj;

        protected override IEnumerable<Word> LookupDet(string query) =>
            from det in _file.Dets
            where det.English.Equals(query, StringComparison.CurrentCultureIgnoreCase)
            select det;

        protected override IEnumerable<Word> LookupAdj(string query) =>
            from adj in _file.Adjs
            where adj.English.Equals(query, StringComparison.CurrentCultureIgnoreCase)
            select adj;

        protected override IEnumerable<Word> LookupNoun(string query)
        {
            foreach (var noun in
                from noun in _file.Nouns
                where noun.English.Equals(query, StringComparison.CurrentCultureIgnoreCase)
                select noun)
                yield return noun;
            //  Third Person -s ending Verbs
            var generator = new Func<Word, Word>(noun =>
            {
                var result = noun;
                result.English = query;
                result.Persian = noun.Persian + "ها";
                result.Count = PersonCount.Plural;
                return result;
            });
            if (query.EndsWith("s"))
            {
                foreach (var noun in
                    from noun in _file.Nouns
                    where noun.English.Equals(query.Substring(0, query.Length - 1))
                    select noun)
                    yield return generator(noun);
            }
            if (query.EndsWith("es"))
            {
                foreach (var noun in
                    from noun in _file.Nouns
                    where noun.English.Equals(query.Substring(0, query.Length - 2))
                    select noun)
                    yield return generator(noun);
            }
            if (query.EndsWith("ies"))
            {
                foreach (var noun in
                    from noun in _file.Nouns
                    where noun.English.Equals(query.Substring(0, query.Length - 3) + "y")
                    select noun)
                    yield return generator(noun);
            }
        }
    }
}
