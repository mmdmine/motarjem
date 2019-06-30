using Gtk;

namespace Motarjem
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Application.Init("motarjem.gtk", ref args);
            new MainWindow().ShowAll();
            Application.Run();
        }
    }
}
