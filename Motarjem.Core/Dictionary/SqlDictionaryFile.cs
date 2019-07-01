using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Motarjem.Core.Dictionary
{
    public class SqlDictionaryFile : IDictionaryFile, IDisposable
    {
        public SqlDictionaryFile(string path)
        {
            connection = new SQLiteConnection("DataSource=" + path);
        }

        private readonly SQLiteConnection connection;

        public IEnumerable<Word> Pronouns => throw new NotImplementedException();
        public IEnumerable<Word> Verbs => throw new NotImplementedException();
        public IEnumerable<Word> Conjs => throw new NotImplementedException();
        public IEnumerable<Word> Dets => throw new NotImplementedException();
        public IEnumerable<Word> Adjs => throw new NotImplementedException();
        public IEnumerable<Word> Nouns => throw new NotImplementedException();

        public void Dispose()
        {
            connection.Close();
            connection.Dispose();
        }
    }
}
