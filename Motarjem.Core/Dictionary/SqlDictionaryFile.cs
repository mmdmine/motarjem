using System;
using System.Linq;
using System.Data.Linq;
using System.Data.SQLite;
using System.Data.Linq.Mapping;
using System.Collections.Generic;
using Motarjem.Core.Dictionary.Tables;

namespace Motarjem.Core.Dictionary
{
    public class SqlDictionaryFile : IDictionaryFile, IDisposable
    {
        public SqlDictionaryFile(string path)
        {
            _connection = new SQLiteConnection("DataSource=" + path);
            var context = new DataContext(_connection);
            _pronounsTable = context.GetTable<Pronouns>();
            _verbsTable = context.GetTable<Verbs>();
            _conjsTable = context.GetTable<Conjunctions>();
            _detsTable = context.GetTable<Determiners>();
            _adjsTable = context.GetTable<Adjectives>();
            _nounsTable = context.GetTable<Nouns>();
        }

        private readonly SQLiteConnection _connection;

        private readonly Table<Pronouns> _pronounsTable;
        private readonly Table<Verbs> _verbsTable;
        private readonly Table<Conjunctions> _conjsTable;
        private readonly Table<Determiners> _detsTable;
        private readonly Table<Adjectives> _adjsTable;
        private readonly Table<Nouns> _nounsTable;

        public IEnumerable<Word> Pronouns =>
            from row in _pronounsTable
            select new Word
            {
                English = row.English,
                Persian = row.Persian,
                Person = (Person) row.Person,
                Count = (PersonCount) row.Count,
                Sex = (PersonSex) row.Sex,
                Pos = PartsOfSpeech.Pronoun,
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
                Tense = (VerbTense) row.Tense,
            };

        public IEnumerable<Word> Conjs =>
            from row in _conjsTable
            select new Word
            {
                Pos = PartsOfSpeech.Conjunction,
                English = row.English,
                Persian = row.Persian,
            };

        public IEnumerable<Word> Dets =>
            from row in _detsTable
            select new Word
            {
                Pos = PartsOfSpeech.Determiner,
                English = row.English,
                Persian = row.Persian,
                Count = (PersonCount) row.Count,
            };

        public IEnumerable<Word> Adjs =>
            from row in _adjsTable
            select new Word
            {
                Pos = PartsOfSpeech.Adjective,
                English = row.English,
                Persian = row.Persian,
            };

        public IEnumerable<Word> Nouns =>
            from row in _nounsTable
            select new Word
            {
                Pos = PartsOfSpeech.Noun,
                Count = PersonCount.Singular,
                English = row.English,
                Persian = row.Persian,
            };

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }

    namespace Tables
    {
        [Table]
        internal class Nouns
        {
            [Column] public string English;
            [Column] public string Persian;
        }

        [Table]
        internal class Adjectives
        {
            [Column] public string English;
            [Column] public string Persian;
        }

        [Table]
        internal class Conjunctions
        {
            [Column] public string English;
            [Column] public string Persian;
        }

        [Table]
        internal class Determiners
        {
            [Column] public string English;
            [Column] public string Persian;
            [Column] public int Count;
        }

        [Table]
        internal class Pronouns
        {
            [Column] public string English;
            [Column] public string Persian;
            [Column] public int Person;
            [Column] public int Count;
            [Column] public int Sex;
        }

        [Table]
        internal class Verbs
        {
            [Column] public string English;
            [Column] public string Persian;
            [Column] public string Persian2;
            [Column] public string Persian3;
            [Column] public int Pos;
            [Column] public int Person;
            [Column] public int Count;
            [Column] public int Tense;
        }
    }
}
