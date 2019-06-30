using Gtk;

namespace Motarjem
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var app = new Application("mmdmine.motarjem.gtk", GLib.ApplicationFlags.None);
            app.Activated += (sender, event_args) =>
            {
                app.AddWindow(new MainWindow());
            };
            app.Run("mmdmine.motarjem.gtk", args);
        }
    }
}
