using Gtk;
using Motarjem.Core;

namespace Motarjem
{
    public class MainWindow : Window
    {
        public MainWindow() : base("Motarjem")
        {
            // MainWindow
            DefaultWidth = 600;
            DefaultHeight = 400;

            // src
            src = new Entry();
            src.Text = "I am a program.";

            // button
            button = new Button
            {
                Label = "Translate"
            };
            button.Clicked += Button_Clicked;

            // hBox
            hBox = new Box(Orientation.Horizontal, 2);
            hBox.PackStart(src, true, true, 2);
            hBox.PackStart(button, false, false, 2);

            // headerBar
            headerBar = new HeaderBar
            {
                ShowCloseButton = true
            };
            headerBar.PackStart(hBox);
            headerBar.Title = "Motarjem";
            Titlebar = headerBar;

            // output
            output = new TextView();
            scrolledOutput = new ScrolledWindow
            {
                output
            };
            Add(scrolledOutput);

            // Setup Display for Translator
            display = new TextDisplay(output.Buffer);

            // Show
            ShowAll();
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            display.Clear();
            var sentence = Sentence.ParseEnglish(src.Buffer.Text);
            foreach (var s in sentence)
            {
                s.Display(display);
                display.PrintLine();
                s.Translate().Display(display);
                display.PrintLine();
            }
        }

        private HeaderBar headerBar;
        private Box hBox;
        private Entry src;
        private Button button;
        private TextView output;
        private ScrolledWindow scrolledOutput;
        private TextDisplay display;
    }
}
