using Gtk;
using Motarjem.Core;
using Pango;
using Style = Pango.Style;

namespace Motarjem
{
    public class TextDisplay : IDisplay
    {
        private static readonly TextTag ColorBlack =
            new TextTag("color_black") {Foreground = "black"};

        private static readonly TextTag ColorRed =
            new TextTag("color_red") {Foreground = "red"};

        private static readonly TextTag ColorBlue =
            new TextTag("color_blue") {Foreground = "blue"};

        private static readonly TextTag ColorGreen =
            new TextTag("color_green") {Foreground = "green"};

        private static readonly TextTag ColorGray =
            new TextTag("color_gray") {Foreground = "gray"};

        private static readonly TextTag Normal =
            new TextTag("normal") {Weight = Weight.Normal};

        private static readonly TextTag Bold =
            new TextTag("bold") {Weight = Weight.Bold};

        private static readonly TextTag Italic =
            new TextTag("italic") {Style = Style.Italic};

        private readonly TextBuffer _buffer;

        private readonly TextTag _textSize =
            new TextTag("size_normal") {SizePoints = 16};

        private TextIter _iter;


        public TextDisplay(TextBuffer tb)
        {
            _buffer = tb;
            _iter = _buffer.StartIter;
            _buffer.TagTable.Add(_textSize);
            _buffer.TagTable.Add(Normal);
            _buffer.TagTable.Add(Bold);
            _buffer.TagTable.Add(Italic);
            _buffer.TagTable.Add(ColorBlack);
            _buffer.TagTable.Add(ColorBlue);
            _buffer.TagTable.Add(ColorGray);
            _buffer.TagTable.Add(ColorGreen);
            _buffer.TagTable.Add(ColorRed);
        }

        public void Print(string text, FontColor color = FontColor.Black, FontStyle style = FontStyle.Default)
        {
        	if (!string.IsNullOrWhiteSpace(text))
            	_buffer.InsertWithTags(ref _iter, text, GetColor(color), GetStyle(style), _textSize);
        }

        private static TextTag GetColor(FontColor color)
        {
            switch (color)
            {
                case FontColor.Red:
                    return ColorRed;
                case FontColor.Blue:
                    return ColorBlue;
                case FontColor.Green:
                    return ColorGreen;
                case FontColor.Gray:
                    return ColorGray;
                case FontColor.LightRed: //TODO
                case FontColor.LightGreen:
                case FontColor.LightBlue:
                case FontColor.Black:
                default:
                    return ColorBlack;
            }
        }

        private static TextTag GetStyle(FontStyle style)
        {
            switch (style)
            {
                case FontStyle.Default:
                default:
                    return Normal;
                case FontStyle.Bold:
                    return Bold;
                case FontStyle.Italic:
                    return Italic;
            }
        }
        
        public void PrintSpace()
        {
            _buffer.InsertWithTags(ref _iter, " ", _textSize);
        }

        public void PrintLine()
        {
            _buffer.InsertWithTags(ref _iter, "\n", _textSize);
        }

        public void Clear()
        {
            _buffer.Clear();
            _iter = _buffer.StartIter;
        }
    }
}