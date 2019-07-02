using System;
using System.IO;
using GLib;
using Motarjem.Core.Dictionary;
using Application = Gtk.Application;

namespace Motarjem
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Word.OpenDictionary(
                new SqlDictionaryFile(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                        "words.sqlite3")));

            var app = new Application("mmdmine.motarjem.gtk", ApplicationFlags.None);
            app.Activated += (sender, eventArgs) => { app.AddWindow(new MainWindow()); };
            app.Run("mmdmine.motarjem.gtk", args);
        }
    }
}