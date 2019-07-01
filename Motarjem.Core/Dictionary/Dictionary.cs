using System.Linq;
using System.Collections.Generic;

namespace Motarjem.Core.Dictionary
{
    public abstract class Dictionary
    {
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

        protected abstract IEnumerable<Word> LookupNoun(string query);
        protected abstract IEnumerable<Word> LookupAdj(string query);
        protected abstract IEnumerable<Word> LookupDet(string query);
        protected abstract IEnumerable<Word> LookupConj(string query);
        protected abstract IEnumerable<Word> LookupVerb(string query);
        protected abstract IEnumerable<Word> LookupPronoun(string query);
    }
}
