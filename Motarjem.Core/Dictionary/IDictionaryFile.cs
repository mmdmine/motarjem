using System.Collections.Generic;

namespace Motarjem.Core.Dictionary
{
    /// <summary>
    /// DictionaryFile Abstract Interface
    /// Implemention can query a database or read a simple plain text file
    /// </summary>
    public interface IDictionaryFile
    {
        /// <summary>
        /// Nouns Table
        /// </summary>
        IEnumerable<WordNoun> Nouns { get; }

        /// <summary>
        /// Pronouns Table
        /// </summary>
        IEnumerable<WordPronoun> Pronouns { get; }

        /// <summary>
        /// Adjectives Table
        /// </summary>
        IEnumerable<WordAdj> Adjectives { get; }

        /// <summary>
        /// Conjunctions Table
        /// </summary>
        IEnumerable<WordConj> Conjunctions { get; }

        /// <summary>
        /// Determiners Table
        /// </summary>
        IEnumerable<WordDet> Determiners { get; }

        /// <summary>
        /// Verbs Table
        /// </summary>
        IEnumerable<WordVerb> Verbs { get; }
    }
}