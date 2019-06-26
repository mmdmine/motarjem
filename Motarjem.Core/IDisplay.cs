namespace Motarjem.Core
{
    public enum FontColor
    {
        Black, Gray,
        Red, LightRed,
        Blue, LightBlue,
        Green, LightGreen,
    }

    public enum FontStyle
    {
        Default, Bold, Italic
    }

    public interface IDisplay
    {
        void Print(string text, FontColor color = FontColor.Black, FontStyle style = FontStyle.Default);
        void PrintLine();
        void PrintSpace();
    }
}
