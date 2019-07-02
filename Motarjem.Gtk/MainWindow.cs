using System;
using Gtk;
using Motarjem.Core;

namespace Motarjem
{
    public class MainWindow : Window
    {
        private readonly TextDisplay _display;
        private readonly Entry _src;

        public MainWindow() : base("Motarjem")
        {
            // MainWindow
            DefaultWidth = 600;
            DefaultHeight = 400;

            // src
            _src = new Entry {Text = "I am a program."};

            // button
            var button = new Button
            {
                Label = "Translate"
            };
            button.Clicked += Button_Clicked;

            // hBox
            var hBox = new Box(Orientation.Horizontal, 2);
            hBox.PackStart(_src, true, true, 2);
            hBox.PackStart(button, false, false, 2);

            // headerBar
            var headerBar = new HeaderBar
            {
                ShowCloseButton = true
            };
            headerBar.PackStart(hBox);
            headerBar.Title = "Motarjem";
            Titlebar = headerBar;

            // output
            var output = new TextView();
            var scrolledOutput = new ScrolledWindow
            {
                output
            };
            Add(scrolledOutput);

            // Setup Display for Translator
            _display = new TextDisplay(output.Buffer);

            // Show
            ShowAll();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                _display.Clear();
                var sentence = Sentence.ParseEnglish(_src.Buffer.Text);
                foreach (var s in sentence)
                {
                    s.Display(_display);
                    _display.PrintLine();
                    s.Translate().Display(_display);
                    _display.PrintLine();
                }
            }
            catch (Exception ex)
            {
                _display.Print("Error: " + ex, FontColor.Red);
            }
        }
    }
}