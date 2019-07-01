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
            connection = new SQLiteConnection("DataSource=" + path);
            context = new DataContext(connection);
            pronouns_table = context.GetTable<Pronouns>();
            verbs_table = context.GetTable<Verbs>();
            conjs_table = context.GetTable<Conjunctions>();
            dets_table = context.GetTable<Determiners>();
            adjs_table = context.GetTable<Adjectives>();
            nouns_table = context.GetTable<Nouns>();
        }

        private readonly SQLiteConnection connection;
        private readonly DataContext context;

        private readonly Table<Pronouns> pronouns_table;
        private readonly Table<Verbs> verbs_table;
        private readonly Table<Conjunctions> conjs_table;
        private readonly Table<Determiners> dets_table;
        private readonly Table<Adjectives> adjs_table;
        private readonly Table<Nouns> nouns_table;

        public IEnumerable<Word> Pronouns =>
            from row in pronouns_table
            select new Word
            {
                english = row.English,
                persian = row.Persian,
                person = row.Person,
                count = row.Count,
                sex = row.Sex,
                pos = PartsOfSpeech.Pronoun,
            };

        public IEnumerable<Word> Verbs =>
            from row in verbs_table
            select new Word
            {
                english = row.English,
                persian = row.Persian,
                persian_2 = row.Persian2,
                persian_verb_identifier = row.Persian3,
                pos = row.POS,
                person = row.Person,
                count = row.Count,
                tense = row.Tense,
            };

        public IEnumerable<Word> Conjs =>
            from row in conjs_table
            select new Word
            {
                pos = PartsOfSpeech.Conjunction,
                english = row.English,
                persian = row.Persian,
            };

        public IEnumerable<Word> Dets =>
            from row in dets_table
            select new Word
            {
                pos = PartsOfSpeech.Determiner,
                english = row.English,
                persian = row.Persian,
                count = row.Count,
            };

        public IEnumerable<Word> Adjs =>
            from row in adjs_table
            select new Word
            {
                pos = PartsOfSpeech.Adjective,
                english = row.English,
                persian = row.Persian,
            };

        public IEnumerable<Word> Nouns =>
            from row in nouns_table
            select new Word
            {
                pos = PartsOfSpeech.Noun,
                count = PersonCount.Singular,
                english = row.English,
                persian = row.Persian,
            };

        public void Dispose()
        {
            connection.Close();
            connection.Dispose();
        }
    }

    namespace Tables
    {
        [Table]
        class Nouns
        {
            [Column] public string English;
            [Column] public string Persian;
        }

        [Table]
        class Adjectives
        {
            [Column] public string English;
            [Column] public string Persian;
        }

        [Table]
        class Conjunctions
        {
            [Column] public string English;
            [Column] public string Persian;
        }

        [Table]
        class Determiners
        {
            [Column] public string English;
            [Column] public string Persian;
            [Column] public PersonCount Count;
        }

        [Table]
        class Pronouns
        {
            [Column] public string English;
            [Column] public string Persian;
            [Column] public Person Person;
            [Column] public PersonCount Count;
            [Column] public PersonSex Sex;
        }

        [Table]
        class Verbs
        {
            [Column] public string English;
            [Column] public string Persian;
            [Column] public string Persian2;
            [Column] public string Persian3;
            [Column] public PartsOfSpeech POS;
            [Column] public Person Person;
            [Column] public PersonCount Count;
            [Column] public VerbTense Tense;
        }
    }
}
