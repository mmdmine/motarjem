using Motarjem.Core;
using System.Windows;
using System.Windows.Media;
using System.Windows.Documents;

namespace Motarjem
{
    class Display : IDisplay
    {
        private InlineCollection _inlines;

        private static readonly Brush black = new SolidColorBrush(Color.FromRgb(0x26, 0x32, 0x38));
        private static readonly Brush gray = new SolidColorBrush(Color.FromRgb(0x78, 0x90, 0x9C));
        private static readonly Brush error = new SolidColorBrush(Color.FromRgb(0xB0, 0x00, 0x20));

        private static readonly Brush B900 = new SolidColorBrush(Color.FromRgb(0x01, 0x57, 0x9B));
        private static readonly Brush B700 = new SolidColorBrush(Color.FromRgb(0x02, 0x88, 0xD1));
        private static readonly Brush B500 = new SolidColorBrush(Color.FromRgb(0x03, 0xA9, 0xF4));
        private static readonly Brush BA400 = new SolidColorBrush(Color.FromRgb(0x00, 0xB0, 0xFF));
        private static readonly Brush BA700 = new SolidColorBrush(Color.FromRgb(0x00, 0x91, 0xEA));


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
                        return B500;
                    case FontColor.Blue:
                        return B900;
                    case FontColor.LightBlue:
                        return BA700;
                    case FontColor.Green:
                        return B700;
                    case FontColor.LightGreen:
                        return BA400;
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
