using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using Mono.Data.Sqlite;
using Motarjem.Core.Dictionary.Tables;

namespace Motarjem.Core.Dictionary
{
    /// <summary>
    /// SQLite Implementation of <see cref="IDictionaryFile"/>
    /// </summary>
    public sealed class SqlDictionaryFile : IDictionaryFile, IDisposable
    {
        private readonly SqliteConnection _connection;
        
        private readonly Table<Adjectives> _adjsTable;
        private readonly Table<Conjunctions> _conjsTable;
        private readonly Table<Determiners> _detsTable;
        private readonly Table<Nouns> _nounsTable;
        private readonly Table<Pronouns> _pronounsTable;
        private readonly Table<Verbs> _verbsTable;
        
        /// <summary>
        /// Open a SQLite database
        /// </summary>
        /// <param name="path">Database file path</param>
        public SqlDictionaryFile(string path)
        {
            // 'DataSource' or 'Data Source' ?
            _connection = new SqliteConnection("Data Source=" + path);
            _connection.Open();

            var context = new DataContext(_connection);

            _pronounsTable  = context.GetTable<Pronouns>();
            _verbsTable     = context.GetTable<Verbs>();
            _conjsTable     = context.GetTable<Conjunctions>();
            _detsTable      = context.GetTable<Determiners>();
            _adjsTable      = context.GetTable<Adjectives>();
            _nounsTable     = context.GetTable<Nouns>();
        }

        /// <summary>
        /// Close connection to database and release resources
        /// </summary>
        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }

        public IEnumerable<WordNoun> Nouns =>
            from row in _nounsTable
            select new WordNoun
            {
                English = row.English,
                Persian = row.Persian,
                Count = PersonCount.Singular
            };

        public IEnumerable<WordPronoun> Pronouns =>
            from row in _pronounsTable
            select new WordPronoun
            {
                English = row.English,
                Persian = row.Persian,
                Person = (Person) row.Person,
                Count = (PersonCount) row.Count,
                Sex = (PersonSex) row.Sex
            };

        public IEnumerable<WordConj> Conjunctions =>
            from row in _conjsTable
            select new WordConj
            {
                English = row.English,
                Persian = row.Persian
            };

        public IEnumerable<WordDet> Determiners =>
            from row in _detsTable
            select new WordDet
            {
                English = row.English,
                Persian = row.Persian,
                Count = (PersonCount) row.Count
            };

        public IEnumerable<WordAdj> Adjectives =>
            from row in _adjsTable
            select new WordAdj
            {
                English = row.English,
                Persian = row.Persian
            };

        public IEnumerable<WordVerb> Verbs
        {
            get
            {
                foreach (var row in _verbsTable)
                {
                    yield return new WordVerb
                    {
                        English = row.English,
                        Persian = row.Persian,
                        Persian2 = row.Persian2,
                        PersianVerbIdentifier = row.Persian3,
                        Person = (Person)row.Person,
                        Count = (PersonCount)row.Count,
                        VerbType = (VerbType)row.VerbType,
                        Tense = (VerbTense)row.Tense
                    };
                }
            }
        }
    }
    
    // Definition of Tables used in SQLite Database file
    namespace Tables
    {
        [Table]
        internal class Nouns
        {
            [Column] public string English = "";
            [Column] public string Persian = "";
        }

        [Table]
        internal class Pronouns
        {
            [Column] public string English = "";
            [Column] public string Persian = "";
            [Column] public int Person;
            [Column] public int Count;
            [Column] public int Sex;
        }

        [Table]
        internal class Adjectives
        {
            [Column] public string English = "";
            [Column] public string Persian = "";
        }

        [Table]
        internal class Conjunctions
        {
            [Column] public string English = "";
            [Column] public string Persian = "";
        }

        [Table]
        internal class Determiners
        {
            [Column] public string English = "";
            [Column] public string Persian = "";
            [Column] public int Count;
        }

        [Table]
        internal class Verbs
        {
            [Column] public string English = "";
            [Column] public string Persian = "";
            [Column] public string Persian2 = "";
            [Column] public string Persian3 = "";
            [Column] public int Person;
            [Column] public int Count;
            [Column] public int VerbType;
            [Column] public int Tense;
        }
    }
}