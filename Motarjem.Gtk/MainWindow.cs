using Gtk;
using Motarjem.Core;

namespace Motarjem
{
    public class MainWindow : Window
    {
        public MainWindow() : base("Motarjem")
        {
            // MainWindow
            Destroyed += (sender, args) =>
            {
                Application.Quit();
            };
            DefaultWidth = 525;
            DefaultHeight = 350;

            // TextView
            var tv = new TextView
            {
                Buffer =
                {
                    Text = "I am a program."
                }
            };

            // Content
            var content = new TextView();
            // - TextDisplay
            var td = new TextDisplay(content.Buffer);

            // Button
            var btn = Button.NewWithLabel("Translate");
            btn.Clicked += (sender, args) =>
            {
                td.Clear();
                foreach (var s in Parser.ParseEnglish(Token.Tokenize(tv.Buffer.Text)))
                {
                    Translator.Translate(s).Display(td);
                }
            };

            // HBox
            var hb = new HBox(false, 0);

            // VBox
            var vb = new VBox();

            // Add Components
            hb.PackStart(tv, true, true, 2);
            hb.PackStart(btn, false, false, 2);
            vb.PackStart(hb, false, false, 2);
            vb.PackStart(content, true, true, 2);
            Add(vb);
        }
    }
}
