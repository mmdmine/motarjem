using System;
using System.IO;
using System.Windows;
using Motarjem.Core.Dictionary;

namespace Motarjem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Word.OpenDictionary(
                new SqlDictionaryFile(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    "words.sqlite3")));
        }
    }
}
