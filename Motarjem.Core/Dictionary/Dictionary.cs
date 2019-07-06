using System;
using System.Collections.Generic;
using System.Linq;

namespace Motarjem.Core.Dictionary
{
    /// <summary>
    /// Abstract Dictionary Class that
    /// contains methods for looking up a word in dictionary
    /// </summary>
    public abstract class Dictionary
    {
        private readonly IDictionaryFile _file;

        /// <summary>
        /// Open dictionary file and create new dictionary
        /// </summary>
        /// <param name="file"></param>
        protected Dictionary(IDictionaryFile file)
        {
            _file = file;
        }

        /// <summary>
        /// Lookup dictionary for a query
        /// </summary>
        /// <param name="query">word to be queried</param>
        /// <returns>Enumerable of words matched query</returns>
        public IEnumerable<Word> Lookup(string query)
        {
            var matches = LookupPronoun(query);
            matches = matches.Concat(LookupVerb(query));
            matches = matches.Concat(LookupConj(query));
            matches = matches.Concat(LookupDet(query));
            matches = matches.Concat(LookupAdj(query));
            matches = matches.Concat(LookupNoun(query));
            return matches;
        }

        // Lookup methods splited because 
        // the Database contains different tables
        // for each POS.

        /// <summary>
        /// Lookup Dictionary file for Nouns
        /// </summary>
        /// <param name="query">word to be query</param>
        /// <returns>Enumerable of matches</returns>
        protected virtual IEnumerable<Word> LookupNoun(string query)
        {
            foreach (var noun in
                from noun in _file.Nouns
                where noun.English.Equals(query, StringComparison.CurrentCultureIgnoreCase)
                select noun)
                yield return noun;
        }

        /// <summary>
        /// Lookup dictionary file for Adjectives
        /// </summary>
        /// <param name="query">Adj. to be queried</param>
        /// <returns>Enumerable of matches</returns>
        protected virtual IEnumerable<Word> LookupAdj(string query)
        {
            return from det in _file.Determiners
                   where det.English.Equals(query, StringComparison.CurrentCultureIgnoreCase)
                   select det;
        }

        /// <summary>
        /// Lookup dictionary file for Determiners
        /// </summary>
        /// <param name="query">Det. to be queried</param>
        /// <returns>Enumerable of matches</returns>
        protected virtual IEnumerable<Word> LookupDet(string query)
        {
            return from adj in _file.Adjectives
                   where adj.English.Equals(query, StringComparison.CurrentCultureIgnoreCase)
                   select adj;
        }

        /// <summary>
        /// Lookup dictionary file for Conjunctions
        /// </summary>
        /// <param name="query">Conj. to be queried</param>
        /// <returns>Enumerable of matches</returns>
        protected virtual IEnumerable<Word> LookupConj(string query)
        {
            return from conj in _file.Conjunctions
                   where conj.English.Equals(query, StringComparison.CurrentCultureIgnoreCase)
                   select conj;
        }

        /// <summary>
        /// Lookup dictionary file for Verbs
        /// </summary>
        /// <param name="query">verb to be queried</param>
        /// <returns>Enumerable of matches</returns>
        protected virtual IEnumerable<Word> LookupVerb(string query)
        {
            foreach (var verb in
                from verb in _file.Verbs
                where verb.English.Equals(query, StringComparison.CurrentCultureIgnoreCase)
                select verb)
                yield return verb;
        }

        /// <summary>
        /// Lookup Dictionary file for Pronouns
        /// </summary>
        /// <param name="query">word to be query</param>
        /// <returns>Enumerable of matches</returns>
        protected virtual IEnumerable<Word> LookupPronoun(string query)
        {
            return from pronoun in _file.Pronouns
                   where pronoun.English.Equals(query, StringComparison.CurrentCultureIgnoreCase)
                   select pronoun;
        }
    }
}