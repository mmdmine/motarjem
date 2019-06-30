using Gtk;
using Motarjem.Core;

namespace Motarjem
{
    public class TextDisplay : IDisplay
    {
        public static readonly TextTag Color_Black =
            new TextTag("color_black") { Foreground = "black" };
        public static readonly TextTag Color_Red =
            new TextTag("color_red") { Foreground = "red" };
        public static readonly TextTag Color_Blue =
            new TextTag("color_blue") { Foreground = "blue" };
        public static readonly TextTag Color_Green =
            new TextTag("color_green") { Foreground = "green" };
        public static readonly TextTag Color_Gray =
            new TextTag("color_gray") { Foreground = "gray" };
        public static readonly TextTag Normal =
            new TextTag("normal") { Weight = Pango.Weight.Normal };
        public static readonly TextTag Bold =
            new TextTag("bold") { Weight = Pango.Weight.Bold };
        public static readonly TextTag Italic =
            new TextTag("italic") { Style = Pango.Style.Italic };

        private readonly TextBuffer buffer;
        private TextIter iter;
        private TextTag textSize =
            new TextTag("size_normal") { SizePoints = 16 };

        public TextDisplay(TextBuffer tb)
        {
            buffer = tb;
            iter = buffer.StartIter;
            buffer.TagTable.Add(textSize);
            buffer.TagTable.Add(Normal);
            buffer.TagTable.Add(Bold);
            buffer.TagTable.Add(Italic);
            buffer.TagTable.Add(Color_Black);
            buffer.TagTable.Add(Color_Blue);
            buffer.TagTable.Add(Color_Gray);
            buffer.TagTable.Add(Color_Green);
            buffer.TagTable.Add(Color_Red);
        }

        public void Print(string text, FontColor color = FontColor.Black, FontStyle style = FontStyle.Default)
        {
            TextTag getColor()
            {
                switch (color)
                {
                    case FontColor.Red:
                        return Color_Red;
                    case FontColor.Blue:
                        return Color_Blue;
                    case FontColor.Green:
                        return Color_Green;
                    case FontColor.Gray:
                        return Color_Gray;
                    case FontColor.LightRed:
                    case FontColor.LightGreen:
                    case FontColor.LightBlue:
                    case FontColor.Black:
                    default:
                        return Color_Black;
                }
            }
            TextTag getStyle()
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
            buffer.InsertWithTags(ref iter, text, getColor(), getStyle(), textSize);
        }

        public void PrintLine()
        {
            buffer.InsertWithTags(ref iter, "\n", textSize);
        }

        public void PrintSpace()
        {
            buffer.InsertWithTags(ref iter, " ", textSize);
        }

        public void Clear()
        {
            buffer.Clear();
            iter = buffer.StartIter;
        }
    }
}
