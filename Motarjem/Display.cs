using Motarjem.Core;
using System.Windows;
using System.Windows.Media;
using System.Windows.Documents;

namespace Motarjem
{
    class Display : IDisplay
    {
        private InlineCollection _inlines;

        private static readonly Brush black = Brushes.Black;
        private static readonly Brush gray = Brushes.Gray;
        private static readonly Brush error = new SolidColorBrush(Color.FromRgb(0xB0, 0x00, 0x20));
        private static readonly Brush light_red = new SolidColorBrush(Color.FromRgb(0xFF, 0x8A, 0x80));
        private static readonly Brush blue = Brushes.Blue;
        private static readonly Brush light_blue = Brushes.LightBlue;
        private static readonly Brush green = Brushes.Green;
        private static readonly Brush light_green = Brushes.LightGreen;


        public Display(InlineCollection inlines)
        {
            _inlines = inlines;
        }

        public void Print(string text, FontColor color = FontColor.Black, Core.FontStyle style = Core.FontStyle.Default)
        {
            Brush getColor()
            {
                switch (color)
                {
                    default:
                    case FontColor.Black:
                        return black;
                    case FontColor.Gray:
                        return gray;
                    case FontColor.Red:
                        return error;
                    case FontColor.LightRed:
                        return light_red;
                    case FontColor.Blue:
                        return blue;
                    case FontColor.LightBlue:
                        return light_blue;
                    case FontColor.Green:
                        return green;
                    case FontColor.LightGreen:
                        return light_green;
                }
            }
            System.Windows.FontStyle getStyle()
            {
                switch (style)
                {
                    default:
                    case Core.FontStyle.Bold:
                    case Core.FontStyle.Default:
                        return FontStyles.Normal;
                    case Core.FontStyle.Italic:
                        return FontStyles.Italic;
                }
            }
            _inlines.Add(new Run
            {
                Text = text,
                FontStyle = getStyle(),
                Foreground = getColor(),
                FontWeight = style == Core.FontStyle.Bold ? FontWeights.Bold : FontWeights.Normal,
            });
        }

        public void PrintLine()
        {
            _inlines.Add(new LineBreak());
        }

        public void PrintSpace()
        {
            _inlines.Add(new Run(" "));
        }
    }
}
