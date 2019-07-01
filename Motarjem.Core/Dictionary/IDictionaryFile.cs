using System.Collections.Generic;

namespace Motarjem.Core.Dictionary
{
    public interface IDictionaryFile
    {
        IEnumerable<Word> Pronouns { get; }
        IEnumerable<Word> Verbs { get; }
        IEnumerable<Word> Conjs { get; }
        IEnumerable<Word> Dets { get; }
        IEnumerable<Word> Adjs { get; }
        IEnumerable<Word> Nouns { get; }
    }
}
