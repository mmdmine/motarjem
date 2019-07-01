using Gtk;
using System;
using System.IO;
using Motarjem.Core.Dictionary;

namespace Motarjem
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Word.OpenDictionary(
                new XmlDictionaryFile(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    "words.xml")));

            var app = new Application("mmdmine.motarjem.gtk", GLib.ApplicationFlags.None);
            app.Activated += (sender, event_args) =>
            {
                app.AddWindow(new MainWindow());
            };
            app.Run("mmdmine.motarjem.gtk", args);
        }
    }
}
