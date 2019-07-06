using System.Collections.Generic;
using System.Linq;

namespace Motarjem.Core.Dictionary
{
    /// <summary>
    /// English Dictionary that looks up a dictionary file.
    /// this Dictionary also matches English suffixes/prefixes.
    /// </summary>
    internal class EnglishDictionary : Dictionary
    {
        public EnglishDictionary(IDictionaryFile file) :
            base(file)
        {
        }

        protected override IEnumerable<WordVerb> LookupVerb(string query)
        {
            var matches = base.LookupVerb(query);

            //  Past Verbs
            WordVerb GeneratePastVerb(WordVerb verb)
            {
                var result = verb.Clone();
                result.English = query;
                //result.Persian = TODO: Lookup Persian Past Verb
                result.Tense = VerbTense.Past;
                return result;
            }

            // Example:
            // 1) Started - ed = Start
            // 2) Lookup(String{Start}) -> Word{Start}
            // 3) Generate(Word{Start}) -> Word{Started}
            if (query.EndsWith("ed"))
            {
                matches = matches.Concat(from verb in base.LookupVerb(query.Substring(0, query.Length - 2))
                               select GeneratePastVerb(verb));
            }

            // Simple Present Third Person -s ending Verbs
            WordVerb GenerateThirdPersonVerb(WordVerb verb)
            {
                var result = verb.Clone();
                result.English = query;
                // Verbs having -s suffix are for Third Person in Simple Present
                result.Person = Person.Third;
                result.Count = PersonCount.Singular;
                result.Tense = VerbTense.Present;

                result.PersianVerbIdentifier = "د";
                return result;
            }

            // Example:
            // 1) flies - ies = fl -> fl + y = fly
            // 2) Lookup(String{fly}) -> Word{fly}
            // 3) Generate(Word{fly}) -> Word{flies}
            if (query.EndsWith("ies"))
            {
                matches = matches.Concat(
                    from verb in base.LookupVerb(query.Substring(0, query.Length - 3) + "y")
                    select GenerateThirdPersonVerb(verb));
            }

            // 1) Does - es = Do
            // 2) Lookup(String{do}) -> Word{do}
            // 3) Generate(Word{do}) -> Word{does}
            if (query.EndsWith("es"))
            {
                matches = matches.Concat(
                    from verb in base.LookupVerb(query.Substring(0, query.Length - 2))
                    select GenerateThirdPersonVerb(verb));
            }

            // 1) Starts - s = Start
            // 2) Lookup(String{start}) -> Word{start}
            // 3) Generate(Word{start}) -> Word{starts}
            if (query.EndsWith("s"))
            {
                matches = matches.Concat(
                    from verb in base.LookupVerb(query.Substring(0, query.Length - 1))
                    select GenerateThirdPersonVerb(verb));
            }

            // TODO: check for re-, de- prefixes

            return matches;
        }

        protected override IEnumerable<WordNoun> LookupNoun(string query)
        {
            var matches = base.LookupNoun(query);

            // Plural nouns -s suffix
            WordNoun GeneratePlural(WordNoun noun)
            {
                var result = noun.Clone();
                result.English = query;
                result.Persian = noun.Persian + "ها"; // make persian translation plural
                result.Count = PersonCount.Plural;
                return result;
            }

            // Example:
            // 1) Flies - ies = Fl -> Fl + y = Fly
            // 2) Lookup(String{Fly}) -> Word{Fly}
            // 3) Generate(Word{Fly}) -> Word{Flies}
            if (query.EndsWith("ies"))
            {
                matches = matches.Concat(
                    from noun in base.LookupNoun(query.Substring(0, query.Length - 3) + "y")
                    select GeneratePlural(noun));
            }
            
            // 1) Matches - es = Match
            // 2) Lookup(String{Match}) -> Word{Match}
            // 3) Generate(Word{Match}) -> Word{Matches}
            if (query.EndsWith("es"))
            {
                matches = matches.Concat(
                    from noun in base.LookupNoun(query.Substring(0, query.Length - 2))
                    select GeneratePlural(noun));
            }
            
            // 1) Cats - s = Cat
            // 2) Lookup(String{Cat}) -> Word{Cat}
            // 3) Generate(Word{Cat}) -> Word{Cats}
            if (query.EndsWith("s"))
            {
                matches = matches.Concat(
                    from noun in base.LookupNoun(query.Substring(0, query.Length - 1))
                    select GeneratePlural(noun));
            }

            return matches;
        }
    }
}