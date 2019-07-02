using System.Collections.Generic;

namespace Motarjem.Core.Dictionary
{
    public interface IDictionaryFile
    {
        IEnumerable<Word> Pronouns { get; }
        IEnumerable<Word> Verbs { get; }
        IEnumerable<Word> Conjunctions { get; }
        IEnumerable<Word> Determiners { get; }
        IEnumerable<Word> Adjectives { get; }
        IEnumerable<Word> Nouns { get; }
    }
}