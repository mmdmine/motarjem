using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using Mono.Data.Sqlite;
using Motarjem.Core.Dictionary.Tables;

namespace Motarjem.Core.Dictionary
{
    public sealed class SqlDictionaryFile : IDictionaryFile, IDisposable
    {
        private readonly SqliteConnection _connection;
        
        private readonly Table<Adjectives> _adjsTable;
        private readonly Table<Conjunctions> _conjsTable;
        private readonly Table<Determiners> _detsTable;
        private readonly Table<Nouns> _nounsTable;
        private readonly Table<Pronouns> _pronounsTable;
        private readonly Table<Verbs> _verbsTable;
        
        public SqlDictionaryFile(string path)
        {
            // 'DataSource' or 'Data Source' ?
            _connection = new SqliteConnection("Data Source=" + path);
            _connection.Open();
            var context = new DataContext(_connection);
            _pronounsTable = context.GetTable<Pronouns>();
            _verbsTable = context.GetTable<Verbs>();
            _conjsTable = context.GetTable<Conjunctions>();
            _detsTable = context.GetTable<Determiners>();
            _adjsTable = context.GetTable<Adjectives>();
            _nounsTable = context.GetTable<Nouns>();
        }

        public IEnumerable<Word> Pronouns =>
            from row in _pronounsTable
            select new Word
            {
                English = row.English,
                Persian = row.Persian,
                Person = (Person) row.Person,
                Count = (PersonCount) row.Count,
                Sex = (PersonSex) row.Sex,
                Pos = PartsOfSpeech.Pronoun
            };

        public IEnumerable<Word> Verbs =>
            from row in _verbsTable
            select new Word
            {
                English = row.English,
                Persian = row.Persian,
                Persian2 = row.Persian2,
                PersianVerbIdentifier = row.Persian3,
                Pos = (PartsOfSpeech) row.Pos,
                Person = (Person) row.Person,
                Count = (PersonCount) row.Count,
                Tense = (VerbTense) row.Tense
            };

        public IEnumerable<Word> Conjunctions =>
            from row in _conjsTable
            select new Word
            {
                Pos = PartsOfSpeech.Conjunction,
                English = row.English,
                Persian = row.Persian
            };

        public IEnumerable<Word> Determiners =>
            from row in _detsTable
            select new Word
            {
                Pos = PartsOfSpeech.Determiner,
                English = row.English,
                Persian = row.Persian,
                Count = (PersonCount) row.Count
            };

        public IEnumerable<Word> Adjectives =>
            from row in _adjsTable
            select new Word
            {
                Pos = PartsOfSpeech.Adjective,
                English = row.English,
                Persian = row.Persian
            };

        public IEnumerable<Word> Nouns =>
            from row in _nounsTable
            select new Word
            {
                Pos = PartsOfSpeech.Noun,
                Count = PersonCount.Singular,
                English = row.English,
                Persian = row.Persian
            };

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
            GC.SuppressFinalize(this);
        }
    }

    namespace Tables
    {
        [Table]
        internal class Nouns
        {
            [Column] public string English = "";
            [Column] public string Persian = "";
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
            [Column] public int Count = 0;
        }

        [Table]
        internal class Pronouns
        {
            [Column] public string English = "";
            [Column] public string Persian = "";
            [Column] public int Person = 0;
            [Column] public int Count = 0;
            [Column] public int Sex = 0;
        }

        [Table]
        internal class Verbs
        {
            [Column] public string English = "";
            [Column] public string Persian = "";
            [Column] public string Persian2 = "";
            [Column] public string Persian3 = "";
            [Column] public int Pos = 0;
            [Column] public int Person = 0;
            [Column] public int Count = 0;
            [Column] public int Tense = 0;
        }
    }
}