using Gtk;
using Motarjem.Core;

namespace Motarjem
{
    public class TextDisplay : IDisplay
    {
        private readonly TextBuffer buffer;

        public TextDisplay(TextBuffer tb)
        {
            buffer = tb;
        }

        public void Print(string text, FontColor color = FontColor.Black, FontStyle style = FontStyle.Default)
        {
            buffer.Text += text;
        }

        public void PrintLine()
        {
            buffer.Text += "\n";
        }

        public void PrintSpace()
        {
            buffer.Text += " ";
        }

        public void Clear()
        {
            buffer.Clear();
        }
    }
}
