using System;
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
            _src = new Entry();
            _src.Text = "I am a program.";

            // button
            _button = new Button
            {
                Label = "Translate"
            };
            _button.Clicked += Button_Clicked;

            // hBox
            _hBox = new Box(Orientation.Horizontal, 2);
            _hBox.PackStart(_src, true, true, 2);
            _hBox.PackStart(_button, false, false, 2);

            // headerBar
            _headerBar = new HeaderBar
            {
                ShowCloseButton = true
            };
            _headerBar.PackStart(_hBox);
            _headerBar.Title = "Motarjem";
            Titlebar = _headerBar;

            // output
            _output = new TextView();
            _scrolledOutput = new ScrolledWindow
            {
                _output
            };
            Add(_scrolledOutput);

            // Setup Display for Translator
            _display = new TextDisplay(_output.Buffer);

            // Show
            ShowAll();
        }

        private void Button_Clicked(object sender, System.EventArgs e)
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

        private HeaderBar _headerBar;
        private Box _hBox;
        private Entry _src;
        private Button _button;
        private TextView _output;
        private ScrolledWindow _scrolledOutput;
        private TextDisplay _display;
    }
}
