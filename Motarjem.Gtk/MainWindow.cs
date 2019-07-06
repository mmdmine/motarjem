using System;
using Gtk;
using Motarjem.Core;
using Motarjem.Core.Sentences;

namespace Motarjem
{
    public class MainWindow : Window
    {
        private readonly TextDisplay _display;
        private readonly Entry _src;

        public MainWindow() : base(title: "Motarjem")
        {
            // MainWindow
            DefaultWidth = 600;
            DefaultHeight = 300;

            // src
            _src = new Entry {Text = "the small cat and beauty butterflies are happy. I am a program." };

            // button
            var button = new Button
            {
                Label = "Translate"
            };
            button.Clicked += Button_Clicked;

            // hBox
            var hBox = new Box(Orientation.Horizontal, 0);
            hBox.PackStart(_src, true, true, 5);
            hBox.PackStart(button, false, false, 5);

            // output
            var output = new TextView();
            var scrolledOutput = new ScrolledWindow
            {
                output
            };

            // vBox
            var vBox = new Box(Orientation.Vertical, 2);
            vBox.PackStart(hBox, false, false, 2);
            vBox.PackStart(scrolledOutput, true, true, 2);
            Add(vBox); // Add vBox to Window

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
                var sentence = Parser.Parse(_src.Buffer.Text);
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